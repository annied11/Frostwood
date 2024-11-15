using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class offerLocation : MonoBehaviour
{
    public collectableManager collectableManager;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (collectableManager.AreAllCollectablesCollected())
            {
                collectableManager.winScreen.SetActive(true);
            }
        }
    }
}
