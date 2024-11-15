using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ladderMovement : MonoBehaviour
{
    public float climbSpeed = 4f;
    [SerializeField] private Rigidbody2D rb;
    private bool isClimbing = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isClimbing)
        {
            float verticalInput = Input.GetAxis("Vertical"); // Use Unity's input system to get vertical input
            rb.velocity = new Vector2(0, verticalInput * climbSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            isClimbing = true;
            rb.gravityScale = 0; // Disable gravity when on the ladder
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            isClimbing = false;
            rb.gravityScale = 2; // Re-enable gravity when not on the ladder
        }
    }
}
