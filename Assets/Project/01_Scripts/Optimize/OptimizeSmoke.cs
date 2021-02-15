using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptimizeSmoke : MonoBehaviour
{
    [SerializeField] private float range;
    
    
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<SphereCollider>().radius = range;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 12 && !other.gameObject.tag.Contains("Peinture"))
        {
            var suiveur = other.gameObject.transform.parent;
            var suiveur_sign = suiveur.transform.GetChild(2);
            suiveur_sign.gameObject.SetActive(true);
        }
        
        if (other.gameObject.tag.Contains("Peinture"))
        {
//            other.gameObject.GetComponent<ParticleSystemRenderer>().enabled = true;
        }
    }
    
    /*private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 12 && !other.gameObject.tag.Contains("Peinture"))
        {
            var suiveur = other.gameObject.transform.parent;
            var suiveur_sign = suiveur.transform.GetChild(2);
            suiveur_sign.gameObject.SetActive(false);
        }
        
        if (other.gameObject.tag.Contains("Peinture"))
        {
      //      other.gameObject.GetComponent<ParticleSystemRenderer>().enabled = false;
        }
    }*/
}
