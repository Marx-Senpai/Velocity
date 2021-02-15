using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CotcSdk;
using TMPro;
public class XtraLifeManager : MonoBehaviour
{
    private UIManager  uiManager;
    private Cloud cloudVestige;

    public TextMeshProUGUI lastConnexionId;



    private string previousLoginKey = "previousLogin";
   
    private string previousPasswordKey = "previousPassword";

    private string networkIdKey = "networkId";

    public static string hasSeenIntroOnceKey = "introSeen";
    public static bool seenIntro;

    private string gamerHighScoreKey = "highScore";
    private float currentHighScore;
    Bundle scoreBundle;

    private string gamerHighTimeKey = "highTime";
    private float currentHighTime;
    Bundle timeBundle;

    private string leaderboardTimeKey = "leaderBoardTime";
    private string leaderboardScoreKey = "leaderBoardScore";

    private Gamer currentGamer;
    private Bundle introData;

    public void CustomAwake()
    {
        introData = Bundle.CreateObject();
        seenIntro = false;

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        

        var cb = FindObjectOfType<CotcGameObject>();
        cb.GetCloud().Done(cloud => {
            cloudVestige = cloud;

        });

      if(PlayerPrefs.HasKey(previousLoginKey) && PlayerPrefs.HasKey(previousPasswordKey))
        {
            uiManager.toggleResumeButton();
            lastConnexionId.text = PlayerPrefs.GetString(networkIdKey);

        }
        
    }


    #region AUTHENTIFICATION


    public void CheckSignUpLogin(string login, string password, bool firstTime)
    {

        cloudVestige.Login(
                network: "email",
                networkId: login,
                networkSecret: password)
            .Done(gamer => {

                uiManager.enableTransition();
                DidLogin(gamer);

                currentGamer = gamer;

                PlayerPrefs.SetString(previousLoginKey, gamer.GamerId);
                PlayerPrefs.SetString(previousPasswordKey, gamer.GamerSecret);
                PlayerPrefs.SetString(networkIdKey, gamer.NetworkId);

                
                if(firstTime)
                {
                    SetIntroSeenData(false);
                    SetHighScore(0, true);
                    SetHighTime(float.MaxValue, true);
                }
                else
                {
                    GetScores();
                }

                GetScoreLeaderboard();
                GetTimeLeaderboard();

                uiManager.RefreshDisplayLogin(gamer["profile"]["displayName"]);

                Debug.Log("Signed in succeeded (ID = " + gamer.GamerId + ")");
                Debug.Log("Login data: " + gamer);
                Debug.Log("Server time: " + gamer["servertime"]);
            }, ex => {
                // The exception should always be CotcException
                CotcException error = (CotcException)ex;
                Debug.LogError("Failed to login: " + error.ErrorCode + " (" + error.HttpStatusCode + ")");
            });
    }


