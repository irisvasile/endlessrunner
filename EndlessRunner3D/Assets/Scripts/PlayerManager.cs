using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Globalization;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public GameObject gameOverPanel;
    public static bool isGameStarted;
    public GameObject startingText;
    public Text coinsText;
    public static int numberOfCoins;
    void Start()
    {
        gameOver = false;
        Time.timeScale = 1;
        isGameStarted = false;
        numberOfCoins = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOver)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0;
            
        }
        coinsText.text = "Coins: " + numberOfCoins;
        if(SwipeManager.tap)
        {
            isGameStarted = true;
            Destroy(startingText);
        }
    }
}
