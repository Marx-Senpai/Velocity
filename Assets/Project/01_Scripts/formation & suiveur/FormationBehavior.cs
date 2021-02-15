using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FormationBehavior : MonoBehaviour
{


	

	

	
	private int loopCount, nameCount;

	public GameObject pointCreated;
	public GameObject player;

	public List<int> numberLignes;
	private int nb;

	private float vitesseMinAvatar;
	private float vitesseMaxAvatar;

	public List<Vector3> pointPositions;

	private int num;


	public float timeLerpSmoothMultiplier;

	
	public int compteur, maxColonne;
	
	public bool isHitted, isHittedRefresh;

	public float timerLimit, timer;
	
	int cp;
	private int counter;

	public GameObject[] allFormations;
	private LineRenderer lr;
	private SoundManager sd;

	public GameObject murSuiveur;
	
	private void Start()
	{
		sd = GameObject.Find("Manager").GetComponent<SoundManager>();
		InstantiateFormation(allFormations[0]);
		lr = this.GetComponent<LineRenderer>();
	}


	public void InstantiateFormation(GameObject formationType)
	{
		player.GetComponent<CustomCharacterController>().FormationAwake(player.GetComponent<DetectionParcours>().suiveursLimit);

		GameObject[] allsuiveurs = GameObject.FindGameObjectsWithTag("Recoltable");

		foreach (GameObject go in allsuiveurs)
		{
			
			go.GetComponent<SuiveursManager>().targetPoint = null;
			
		}

		foreach (GameObject go in allFormations)
		{
			if (formationType.Equals(go))
			{
				go.SetActive(true);
			}
			else
			{
				go.SetActive(false);
			}
		}
	}

	
	public IEnumerator Reforme()
	{	
		for (float f = 0f; f < timerLimit; f += Time.deltaTime)
		{
			timer = f;
			if (isHittedRefresh)
			{
				isHittedRefresh = false;
				f = 0f;
			}

			yield return null;
			
		}
		
		isHitted = false;
		
		if (player.GetComponent<CharacterInputDetector>().stance == CharacterInputDetector.stanceFormation.Shield)
		{
			InstantiateFormation(allFormations[0]);
			isHitted = false;
		}
		else if (player.GetComponent<CharacterInputDetector>().stance == CharacterInputDetector.stanceFormation.Lance)
		{
			InstantiateFormation(allFormations[1]);
			isHitted = false;
		}
		else if (player.GetComponent<CharacterInputDetector>().stance == CharacterInputDetector.stanceFormation.Dash)
		{
			InstantiateFormation(allFormations[2]);
			isHitted = false;
		}

		

	}
	
	
	
	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Obstacle")
		{
			GetComponent<FormationBehavior>().isHittedRefresh = true;
		}
	}



	public void ActivateSpear()
	{
		Vector3 tempV = allFormations[1].transform.localScale;
		tempV.z = Mathf.MoveTowards(tempV.z, 1f, Time.deltaTime * 5f);
		
		allFormations[1].transform.localScale = tempV;
	}

	public void UnActivateSpear()
	{
		Vector3 tempV = allFormations[1].transform.localScale;
		tempV.z = Mathf.MoveTowards(tempV.z, 0.4f, Time.deltaTime * 3f);
		
		allFormations[1].transform.localScale = tempV;
	}

	public void PreviewShoot()
	{
		lr.enabled = true;
		lr.SetPosition(0, player.transform.position);
		lr.SetPosition(1, player.transform.position + (player.GetComponent<CharacterInputDetector>().TmpTrueDirection.normalized) * 500);
	}

	public void UnPreviewShoot()
	{
		lr.enabled = false;
	}
	
	public void ShootOne()
	{
		lr.enabled = false;
		GameObject[] allsuiveurs = GameObject.FindGameObjectsWithTag("Recoltable");

		foreach (GameObject go in allsuiveurs)
		{
			if (go.GetComponent<SuiveursManager>().isPicked)
			{
				go.transform.position = transform.position;
			
				go.GetComponent<SuiveursManager>().isShooted = true;
				go.GetComponent<SuiveursManager>().isPicked = false;
				go.GetComponent<SuiveursManager>().shootDirection =
					player.GetComponent<CharacterInputDetector>().TmpTrueDirection.normalized;
				go.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
				player.GetComponent<CustomCharacterController>().FormationAwake(player.GetComponent<DetectionParcours>().suiveursLimit);
				player.GetComponent<DetectionParcours>().suiveurs.Remove(go);
				StartCoroutine(Reforme());
				sd.PlayShootSound();
				break;
			}
			
		}
	}
	
	public void putShieldDown()
	{
		GameObject[] allsuiveurs = GameObject.FindGameObjectsWithTag("Recoltable");

		foreach (GameObject go in allsuiveurs)
		{
			if (go.GetComponent<SuiveursManager>().isPicked)
			{
				go.GetComponent<SuiveursManager>().DetachAndDestroy();
				

				GameObject mur = Instantiate(murSuiveur, transform.position, transform.rotation);
					
				StartCoroutine(Reforme());
				
				break;
			}
			
		}
	}




}
