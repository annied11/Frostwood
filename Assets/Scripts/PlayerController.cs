using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float jumpForce = 6f;
    public int maxJumpCount = 2; // Maximum number of consecutive jumps
    private int jumpCount = 0; // Current number of consecutive jumps
    public collectableManager collectableCount;
    [SerializeField] private bool _isMoving = false;
    Vector2 moveInput;
    Rigidbody2D rb;
    AudioSource footstep;
    Animator animator;
    Damageable damageable;
    TouchingDirections touchingDirections;

    public bool IsMoving
    {
        get { return _isMoving; }
        private set
        {
            _isMoving = value;
            animator.SetBool("isMoving", value);
        }
    }
    
    public bool CanMove { get {
            return animator.GetBool("canMove");
        } 
    }

    public bool IsAlive 
    {
        get
        {
            return animator.GetBool("isAlive");
        }
    }

    public bool _isFacingRight = true;
    public bool IsFacingRight 
    {
        get {
            return _isFacingRight;
        }
        private set {
            if (_isFacingRight != value)
            {
                // flip local scale for player to face the other direction
                transform.localScale *=  new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    public bool LockVelocity
    {
        get
        {
            return animator.GetBool("lockVelocity");
        }
        set
        {
            animator.SetBool("lockVelocity", value);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.ResetTrigger("attack");
        damageable = GetComponent<Damageable>();
        footstep = GetComponent<AudioSource>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    private void Update()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame && jumpCount < maxJumpCount && CanMove)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
        }

        float movementThreshold = 0.1f;

        if (touchingDirections.IsGrounded && Mathf.Abs(rb.velocity.x) > movementThreshold)
        {
            if (!footstep.isPlaying)
            {
                footstep.Play();
            }
        }
        else
        {
            footstep.Stop();
        }
        // Check if player is out of the camera view
        CheckBounds();
    }

    private void FixedUpdate()
    {
        if (CanMove && !damageable.LockVelocity) {
            rb.velocity = new Vector2(moveInput.x * walkSpeed, rb.velocity.y);
        } else {
            // Optionally stop movement entirely by setting horizontal speed to zero
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        } else {
            IsMoving = false;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            // face right
            IsFacingRight = true;
        } else if (moveInput.x < 0 && IsFacingRight) {
            // face left
            IsFacingRight = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reset jump count when the player touches the ground or another surface
        jumpCount = 0;
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);
            foreach (var contact in contacts)
            {
                if (contact.normal.y > 0.5f) // Check if the contact point is from above
                {
                    // Call a method on the enemy to handle its damage
                    collision.gameObject.GetComponent<Damageable>().Hit(10, Vector2.zero); // Assuming 1 damage for step on

                    // Apply bounce effect to the player
                    rb.velocity = new Vector2(rb.velocity.x, 10f);
                    break;
                }
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started && CanMove)
        {
            animator.SetTrigger("attack");
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        Debug.Log("Before applying knockback: " + rb.velocity);
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        Debug.Log("After applying knockback: " + rb.velocity);
        Debug.DrawRay(transform.position, new Vector3(knockback.x, knockback.y, 0) * 2, Color.red, 2.0f);
    }

    private void CheckBounds()
    {
        if (Camera.main)
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
            bool outOfBounds = screenPoint.y < 0 || screenPoint.y > 1 || screenPoint.x < 0 || screenPoint.x > 1;
            if (outOfBounds && damageable.IsAlive)
            {
                damageable.Health = 0; // Immediately set health to 0 when out of bounds
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectable"))
        {
            Destroy(collision.gameObject);
            collectableCount.count++;
        }
    }
}
