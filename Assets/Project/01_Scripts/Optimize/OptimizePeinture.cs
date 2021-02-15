using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptimizePeinture : MonoBehaviour
{
    [SerializeField] private float range;
    
    
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<SphereCollider>().radius = range;
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.tag.Contains("Peinture"))
        {
//            other.gameObject.GetComponent<ParticleSystemRenderer>().enabled = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Contains("Peinture"))
        {
   //         other.gameObject.GetComponent<ParticleSystemRenderer>().enabled = false;
        }
    }
}
