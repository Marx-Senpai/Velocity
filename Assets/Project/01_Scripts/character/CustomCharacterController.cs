using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CustomCharacterController : MonoBehaviour
{

	public Vector3 directionPlayerMove = Vector3.zero;
	public Vector3 directionFormationMove = Vector3.zero;
	public Vector3 directionPlayerFormation;

	public float distanceFormation;
	public GameObject formationStartingPos;

	public float axisXGauche;
	public float axisYGauche;

	public float axisXDroit;
	public float axisYDroit;

	public float axisXPad;
	public float axisYPad;

	public GameObject player;

	public GameObject suiveur;

	public GameObject formationGO;

	public bool[] formation;


	Quaternion rotationCamera;
	Vector3 camYRotation;


	public float moveSpeed = 5f;

	public bool isPausedOn, canNextStepCam;

	public GameObject cam;

	public GameObject pauseText;
	[SerializeField] private GameObject inGame_panel;

	//public Vector3 reboundDirection;

	public bool isFollowed;

	public LayerMask layerSol, layerOuterRing;

	public Quaternion smoothTilt;

	public float minimalDistance;

	public float GroundDis;

	

	public GameObject mesh;

	public GameObject manager;

	private RaycastHit hitDash;


	public bool isDashing, canDash, beginDash;
	public float cdDash, timerDash;

	public float distanceDash;
	public Vector3 dashTarget;

	public float dashSpeed;

	private Vector3 directionXCamera;

	public int previousHp;

	public float cdAloneGimme, timerGimme;
	

	// Abilities
	public bool abilityUsed;

	public bool isArenaStarted;

	public int hp;

	public Text playerHpText;

	public float packRotateSpeed;

	public GameObject playerMesh;

	private UIManager ui_manager;

	public TextMeshProUGUI surviveTimerText;
	public Text highscore;

	private bool dying = false;
	
	public float timeInvincibility;
	public bool isInvincible;
	[SerializeField] private Material mat_avatar;
	[SerializeField] private Material mat_avatar_invincible;
	[SerializeField] private Material mat_cloth;
	[SerializeField] private Material mat_cloth_invincible;
	
	// components 
	private SoundManager sd;
	private ScoreManager sc;
	private DetectionParcours detectParcours;
	private CharacterInputDetector inputDetect;
	private CameraController camControl;
	private Rigidbody rb;
	private spawnManager sm;
	private GameManager gm;

	[SerializeField] private GameObject dash_feedback;
	private ParticleSystem dash_particles;
	[SerializeField] private GameObject impact_enemy_feedback;
	[SerializeField] private GameObject pp_damage_taken;
	[SerializeField] private GameObject ps_destructive_wave;
	private GameObject destructiveWave;
	[SerializeField] private GameObject ps_gainHP;
	[SerializeField] private GameObject ps_HP1;
	[SerializeField] private GameObject ps_HP2;
	[SerializeField] private GameObject ps_HP3;
	[SerializeField] private GameObject viseur_dash;
	[SerializeField] private GameObject viseur_offensif;
	public GameObject cursor_sign;


	public TextMeshProUGUI viAndD;
	
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		smoothTilt = new Quaternion();
		player = GameObject.Find("Player");
		formationGO = GameObject.Find("Formation");
		manager = GameObject.Find("Manager");
		sd = manager.GetComponent<SoundManager>();
		sc = manager.GetComponent<ScoreManager>();
		sm = manager.GetComponent<spawnManager>();
		ui_manager = GameObject.Find("Canvas").GetComponent<UIManager>();
		detectParcours = player.GetComponent<DetectionParcours>();
		inputDetect = player.GetComponent<CharacterInputDetector>();
		camControl = cam.GetComponent<CameraController>();
		playerHpText.text = "HP : " + hp;
		dash_particles = dash_feedback.GetComponent<ParticleSystem>();
		gm = manager.GetComponent<GameManager>();
		
	}

	public void FormationAwake(int formationLenght)
	{
		
	
		formation = new bool[formationLenght];
		

		for (int i = 0; i < formation.Length; i++)
		{
			formation[i] = false;
		}
	}

	public void CustomUpdateBeforeControl()
	{
		detectParcours.CheckNumberFollowers();
		
	
		//StartCoroutine(CheckHP());

		if (!dying)
		{

			if (detectParcours.suiveurs.Count == 0 && SetWaveKick.isStart && SpawnSuiveurBehavior.canSpawn)
			{
				timerGimme += Time.deltaTime;

				if (timerGimme >= cdAloneGimme)
				{
					GameObject go = Instantiate(suiveur, transform.position, transform.rotation);
					var pos = transform.position;
					pos.z -= 2;
					go.transform.position = pos;
					go.GetComponent<SuiveursManager>().player = player;
					go.GetComponent<SuiveursManager>().isPicked = true;
					go.transform.GetChild(2).gameObject.SetActive(false);
			
					detectParcours.suiveurs.Add(go);

					go.gameObject.layer = 13;
					go.gameObject.transform.GetChild(1).gameObject.layer = 13;

					timerGimme = 0f;
				}
				
			}
			else
			{
				timerGimme = 0f;
			}
			
			directionPlayerMove = new Vector3(axisXGauche, 0, -axisYGauche);
			if (directionPlayerMove == Vector3.zero)
			{
				sd.PlayFootstepsSound();
			}
			
			if (canDash && isDashing)
			{
				if (!beginDash)
				{
					sd.PlayDashSound();
					gameObject.layer = 24;
					beginDash = true;
					dashTarget = transform.position +
					             directionPlayerMove.normalized * distanceDash;
					

					if (hitDash.point != Vector3.zero)
					{
						if (Vector3.Distance(transform.position, dashTarget) > Vector3.Distance(transform.position, hitDash.point))
						{
							dashTarget = hitDash.point;
							Debug.Log("shortDash");
						}
					}
					
				}
				if (beginDash)
				{
					playerMesh.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = mat_avatar_invincible;
					playerMesh.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Renderer>().material = mat_cloth_invincible;
					dash_particles.Play();
					transform.position = Vector3.MoveTowards(transform.position, dashTarget, Time.deltaTime * 50f);

					if (transform.position == dashTarget)
					{
						gameObject.layer = 9;
						beginDash = false;
						StartCoroutine(ResetDashCD(cdDash));
						isDashing = false;

					}
				}
			}

			camYRotation = cam.transform.rotation.eulerAngles;
			camYRotation.x = 0;
			camYRotation.z = 0;

			rotationCamera = Quaternion.Euler(camYRotation);

			if (!isDashing)
			{
				dash_particles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
				MovePlayer(directionPlayerMove.normalized);
			}

			if (SetWaveKick.isStart)
			{
				if ((axisXDroit > 0.2f || axisXDroit < -0.2f) || (axisYDroit > 0.2f || axisYDroit < -0.2f))
				{
					directionFormationMove = new Vector3(axisXDroit, 0, axisYDroit);
				}

				//Debug.Log(directionFormationMove);

				directionFormationMove = directionFormationMove.normalized * distanceFormation;

				directionPlayerFormation = formationGO.transform.position - transform.position;

				if (inputDetect.stance == CharacterInputDetector.stanceFormation.Dash)
				{
					formationGO.transform.position =
						transform.position + formationGO.transform.forward * distanceFormation;
					formationGO.transform.forward = Vector3.forward;

				}
				else
				{
					MoveFormation(directionFormationMove);
				}

			}


			AlignWithGround();

			if (!rb.IsSleeping())
			{
				rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * 2f);
			}


			
		}
	}

	public void MoveFormation(Vector3 direction)
	{
		
		Quaternion rot = cam.transform.rotation;
		rot.z = 0;
		rot.x = 0;
		
		Vector3 directionXCamera =  rot * direction;

		var pos = formationStartingPos.transform.position;
		pos += directionXCamera;

		

		float angle = Vector3.Angle((formationGO.transform.position - transform.position), directionXCamera);
		 Vector3 vTemp = Vector3.Cross((formationGO.transform.position - transform.position), directionXCamera);
		if (vTemp.y < 0) angle = -angle;


		float ang = 2f;
		
		
		formationGO.transform.forward = Vector3.RotateTowards(formationGO.transform.forward, (pos - transform.position),
			Time.deltaTime * packRotateSpeed, ang);

		formationGO.transform.position = transform.position + formationGO.transform.forward * distanceFormation;
		

	}

	public void MovePlayer(Vector3 direction)
	{
		
		
		Quaternion rot = cam.transform.rotation;
		rot.z = 0;
		rot.x = 0;
		
		 directionXCamera =  rot * direction;

		Physics.Raycast(transform.position, directionXCamera, out hitDash, 15f, layerOuterRing);
		
		
		
		transform.Translate(directionXCamera * moveSpeed * Time.deltaTime);
		
		//rb.velocity = directionXCamera * moveSpeed;
		
		playerMesh.transform.LookAt(transform.position + directionXCamera);
	}
	
	

	
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		
		Gizmos.DrawLine(transform.position, transform.TransformPoint(Vector3.down*40));
		
		Gizmos.color = Color.magenta;
		
		Gizmos.DrawWireSphere(transform.position, distanceFormation);
		
	
		Gizmos.color = Color.black;
		
		Gizmos.DrawLine(transform.position, transform.position + (directionXCamera*3f) );
		
		Gizmos.color = Color.red;
		
		Gizmos.DrawSphere(hitDash.point, 1f);
	}
	


	public void ApplyPause()
	{
		inGame_panel.SetActive(false);
		pauseText.SetActive(true);
		sd.StopSound(sd.footstepsFmod);
		Time.timeScale = 0f;
	}

	public void UndoPause(float previousTimeScale)
	{
		pauseText.SetActive(false);
		ui_manager.Unpause();
		Time.timeScale = previousTimeScale;
	}

	public void AddSuiveurs(int nb)
	{
		for (int i = 0; i < nb; i++)
		{
			if ( detectParcours.suiveurs.Count < detectParcours.suiveursLimit)
			{
				GameObject go = Instantiate(suiveur, transform.position, transform.rotation);
				var pos = transform.position;
				pos.z -= 10;
				go.transform.position = pos;
				go.GetComponent<SuiveursManager>().player = player;
				go.GetComponent<SuiveursManager>().isPicked = true;
				go.transform.GetChild(2).gameObject.SetActive(false);
			
				detectParcours.suiveurs.Add(go);

				go.gameObject.layer = 13;
				go.gameObject.transform.GetChild(1).gameObject.layer = 13;
			}
			else
			{
				break;
			}
		}
	}

	
	public  IEnumerator fixDeMerde()
	{
		canNextStepCam = true;
		CameraController.currentCamPoint++;
		
		for (float f = 0;  f < 1f; f+= Time.deltaTime)
		{
			yield return 0;
		}

		canNextStepCam = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Recoltable" && !other.GetComponent<SuiveursManager>().isPicked)
		{
			
			detectParcours.testCollision(other);
		}

		if (other.gameObject.tag == "CamFollowShort")
		{
			
			CameraController.beforeArena = true;
		}

		if (other.gameObject.tag == "CamTrigger" && !canNextStepCam)
		{
			
			StartCoroutine(fixDeMerde());

			other.gameObject.SetActive(false); // solution temporaire au flicking de merde 

		}
		
		
		if (other.gameObject.tag == "DoorTrigger")
		{
			other.gameObject.transform.parent.GetChild(1).GetComponent<DoorBehavior>().isDoorTriggered = true;
			other.gameObject.transform.parent.GetChild(1).GetComponent<NavMeshObstacle>().carving = true;
			other.gameObject.transform.parent.GetChild(1).GetComponent<NavMeshObstacle>().size = new Vector3(1,1,1);
			camControl.isOffsetActif = true;
		}
		
		if (other.gameObject.tag == "Water")
		{
			sd.footstepsTextureParameter.setValue(1);
		}

		if (other.gameObject.tag == "EndTrigger")
		{
			viAndD.text = "Village Cleared";
			SetEndGame();
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "StartTrigger")
		{
			CharacterInputDetector.canStartArena = true;
			cursor_sign.SetActive(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Water")
		{
			sd.footstepsTextureParameter.setValue(0);
		}
		
		if (other.tag == "StartTrigger")
		{
			cursor_sign.SetActive(false);
		}
	}

	private void FixedUpdate()
	{
		CharacterInputDetector.canStartArena = false;
	}

	private void OnCollisionEnter(Collision other)
	{

		if (other.gameObject.tag == "Fonceur")
		{
			
			LooseHp();
			Vector3 vectorForce = other.contacts[0].normal * 10f;
			//other.gameObject.GetComponent<Rigidbody>().AddForce(-vectorForce, ForceMode.VelocityChange);
			other.gameObject.GetComponent<NavMeshAgent>().velocity = -vectorForce/2;
			GetComponent<Rigidbody>().AddForce(vectorForce, ForceMode.VelocityChange);
			
			//other.gameObject.GetComponent<DestroyBehavior>().SelfDestruct();
			var ps = impact_enemy_feedback.GetComponent<ParticleSystem>().main;
			ps.startColor = Color.red;
			var go = Instantiate(impact_enemy_feedback, transform.position, Quaternion.identity);
			//manager.GetComponent<spawnManager>().ennemyCount--;
		}
		
		if (other.gameObject.tag == "Tracer")
		{
			LooseHp();
			Vector3 vectorForce = other.contacts[0].normal * 10f;
			//other.gameObject.GetComponent<Rigidbody>().AddForce(-vectorForce, ForceMode.VelocityChange);
			other.gameObject.GetComponent<NavMeshAgent>().velocity = -vectorForce/2;
			GetComponent<Rigidbody>().AddForce(vectorForce, ForceMode.VelocityChange);
			
			//other.gameObject.GetComponent<DestroyBehavior>().SelfDestruct();
			var ps = impact_enemy_feedback.GetComponent<ParticleSystem>().main;
			ps.startColor = Color.red;
			var go = Instantiate(impact_enemy_feedback, transform.position, Quaternion.identity);
			//manager.GetComponent<spawnManager>().ennemyCount--;
		}
	}

	public void AlignWithGround()
	{
		RaycastHit rcHit;
		Vector3 theRay = transform.TransformDirection(Vector3.down);

		if (Physics.Raycast(transform.position, theRay, out rcHit,25f,  layerSol  ))
		{
			GroundDis = rcHit.distance;
			Quaternion grndTilt = Quaternion.FromToRotation(Vector3.up, rcHit.normal);
			
			grndTilt *= Quaternion.Euler(0,transform.rotation.eulerAngles.y,0);
			smoothTilt = Quaternion.Lerp(smoothTilt, grndTilt, Time.deltaTime * 2f);

			Quaternion newRot = new Quaternion();
			Vector3 vec = new Vector3();
			vec.x = smoothTilt.eulerAngles.x;
			vec.y = transform.rotation.eulerAngles.y;
			vec.z = smoothTilt.eulerAngles.z;
			newRot.eulerAngles = vec;

			transform.rotation = newRot;
			
			//distance = rcHit.distance;


			Vector3 estimatedPosition;


			estimatedPosition = ((transform.position - rcHit.point).normalized * minimalDistance) + rcHit.point;

			estimatedPosition.x = transform.position.x;
			estimatedPosition.z = transform.position.z;
		
			transform.position = Vector3.Lerp(transform.position, estimatedPosition, Time.deltaTime*20f);
		}
		else
		{
			Debug.Log("noWorking");
		}
	}
	
		public IEnumerator ResetDashCD(float waitTime)
		{
			yield return new WaitForSeconds(waitTime);
			canDash = false;
			playerMesh.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = mat_avatar;
			playerMesh.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Renderer>().material = mat_cloth;
		}
	
	public void LooseHp()
	{
		
		if(hp > 0)
		{
			sd.PlayDamageTakenSound();
			sc.BreakChain();
			pp_damage_taken.GetComponent<Animator>().Play("PP_DamageTaken_FadeOut");
			hp--;
			playerHpText.text = "HP : " + hp;
			
			if(hp >= 1)
			{
				
				GameObject go = Instantiate(suiveur, transform.position, transform.rotation);
				var pos = transform.position;
				pos.z -= 2;
				go.transform.position = pos;
				go.GetComponent<SuiveursManager>().player = player;
				go.GetComponent<SuiveursManager>().isPicked = true;
				go.transform.GetChild(2).gameObject.SetActive(false);
			
				detectParcours.suiveurs.Add(go);

				go.gameObject.layer = 13;
				go.gameObject.transform.GetChild(1).gameObject.layer = 13;
				
				destructiveWave = Instantiate(ps_destructive_wave, transform);
				destructiveWave.transform.parent = player.transform;
			}
			
			StartCoroutine(TookAHit());
			
			manager.GetComponent<spawnManager>().checkPvCount = 0;
		
			StartCoroutine(CheckHP());
		}
	}

	public void GainHp()
	{
		if (hp < 3)
		{
			ps_gainHP.GetComponent<ParticleSystem>().Play();
		}
		
		hp++;
		
		if(hp > gm.hp)
		{
			hp = gm.hp;
		}

		

		StartCoroutine(CheckHP());
		playerHpText.text = "HP : " + hp;
	
		
	}


	public void SetEndGame()
	{
		isArenaStarted = false;
		sc.timeArena = sc.surviveTimer;
		sc.ActiveInGamePanel(false);
		sd.windFadingInSolo = true;


		XtraLifeManager xtraLife = GameObject.Find("Manager").GetComponent<XtraLifeManager>();


		float newScore = xtraLife.CompareScores(sc.score);
		float newTime = xtraLife.CompareTimes(sc.surviveTimer);



		if(newTime >= sc.surviveTimer)
        {
			sc.txt_bestTime.text = sc.FormatTime(sc.surviveTimer);
		}
		else
        {
			sc.txt_bestTime.text = sc.FormatTime(newTime);
		}

		if (newScore < sc.score)
		{
			sc.txt_highscore.text = sc.score.ToString();
		}
		else
		{
			sc.txt_highscore.text = newScore.ToString();
		}


		sd.StopSound(sd.footstepsFmod);
		ps_HP1.transform.parent.gameObject.GetComponent<ParticleSystem>().Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
				
		sd.StopAllSounds(false);
		sc.FinalScore();
		
		manager.GetComponent<GameManager>().enabled = false;
		playerMesh.transform.GetChild(0).GetComponent<Animator>().enabled = false;
		gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

		Time.timeScale = 0f;
		//SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	
	

	public IEnumerator CheckHP()
	{

		if (hp == 3 && previousHp == 2)
		{
			previousHp = 3;
			ps_HP1.GetComponent<ParticleSystem>().Play();
		}
		else if (hp == 2 && previousHp == 1)
		{
			previousHp = 2;
			ps_HP2.GetComponent<ParticleSystem>().Play();
			
		}
		else if (hp == 2 && previousHp == 3 )
		{
			previousHp = 2;
			ps_HP1.GetComponent<ParticleSystem>().Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
			ps_HP1.transform.GetChild(0).gameObject.SetActive(true);
		}
		else if (hp == 1 && previousHp == 2)
		{
			previousHp = 1;
			ps_HP2.GetComponent<ParticleSystem>().Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
			ps_HP2.transform.GetChild(0).gameObject.SetActive(true);
		}
		else if (hp == 0 && previousHp == 1)
		{
			ps_HP3.GetComponent<ParticleSystem>().Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
			ps_HP3.transform.GetChild(0).gameObject.SetActive(true);
		}
		
		if (hp <= 0)
		{
			isArenaStarted = false;
			viAndD.text = "Defeat";
			sc.timeArena = sc.surviveTimer;
			sc.ActiveInGamePanel(false);
			sd.windFadingInSolo = true;



			ui_manager.PanelSwitch.SetActive(false);


			dying = true;
			sd.StopSound(sd.footstepsFmod);
			sd.PlayDeathSound();
			ps_HP1.transform.parent.gameObject.GetComponent<ParticleSystem>().Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);

			player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			playerMesh.transform.GetChild(0).GetComponent<AnimationManager>().PlayDeathAnimation();
			playerMesh.transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(false);
			playerMesh.transform.GetChild(0).transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(true);
			Camera.main.transform.GetChild(2).gameObject.SetActive(true);

			for (int i = 0; i < viseur_dash.transform.childCount; i++)
			{
				viseur_dash.transform.GetChild(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
				viseur_dash.transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
			}
			
			for (int i = 0; i < viseur_offensif.transform.childCount; i++)
			{
				viseur_offensif.transform.GetChild(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
				viseur_offensif.transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
			}
			
			for (int i = 0; i < detectParcours.suiveurs.Count; i++)
			{
				if (detectParcours.suiveurs[i] != null)
				{
					detectParcours.suiveurs[i].GetComponent<SuiveursManager>().ModulesFalling();
					detectParcours.suiveurs[i].layer = 21;
					detectParcours.suiveurs[i].GetComponent<SuiveursManager>().enabled = false;
				}
			}
			
			yield return new WaitForSecondsRealtime(5);
			sd.StopAllSounds(false);
			sc.FinalScore();
			
			
			manager.GetComponent<GameManager>().enabled = false;
			//SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
	
	public IEnumerator TookAHit()
	{
	
		Time.timeScale = 0.25f;

			for (float f = 0; f < timeInvincibility / 2; f += Time.deltaTime)
			{

				isInvincible = true;
				gameObject.layer = 24;

				playerMesh.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = mat_avatar_invincible;
				playerMesh.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Renderer>().material =
					mat_cloth_invincible;

				
				yield return 0;
			}

			Time.timeScale = 1;
			isInvincible = false;
			Destroy(destructiveWave);
			playerMesh.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = mat_avatar;
			playerMesh.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Renderer>().material = mat_cloth;

			gameObject.layer = 9;
	}
		
	}

		
	