using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Timers;
using UnityEngine;

public class CharacterInputDetector : MonoBehaviour {



	public string inputGaucheHorizontal;
	public string inputGaucheVertical;

	public string inputAccelerer;
	public string inputDecelerer;

	public string inputDroitHorizontal;
	public string inputDroitVertical;
	
	public string inputPadHorizontal;
	public string inputPadVertical;

	public string inputAbility;

	public string inputPause;

	public string inputSuiveurUp;
	
	public string inputShield;
	public string inputDash;
	public string inputLance;
	
	public string inputCristallisation;
	
	public float axisXGauche;
	public float axisYGauche;
	
	public float axisXDroit;
	public float axisYDroit;

	public bool isAccelOn;
	public bool isDecelOn;

	public bool isPauseOn;

	public bool abilityUsed;

	public bool isCristalOn;

	public bool isMouseActive;

	public static bool canStartArena;

	public Transform tpPoint;
	

	public enum stanceFormation
	{
		Shield,
		Lance,
		Dash
	}

	public LayerMask layerGround;

	private float previousTimeScale;

	public stanceFormation stance;
	public Color offensive_color;
	public Color defense_color;
	[SerializeField] private Color mobility_color;
	public Color stance_color;
	private ParticleSystem suiveurs_circle;
	public GameObject circleGo;
	private ParticleSystem.MainModule suiveurs_circle_main;

	public GameObject formation;

	public GameObject cam, manager;
	private bool boostOn;

	private float timerBoost;

	private GameObject[] allSpawns;
	
	// components

	private CustomCharacterController customChara;
	private FormationBehavior formationBe;
	private SoundManager sd;
	private GameManager gm;

	private spawnManager.SpawnMode spawn;
	
	RaycastHit hit;
	
	public ParticleSystem thunderStorm;

	public GameObject boss;

	private void Awake()
	{
		
		manager = GameObject.Find("Manager");
		customChara = GetComponent<CustomCharacterController>();
		formationBe = formation.GetComponent<FormationBehavior>();
		sd = manager.GetComponent<SoundManager>();
		this.spawn = manager.GetComponent<spawnManager>().spawn;
		suiveurs_circle = circleGo.GetComponent<ParticleSystem>();
		suiveurs_circle_main = suiveurs_circle.main;
		gm = manager.GetComponent<GameManager>();

	}


