using System.Collections;
using System.Collections.Generic;
using KdTree;
using UnityEngine;

public class formationPointBehavior : MonoBehaviour
{
	private RaycastHit hit;

	public float minimalDistance;

	private Vector3 estimatedPosition;

	public float distance;

	public LayerMask layerSol;

	public GameObject player;

	private void Awake()
	{
		player = GameObject.Find("Player");
	}


	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		
		Gizmos.DrawSphere(transform.position, 0.1f);

		Gizmos.color = Color.blue;
		
		Gizmos.DrawLine(transform.position, (transform.position + (Vector3.down*20)));
	}
	
	

	public void UpdatePositionFromGround()
	{

		if (Physics.Raycast(transform.position, Vector3.down, out hit, 20f, layerSol))
		{
			distance = hit.distance;

			estimatedPosition = ((transform.position - hit.point).normalized * minimalDistance) + hit.point;

			transform.position = Vector3.Lerp(transform.position, estimatedPosition, Time.deltaTime *20f);
		}

	    	transform.forward = Vector3.ProjectOnPlane(player.transform.forward, hit.normal);

	}

	

}