    public void CheckUserDuringLogin(string login, string password)  // check if user already exists in database
    {
        cloudVestige.UserExists(login, password)
       .Done(userExistsRes => {

           CheckSignUpLogin(login, password, false); // if user already exist

           Debug.Log("User: " + userExistsRes.ToString());
           
       }, ex => {
        
           CheckSignUpLogin(login, password, true); // if user doesn't exist yet

         
           // The exception should always be CotcException
           CotcException error = (CotcException)ex;
           Debug.LogError("Failed to check user: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
       });
    }


    public void ResumeLastSession() 
    {
       
            cloudVestige.ResumeSession(
                gamerId: PlayerPrefs.GetString(previousLoginKey),
                gamerSecret: PlayerPrefs.GetString(previousPasswordKey))
            .Done(gamer => {

                uiManager.enableTransition();
                DidLogin(gamer);
                currentGamer = gamer;

                GetScores();
                GetScoreLeaderboard();
                GetTimeLeaderboard();

                uiManager.RefreshDisplayLogin(gamer["profile"]["displayName"]);

                Debug.Log("Signed in succeeded (ID = " + gamer.GamerId + ")");
                Debug.Log("Login data: " + gamer);
                Debug.Log("Server time: " + gamer["servertime"]);
            }, ex => {
                // The exception should always be CotcException
                CotcException error = (CotcException)ex;
                Debug.LogError("Failed to login: " + error.ErrorCode + " (" + error.HttpStatusCode + ")");
            });
          

    }

    public void SendLogout()
    {

        cloudVestige.Logout(currentGamer)
     .Done(result => {

         Loop.Stop();
         Debug.Log("Logout succeeded");
     }, ex => {
         // The exception should always be CotcException
         CotcException error = (CotcException)ex;
         Debug.LogError("Failed to logout: " + error.ErrorCode + " (" + error.HttpStatusCode + ")");
     });
    }

    #endregion

    DomainEventLoop Loop = null;
    void DidLogin(Gamer newGamer)
    {
        // Another loop was running; unless you want to keep multiple users active, stop the previous
        if (Loop != null)
            Loop.Stop();
        Loop = newGamer.StartEventLoop();
        Loop.ReceivedEvent += Loop_ReceivedEvent;
    }
    void Loop_ReceivedEvent(DomainEventLoop sender, EventLoopArgs e)
    {
        Debug.Log("Received event of type " + e.Message.Type + ": " + e.Message.ToJson());
    }



    #region INTRO BOOL


    public void TestSeenIntro() // check to see if user has already seen the intro section of the game
    {
        firstCam camScript = GameObject.Find("Camera_TitleScreen").GetComponent<firstCam>();

        currentGamer.GamerVfs.Domain("private").GetValue(hasSeenIntroOnceKey)
        .Done(getUserValueRes => {
         
           introData = getUserValueRes["result"][hasSeenIntroOnceKey]["value"];

            seenIntro = introData.AsBool();

            camScript.AfterPromiseStart();

            Debug.Log("User data: " + introData.ToString());
        }, ex => {

            camScript.AfterPromiseStart();
            // The exception should always be CotcException
            CotcException error = (CotcException)ex;
            Debug.LogError("Could not get user data due to error: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
        });


       

    }



    public void SetIntroSeenData(bool seen)
    {
        introData = Bundle.CreateObject();
        introData["value"] = seen;

        currentGamer.GamerVfs.Domain("private").SetValue(hasSeenIntroOnceKey, introData)
        .Done(introData => {
            Debug.Log("User data set: " + introData.ToString());
        }, ex => {
            // The exception should always be CotcException
            CotcException error = (CotcException)ex;
            Debug.LogError("Could not set user data due to error: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
        });
    }


    #endregion

    #region LEADERBOARD & SCORES
    public void GetScores() // check scores when user already have scores stored in database
    {

        currentGamer.GamerVfs.Domain("private").GetValue(gamerHighScoreKey)
        .Done(getUserValueRes => {

            currentHighScore = getUserValueRes["result"][gamerHighScoreKey]["value"].AsFloat();

                    currentGamer.GamerVfs.Domain("private").GetValue(gamerHighTimeKey)
                    .Done(getUserRes => {

                        currentHighTime = getUserRes["result"][gamerHighTimeKey]["value"].AsFloat();

                        Debug.Log("User data: " + getUserRes.ToString());
                    }, ex => {


                        // The exception should always be CotcException
                        CotcException error = (CotcException)ex;
                        Debug.LogError("Could not get user data due to error: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
                    });



            Debug.Log("User data: " + getUserValueRes.ToString());
        }, ex => {


            // The exception should always be CotcException
            CotcException error = (CotcException)ex;
            Debug.LogError("Could not get user data due to error: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
        });

    }




    public float CompareScores(float score)
    {
        if(score > currentHighScore)
        {
            currentHighScore = score;
            SetHighScore(score, false);
        }

        return currentHighScore;


    }

    public float CompareTimes(float time)
    {
        if (time < currentHighTime)
        {
            currentHighTime = time;
            SetHighTime(time, false);
        }

        return currentHighTime;
    }



    public void SetHighScore(float score, bool firstTime)
    {
        scoreBundle = Bundle.CreateObject();
        currentHighScore = score;
        scoreBundle["value"] = score;

        currentGamer.GamerVfs.Domain("private").SetValue(gamerHighScoreKey, scoreBundle)
        .Done(scoreBundle => {

            if(!firstTime)
            {
                PostHighScore();
            }
            
            Debug.Log("User data set: " + scoreBundle.ToString());
        }, ex => {
            // The exception should always be CotcException
            CotcException error = (CotcException)ex;
            Debug.LogError("Could not set user data due to error: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
        });
    }


    public void SetHighTime(float time, bool firstTime)
    {
        timeBundle = Bundle.CreateObject();
        currentHighTime = time;
        timeBundle["value"] = time;

        currentGamer.GamerVfs.Domain("private").SetValue(gamerHighTimeKey, timeBundle)
        .Done(timeBundle => {

            if (!firstTime)
            {
                PostHighTime();
            }

            Debug.Log("User data set: " + timeBundle.ToString());
        }, ex => {
            // The exception should always be CotcException
            CotcException error = (CotcException)ex;
            Debug.LogError("Could not set user data due to error: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
        });
    }

    public void PostHighScore()
    {
        currentGamer.Scores.Domain("private").Post(Mathf.FloorToInt(currentHighScore) , leaderboardScoreKey, ScoreOrder.HighToLow)
       .Done(postScoreRes => {

           GetScoreLeaderboard(); 
           Debug.Log("Post score: " + postScoreRes.ToString());
       }, ex => {
            // The exception should always be CotcException
            CotcException error = (CotcException)ex;
           Debug.LogError("Could not post score: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
       });
    }

    public void PostHighTime()
    {
        currentGamer.Scores.Domain("private").Post(Mathf.CeilToInt(currentHighTime), leaderboardTimeKey, ScoreOrder.LowToHigh)
       .Done(postScoreRes => {

           GetTimeLeaderboard();
           Debug.Log("Post score: " + postScoreRes.ToString());
       }, ex => {
           // The exception should always be CotcException
           CotcException error = (CotcException)ex;
           Debug.LogError("Could not post score: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
       });
    }


    public void GetTimeLeaderboard() // refresh Time LeaderBoard
    {
        currentGamer.Scores.Domain("private").BestHighScores(leaderboardTimeKey, 5, 1)
       .Done(bestHighTimesRes => {

           List<string> tempNames = new List<string>();
           List<long> tempValues = new List<long>();

           foreach (var score in bestHighTimesRes)
           {
               tempNames.Add(score.GamerInfo["profile"]["displayName"]);
               tempValues.Add(score.Value);

               Debug.Log(score.Rank + ". " + score.GamerInfo["profile"]["displayName"] + ": " + score.Value);
           }
           uiManager.ToggleTimeLBPanelVisibility(false);
           uiManager.RefreshTimeLeaderBoard(tempNames, tempValues);
               
       }, ex => {
            // The exception should always be CotcException
            CotcException error = (CotcException)ex;
           Debug.LogError("Could not get best high scores: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
       });
    }

    public void GetScoreLeaderboard()   // refresh Score LeaderBoard
    {
        currentGamer.Scores.Domain("private").BestHighScores(leaderboardScoreKey, 5, 1)
      .Done(bestHighScoresRes => {

          List<string> tempoNames = new List<string>();
          List<string> tempoValues = new List<string>();

          foreach (var score in bestHighScoresRes)
          {
              tempoNames.Add(score.GamerInfo["profile"]["displayName"]);
              tempoValues.Add(score.Value.ToString());

              Debug.Log(score.Rank + ". " + score.GamerInfo["profile"]["displayName"] + ": " + score.Value);
          }
          uiManager.ToggleScoreLBPanelVisibility(false);
          uiManager.RefreshScoreLeaderBoard(tempoNames, tempoValues);

      }, ex => {
           // The exception should always be CotcException
           CotcException error = (CotcException)ex;
          Debug.LogError("Could not get best high scores: " + error.ErrorCode + " (" + error.ErrorInformation + ")");
      });
    }



    #endregion


}
