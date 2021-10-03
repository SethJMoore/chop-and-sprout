using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject segment;
    [SerializeField]
    private float timeBetweenNewSegments = 1f;
    private GameObject lastSegment;
    private Transform lastSegmentTransform;
    private float timeSinceLastSegment = 0;
    private List<GameObject> listOfSegments = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        lastSegmentTransform = transform;
        addSegment();
    }

    // Update is called once per frame
    void Update()
    {
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
        int index = listOfSegments.IndexOf(segment);
        if (index == 0)
        {
            // TODO: Destroy this whole branch, probably?
        }
        else
        {
            int removeCount = listOfSegments.Count - index;
            List<GameObject> brokenSegments = listOfSegments.GetRange(index, removeCount);
            listOfSegments.RemoveRange(index, removeCount);
            brokenSegments.ForEach(delegate(GameObject segment)
            {
                GameObject.Destroy(segment);
                // TODO: If child branches are child game objects of the last
                //       segment, I shouldn't have to worry about manually
                //       taking care of them.
            });
            lastSegment = listOfSegments[index -1];
        }
    }

    private void addSegment()
    {
        //lastSegment = Instantiate(segment, lastSegmentTransform);
        if (lastSegment)
        {
            lastSegment = Instantiate(segment, transform);
            lastSegment.transform.localPosition = lastSegmentTransform.localPosition + Vector3.down * 0.05f;
            lastSegmentTransform = lastSegment.transform;
            listOfSegments.Add(lastSegment);
        }
        else
        {
            lastSegment = Instantiate(segment, transform);
            lastSegment.transform.position = lastSegmentTransform.localPosition + Vector3.down * 0.05f;
            lastSegmentTransform = lastSegment.transform;
            listOfSegments.Add(lastSegment);
        }
    }

    Quaternion randomRotation()
    {
        return Quaternion.Euler(0.0f, 0.0f, Random.Range(0, 360));
    }
}
