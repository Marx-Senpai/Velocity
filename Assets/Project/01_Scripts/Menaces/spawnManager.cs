using System.Collections;
using System.Collections.Generic;
using System.Xml;
using FMODUnity;
using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.UI;

public class spawnManager : MonoBehaviour
{

    public GameObject arene;
    public Vector3 areneCenter;
    public float arenaLimites;

    public float spawnRadius;

    public List<GameObject> ennemies;

    public float spawnTimerInterval;

    public List<GameObject> spawningPool;
    public int spawningPoolSize;

    public int spawnCount ;
    int customSpawnCount, subCount;

    private GameObject player;

    public int checkPvCount;

    public int nbrWaveRegain = 3;

    public float spawnTimer, spawnTimerLimit;

    public float randomRadius;

    public bool deathAloneIsDelayed;

    public float deathDelay;
    public int waveCount;

    public  bool canSpawnWave;

    private bool isLastOfWave;

    public GameObject enterDoor, finishDoor;


    public List<GameObject> partiesMonolith;
    [SerializeField] private GameObject ps_EndWave;
    [SerializeField] private GameObject ps_UnitWave;

    [SerializeField] private GameObject spawning_feedback;
    private SoundManager sd;
    
    
    [System.Serializable]
    public class SubWave
    {
        public float waveTimerStart;
        //public int regainDeVieParVague;
        public List<GameObject> ennemis;
        public List<Transform> spawnTransforms;

        public SubWave(float timer, List<GameObject> en, List<Transform> tran)
        {
            waveTimerStart = timer;
            ennemis = en;
            spawnTransforms = tran;
        }
    }
    
    [System.Serializable]
    public class Wave
    {
        public List<SubWave> subWaves;

        public Wave( List<SubWave> sw)
        {
            subWaves = sw;
        }
        
        
    }

   

    [SerializeField]
    public List<Wave> customWaves;

    
    public enum SpawnMode
    {
        PressingButton,
        CustomWaves
    }

    private GameObject[] allSuiveurs;

    public SpawnMode spawn = SpawnMode.PressingButton;

    public int ennemyCount;
    public int ennemyMin;

    private GameObject[] allSpawns;

    private GameManager gm;


    public void Awaken()
    {
        sd = GameObject.Find("Manager").GetComponent<SoundManager>();
        areneCenter = arene.transform.position;
        player = GameObject.Find("Player");

        GameObject.Find("Main Camera").GetComponent<CameraController>().areneCenterPoint = areneCenter;
        randomRadius = GameObject.Find("Manager").GetComponent<GameManager>().randomRadius;

        gm = GameObject.Find("Manager").GetComponent<GameManager>();

        /*if(spawn == SpawnMode.CustomWaves)
        {
            poolWaves();
        }*/
    }



    public void StartCustomWaveInGame()
    {
        if(spawn == SpawnMode.CustomWaves)
        {
            poolWaves();
        }
    }

