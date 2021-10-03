using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject segment; // Segment prefab
    [SerializeField]
    private GameObject branch; // Branch prefab
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
    }

    public void sproutNewBranches()
    {
        if (childBranches == null)
        {
            isGrowing = false;
            // TODO: Instantiate two branches with same position as lastSegment and
            //       attach lastSegment to them with FixedJoint. They need to have
            //       their rotations set at randomly downward angles.
            //       childBranches = {branch1, branch2}
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
            lastSegment.transform.localPosition = lastSegmentTransform.localPosition + Vector3.down * 0.05f;
        }
        else
        {
            lastSegment = Instantiate(segment, transform);
            lastSegment.transform.position = lastSegmentTransform.localPosition + Vector3.down * 0.05f;
        }
        lastSegmentTransform = lastSegment.transform;
        listOfSegments.Add(lastSegment);
    }

    Quaternion randomRotation()
    {
        return Quaternion.Euler(0.0f, 0.0f, Random.Range(0, 360));
    }
}
