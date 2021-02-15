using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerManager : MonoBehaviour {


public 	GameObject cam;

	private CharacterInputDetector charaInput;
	private DetectionParcours detectP;
	private CustomCharacterController customChara;
	private CameraControllerManager cameraControl;

	private void Awake()
	{
		charaInput = GetComponent<CharacterInputDetector>();
		detectP = GetComponent<DetectionParcours>();
		customChara = GetComponent<CustomCharacterController>();
		cameraControl = cam.GetComponent<CameraControllerManager>();
	}


	public void CustomManagerControllerUpdate () 
	{
		charaInput.CharacterInputDetection ();
		customChara.CustomUpdateBeforeControl ();
		cameraControl.CustomUpdateCamera();
	}
}
