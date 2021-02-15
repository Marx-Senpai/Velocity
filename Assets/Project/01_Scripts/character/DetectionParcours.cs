using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectionParcours : MonoBehaviour
{
   private RaycastHit hit;
   public GameObject obstacleHitted;
   public int suiveursLimit;
   public List<GameObject> suiveurs;
   public GameObject formation;

   private GameObject manager;
   private ScoreManager sc;
   private float timer, cd = 2f;
   
   [SerializeField] private GameObject ps_maxModules;

   private GameObject[] allSuiveurs;
   
  private void Awake()
  {
     manager = GameObject.Find("Manager");
     sc = manager.GetComponent<ScoreManager>();
     
     suiveurs = new List<GameObject>();
     formation = GameObject.Find("formation");
  }

  private void OnDrawGizmos()
  {
     Gizmos.color = Color.red;
     Gizmos.DrawWireSphere(transform.position,  GetComponent<CapsuleCollider>().radius);
  }

  public void testCollision(Collider c)
  {
        if (c.gameObject.CompareTag("Recoltable")  &&  c.gameObject.GetComponent<SuiveursManager>().isPicked == false && suiveurs.Count < suiveursLimit)
        {
           sc.AddScore(c.gameObject, false);
           
           if (c.transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
           {
              c.transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystem>().Play();
           }
           
           suiveurs.Add(c.gameObject);
          
           GameObject.Find("Manager").GetComponent<SoundManager>().PlayPickModuleSound();
          
           if (c.gameObject.GetComponent<SuiveursManager>().spawnAttached != null)
           {
              c.gameObject.GetComponent<SuiveursManager>().spawnAttached.GetComponent<SpawnSuiveurBehavior>().isFull = false;
              c.gameObject.GetComponent<SuiveursManager>().spawnAttached = null;
           }

           c.gameObject.GetComponent<SuiveursManager>().isFromEnnemy = false;
           c.gameObject.GetComponent<SuiveursManager>().isPicked = true;
        
         //  c.gameObject.layer = 13;
         //  c.gameObject.layer = 13;
        }
        else if (c.gameObject.CompareTag("Obstacle") || c.gameObject.CompareTag("MovingObstacle"))
        {
           obstacleHitted = hit.collider.gameObject;
        }
  }

   public void CheckNumberFollowers()
   {
      if (suiveurs.Count >= suiveursLimit)
      {
         ps_maxModules.GetComponent<ParticleSystem>().Play();
         ps_maxModules.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
      }
      else
      {
         ps_maxModules.GetComponent<ParticleSystem>().Stop();
         ps_maxModules.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
      }
   }


   private void Update()
   {
     /* timer += Time.deltaTime;

      if (timer >= cd)
      {
         for (int i = 0; i < suiveurs.Count; i++)
         {
            if (suiveurs[0] == null)
            {
               suiveurs.RemoveAt(i);
               i--;
            }
         }

         timer = 0f;
      }*/

      if (suiveurs.Count == suiveursLimit )
      {
        // gameObject.layer = 27;
          allSuiveurs = GameObject.FindGameObjectsWithTag("Recoltable");

         foreach (GameObject go in allSuiveurs)
         {
            if (!go.GetComponent<SuiveursManager>().isPicked && !go.GetComponent<SuiveursManager>().isShooted)
            {
               go.layer = 24;
            }
         }

      }
      else if(suiveurs.Count != suiveursLimit )
      {
         //gameObject.layer = 9;
         
         allSuiveurs = GameObject.FindGameObjectsWithTag("Recoltable");

         foreach (GameObject go in allSuiveurs)
         {
            if (!go.GetComponent<SuiveursManager>().isPicked && !go.GetComponent<SuiveursManager>().isShooted)
            {
               go.layer = 12;
            }
         }
      }
   }
   
   
}