using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    private float movementForce = 1;
    private List<Collider2D> otherCollidersBeingTouched = new List<Collider2D>();
    private Rigidbody2D rb;
    private Vector2 moveVal = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // No jumping or climbing allowed. Only climbing.
        if (otherCollidersBeingTouched.Count > 0)
        {
            rb.AddForce(moveVal * movementForce);
        }
    }

    void OnTriggerEnter2D (Collider2D otherCollider)
    {
        otherCollidersBeingTouched.Add(otherCollider);
        if (otherCollidersBeingTouched.Count > 0)
        {
            rb.gravityScale = 0;
            rb.drag = 10;
        }
    }

    void OnTriggerExit2D (Collider2D otherCollider)
    {
        otherCollidersBeingTouched.Remove(otherCollider);
        if (otherCollidersBeingTouched.Count == 0)
        {
            rb.gravityScale = 1;
            rb.drag = 0;
        }
    }
    
    void OnMove(InputValue value)
    {
        moveVal = value.Get<Vector2>();
    }

    void OnChop()
    {

    }

    void OnSprout()
    {

    }
}
