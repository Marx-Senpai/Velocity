using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class ObstacleBehavior : MonoBehaviour
{

	public GameObject player;
	public GameObject formation;

	public int hp;
	
	
	public bool[] pointsFormation;
	
	// components

	private FormationBehavior formationBe;

	private void Awake()
	{
		formationBe = formation.GetComponent<FormationBehavior>();
		hp = GameObject.Find("Manager").GetComponent<GameManager>().hpS;
	}

	private void Update()
	{
		pointsFormation = player.GetComponent<CustomCharacterController>().formation;
		
		if (hp <= 0)
		{
			Destroy(this.gameObject);
		}
	}

	/*private void OnCollisionEnter(Collision other)
	{
		if (other.collider.CompareTag("Recoltable"))
		{
			//GameObject.Find("Manager").GetComponent<SoundManager>().PlayCollisionFollowerSound(other.gameObject);
			pointsFormation[other.gameObject.GetComponent<SuiveursManager>().indexPoint] = false;
			
			
			other.gameObject.GetComponent<SuiveursManager>().DetachAndDestroy();
			
			if (!formationBe.isHitted)
			{
				formationBe.isHitted = true;
				StartCoroutine(formationBe.Reforme());
			}
			else
			{
				formationBe.isHittedRefresh = true;
			}

			hp--;

			
		}
		
	}*/

	
}
