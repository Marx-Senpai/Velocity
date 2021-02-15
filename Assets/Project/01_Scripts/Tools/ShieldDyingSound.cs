using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;

public class ShieldDyingSound : MonoBehaviour
{
    private SoundManager sd;

    void Start()
    {
        sd = GameObject.Find("Manager").GetComponent<SoundManager>();
        sd.PlayShieldDeploySound(gameObject);
    }

}
