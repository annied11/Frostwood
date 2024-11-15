using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthRestore = 20;
    public float floatStrength = 0.1f; // Amplitude of the float effect
    public float floatSpeed = 4f; // Speed of the float effect

    private float originalY; // Original Y position
    AudioSource pickupSource;

    private void Awake()
    {
        pickupSource = GetComponent<AudioSource>();
        originalY = transform.position.y; // Capture the original Y position
    }

    void Update()
    {
        // Floating effect: oscillate the Y position using a sine wave
        float newY = originalY + Mathf.Sin(Time.time * floatSpeed) * floatStrength;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Triggered with: " + collision.gameObject.name + ", Collider: " + collision.name);
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable)
        {
            bool wasHealed = damageable.Heal(healthRestore);

            if (wasHealed)
            {
                if(pickupSource)
                {
                    AudioSource.PlayClipAtPoint(pickupSource.clip, transform.position, pickupSource.volume);
                }
                Destroy(gameObject); // Destroy the pickup after use
            }
        }
    }
}
