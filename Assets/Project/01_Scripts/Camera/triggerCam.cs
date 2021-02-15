using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerCam : MonoBehaviour
{
   public GameObject pointAssociated;


   private void OnTriggerEnter(Collider other)
   {
      pointAssociated.GetComponent<pointCamBehavior>().letsGo = true;
   }
}
