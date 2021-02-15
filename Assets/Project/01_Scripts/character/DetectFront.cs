using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectFront : MonoBehaviour
{
    public static bool isInFront;

    private void Awake()
    {
        isInFront = false;
    }


    private void FixedUpdate()
    {
        
        isInFront = false;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            isInFront = true;
        }
    }
    
    
    
}
