using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
public float walkSpeed = 3f;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;
    Rigidbody2D rb;
    Animator animator;
    Damageable damageable;
    public enum WalkableDirection {Right, Left}
    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.left;
    TouchingDirections touchingDirections;
    public float walkStopRate = 0.05f;
    

    public bool _isFacingRight = false;
    public bool IsFacingRight 
    {
        get {
            return _isFacingRight;
        }
        private set {
            if (_isFacingRight != value)
            {
                Debug.Log($"IsFacingRight changed from {_isFacingRight} to {value}, flipping sprite.");
                // flip local scale for player to face the other direction
                transform.localScale *=  new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    private WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                // Direction flipped
                gameObject.transform.localScale = new Vector2(-gameObject.transform.localScale.x, gameObject.transform.localScale.y);
                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.left;
                    //IsFacingRight = false;
                }
                else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.right;
                    //IsFacingRight = true;
                }
            }

            _walkDirection = value;
        }
    }

    public bool _hasTarget = false;
    public bool HasTarget { get
    {
        return _hasTarget;
    } private set
    {
        _hasTarget = value;
        animator.SetBool("hasTarget", value);
    }}

    public bool CanMove 
    {
        get 
        {
            return animator.GetBool("canMove");
        }
    }

    public float AttackCooldown
    {
        get 
        {
            return animator.GetFloat("attackCooldown");
        } private set {
            animator.SetFloat("attackCooldown", Mathf.Max(value, 0));
        }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    private void FixedUpdate()
    {
        if (touchingDirections.IsOnWall && touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
        if (!damageable.LockVelocity)
        {
            if (CanMove)
            {
                rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
            } else {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            }
        }
        
        
    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        } else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        } else 
        {
            Debug.LogError("Current walkable direction is not set to legal values of left/right.");
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
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

    public void OnCliffDetected()
    {
        if (touchingDirections.IsGrounded || touchingDirections.IsOnWall)
        {
            FlipDirection();
        }
    }

}
