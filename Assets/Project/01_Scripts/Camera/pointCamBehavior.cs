using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointCamBehavior : MonoBehaviour
{

    public Transform nextStep;

    public bool letsGo;
    
    private void Update()
    {
        if (letsGo)
        {
            transform.position = Vector3.Lerp(transform.position, nextStep.position, Time.deltaTime * 6f);
        }
    }
}
