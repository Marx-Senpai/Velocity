using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.AI;

public class DestroyBehavior : MonoBehaviour
{

    public float destroyRadius;

    private LayerMask playerAndRecolt;

    public string[] layers;

    private Collider[] colliders;

    public GameObject destroyFeedback;

    private GameObject player;

    public GameObject prefabRecoltable;

    public bool spawnSuiveurOnDeath;

    public GameObject devant, tir, gauche, droite;

    private bool hasallSuiveur;

    public bool isDying;

    public float timeStartup;

    public int numberSuiveursSpawned;
    

    [SerializeField] private float exploForce, exploRadius;
    [SerializeField] private Material mat_dead;
    [SerializeField] private GameObject ps_destroy_enemy;

    private GameObject manager;
    private SoundManager sd;
    private ScoreManager sc;
    
    
    private void Awake()
    {
        manager = GameObject.Find("Manager");
        sd = manager.GetComponent<SoundManager>();
        sc = manager.GetComponent<ScoreManager>();
        
        playerAndRecolt = LayerMask.GetMask(layers);
        player = GameObject.Find("Player");

        if (gameObject.tag == "Infanterie" && transform.parent == null)
        {
            devant = transform.GetChild(0).gameObject;
            tir = transform.GetChild(1).gameObject;
            gauche = transform.GetChild(2).gameObject;
            droite = transform.GetChild(3).gameObject;
        }
        else if (gameObject.tag == "WidowMaker" && transform.parent == null)
        {
            devant = transform.GetChild(0).transform.GetChild(0).gameObject;
            tir = transform.GetChild(0).transform.GetChild(1).gameObject;     
        }
        
            spawnSuiveurOnDeath = GameObject.Find("Manager").GetComponent<GameManager>().spawnSuiveurOnDeath;
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "sol")
        {
            sd.PlayCollisionGroundSound(this.gameObject);
        }
        
