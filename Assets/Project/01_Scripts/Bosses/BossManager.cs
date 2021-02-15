using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public List<GameObject> InsideWalls;

    public GameObject prefabBossTir, prefabBossLaser;


    public List<GameObject> PartiesHitted;


    public static  int nbHeartsAlive;

    private int previousAttack;

    private int currentAttack;

    public float attackDuration, attackRecovery, randomAddition;

    public float attackStandardInterval, attackTargetInterval;

    public float attackSpreadInterval, attackCircleInterval, attackLaserInterval;

    private int count = 1;

    private int majorCount = 0;

    private bool isOver;
    
    public int numberOfBullet;
    public float angleOfSpread;
    
    public GameObject modulesNormal, modulesBoss;

    private GameObject player;

    public GameObject PrefabBullet;

    public GameObject coreBoss;


    private bool canAttack;

    private SoundManager sd;

    public GameObject fxBoss;


    private void Awake()
    {
        sd = GameObject.Find("Manager").GetComponent<SoundManager>();
    }

    public void CustomStart()
    {
        previousAttack = 15;
        nbHeartsAlive = 3;
        BossSetup();
        canAttack = true;
        
        modulesNormal.SetActive(false);
        modulesBoss.SetActive(true);
        
        player = GameObject.Find("Mesh");
       
    }

    public void SingleHeartDestroy()
    {
        
        nbHeartsAlive--;

        for (int i = 0; i < PartiesHitted.Count; i++)
        {
            PartiesHitted[i].SetActive(true);
            
         //   if (PartiesHitted[i].tag == "BossHeart") // coeur remplacement
           // {
               // PartiesHitted[i].tag = "BossStandard";
//                PartiesHitted[i].GetComponent<Renderer>().material.color = Color.green;
          //  }
        }
        
        PartiesHitted.Clear();

        if (nbHeartsAlive == 0 )
        {
            GameObject.Find("Manager").GetComponent<spawnManager>().EndGame();
            gameObject.SetActive(false);
        }
        
        Debug.Log(nbHeartsAlive);
    }


    public void BossSetup()
    {
        GameObject[] allWalls = GameObject.FindGameObjectsWithTag("Obstacle");

        for (int i = 0; i < allWalls.Length; i++)
        {
            if (allWalls[i].layer == 17)
            {
                
                InsideWalls.Add(allWalls[i]);
                
            }
        }
        StartCoroutine(WallDescending());

        Instantiate(fxBoss, transform.position, Quaternion.identity);
    }


    public IEnumerator WallDescending()
    {
        for (float f = 0f; f < 3f; f += Time.deltaTime)
        {
            foreach (GameObject go in InsideWalls)
            {
                 go.transform.position = Vector3.MoveTowards(go.transform.position, go.transform.position + (Vector3.down * 25f), Time.deltaTime*5);
            }
            yield return 0;
        }

        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            if (i != 3)
            {
                transform.GetChild(i).gameObject.GetComponent<CoucheSetup>().customStart();
            }
            
        }
        coreBoss.SetActive(false);
        StartCoroutine(StartAttacks());
        
        SpawnSuiveurBehavior.canSpawn = true;
    }



    public IEnumerator StartAttacks()
    {
        for (float f = 0f; f < 500f; f += Time.deltaTime)
        {
            if (majorCount * 12 <= f && canAttack)
            {
               ChooseAttack();
                majorCount++;
            }
            
            yield return 0;
        }
    }


    public void ChooseAttack()
    {

        if (nbHeartsAlive == 3)
        {
            
            sd.SetEnemyKickParameter(3);
            do
            {
                currentAttack = Random.Range(1, 4);
            } while (currentAttack == previousAttack);
        }
        else if (nbHeartsAlive == 2)
        {
            sd.SetEnemyKickParameter(4);
            do
            {
                currentAttack = Random.Range(1, 5);
            } while (currentAttack == previousAttack);
        }
        else if (nbHeartsAlive == 1)
        {
            sd.SetEnemyKickParameter(5);
            do
            {
                currentAttack = Random.Range(1, 6);
            } while (currentAttack == previousAttack);
        }
        
        else if (nbHeartsAlive == 0)
        {
         GameObject.Find("Manager").GetComponent<spawnManager>().EndGame();
        }

        previousAttack = currentAttack;
        
        launchAttack(currentAttack);

    }

    public void launchAttack(int numberAttack)
    {
        StopCoroutine(AttackCircle());
        StopCoroutine(AttackStandard());
        StopCoroutine(AttackSpread());
        StopCoroutine(AttackTarget());
        StopCoroutine(AttackLaser());

        canAttack = false;
        
        count = 1;
        
        if (numberAttack == 1)
        {
            
            StartCoroutine(AttackCircle());
        }
        else  if (numberAttack == 2)
        {
            
            StartCoroutine(AttackStandard());
        }
        else  if (numberAttack == 3)
        {
            
            StartCoroutine(AttackSpread());
        }
        else  if (numberAttack == 4)
        {
            
            StartCoroutine(AttackTarget());
        }
        else  if (numberAttack == 5)
        {
           
            StartCoroutine(AttackLaser());
        }
    }


    public IEnumerator WaitAfterAttack()
    {
        float rd = Random.Range(0, randomAddition);
        
        for (float f = 0f; f < attackRecovery + rd; f += Time.deltaTime)
        {
            yield return 0;
        }

        canAttack = true;
    }


    public IEnumerator AttackCircle()
    {
        for (float f = 0f;  f < attackDuration; f += Time.deltaTime)
        {

            if (count * attackCircleInterval <= f)
            {
                count++;
                float angleRef = angleOfSpread / numberOfBullet;

                for (int i = 0; i < numberOfBullet; i++)
                {
                    // float rdAngle = Random.Range(-angleOfSpread.value / 2, angleOfSpread.value / 2);
                    float rdAngle = -angleOfSpread / 2 + angleRef * i;
                    Vector3 eulerRotation = transform.rotation.eulerAngles + new Vector3(0, rdAngle, 0);
                    GameObject bullet = (GameObject) Object.Instantiate(PrefabBullet, transform.position,
                        Quaternion.Euler(eulerRotation));
                    // clone.transform.position += new Vector3(0, 0, 0.5f);
                }

            }

            yield return 0;
        }

        StartCoroutine(WaitAfterAttack());
    }
    
    
    
    public IEnumerator AttackStandard()
    {
        for (float f = 0f;  f < attackDuration; f += Time.deltaTime)
        {
            if (count * attackStandardInterval <= f)
            {
                count++;
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).gameObject.activeSelf)
                    {
                        if (i != 3)
                        {
                            transform.GetChild(i).GetComponent<CoucheSetup>().OrderAttackStandard();
                        }
                    }
                  
                }
            }
            
            yield return 0;
        }
        StartCoroutine(WaitAfterAttack());
    }
    
    public IEnumerator AttackSpread()
    {
        for (float f = 0f;  f < attackDuration; f += Time.deltaTime)
        {
            
            if (count * attackSpreadInterval <= f)
            {
                count++;
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).gameObject.activeSelf)
                    {
                        if (i != 3)
                        {
                            transform.GetChild(i).GetComponent<CoucheSetup>().OrderAttackSpread();
                        }
                    }
                }
            }
            yield return 0;
        }
        StartCoroutine(WaitAfterAttack());
    }
    
    public IEnumerator AttackTarget()
    {
        for (float f = 0f;  f < attackDuration; f += Time.deltaTime)
        {
            if (count * attackTargetInterval <= f)
            {
                count++;
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).gameObject.activeSelf)
                    {
                        if (i != 3)
                        {
                            transform.GetChild(i).GetComponent<CoucheSetup>().OrderAttackTarget();
                        }
                    }
                }
            }
            
            yield return 0;
        }
        StartCoroutine(WaitAfterAttack());
    }
    
    public IEnumerator AttackLaser()
    {
        for (float f = 0f;  f < attackDuration; f += Time.deltaTime)
        {
            if (count * attackLaserInterval <= f)
            {
                count++;
                Vector3  tempEuler = new Vector3(Random.Range(-90, 90), 0, Random.Range(-90, 90));

                GameObject go =  Instantiate(prefabBossLaser, transform.position, Quaternion.LookRotation((player.transform.position + tempEuler) - transform.position));  
            }

            yield return 0;
        }
        StartCoroutine(WaitAfterAttack());
    }
    
}