	public  Vector3 TmpTrueDirection;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		
		Gizmos.DrawSphere(hit.point, 0.5f);
	}

	public void CharacterInputDetection()
	{

		
		this.spawn = manager.GetComponent<spawnManager>().spawn;
		axisXGauche = Input.GetAxis (inputGaucheHorizontal);
		axisYGauche = Input.GetAxis (inputGaucheVertical);
		
		axisXDroit = Input.GetAxis (inputDroitHorizontal);
		axisYDroit = Input.GetAxis (inputDroitVertical);


		if (isMouseActive)
		{
			Ray rere = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			Physics.Raycast(rere, out hit, 150f, layerGround);

			Camera.main.GetComponent<CameraController>().mousPosHit =  hit.point;
			
			Vector3 direction = (hit.point - transform.position).normalized;
			direction.y = 0;
			TmpTrueDirection = direction;
			//direction.z = 0;
			customChara.axisXDroit = direction.x;
			customChara.axisYDroit = direction.z;
		}
		else
		{
			customChara.axisXDroit = this.axisXDroit;
			customChara.axisYDroit = this.axisYDroit;
		}
		customChara.axisXGauche = this.axisXGauche;
		customChara.axisYGauche = this.axisYGauche;

		if (Input.GetAxis(inputLance) < 0.2f)
		{
			isAccelOn = false;
		}
		
		if (Input.GetButtonDown(inputCristallisation) && isCristalOn && GetComponent<CustomCharacterController>().isFollowed)
		{
			//sd.PlayPaintModeOnSound();
		}

		if (!CameraControllerManager.isInArena && Input.GetKeyDown(KeyCode.F) && XtraLifeManager.seenIntro)
		{
			//DoorBehavior.startArenaCam();
			transform.position = tpPoint.position;
			GameObject.Find("Main Camera").GetComponent<CameraController>().putCamRightArena();
		}
		
		
		

		if (stance == stanceFormation.Lance && !isPauseOn)
		{
			formationBe.PreviewShoot();
		}
		
		if (Input.GetButtonDown(inputPause) && !isPauseOn)
		{
			previousTimeScale = Time.timeScale;
			isPauseOn = true;
			customChara.ApplyPause();
			Cursor.visible = true;
			
		}
		 else if (Input.GetButtonDown(inputPause) && isPauseOn)
		{
			isPauseOn = false;
			
			customChara.UndoPause(previousTimeScale);
		}
		
		if (Input.GetButtonDown(inputSuiveurUp) && gm.IsCheatActive)
		{
			customChara.AddSuiveurs(10);
		}


		if (Input.GetButtonDown(inputDash) && stance == stanceFormation.Dash && (axisXGauche != 0 || axisYGauche != 0) && !isPauseOn)
		{
			if (stance == stanceFormation.Dash && !customChara.canDash && GetComponent<DetectionParcours>().suiveurs.Count > 0)
			{
				GameObject[] allsuiveurs = GameObject.FindGameObjectsWithTag("Recoltable");

				foreach (GameObject go in allsuiveurs)
				{
					if (go.GetComponent<SuiveursManager>().isPicked)
					{
						
						go.GetComponent<SuiveursManager>().DetachAndDestroy();
						
						formationBe.Reforme();
						break;
					}
			
				}
				customChara.canDash = true;
				customChara.isDashing = true;
			}
		}
		
		
		if ((Input.GetAxis(inputLance) >= 0.8f || Input.GetButtonDown(inputLance))  && !isAccelOn && !isPauseOn && !DetectFront.isInFront)
		{
			if (stance == stanceFormation.Lance)
			{
				isAccelOn = true;
				formationBe.ShootOne();
			}
		}

		if (Input.GetKeyDown(KeyCode.N) && spawn == spawnManager.SpawnMode.PressingButton)
		{
			manager.GetComponent<spawnManager>().poolRandomEnnemies();
		}
		
		if (Input.GetKeyDown(KeyCode.H) && gm.IsCheatActive)
		{
			
			customChara.GainHp();
		}


		if ((Input.GetButtonDown(inputShield) || Input.GetButtonDown(inputDash) || Input.GetButtonDown(inputLance)) &&
		    canStartArena)
		{
			activateArena();
			GameObject.Find("Main Camera").GetComponent<CameraController>().putCamRightArena();
		}
		
		
		
		
		

		
		if( !isPauseOn)
		{

			if (Input.GetKeyDown(KeyCode.Keypad9) && CameraControllerManager.isInArena && gm.IsCheatActive)
			{
				boss.GetComponent<BossManager>().CustomStart();
			}
			
			if (Input.GetButtonDown(inputShield) && GetComponent<DetectionParcours>().suiveurs.Count == 0 && stance == stanceFormation.Shield && customChara.isArenaStarted)
			{
				sd.PlayNoModuleSound();	
			}
			else if (Input.GetButtonDown(inputLance) && stance == stanceFormation.Lance &&  customChara.isArenaStarted && GetComponent<DetectionParcours>().suiveurs.Count == 0)
			{
				sd.PlayNoModuleSound();	
			}
			else if (Input.GetButtonDown(inputDash) && stance == stanceFormation.Dash && customChara.isArenaStarted && GetComponent<DetectionParcours>().suiveurs.Count == 0)
			{
				sd.PlayNoModuleSound();	
			}
			
			if (Input.GetButtonDown(inputShield) && stance != stanceFormation.Shield && customChara.isArenaStarted)
			{
				
				sd.PlayStanceSwitchSound();
				formationBe.UnPreviewShoot();
				stance = stanceFormation.Shield;
				formationBe.InstantiateFormation(formationBe.GetComponent<FormationBehavior>().allFormations[0]);
				stance_color = defense_color;
			}
			else if (Input.GetButtonDown(inputShield) && GetComponent<DetectionParcours>().suiveurs.Count > 0 && stance == stanceFormation.Shield && !DetectFront.isInFront )
			{
				if (manager.GetComponent<GameManager>().allMurs.Count == MurSuiveurBehavior.maxNbr)
				{
					Destroy(manager.GetComponent<GameManager>().allMurs[0]);
					manager.GetComponent<GameManager>().allMurs.RemoveAt(0);
					formationBe.putShieldDown();
				}
				else
				{
					formationBe.putShieldDown();
				}
				
			}
			else if (Input.GetAxis(inputLance) >= 0.8f && stance != stanceFormation.Lance && !isAccelOn && customChara.isArenaStarted)
			{
				
				sd.PlayStanceSwitchSound();
				isAccelOn = true;
				stance = stanceFormation.Lance;
				formationBe.InstantiateFormation(formationBe.GetComponent<FormationBehavior>().allFormations[1]);
				stance_color = offensive_color;
			}
			else if (Input.GetButtonDown(inputDash) && stance != stanceFormation.Dash && customChara.isArenaStarted)
			{
				
				sd.PlayStanceSwitchSound();
				formationBe.UnPreviewShoot();
				stance = stanceFormation.Dash;
				formationBe.InstantiateFormation(formationBe.GetComponent<FormationBehavior>().allFormations[2]);
				stance_color = mobility_color;
			}
		}



		if (circleGo.activeSelf)
		{
			suiveurs_circle_main.startColor = stance_color;
		}
		
		customChara.abilityUsed = this.abilityUsed;

		
	}


	public void activateArena()
	{
		customChara.isArenaStarted = true;
		customChara.cursor_sign.SetActive(false);
		sd.SetEnemyKickParameter(2);
		thunderStorm.Stop();
		
		manager.GetComponent<spawnManager>().StartCustomWaveInGame();
		manager.GetComponent<ScoreManager>().txt_timer.gameObject.SetActive(true);
		manager.GetComponent<ScoreManager>().txt_timer.gameObject.transform.parent.gameObject.SetActive(true);
		manager.GetComponent<ScoreManager>().txt_score.gameObject.transform.parent.gameObject.SetActive(true);
		manager.GetComponent<ScoreManager>().txt_chainMultiplier.gameObject.transform.parent.gameObject.SetActive(true);
		circleGo.SetActive(true);
		SpawnSuiveurBehavior.canSpawn = true;

		GameObject.Find("ps_StartArena").GetComponent<ParticleSystem>().Stop(false, ParticleSystemStopBehavior.StopEmitting);
		GameObject.Find("StartTrigger").SetActive(false);

		 allSpawns = GameObject.FindGameObjectsWithTag("SpawnSuiveur");


		foreach (GameObject go in allSpawns)
		{
			go.transform.GetChild(0).gameObject.SetActive(true);
		}
		
		
	}

}
