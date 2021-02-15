using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShockWave : MonoBehaviour
{
    private SphereCollider sc;
    private Transform player;
    [SerializeField] private float range = 20f;
    [SerializeField] private GameObject ps_destroy_enemy_bullet;
    
    void Start()
    {
        player = GameObject.Find("Player").transform;
        sc = GetComponent<SphereCollider>();
    }

    
    void Update()
    {
       // transform.parent.position = player.position;
        sc.radius = Mathf.Lerp(sc.radius, range, Time.deltaTime * 1.2f);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet" && other.gameObject.layer == 18)
        {
            Instantiate(ps_destroy_enemy_bullet, other.transform.position, Quaternion.identity);
			Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Tracer")
        {
            Vector3 vectorForce = (other.gameObject.transform.position - transform.position).normalized * 10f;
            //other.gameObject.GetComponent<Rigidbody>().AddForce(-vectorForce, ForceMode.VelocityChange);
            other.gameObject.GetComponent<NavMeshAgent>().velocity = vectorForce;
           
        }

        if (other.gameObject.tag == "Fonceur")
        {
            Debug.Log("wala");
            Vector3 vectorForce = (other.gameObject.transform.position - transform.position).normalized * 10f;
            //other.gameObject.GetComponent<Rigidbody>().AddForce(-vectorForce, ForceMode.VelocityChange);
            other.gameObject.GetComponent<NavMeshAgent>().velocity = vectorForce;
          
        }

        if (other.gameObject.tag == "Infanterie" && other.gameObject.transform.parent == null)
        {
            Vector3 vectorForce = (other.gameObject.transform.position - transform.position).normalized * 10f;
            //other.gameObject.GetComponent<Rigidbody>().AddForce(-vectorForce, ForceMode.VelocityChange);
            other.gameObject.transform.parent.gameObject.GetComponent<NavMeshAgent>().velocity = vectorForce;
       
        }
        
        
    }
    
}
