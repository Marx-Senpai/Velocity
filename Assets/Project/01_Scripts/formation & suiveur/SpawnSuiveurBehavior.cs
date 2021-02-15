using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnSuiveurBehavior : MonoBehaviour
{

	public GameObject prefabRecoltable;
         
         	public float timerSpawn, timerLimit;
         	//private GameObject parent;
         	public GameObject go;
         	private GameObject player;
         	private GameObject timer;
         
         	private RaycastHit hit;
         
         	public bool isFull;
         
         	public LayerMask layerSol;
         	public float distanceFromGround;

			public static bool canSpawn;

	private void Awake()
	{
		//parent = GameObject.Find("Recoltables");
		player = GameObject.Find("Player");
		//timer = GameObject.Find("timer");
         
		timerSpawn = timerLimit;
         
		Physics.Raycast(transform.position, Vector3.down, out hit, 50f, layerSol);
         
		transform.position = hit.point + new Vector3(0, distanceFromGround, 0);
		        
		timerSpawn = timerLimit - 1f;

		canSpawn = false;

	}


	/*private void OnEnable()
	{
		//parent = GameObject.Find("Recoltables");
		player = GameObject.Find("Player");
		//timer = GameObject.Find("timer");
         
		timerSpawn = timerLimit;
         
		Physics.Raycast(transform.position, Vector3.down, out hit, 50f, layerSol);
         
		transform.position = hit.point + new Vector3(0, distanceFromGround, 0);
		        
		timerSpawn = timerLimit - 1f;

		canSpawn = true;
	}*/


	public void Update()
         	{
		         if (canSpawn)
		         {


			         if (!isFull)
			         {
				         timerSpawn += Time.deltaTime;
				         if (timerSpawn >= timerLimit)
				         {

					         go = Instantiate(prefabRecoltable, transform);

					         go.GetComponent<SuiveursManager>().player = player;
					         go.GetComponent<SuiveursManager>().spawnAttached = this.gameObject;
					         go.GetComponent<SuiveursManager>().isFromSpawn = true;
					         timerSpawn = 0f;

					         if (player.GetComponent<DetectionParcours>().suiveurs.Count >=
					             player.GetComponent<DetectionParcours>().suiveurs.Count)
					         {
						         go.layer = 12;
					         }

					         isFull = true;
				         }
			         }
			         else
			         {
				         timerSpawn = 0f;
			         }

		         }




	         }
	}