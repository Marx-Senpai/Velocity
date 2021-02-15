using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoGirl : MonoBehaviour
{
    private float radius = 2f;
    
    void Start()
    {
        radius = GameObject.Find("Manager").GetComponent<GameManager>().randomRadius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
