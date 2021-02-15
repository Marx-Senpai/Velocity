using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {
	[SerializeField] private float turnSpeed = 10f;
	
	private enum AxisDirection {X, Y, Z};
	[SerializeField] private AxisDirection axis = AxisDirection.X;

	private Vector3 rotation_vec;

	public bool isTop;

	void Update () {

		if (!isTop)
		{
			switch(this.axis) {
				case AxisDirection.X:
					this.rotation_vec = Vector3.left;
					break;
				case AxisDirection.Y:
					this.rotation_vec = Vector3.up;
					break;
				case AxisDirection.Z:
					this.rotation_vec = Vector3.forward;
					break;
			}

			//Rotate Sun with Speed
			transform.RotateAround(Vector3.zero, this.rotation_vec, turnSpeed * Time.deltaTime);
			// transform.LookAt(Vector3.zero);
		}
		
		if(transform.rotation.eulerAngles.x >= 90)
		{
			Debug.Log("wala");
			isTop = true;
		}




	}

	
}
