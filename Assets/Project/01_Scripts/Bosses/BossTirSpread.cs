using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTirSpread : MonoBehaviour
{
   public int numberOfBullet;
    public float angleOfSpread;

    public GameObject PrefabBullet;
    
   


    public void shootSpread()
    {

        if (gameObject.activeSelf)
        {

            float angleRef = angleOfSpread / numberOfBullet;

            for (int i = 0; i < numberOfBullet; i++)
            {
                // float rdAngle = Random.Range(-angleOfSpread.value / 2, angleOfSpread.value / 2);
                float rdAngle = -angleOfSpread / 2 + angleRef * i;
                Vector3 eulerRotation = transform.rotation.eulerAngles + new Vector3(0, rdAngle, 0);
                GameObject bullet = (GameObject) Object.Instantiate(PrefabBullet, transform.position,
                    Quaternion.Euler(eulerRotation));
                // clone.transform.position += new Vector3(0, 0, 0.5f);
            }

        }
    }

    
}