    private void Update()
    {

        
        
        if (Input.GetKeyDown(KeyCode.Keypad1) && gm.IsCheatActive)
        {
            GameObject tempGO = ennemies[0];
            
            Vector3 tempArenaV = new Vector3(0, 3.41f, 0) + areneCenter;
            
            GameObject spawningEnnemy =   Instantiate(tempGO, tempArenaV, Quaternion.identity);
      
            spawningEnnemy.GetComponent<Blackboard>().SetValue("Target", player);
            spawningEnnemy.GetComponent<StudioEventEmitter>().enabled = true;
            ennemyCount++;
             
                    partiesMonolith[2].SetActive(false);
                    PlayerPrefs.SetInt("arenaOne", 1);
                    enterDoor.GetComponent<DoorBehavior>().DesactivateDoor();

                    GameObject manager = GameObject.Find("Manager");
                 
                    manager.GetComponent<ScoreManager>().txt_timer.gameObject.SetActive(true);
                    manager.GetComponent<ScoreManager>().txt_timer.gameObject.transform.parent.gameObject.SetActive(false);
                    manager.GetComponent<ScoreManager>().txt_score.gameObject.transform.parent.gameObject.SetActive(false);
                    manager.GetComponent<ScoreManager>().txt_chainMultiplier.gameObject.transform.parent.gameObject.SetActive(false);
                    player.GetComponent<CharacterInputDetector>().circleGo.SetActive(false);
                    SpawnSuiveurBehavior.canSpawn = false;

                   // GameObject.Find("StartTrigger").SetActive(false);

                    allSpawns = GameObject.FindGameObjectsWithTag("SpawnSuiveur");


                    player.GetComponent<CharacterInputDetector>().stance =
                        CharacterInputDetector.stanceFormation.Shield;
                    
                    
                    FormationBehavior   formationBe = GameObject.Find("Formation").GetComponent<FormationBehavior>();
                    
                    formationBe.InstantiateFormation(formationBe.GetComponent<FormationBehavior>().allFormations[0]);
                    player.GetComponent<CharacterInputDetector>().stance_color =  player.GetComponent<CharacterInputDetector>().defense_color;
                    
                    formationBe.UnPreviewShoot();

                    allSuiveurs = GameObject.FindGameObjectsWithTag("Recoltable");

                    foreach (GameObject go in allSuiveurs)
                    {
                       DestroyObject(go); 
                    }

                    foreach (GameObject go in allSpawns)
                    {
                        go.transform.GetChild(0).gameObject.SetActive(false);
                    }
        }
        if (Input.GetKeyDown(KeyCode.Keypad2) && gm.IsCheatActive)
        {
            GameObject tempGO = ennemies[1];
            
            Vector3 tempArenaV = new Vector3(0, 3.41f, 0) + areneCenter;
            
            GameObject spawningEnnemy =   Instantiate(tempGO, tempArenaV, Quaternion.identity);
      
            spawningEnnemy.GetComponent<Blackboard>().SetValue("Target", player);
       
            ennemyCount++;
        }
        if (Input.GetKeyDown(KeyCode.Keypad3) && gm.IsCheatActive)
        {
            GameObject tempGO = ennemies[2];
            
            Vector3 tempArenaV = new Vector3(0, 3.41f, 0) + areneCenter;
            
            GameObject spawningEnnemy =   Instantiate(tempGO, tempArenaV, Quaternion.identity);
      
            spawningEnnemy.GetComponent<Blackboard>().SetValue("Target", player);
       
            ennemyCount++;
        }
        if (Input.GetKeyDown(KeyCode.Keypad4) && gm.IsCheatActive)
        {
            GameObject tempGO = ennemies[3];
            
            Vector3 tempArenaV = new Vector3(0, 3.41f, 0) + areneCenter;
            
            GameObject spawningEnnemy =   Instantiate(tempGO, tempArenaV, Quaternion.identity);
      
            spawningEnnemy.GetComponent<Blackboard>().SetValue("Target", player);
       
            ennemyCount++;
        } 
        if (Input.GetKeyDown(KeyCode.Keypad5) && gm.IsCheatActive)
        {
            GameObject tempGO = ennemies[4];
            
            Vector3 tempArenaV = new Vector3(0, 3.41f, 0) + areneCenter;
            
            GameObject spawningEnnemy =   Instantiate(tempGO, tempArenaV, Quaternion.identity);
      
            spawningEnnemy.GetComponent<Blackboard>().SetValue("Target", player);
       
            ennemyCount++;
        }

        if (Input.GetKeyDown(KeyCode.Keypad6) && gm.IsCheatActive)
        {
            EndGame();
        }



        if(spawn == SpawnMode.CustomWaves)
        {
            if (ennemyCount <= ennemyMin && canSpawnWave)
            {
                waveCount++;
                canSpawnWave = false;
                
                if (!deathAloneIsDelayed)
                {
                    var allsuiveurs = GameObject.FindGameObjectsWithTag("Recoltable");
                
                    for (int i = 0; i < allsuiveurs.Length; i++)
                    {
                  
                        SuiveursManager tempSM = allsuiveurs[i].GetComponent<SuiveursManager>();
                   

                        if (!tempSM.isPicked && !tempSM.isShooted && !tempSM.isFromSpawn)
                        {
                            allsuiveurs[i].GetComponent<SuiveursManager>().DieButNow();
                        }
                    }
                }
                else
                {
                    var allsuiveurs = GameObject.FindGameObjectsWithTag("Recoltable");
                
                    for (int i = 0; i < allsuiveurs.Length; i++)
                    {
                  
                        SuiveursManager tempSM = allsuiveurs[i].GetComponent<SuiveursManager>();
                   

                        if (!tempSM.isPicked && !tempSM.isShooted && !tempSM.isFromSpawn && !tempSM.isFromEnnemy)
                        {
                            StartCoroutine(allsuiveurs[i].GetComponent<SuiveursManager>().DieButWithADelay(deathDelay));

                        }
                    }
                }

                if (waveCount < customWaves.Count)
                {
                    poolWaves(); 
                }
                else if( waveCount >= customWaves.Count && ennemyCount == 0 &&  enterDoor.activeSelf  ) ///// endgame
                {
                    if (gm.isBossActive)
                    {
                        GameObject.Find("Boss").GetComponent<BossManager>().CustomStart();
                    }
                    else
                    {
                        EndGame();
                    }
                }

                sd.PlayEndWaveSound(partiesMonolith[waveCount - 1]);
                Instantiate(ps_UnitWave, partiesMonolith[waveCount - 1].transform.position, ps_UnitWave.transform.rotation);
                partiesMonolith[waveCount - 1].SetActive(false);
                Instantiate(ps_EndWave, new Vector3(areneCenter.x, 1.2f, areneCenter.z), ps_EndWave.transform.rotation);
                
            }  
          
        }

      
        
    }


