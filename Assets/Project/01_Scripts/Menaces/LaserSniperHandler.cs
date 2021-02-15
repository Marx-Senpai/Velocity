using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using UnityEngine;

public class LaserSniperHandler : MonoBehaviour
{
    [SerializeField] private GameObject chargingLaser;

    private List<ParticleSystem> particles;
    private LineRenderer sniper_lr_preview;
    private bool isplayed;
    private SoundManager sd;
    [HideInInspector] public EventInstance chargingSound_event;
    
    void Start()
    {
        sd = GameObject.Find("Manager").GetComponent<SoundManager>();
        sniper_lr_preview = gameObject.GetComponent<LineRenderer>();
        particles = new List<ParticleSystem>();
        for (int i = 0; i < chargingLaser.transform.childCount; i++)
        {
            particles.Add(chargingLaser.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>());
        }
    }

    void Update()
    {
        if (sniper_lr_preview.isVisible)
        {
            if (!isplayed)
            {
                chargingSound_event = sd.PlayChargingSniperSound(gameObject);
                isplayed = true;
            }
        }
        else
        {
            isplayed = false;
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Play();
            }
        }
    }
}
