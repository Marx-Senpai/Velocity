using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            if (this.gameObject.name == "Trigger_PP_SunAdapt")
            {
                Destroy(this.gameObject);
            }  
        }
    }
}
