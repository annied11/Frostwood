using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D castFilter;
    CapsuleCollider2D touchingCol;
    PolygonCollider2D polygonCollider;
    Animator animator;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    [SerializeField]
    private bool _isGrounded = true;
    public bool IsGrounded { get
        {
            return _isGrounded;
        } private set {
            _isGrounded = value;
            animator.SetBool("isGrounded", value);
        } 
    }

    [SerializeField]
    private bool _isOnWall;
    public bool IsOnWall { get
        {
            return _isOnWall;
        } private set {
            _isOnWall = value;
            animator.SetBool("isOnWall", value);
        } 
    }

    public void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (touchingCol != null)
        {
            IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
            IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        }
        
        if (polygonCollider != null) {
            // Implement similar ground and wall checks for polygon collider if necessary
            // Example:
            IsGrounded = polygonCollider.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
            IsOnWall = polygonCollider.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        }
    }
}