        if (other.gameObject.tag == "Water")
        {
            sd.PlayCollisionWaterSound(this.gameObject);
        }
    }

    public void SelfDestruct(Vector3 hitPoint)
    {
        gameObject.layer = 26;
        sd.PlayLosePartSound(this.gameObject);
        
        if (gameObject.tag == "Fonceur" || gameObject.tag == "Tracer")
        {
            sc.AddScore(gameObject, true);
            Instantiate(ps_destroy_enemy, gameObject.transform.position, Quaternion.identity);
            
            gameObject.GetComponent<BehaviourTreeOwner>().StopBehaviour();
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            gameObject.GetComponent<Blackboard>().enabled = false;
            
            var rigid = gameObject.GetComponent<Rigidbody>();
            rigid.useGravity = true;
            rigid.constraints = RigidbodyConstraints.None;
            rigid.AddExplosionForce(exploForce, hitPoint, exploRadius, 1, ForceMode.Impulse);
        }

        if (gameObject.tag == "Fonceur")
        {
            gameObject.GetComponent<StudioEventEmitter>().Stop();
            gameObject.transform.GetChild(0).GetComponent<AnimRotation>().enabled = false;

            var children = transform.GetChild(0).transform.GetChild(0).transform.GetComponentsInChildren<ParticleSystem>();
            foreach (var child in children)
            {
                child.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            
            transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Renderer>().material = mat_dead;
            manager.GetComponent<spawnManager>().ennemyCount--;
            SpawnSuiveur();
            StartCoroutine(CountDown());
        }
        
        if (gameObject.tag == "Tracer")
        {
            gameObject.transform.GetChild(0).GetComponent<LookAtPlayer>().enabled = false;
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>().enabled = false;
            transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Renderer>().material = mat_dead;
            manager.GetComponent<spawnManager>().ennemyCount--;
            SpawnSuiveur();
            StartCoroutine(CountDown());
        }

        if (gameObject.tag == "Infanterie")
        {
            if (gameObject.name == "PartieTir")
            {
                
                Instantiate(ps_destroy_enemy, gameObject.transform.position, Quaternion.identity);
                sc.AddScore(gameObject, true);
                DestroyBehavior scriptParent = gameObject.transform.parent.gameObject.GetComponent<DestroyBehavior>();

                if (scriptParent.devant != null)
                {
                    
                    scriptParent.devant.GetComponent<DestroyBehavior>().SelfDestruct(hitPoint);
                }
                
                if (scriptParent.droite != null)
                {
                   
                    scriptParent.droite.GetComponent<DestroyBehavior>().SelfDestruct(hitPoint);
                }
                
                if (scriptParent.gauche != null)
                {
                    
                    scriptParent.gauche.GetComponent<DestroyBehavior>().SelfDestruct(hitPoint);
                }
                
                gameObject.GetComponent<Animator>().enabled = false;
                var rigid = gameObject.GetComponent<Rigidbody>();
                rigid.useGravity = true;
                rigid.constraints = RigidbodyConstraints.None;
                rigid.AddExplosionForce(exploForce, hitPoint, exploRadius, 1, ForceMode.Impulse);
                isDying = true;
                transform.parent = null;
            
                transform.GetChild(0).gameObject.GetComponent<Renderer>().material = mat_dead;
                manager.GetComponent<spawnManager>().ennemyCount--;
                SpawnSuiveur();
                StartCoroutine(CountDown());
            }
            else if (gameObject.name == "Devant")
            {
                gameObject.GetComponent<Animator>().enabled = false;
                var rigid = gameObject.GetComponent<Rigidbody>();
                rigid.useGravity = true;
                rigid.constraints = RigidbodyConstraints.None;
                rigid.AddExplosionForce(exploForce, hitPoint, exploRadius, 1, ForceMode.Impulse);
                isDying = true;
                transform.parent = null;
            
                transform.GetChild(0).gameObject.GetComponent<Renderer>().material = mat_dead;
                StartCoroutine(CountDown());
                sc.AddScore(gameObject, false);
            }
            else if (gameObject.name == "PartieGauche")
            {
                gameObject.GetComponent<Animator>().enabled = false;
                var rigid = gameObject.GetComponent<Rigidbody>();
                rigid.useGravity = true;
                rigid.constraints = RigidbodyConstraints.None;
                rigid.AddExplosionForce(exploForce, hitPoint, exploRadius, 1, ForceMode.Impulse);
                isDying = true;
                transform.parent = null;
            
                transform.GetChild(0).gameObject.GetComponent<Renderer>().material = mat_dead;
                StartCoroutine(CountDown());
                sc.AddScore(gameObject, false);
            }
            else if (gameObject.name == "PartieDroite")
            {
                gameObject.GetComponent<Animator>().enabled = false;
                var rigid = gameObject.GetComponent<Rigidbody>();
                rigid.useGravity = true;
                rigid.constraints = RigidbodyConstraints.None;
                rigid.AddExplosionForce(exploForce, hitPoint, exploRadius, 1, ForceMode.Impulse);
                isDying = true;
                transform.parent = null;
            
                transform.GetChild(0).gameObject.GetComponent<Renderer>().material = mat_dead;
                StartCoroutine(CountDown());
                sc.AddScore(gameObject, false);
            }
            
            
        }

        if (gameObject.tag == "WidowMaker")
        {
            if (gameObject.name == "Rayon")
            {             
                sd.StopSound(gameObject.transform.parent.parent.GetComponent<LaserSniperHandler>().chargingSound_event);
                DestroyBehavior scriptParent = gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<DestroyBehavior>();
                Instantiate(ps_destroy_enemy, gameObject.transform.position, Quaternion.identity);

                if (scriptParent.devant != null)
                {
                    scriptParent.devant.GetComponent<DestroyBehavior>().SelfDestruct(hitPoint);
                }
                
                var rigid = gameObject.GetComponent<Rigidbody>();
                rigid.useGravity = true;
                rigid.constraints = RigidbodyConstraints.None;
                rigid.AddExplosionForce(exploForce, hitPoint, exploRadius, 1, ForceMode.Impulse);
                isDying = true;
                transform.parent = null;
                
                transform.GetChild(0).gameObject.GetComponent<Renderer>().material = mat_dead;
       
                transform.GetChild(1).gameObject.GetComponent<Renderer>().material = mat_dead;
             
                SpawnSuiveur();
                StartCoroutine(CountDown());
                manager.GetComponent<spawnManager>().ennemyCount--;
                sc.AddScore(gameObject, true);
            }
            else
            {
                sc.AddScore(gameObject, false);
                var rigid = gameObject.GetComponent<Rigidbody>();
                rigid.useGravity = true;
                rigid.constraints = RigidbodyConstraints.None;
                rigid.AddExplosionForce(exploForce, hitPoint, exploRadius,1, ForceMode.Impulse);
                isDying = true;
                transform.parent = null;
                
                transform.GetChild(0).gameObject.GetComponent<Renderer>().material = mat_dead;
                StartCoroutine(CountDown());
            }
        }

        
       
    }


    public void SpawnSuiveur()
    {
        if (spawnSuiveurOnDeath)
        {
            for (int i = 0; i < numberSuiveursSpawned; i++)
            {
                Vector3 tempV = transform.position ;
                tempV.y = 3.5f;
            
                GameObject gom = Instantiate(prefabRecoltable, tempV + new Vector3(i,0,0), Quaternion.identity);
         		
                gom.GetComponent<SuiveursManager>().player = player;
                gom.GetComponent<ParticleSystem>().Stop();

                gom.GetComponent<SuiveursManager>().isFromEnnemy = true;
            } 
        }   
    }

    
    

    
    IEnumerator CountDown()
    {
        for (float f = 0;  f < 2; f+= Time.deltaTime)
        {
            yield return 0;
        }
       
        gameObject.GetComponent<DissolveControl>().enabled = true;
       
        
        for (float f = 0;  f < 3; f+= Time.deltaTime)
        {
            yield return 0;
        }
        //GameObject gogo = Instantiate(destroyFeedback, transform.position, Quaternion.identity);
 
        Destroy(gameObject);
        
    }


    public IEnumerator StartUp()
    {
        for (float f = 0; f < timeStartup; f += Time.deltaTime)
        {
            yield return null;
        }
        
        this.gameObject.SetActive(true);
        if (this.gameObject.tag == "Fonceur")
        {
            this.gameObject.GetComponent<StudioEventEmitter>().enabled = true;        
        }
    }


    private void Update()
    {
        
        if (gameObject.tag == "Infanterie" && transform.parent == null && isDying == false) 
        {
            if ( tir == null)
            {
                Destroy(gameObject);
            }
            else
            {
                if (tir.GetComponent<DestroyBehavior>().isDying)
                {
                    GetComponent<BehaviourTreeOwner>().StopBehaviour();
                    gameObject.GetComponent<NavMeshAgent>().enabled = false;
                }
            }
        
        }
        else if(gameObject.tag == "WidowMaker" && transform.parent == null  && isDying == false)
        {
            if ( tir == null)
            {
                Destroy(gameObject);
            }
            else
            {
                if (tir.GetComponent<DestroyBehavior>().isDying)
                {
                    transform.GetChild(0).gameObject.GetComponent<LookAtPlayer>().enabled = false;
                    GetComponent<LineRenderer>().enabled = false;
                    GetComponent<BehaviourTreeOwner>().StopBehaviour();
                    gameObject.GetComponent<NavMeshAgent>().enabled = false;
                }
            }
        }

        
    }
}
