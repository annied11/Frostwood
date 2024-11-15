using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startScreen : MonoBehaviour
{
    // Reference to the start screen GameObject
    public GameObject startScreenUI;

    void Start()
    {
        Setup();
    }

    public void Setup()
    {
        // Enable the start screen at the beginning
        startScreenUI.SetActive(true);
    }

    public void playAgain()
    {
        // Disable the start screen
        startScreenUI.SetActive(false);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
