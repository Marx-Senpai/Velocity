using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class DoorBehavior : MonoBehaviour
{
    private float timer = 0;
    [SerializeField] private float fill;
    [SerializeField] private float maxFill = .9f;
    public bool isDoorTriggered = false;

    [SerializeField] public  GameObject trigger;
    [SerializeField] private GameObject door;
    private Material mat_door;

    private BoxCollider collider_door;

    public GameObject otherDoor;

    private SoundManager sd;

    void Start()
    {
        sd = GameObject.Find("Manager").GetComponent<SoundManager>();
        door = gameObject;
        mat_door = door.GetComponent<Renderer>().material;
        collider_door = door.GetComponent<BoxCollider>();
    }


    public void TriggerThisDoor()
    {
        isDoorTriggered = true;
        door.transform.GetChild(0).GetComponent<Animator>().enabled = true;
        door.transform.GetChild(1).GetComponent<Animator>().enabled = true;
        sd.PlayDoorClosingSound(door);

        door.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        door.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            
        door.transform.GetChild(2).gameObject.SetActive(true);
            
        collider_door.isTrigger = false;
        
        door.transform.parent.GetChild(0).gameObject.SetActive(false);

        StartCoroutine(wait());
    }

    public IEnumerator wait()
    {
        for (float f = 0; f < 1f; f+= Time.deltaTime)
        {
            yield return 0;
        }
        
        triggerOtherDoor();
    }

    public void triggerOtherDoor()
    {
        otherDoor.transform.GetChild(0).GetComponent<Animator>().enabled = true;
        otherDoor.transform.GetChild(1).GetComponent<Animator>().enabled = true;
        sd.PlayDoorClosingSound(otherDoor);

        otherDoor.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        otherDoor.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            
        otherDoor.transform.GetChild(2).gameObject.SetActive(true);
        
       BoxCollider bobox = otherDoor.GetComponent<BoxCollider>();
            
        bobox.isTrigger = false;
        
    }
    
    


    public void DesactivateDoor()
    {
        gameObject.SetActive(false);
        sd.PlayDoorOpeningSound(gameObject);
    }

    public static void startArenaCam()
    {
        CameraControllerManager.isInArena = true;
        GameObject.Find("Manager").GetComponent<XtraLifeManager>().SetIntroSeenData(true);

        //GameObject.Find("Manager").GetComponent<PlayerSave>().panelSkip.SetActive(false);
    }




}
