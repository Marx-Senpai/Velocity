using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ShieldBossBehavior : MonoBehaviour

{
    
    private GameObject manager;
    private SoundManager sd;
    private ScoreManager sc;

    private Vector3 normalPosition, normalRotationEuler;
    
    [SerializeField] private float exploForce, exploRadius;
    [SerializeField] private Material mat_dead;
    [SerializeField] private Material mat_alive;
    [SerializeField] private GameObject ps_destroy_enemy;
    

    public GameObject tir;
    
    
    private void Awake()
    {
        manager = GameObject.Find("Manager");
        sd = manager.GetComponent<SoundManager>();
        sc = manager.GetComponent<ScoreManager>();
        normalPosition = transform.localPosition;
        normalRotationEuler = transform.localRotation.eulerAngles;
    }

    private void OnEnable()
    {
  
        var rigid = gameObject.GetComponent<Rigidbody>();
        rigid.useGravity = false;
        rigid.constraints = RigidbodyConstraints.FreezeAll;
        
        transform.GetChild(0).gameObject.GetComponent<Renderer>().material = mat_alive;
        
        transform.localPosition = normalPosition;
        transform.localRotation = Quaternion.Euler(normalRotationEuler);
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
      
            
               
                DestroyBehavior scriptParent = gameObject.transform.parent.gameObject.transform.parent.gameObject
                    .GetComponent<DestroyBehavior>();
                Instantiate(ps_destroy_enemy, gameObject.transform.position, Quaternion.identity);

               /* if (scriptParent.devant != null)
                {
                    scriptParent.devant.GetComponent<DestroyBehavior>().SelfDestruct(hitPoint);
                }*/

                var rigid = gameObject.GetComponent<Rigidbody>();
                rigid.useGravity = true;
                rigid.constraints = RigidbodyConstraints.None;
                rigid.AddExplosionForce(exploForce, hitPoint, exploRadius, 1, ForceMode.Impulse);
               // isDying = true;
               // transform.parent = null;

                transform.GetChild(0).gameObject.GetComponent<Renderer>().material = mat_dead;

             //   transform.GetChild(1).gameObject.GetComponent<Renderer>().material = mat_dead;

                
                StartCoroutine(CountDown());
              //  manager.GetComponent<spawnManager>().ennemyCount--;
                sc.AddScore(gameObject, true);
          
             

                transform.GetChild(0).gameObject.GetComponent<Renderer>().material = mat_dead;
                StartCoroutine(CountDown());
            
        

    }
    
    
    IEnumerator CountDown()
    {
        for (float f = 0;  f < 2; f+= Time.deltaTime)
        {
            yield return 0;
        }
       
       // gameObject.GetComponent<DissolveControl>().enabled = true;
       
        
        for (float f = 0;  f < 3; f+= Time.deltaTime)
        {
            yield return 0;
        }
        //GameObject gogo = Instantiate(destroyFeedback, transform.position, Quaternion.identity);
 
        gameObject.SetActive(false);
        
    }

    private void Update()
    {
        if (tir.activeSelf == false)
        {
            GameObject.Find("Boss").GetComponent<BossManager>().PartiesHitted.Add(gameObject);
            GameObject.Find("Boss").GetComponent<BossManager>().PartiesHitted.Add(gameObject.transform.parent.gameObject);
            gameObject.SetActive(false);
        }
    }
}
