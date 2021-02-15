using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetKick : MonoBehaviour
{
    private SoundManager sd;
    
    void Start()
    {
        sd = GameObject.Find("Manager").GetComponent<SoundManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        sd.SetEnemyKickParameter(2);
    }
    
    private void OnTriggerExit(Collider other)
    {
        Destroy(gameObject);
    }
}
