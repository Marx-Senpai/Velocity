using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public float score = 0;
    private int chain = 1;
    private int chainLength = 0;
    [HideInInspector] public float timeArena;
    private enum Rank { S, A, B, C, D }
    private Rank rank;
    public float surviveTimer;
    
    [Header("Points by action")] 
    [SerializeField] private float pointsByEnemy;
    [SerializeField] private float pointsByEnemyModule;
    
    [Header("Points by time")] 
    [SerializeField] private float pointsBaseByTime;
    [SerializeField] private float pointsMultiplierByTime;
    [SerializeField] private float pointsMaxByTime;
    
    [Header("Waves timers")] 
    [SerializeField] private float minTimeWave;
    [SerializeField] private float maxTimeWave;
    
    [Header("UI - Chaining feedbacks")]
    [SerializeField] private int chainMax;
    [SerializeField] private Image img;
    [SerializeField] private GameObject sphere;
    [SerializeField] private Image img_ui;
    
    [SerializeField] private float timeMax;
    [SerializeField] private float timeSpeed;
    [SerializeField] private float speed_SphereRot;
    private float timeValue = 0;
    private Vector3 pos_startSphere;
    private Vector3 pos_startSphere_ui;
    private Transform player_transform;

    [Header("UI - Score")]
    [SerializeField] public TextMeshProUGUI txt_score;
    [SerializeField] private TextMeshProUGUI txt_tempScore;
    [SerializeField] public TextMeshProUGUI txt_chainMultiplier;
    [SerializeField] private TextMeshProUGUI txt_chainLength;
    [SerializeField] public TextMeshProUGUI txt_timer;
    
    [SerializeField] private TextMeshProUGUI txt_rank;
    [SerializeField] private TextMeshProUGUI txt_scoreDone;
    [SerializeField] public TextMeshProUGUI txt_highscore;
    [SerializeField] public TextMeshProUGUI txt_bestTime;
    [SerializeField] private TextMeshProUGUI txt_timeDone;
    
    [Header("UI - Panels")]
    [SerializeField] private GameObject panel_inGame;
    [SerializeField] private GameObject panel_endGame;
    

    void Start()
    {
        pos_startSphere = sphere.transform.localPosition;
        player_transform = GameObject.Find("Player").transform;

        txt_chainMultiplier.text = "x" + chain.ToString();
        txt_chainLength.text = chainLength.ToString();
        txt_score.text = score.ToString();
    }
    
    void Update()
    {

        if (txt_timer.gameObject.activeSelf)
        {
            surviveTimer += Time.deltaTime;
            txt_timer.text = FormatTime(surviveTimer);
        }
      
        
        if (timeValue <= 0)
        {
            // UI Update
            chain = 1;
            txt_chainMultiplier.text = "x" + chain.ToString();
            chainLength = 0;
            txt_chainLength.text = chainLength.ToString();
            
            // Feedbacks Update
            sphere.SetActive(false);
            img.gameObject.SetActive(false);
            
            img_ui.gameObject.SetActive(false);
        }
        
        DecreaseChainTimer();
    }

    
    // --------------------------------------------- UI ---------------------------------------------
    void DecreaseChainTimer()
    {
        if (timeValue > 0)
        {
            timeValue -= (timeSpeed * Time.deltaTime);
            img.fillAmount = timeValue / timeMax;
            sphere.transform.RotateAround(player_transform.position, Vector3.up, -speed_SphereRot * Time.deltaTime);
            
            img_ui.fillAmount = timeValue / timeMax;
        }
    }

    void ResetChainTimer(int incr, bool isCoeur)
    {

        if (isCoeur)
        {
            chainLength += incr;
            
            if (chain < chainMax && chainLength >= 2 )
            {
                chain += incr;
            }
            
        }
        
        // UI Update
        txt_chainMultiplier.text = "x" + chain.ToString();
        txt_chainLength.text = chainLength.ToString();

        if (isCoeur)
        {
            // Feedbacks Update
            img.fillAmount = 1;
            img_ui.fillAmount = 1;
            timeValue = timeMax;
        }

        sphere.SetActive(true);
        img.gameObject.SetActive(true);
        img_ui.gameObject.SetActive(true);
        
        sphere.transform.localPosition = pos_startSphere;
        
        DecreaseChainTimer();
    }

    public void BreakChain()
    {
        chain = 1;
        chainLength = 0;
        
        // UI Update
        txt_chainMultiplier.text = "x" + chain.ToString();
        txt_chainLength.text = chainLength.ToString();
        
        // Feedbacks Update
        img.fillAmount = 0;
        img_ui.fillAmount = 0;
        timeValue = 0;
        
        sphere.SetActive(false);
        img.gameObject.SetActive(false);
        img_ui.gameObject.SetActive(false);
        
        sphere.transform.localPosition = pos_startSphere;
    }

    IEnumerator FadeTempScore()
    {
        var txt_color = txt_tempScore.color;
        for (float i = 1f; i >= 0; i -= Time.deltaTime*2)
        {
            txt_tempScore.color = new Color(txt_color.r, txt_color.g, txt_color.b, i);
            yield return null;
        }
        txt_tempScore.color = new Color(txt_color.r, txt_color.g, txt_color.b, 0);
    }
    
    public string FormatTime (float time){
        int intTime = (int)time;
        int minutes = intTime / 60;
        int seconds = intTime % 60;
        string timeText = String.Format ("{0:00}:{1:00}", minutes, seconds);
        return timeText;
    }
    
    // --------------------------------------------- SCORING ---------------------------------------------
    public void AddScore(GameObject obj, bool isCoeur)
    {
        var addingScore = 0f;
        var chaining = chain;
        
        if (obj.layer == 26)
        {
            addingScore = pointsByEnemy;
            ResetChainTimer(1, isCoeur);
        }

        
        if (obj.CompareTag("Recoltable"))
        {
            var suiveur_mng = obj.GetComponent<SuiveursManager>();

            if (suiveur_mng.isFromEnnemy)
            {
                addingScore = pointsByEnemyModule;
            }

            if (chainLength >= 1)
            {
                ResetChainTimer(0, true);
            }
        }
        
        score += addingScore * chaining;
        
        if(addingScore > 0)
        {
            txt_score.text = score.ToString();
            txt_tempScore.text = "+" + (addingScore * chaining).ToString();
            txt_tempScore.alpha = 1f;
            StartCoroutine(FadeTempScore());
        }
    }

    public void FinalScore()
    {
        var timeScore = 0f;

        if (timeArena < minTimeWave)
        {
            timeScore = pointsMaxByTime;
        }
        else if(timeArena > maxTimeWave)
        {
            timeScore = 0;
        }
        else
        {
            timeScore = pointsBaseByTime + pointsMultiplierByTime * (1 - (timeArena - minTimeWave) / (maxTimeWave - minTimeWave)); 
        }

        score += timeScore;

        txt_scoreDone.text = score.ToString();
        txt_timeDone.text = FormatTime(timeArena);
        
        panel_endGame.GetComponent<Image>().color = new Color(0, 0, 0, 0.2f);
        panel_endGame.SetActive(true);
    }

    private void AttributeRank()
    {
        switch (rank)
        {
            case Rank.D:
                break;
            case Rank.C:
                break;
            case Rank.B:
                break;
            case Rank.A:
                break;
            case Rank.S:
                break;
        }
    }

    public void ActiveInGamePanel(bool hiding)
    {
        panel_inGame.SetActive(hiding);
    }
}
