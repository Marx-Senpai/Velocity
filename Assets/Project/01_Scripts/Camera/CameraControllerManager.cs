using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerManager : MonoBehaviour
{

	
	  // bool for now, can be enum later if more camera type required
	
	// components

	private CameraController camControl;
	public static bool isInArena;

	private void Awake()
	{
		camControl = GetComponent<CameraController>();
		

		isInArena = false;
	}


	public void CustomUpdateCamera () {

       // cameraInputDetector.CamInputDetection();


		if (isInArena)
		{
			camControl.OrbitAround();
			camControl.SimpleLookAt();
		}
		else
		{
			camControl.UpdateHorsArena();
		}
		    
	   

	   
      

		


	}
}
