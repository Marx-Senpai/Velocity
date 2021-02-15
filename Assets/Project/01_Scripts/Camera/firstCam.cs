using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstCam : MonoBehaviour
{
    public GameObject bigFatMur;



    public void OnPlayPressed()
    {
        StartCoroutine(Goforward());
        GetComponent<Fading>().BeginFade(1);
        //GetComponent<Fading>().LevelComplete();
    }

    public IEnumerator Goforward()
    {
        for (float f = 0;  f < 2f; f += Time.deltaTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, (transform.position + transform.forward), Time.deltaTime*1.5f);
            yield return 0;
        }

        StartCoroutine(RealStart());
    }


    public IEnumerator RealStart()
    {
        for (float f = 0;  f < 0.5f; f += Time.deltaTime)
        {
            yield return 0;
        }

        XtraLifeManager xtraLife = GameObject.Find("Manager").GetComponent<XtraLifeManager>();
        xtraLife.TestSeenIntro();

       
    }


    public void AfterPromiseStart()
    {
        gameObject.GetComponent<Fading>().enabled = false;

        GameObject.Find("Main Camera").GetComponent<Camera>().enabled = true;
        GameObject.Find("Manager").GetComponent<GameManager>().enabled = true;
        GameObject.Find("Main Camera").GetComponent<CameraController>().PressPlayCam();

        gameObject.SetActive(false);

        bigFatMur.SetActive(true);

    }
}
