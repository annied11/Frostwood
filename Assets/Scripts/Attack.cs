using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // Collider2D attackCollider;
    [SerializeField]
    private int attackDamage;
    public Vector2 knockback = Vector2.zero;
    // private void Awake()
    // {
    //     attackCollider = GetComponent<Collider2D>();

    // }
    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggered with: " + collision.name);
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null)
        {
            Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);
            if (gotHit)
            {
                Debug.Log(collision.name + " hit for " + attackDamage + " with knockback: " + deliveredKnockback);
            }
        }
    }
}