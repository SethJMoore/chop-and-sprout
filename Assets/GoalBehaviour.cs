using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBehaviour : MonoBehaviour
{
    private SpriteRenderer winningBeastSpriteRenderer;
    private float timeSinceColorChange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceColorChange += Time.deltaTime;
        if (winningBeastSpriteRenderer != null && timeSinceColorChange >= 0.2)
        {
            winningBeastSpriteRenderer.color = Random.ColorHSV();
            timeSinceColorChange = 0;
        }
    }

    void OnTriggerEnter2D (Collider2D otherCollider)
    {
        if (otherCollider.gameObject.name == "Beast")
        {
            Debug.Log("WIN");
            winningBeastSpriteRenderer = otherCollider.GetComponent<SpriteRenderer>();
        }
    }
}
