using System.Collections;
using System.Collections.Generic;
using ParadoxNotion.Serialization;
using UnityEngine;

public class StartArena : MonoBehaviour
{
    private GameObject manager;
    private GameManager GM;
    private SoundManager SM;
    private bool arena_started = false;
    private bool arena_in_progress = false;
    
    private float timer = 0;
    
    void Start()
    {
        manager = GameObject.Find("Manager");
        GM = manager.GetComponent<GameManager>();
        SM = manager.GetComponent<SoundManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Player") && !arena_in_progress)
        {
            arena_started = true;
        }
    }
    
    void Update()
    {
        /*if (manager.GetComponent<spawnManager>().spawn == spawnManager.SpawnMode.Timer)
        {
            if (arena_started && !arena_in_progress)
            {
                timer += Time.deltaTime;
                if (timer >= 2)
                {
                    SM.PlayKick1Sound();
                }

                if (timer >= 4)
                {
                    SM.PlayKick2Sound();
                }

                if (timer >= 5)
                {
                    arena_started = false;
                    arena_in_progress = true;
                    SM.PlayWaveSound();
                    manager.GetComponent<spawnManager>().poolRandomEnnemies();
                }
            }
        }*/
    }
}
