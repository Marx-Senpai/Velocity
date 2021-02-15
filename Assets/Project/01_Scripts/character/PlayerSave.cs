using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSave : MonoBehaviour
{
    public string activePlayerKey;
    public string activeBestTimeKey;
    public string activeHighScoreKey;
    public string activeArenaOneKey;
    public string playerName;
    public Text textPlayerName;
    public float bestTime;
    public float highScore;
    public bool arenaOneFinished;
    public Text highscoreText;


    public List<string> playerKeys;
    public List<string> BestTimeKeys;
    public List<string> HighScoreKeys;
    public string xmlKey;
    public string arenaOneKey;
    public string introKey;
    
    public GameObject panelSkip;


    public static bool asSeenIntro;
    
    bool hasKey; 


   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            PlayerPrefs.DeleteAll();
           // TestPlayerKeys();
            Debug.Log("Keys Cleared");
        }
    }

}
