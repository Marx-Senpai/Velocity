using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;

public class MurSuiveurBehavior : MonoBehaviour
{

    public float CdTimerDestruct, timerDestruct;

    public static int nbrAllMur;

    public static int maxNbr = 4;

    public int NbrShoot = 5;

    private GameObject manager;
    
    [SerializeField] private Material mat_lastHit;
    private Renderer rend;
    private Animator anim;
    private SoundManager sd;
    private bool dying;
    
    void Start()
    {
        manager = GameObject.Find("Manager");
        
        sd = manager.GetComponent<SoundManager>();
        sd.PlayShieldDeploySound(gameObject);
        rend = gameObject.GetComponent<Renderer>();
        anim = gameObject.GetComponent<Animator>();
        
        nbrAllMur++;
        manager.GetComponent<GameManager>().allMurs.Add(gameObject);
    }

    
    void Update()
    {
        timerDestruct += Time.deltaTime;

        if (timerDestruct >= CdTimerDestruct || NbrShoot <= 0)
        {
            StartCoroutine(DestroyShield());
        }

        if (dying)
        {
            gameObject.GetComponent<ShieldDyingSound>().enabled = true;
        }
    }

    IEnumerator DestroyShield()
    {
        rend.material.Lerp(rend.material, mat_lastHit, Time.deltaTime * 2);
        manager.GetComponent<GameManager>().allMurs.Remove(gameObject);
        nbrAllMur--;
        anim.Play("Shield_Destroying");
        dying = true;
        yield return new WaitForSecondsRealtime(1);
        Destroy(gameObject);
    }
}

