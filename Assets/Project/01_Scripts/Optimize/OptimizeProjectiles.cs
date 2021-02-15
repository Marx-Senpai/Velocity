using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptimizeProjectiles : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet" || other.gameObject.tag == "Recoltable")
        {
            Destroy(other.gameObject);
        }
       
    }
}
