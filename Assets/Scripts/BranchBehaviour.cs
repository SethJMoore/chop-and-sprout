using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchBehaviour : MonoBehaviour
{
    private float growthSpeed = 0.001f;
    private Vector3 scaleChange;

    // Start is called before the first frame update
    void Start()
    {
        scaleChange = new Vector3(0, growthSpeed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        grow();
    }

    void grow()
    {
        transform.localScale += scaleChange;
    }
}
