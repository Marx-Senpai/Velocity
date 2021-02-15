using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWaveKick : MonoBehaviour
{
    [SerializeField] private bool isFirstDoor;
    private SoundManager sd;

    public GameObject backDoor;
    public static bool isStart;

    private GameObject manager;
    
    void Start()
    {
        sd = GameObject.Find("Manager").GetComponent<SoundManager>();
        manager = GameObject.Find("Manager");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
           
               StartCoroutine( setupDoors());
            
            
            

            if(!isStart)
            {
               
                isStart = true;
            }
            
            if (isFirstDoor)
            {
               // Debug.Log("wala");
               
                sd.SetEnemyKickParameter(1);
            }
            else
            {
               // Debug.Log("fuck");
                sd.SetEnemyKickParameter(2);
            }


            GetComponent<BoxCollider>().enabled = false;

        }
    }
    
    
    public IEnumerator setupDoors()
    {
        backDoor.GetComponent<DoorBehavior>().TriggerThisDoor();
        
        
        for (float f = 0; f < 1.5f; f += Time.deltaTime)
        {
            yield return 0;
        }
        
        startArenaCam();
        
        
       // gameObject.SetActive(false);
    }
 
    public static void startArenaCam()
    {

        GameObject.Find("Manager").GetComponent<XtraLifeManager>().SetIntroSeenData(true);
        // GameObject.Find("Manager").GetComponent<PlayerSave>().panelSkip.SetActive(false);
        // GameObject.Find("Main Camera").GetComponent<CameraController>().putCamRightArena();
    }

    
}
