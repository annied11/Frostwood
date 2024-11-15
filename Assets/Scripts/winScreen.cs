using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class winScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup()
    {
        gameObject.SetActive(true);
    }

    public void playAgain()
    {
        SceneManager.LoadScene("GameplayScene");
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
