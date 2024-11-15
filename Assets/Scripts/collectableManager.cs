using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class collectableManager : MonoBehaviour
{
    public int count;
    public int requiredCount = 10;
    public TextMeshProUGUI collectableText;
    public GameObject winScreen; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        collectableText.text = count.ToString();
        // if (count = 10)
        // {

        // }
    }

    public bool AreAllCollectablesCollected()
    {
        return count >= requiredCount;
    }
}
