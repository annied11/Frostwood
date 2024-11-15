using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    // implement feature for enemy to move towards player when detected by hitboxDetectionZone & only initialize attack when player enters attackZone
    // fix glitch between idle & walk when ?
    // is not stopping when attacking player?
    
    // Start is called before the first frame update
    public float walkSpeed = 3f;
    public DetectionZone hitboxDetectionZone;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;
    Rigidbody2D rb;
    Animator animator;
    Damageable damageable;
    public enum WalkableDirection {Right, Left}
    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;
    TouchingDirections touchingDirections;
    public float walkStopRate = 0.05f;
    private Vector2 spawnPosition;
    

    public bool _isFacingRight = true;
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
        set { 
            if (_walkDirection != value)
            {
                // Direction flipped
                gameObject.transform.localScale = new Vector2(-gameObject.transform.localScale.x, gameObject.transform.localScale.y);
                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                } else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            
            _walkDirection = value; }
    }

    public bool _hasTarget = false;
    public bool HasTarget { get
    {
        return _hasTarget;
    } private set
    {
        _hasTarget = value;
        animator.SetBool("hasTarget", value);
    }
    }

    // public bool _inAttackZone = false;
    // public bool InAttackZone { get
    // {
    //     return _inAttackZone;
    // } private set
    // {
    //     _inAttackZone = value;
    //     animator.SetBool("hasTarget", value);
    // }
    // }

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
        if (touchingDirections.IsOnWall && touchingDirections.IsGrounded && !HasTarget)
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
        //SetFacingDirection(?);
    }
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = Vector2.zero;
        spawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = hitboxDetectionZone.detectedColliders.Count > 0;
        // bool playerInAttackZone = attackZone.detectedColliders.Count > 0;

        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
        animator.SetBool("canMove", HasTarget && attackZone.detectedColliders.Count == 0);

        // Return to spawn if no target
        if (!HasTarget)
        {
            ReturnToSpawn();
        }
        // if (touchingDirections.IsOnWall && HasTarget)
        // {
        //     HasTarget = !HasTarget;
        // }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        bool targetFacingRight = moveInput.x > 0; // Determines if the movement input suggests facing right

        if (targetFacingRight != IsFacingRight)
        {
            // Change the facing direction only if there's a discrepancy
            IsFacingRight = targetFacingRight;
        }
    }

    public void OnCliffDetected()
    {
        if (touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
    }

    void ReturnToSpawn()
    {
        if (Vector2.Distance(transform.position, spawnPosition) > 0.1f && !HasTarget) // Check if Golem is not at spawn
        {
            Vector2 moveDirection = (spawnPosition - (Vector2)transform.position).normalized;
            rb.velocity = moveDirection * walkSpeed;
            SetFacingDirection(moveDirection); // Adjust facing direction based on moving direction
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop moving when at spawn
            if (!_isFacingRight) // Check if facing direction needs to be reset
            {
                IsFacingRight = true; // Reset facing direction to right
            }
        }
    }
}
