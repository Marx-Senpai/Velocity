using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoucheSetup : MonoBehaviour
{
   public List<GameObject> objectsOnwed;

    public float distanceFromCenter;

    private float angleSetApart;

    public bool isRotatingClockwise;

    public float rotateSpeedP1, rotateSpeedP2, rotateSpeedP3;
    
  public void customStart()
    {
        objectsOnwed.Clear();
        
        for (int i = 0; i < transform.childCount; i++)
        {
            objectsOnwed.Add(transform.GetChild(i).gameObject);
        }

        angleSetApart = 360f / objectsOnwed.Count;
        
        for (int j = 0; j < objectsOnwed.Count; j++)
        {
            objectsOnwed[j].transform.position = transform.position;
            objectsOnwed[j].transform.rotation = Quaternion.identity;

            var tempEuler = objectsOnwed[j].transform.rotation.eulerAngles;

            tempEuler.y += angleSetApart * j;
            
            objectsOnwed[j].transform.rotation = Quaternion.Euler(tempEuler);

            var tempPosition = objectsOnwed[j].transform.position;

            tempPosition += objectsOnwed[j].transform.forward * distanceFromCenter;

            objectsOnwed[j].transform.position = tempPosition;

        }
        
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OrderAttackStandard();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OrderAttackSpread();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OrderAttackTarget();
        }

        if (BossManager.nbHeartsAlive == 3)
        {
           
            if (isRotatingClockwise)
            {
                transform.Rotate(Vector3.up, rotateSpeedP1);
            }
            else
            {
                transform.Rotate(Vector3.up, -rotateSpeedP1);
            }
        }
        else if (BossManager.nbHeartsAlive == 2)
        {
            
            if (isRotatingClockwise)
            {
                transform.Rotate(Vector3.up, rotateSpeedP2);
            }
            else
            {
                transform.Rotate(Vector3.up, -rotateSpeedP2);
            }
        }
        else if (BossManager.nbHeartsAlive == 1)
        {
            
            if (isRotatingClockwise)
            {
                transform.Rotate(Vector3.up, rotateSpeedP3);
            }
            else
            {
                transform.Rotate(Vector3.up, -rotateSpeedP3);
            }
        }
       
    }

    public void OrderAttackStandard()
    {
        for (int j = 0; j < objectsOnwed.Count; j++)
        {
            if (objectsOnwed[j].tag == "BossStandard")
            {
                objectsOnwed[j].GetComponent<BossTirStandardBehavior>().shootInFront();
            }
        }
    }
    
    public void OrderAttackSpread()
    {
        for (int j = 0; j < objectsOnwed.Count; j++)
        {
            if (objectsOnwed[j].tag == "BossSpread")
            {
                objectsOnwed[j].GetComponent<BossTirSpread>().shootSpread();
            }
        }
    }
    
    
    public void OrderAttackTarget()
    {
        for (int j = 0; j < objectsOnwed.Count; j++)
        {
            if (objectsOnwed[j].tag == "BossStandard")
            {
                objectsOnwed[j].GetComponent<BossTirStandardBehavior>().shootTarget();
            }
        }
    }
    
}
