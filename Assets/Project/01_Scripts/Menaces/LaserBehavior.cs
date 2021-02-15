using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;

public class LaserBehavior : MonoBehaviour
{
    public float travelSpeed;

    public float timerDestruct;

    private float timer;

    private Vector3 startPosition;

    private Vector3 ZScale;
    
    [SerializeField] private GameObject ps_impact;
    
    void Start()
    {
        startPosition = transform.localPosition;
        //transform.LookAt(GameObject.Find("Player").transform);
        ZScale = new Vector3(1,1,1);
        Camera.main.GetComponent<RFX4_CameraShake>().enabled = true;
    }

   
    void Update()
    {
        ZScale.z += Time.deltaTime * travelSpeed;

        transform.localScale = ZScale;
        
       // transform.localPosition = new Vector3(startPosition.x , startPosition.y, startPosition.z + (ZScale.z/2));

        timer += Time.deltaTime;

        if (timer >= timerDestruct)
        {
            Camera.main.GetComponent<RFX4_CameraShake>().enabled = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !other.isTrigger)
        {
            other.GetComponent<CustomCharacterController>().LooseHp();
            Instantiate(ps_impact, other.transform.position, Quaternion.identity);
        }

        if (other.tag == "Recoltable")
        {
            other.GetComponent<SuiveursManager>().DetachAndDestroy();
        }
    }
}
