using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoMan : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        Gizmos.DrawWireSphere(transform.position, 1f);

    }
}
