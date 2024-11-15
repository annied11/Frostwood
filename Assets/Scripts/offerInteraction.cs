using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class offerInteraction : MonoBehaviour
{
    public GameObject offerPrompt; // Assign the "Press F to Offer" GameObject
    public GameObject incompletePrompt; // Assign the "You have not collected all diamonds" GameObject
    public collectableManager collectableManager; // Reference to your collectableManager script

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) // Ensure your player GameObject has the tag "Player"
        {
            // Check if all collectables are collected
            if (collectableManager.AreAllCollectablesCollected())
            {
                if (offerPrompt != null)
                {
                    offerPrompt.SetActive(true); // Show the offer prompt
                }
                if (incompletePrompt != null)
                {
                    incompletePrompt.SetActive(false); // Ensure the incomplete prompt is hidden
                }
            }
            else
            {
                if (incompletePrompt != null)
                {
                    incompletePrompt.SetActive(true); // Show the incomplete prompt
                }
                if (offerPrompt != null)
                {
                    offerPrompt.SetActive(false); // Ensure the offer prompt is hidden
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Hide both prompts when the player leaves the area
            if (offerPrompt != null)
            {
                offerPrompt.SetActive(false);
            }
            if (incompletePrompt != null)
            {
                incompletePrompt.SetActive(false);
            }
        }
    }
}
