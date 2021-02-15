using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBasse : MonoBehaviour
{
    private SoundManager sd;

    void Start()
    {
        sd = GameObject.Find("Manager").GetComponent<SoundManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject.Find("Manager").GetComponent<PlayerSave>().panelSkip.SetActive(false);
        
        if (this.gameObject.tag == "Trigger/In" && !sd.basseOn)
        {
            sd.windFadingToBasse = true;
        }
        
        if (this.gameObject.tag == "Trigger/Out" && sd.basseOn)
        {
            sd.windFadingInSolo = true;
        }
    }
}
