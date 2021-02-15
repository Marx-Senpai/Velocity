using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInputDetector : MonoBehaviour {

    private CameraController cameraController;
    private CameraControllerManager cameraControllerManager;

    public float xAxisCam;
    public float yAxisCam;

	public string inputDroitHorizontal;
	public string inputDroitVertical;

    private void Awake()
    {
        cameraController = GetComponent<CameraController>();
        cameraControllerManager = GetComponent<CameraControllerManager>();
    }


    /*public void CamInputDetection()
    {

        xAxisCam = Input.GetAxis("HorizontalDroit");
        yAxisCam = Input.GetAxis("VerticalDroit");

    }*/



}