    public void EndGame()
    {
        //Instantiate(ps_UnitWave, partiesMonolith[waveCount - 1].transform.position, ps_UnitWave.transform.rotation);
        // partiesMonolith[2].SetActive(false);
        //Instantiate(ps_EndWave, new Vector3(areneCenter.x, 1.2f, areneCenter.z), ps_EndWave.transform.rotation);

        /* var levitatingParts = GameObject.Find("ObeliskShards");
         foreach (Transform child in levitatingParts.transform)
         {
             child.gameObject.GetComponent<Floater>().enabled = false;
             child.gameObject.AddComponent<Rigidbody>();
         }*/

                    
                    
                    PlayerPrefs.SetInt("arenaOne", 1);
                    finishDoor.GetComponent<DoorBehavior>().DesactivateDoor();

                    GameObject manager = GameObject.Find("Manager");
                 
                    manager.GetComponent<ScoreManager>().txt_timer.gameObject.SetActive(true);
                    manager.GetComponent<ScoreManager>().txt_timer.gameObject.transform.parent.gameObject.SetActive(false);
                    manager.GetComponent<ScoreManager>().txt_score.gameObject.transform.parent.gameObject.SetActive(false);
                    manager.GetComponent<ScoreManager>().txt_chainMultiplier.gameObject.transform.parent.gameObject.SetActive(false);
                    player.GetComponent<CharacterInputDetector>().circleGo.SetActive(false);
                    SpawnSuiveurBehavior.canSpawn = false;

                   // GameObject.Find("StartTrigger").SetActive(false);

                    allSpawns = GameObject.FindGameObjectsWithTag("SpawnSuiveur");


                    player.GetComponent<CharacterInputDetector>().stance =
                        CharacterInputDetector.stanceFormation.Shield;
                    
                    
                    FormationBehavior   formationBe = GameObject.Find("Formation").GetComponent<FormationBehavior>();
                    
                    formationBe.InstantiateFormation(formationBe.GetComponent<FormationBehavior>().allFormations[0]);
                    player.GetComponent<CharacterInputDetector>().stance_color =  player.GetComponent<CharacterInputDetector>().defense_color;
                    
                    formationBe.UnPreviewShoot();

                    allSuiveurs = GameObject.FindGameObjectsWithTag("Recoltable");

                    foreach (GameObject go in allSuiveurs)
                    {
                       DestroyObject(go); 
                    }

                    foreach (GameObject go in allSpawns)
                    {
                        go.transform.GetChild(0).gameObject.SetActive(false);
                    }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        
        Gizmos.DrawWireSphere(areneCenter, arenaLimites);

    }


