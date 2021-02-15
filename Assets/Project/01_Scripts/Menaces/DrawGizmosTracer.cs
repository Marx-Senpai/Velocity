using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using UnityEngine;

public class DrawGizmosTracer : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float tempF = (float)GetComponent<Blackboard>().GetVariable("DetectRange").value;
        Gizmos.DrawWireSphere(transform.position, tempF);
    }
}
