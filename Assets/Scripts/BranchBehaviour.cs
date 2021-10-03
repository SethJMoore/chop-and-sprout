using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject segment; // Segment prefab
    [SerializeField]
    public GameObject branch; // Branch prefab
    [SerializeField]
    private float timeBetweenNewSegments = 1f;
    private GameObject lastSegment;
    private Transform lastSegmentTransform;
    private float timeSinceLastSegment = 0;
    private List<GameObject> listOfSegments = new List<GameObject>();
    private bool isGrowing;
    private GameObject[] childBranches;

    // Start is called before the first frame update
    void Start()
    {
        isGrowing = true;
        lastSegmentTransform = transform;
        addSegment();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGrowing)
        {
            return;
        }
        if (timeSinceLastSegment >= timeBetweenNewSegments)
        {
            addSegment();
            timeSinceLastSegment = 0;
        }
        else
        {
            timeSinceLastSegment += Time.deltaTime;
        }
    }

    public void chopAt(GameObject segment)
    {
        isGrowing = false;
        if (childBranches != null)
        {
            foreach (var branch in childBranches)
            {
                branch.GetComponent<BranchBehaviour>().dieOff();
            }
            childBranches = null;
        }
        int index = listOfSegments.IndexOf(segment);
        if (index == 0)
        {
            // We're gonna leave a bud here instead of destroying the whole branch.
            index = 1;
        }
        int removeCount = listOfSegments.Count - index;
        List<GameObject> brokenSegments = listOfSegments.GetRange(index, removeCount);
        listOfSegments.RemoveRange(index, removeCount);
        brokenSegments.ForEach(delegate(GameObject segment)
        {
            GameObject.Destroy(segment);
        });
        lastSegment = listOfSegments[index -1];
        lastSegmentTransform = lastSegment.transform;
    }

    public void sproutNewBranches()
    {
        if (childBranches == null)
        {
            isGrowing = false;
            GameObject branch1 = Instantiate(branch, lastSegmentTransform.position, randomRotationDown());
            branch1.SetActive(true);
            branch1.GetComponent<BranchBehaviour>().branch = branch;
            gameObject.AddComponent<FixedJoint2D>().connectedBody = branch1.GetComponent<Rigidbody2D>();
            GameObject branch2 = Instantiate(branch, lastSegmentTransform.position, randomRotationDown());
            branch2.SetActive(true);
            branch2.GetComponent<BranchBehaviour>().branch = branch;
            gameObject.AddComponent<FixedJoint2D>().connectedBody = branch2.GetComponent<Rigidbody2D>();
            childBranches = new GameObject[] {branch1, branch2};
        }
        else
        {
            foreach (var branch in childBranches)
            {
                branch.GetComponent<BranchBehaviour>().sproutNewBranches();
            }
        }
    }

    public void dieOff()
    {
        if (childBranches != null)
        {
            foreach (var branch in childBranches)
            {
                branch.GetComponent<BranchBehaviour>().dieOff();
            }
        }
        GameObject.Destroy(gameObject);
    }

    private void addSegment()
    {
        // The positioning logic is a little different if no segments have been added yet.
        if (lastSegment)
        {
            lastSegment = Instantiate(segment, transform);
            lastSegment.transform.localPosition = lastSegmentTransform.localPosition + Vector3.down * 0.125f;
        }
        else
        {
            lastSegment = Instantiate(segment, transform);
            lastSegment.transform.position = lastSegmentTransform.localPosition + Vector3.down * 0.125f;
        }
        lastSegmentTransform = lastSegment.transform;
        listOfSegments.Add(lastSegment);
    }

    Quaternion randomRotationDown()
    {
        return Quaternion.Euler(0.0f, 0.0f, Random.Range(-90, 90));
    }
}