    public IEnumerator spawnRandomEnnemis()
    {
        for (float f = 0f; f < 100f; f += Time.deltaTime)
        {
            if (f >= spawnTimerInterval * spawnCount && spawningPool.Count > spawnCount-1)
            {
                Vector3 spawnRandomLocation = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f,1f));

                spawnRandomLocation = spawnRandomLocation.normalized;

                spawnRandomLocation *= spawnRadius;

                var go_feedback = Instantiate(spawning_feedback, spawnRandomLocation + areneCenter,
                    Quaternion.identity);
                sd.PlaySpawnEnemySound(go_feedback);

                StartCoroutine(spawningPool[spawnCount - 1].GetComponent<DestroyBehavior>().StartUp());
                
                //spawningPool[spawnCount-1].SetActive(true);
                spawningPool[spawnCount - 1].transform.position = spawnRandomLocation + new Vector3(0, 3.41f, 0) + areneCenter;
                
                spawningPool[spawnCount - 1].GetComponent<Blackboard>().SetValue("Target", player);
                
                spawnCount++;
          
                ennemyCount++;
            }

            if (spawningPool.Count <= spawnCount - 1)
            {
                yield break;
            }
       
            yield return 0;
        }
    }
    
    
    
    

    public void poolRandomEnnemies()
    {
       
        spawningPool.Clear();
        spawnCount = 1;
        
        for (int i = 0; i < spawningPoolSize; i++)
        {
            GameObject tempGO = ennemies[Random.Range(0, ennemies.Count)];
            GameObject spawningEnnemy =   Instantiate(tempGO, areneCenter, Quaternion.identity);
            spawningEnnemy.SetActive(false);
            
            spawningPool.Add(spawningEnnemy);
            
        }
          
        StartCoroutine("spawnRandomEnnemis");
    }


    public void poolWaves()
    {
        customSpawnCount = 0;
        StartCoroutine(spawnWaves());
    }

    public IEnumerator spawnWaves()
    {
        for (float f = 0f; f < 1000f; f += Time.deltaTime)
        {
            if (customSpawnCount >= customWaves[waveCount].subWaves.Count)
            {
                isLastOfWave = true;
                 
                 yield  break;
            }
            if (f >= customWaves[waveCount].subWaves[customSpawnCount].waveTimerStart)
            {
               Debug.Log("WavePing");
                checkPvCount++;
                if (checkPvCount >= nbrWaveRegain)
                {
                    checkPvCount = nbrWaveRegain;
                    player.GetComponent<CustomCharacterController>().GainHp();
                }

               
                
                
                
                subCount = 1;
                
                StartCoroutine(spawnSubWaves(customSpawnCount));
                
               
                customSpawnCount++;
                
                f = 0;
            }
       
            yield return 0;
        }
    }


    public IEnumerator spawnSubWaves( int numSub)
    {
        for (float f = 0f; f < 100f; f += Time.deltaTime)
        {
            if (f >= spawnTimerInterval * subCount && customWaves[waveCount].subWaves[numSub].ennemis.Count > subCount-1)
            {
                Vector3 spawnRandomLocation = new Vector3(customWaves[waveCount].subWaves[numSub].spawnTransforms[subCount-1].position.x, customWaves[waveCount].subWaves[numSub].spawnTransforms[subCount-1].position.y, customWaves[waveCount].subWaves[numSub].spawnTransforms[subCount-1].position.z);

                //spawnRandomLocation = spawnRandomLocation.normalized;

                //spawnRandomLocation *= spawnRadius;
                Vector3 randomFactor = new Vector3(Random.Range(-randomRadius, randomRadius), 0 ,Random.Range(-randomRadius, randomRadius) );
                
               

                if (customWaves[waveCount].subWaves[numSub].ennemis[subCount - 1].layer == 19) // menace
                {
                    var go_feedback = Instantiate(spawning_feedback, spawnRandomLocation + randomFactor,
                        Quaternion.identity);
                    sd.PlaySpawnEnemySound(go_feedback);

                    
                
                    GameObject go = Instantiate(customWaves[waveCount].subWaves[numSub].ennemis[subCount - 1], spawnRandomLocation + new Vector3(0, 3.41f, 0) + randomFactor, Quaternion.identity);
                    go.SetActive(false);
                    StartCoroutine(go.GetComponent<DestroyBehavior>().StartUp());
                
                
               
                

                    go.GetComponent<Blackboard>().SetValue("Target", player);
                
                    subCount++;
                
                    ennemyCount++;
                }
                else if (customWaves[waveCount].subWaves[numSub].ennemis[subCount - 1].layer == 12) // suiveur
                {
                    GameObject go = Instantiate(customWaves[waveCount].subWaves[numSub].ennemis[subCount - 1], spawnRandomLocation + new Vector3(0, 2.5f, 0) + randomFactor, Quaternion.identity);
                    subCount++;
                }
                else
                {
                    Debug.Log("ennemy not spawning");
                }
                

                
            }

            if (customWaves[waveCount].subWaves[numSub].ennemis.Count <= subCount - 1)
            {
                if (isLastOfWave)
                {
                  
                    canSpawnWave = true;
                    isLastOfWave = false;
                }
                yield break;
            }
       
            yield return 0;
        }
    }



   /* public void saveActualCustomWaves(InputField field)
    {
        
        var XMLDoc = new XmlDocument();

        XmlNode root = XMLDoc.CreateElement("root");

        for (int i = 0; i < customWaves.Count; i++)
        {
            XmlNode xmlWave = XMLDoc.CreateElement("Wave");
            {
                xmlWave.InnerText = i.ToString();
            }
            
            
            XmlNode xmlTimer = XMLDoc.CreateElement("Timer");
            {
                xmlTimer.InnerText = customWaves[waveCount].subWaves[i].waveTimerStart.ToString();
            }

            xmlWave.AppendChild(xmlTimer);
            
            for (int j = 0; j < customWaves[i].ennemis.Count; j++)
            {
                XmlNode xmlEnnemi = XMLDoc.CreateElement("Ennemi");
                {
                    xmlEnnemi.InnerText = customWaves[i].ennemis[j].name;
                }

                xmlWave.AppendChild(xmlEnnemi);
            }

            root.AppendChild(xmlWave);
        }

        XMLDoc.AppendChild(root);
        
        XMLDoc.Save(Application.dataPath + "/Project/14_XML/"+field.text +".xml");
        Debug.Log("savedXML");

       


    }



    public void loadXMLFile()
    {       
        var xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + "/Project/14_XML/"+PlayerPrefs.GetString(GetComponent<PlayerSave>().xmlKey) +".xml");

        var xmlNodes = xmlDoc.SelectSingleNode("root").ChildNodes;

        List<SubWave> tempWaves = new List<SubWave>();
       

        foreach (XmlNode waveNode in xmlNodes)
        { 
            
            float tempTimer = new float();
            //var subWaveNode = node.SelectSingleNode("Wave");
            List<GameObject> tempEnnemis = new List<GameObject>();

            for (int i = 0; i < waveNode.ChildNodes.Count; i++)
            {
                if (i == 0)
                {
                    tempTimer = float.Parse(waveNode.SelectSingleNode("Timer").InnerText);
                }

                string ennemiName = waveNode.ChildNodes[i].InnerText;
                
                foreach (GameObject go in ennemies)
                {
                    if (go.name == ennemiName)
                    {
                        tempEnnemis.Add(go);
                    }
                }
            }
            
            Wave tempWave = new Wave(tempTimer, tempEnnemis);
            
            tempWaves.Add(tempWave);
            
        }


        customWaves = tempWaves;
        Debug.Log("done");
    }*/


   /* public void SetXMLName(InputField field)
    {
        PlayerPrefs.SetString(GetComponent<PlayerSave>().xmlKey, field.text);
        
        loadXMLFile();
    }*/



}
