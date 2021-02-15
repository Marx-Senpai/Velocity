using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTirStandardBehavior : MonoBehaviour
{

    public GameObject prefabBullet, prefabBulletTarget;

    public GameObject player;

    public float randomTargetRadius;

   

    private void Start()
    {
        player = GameObject.Find("Mesh");
    }


    public void shootInFront()
    {
        if (gameObject.activeSelf)
        {
            Instantiate(prefabBullet, transform.position, transform.rotation); 
        }
        
    }


    public void shootTarget()
    {
        if (gameObject.activeSelf)
        {
            Vector3  tempEuler = new Vector3(Random.Range(-randomTargetRadius, randomTargetRadius), 0, Random.Range(-randomTargetRadius, randomTargetRadius));

            GameObject go =  Instantiate(prefabBulletTarget, transform.position, Quaternion.LookRotation((player.transform.position + tempEuler) - transform.position));  
        }

       
        
    }

   
}
