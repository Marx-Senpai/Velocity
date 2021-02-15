using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	[Header("Inputs Joystick Droit")]
    public string inputDroitHorizontal;
	public string inputDroitVertical;

	[Header("Inputs Joystick Gauche")]
	public string inputGaucheHorizontal;
	public string inputGaucheVertical;
	
	[Header("Inputs Pad Directionnel")]
	public string inputPadHorizontal;
	public string inputPadVertical;
	
	[Header("Inputs Joueur")]
	public string inputAccel;
	public string inputDecel;
	public string inputAccelDriftDroit;
	public string inputAccelDriftGauche;
	public string inputAbility;
	public string inputPause;
	public string inputSuiveurBase;
	public string inputSuiveurAiguille;
	public string inputSuiveurRateau;
	public string inputCristallisation;


	[Header("Cheat input Settings")] 
	public string inputSuiveurUp;
	
	[Header("Global Settings")] 
	public bool isMouseInputActive;

	public bool IsCheatActive;
	public bool isBossActive;


	[Header("Player Settings")]
	public float moveSpeed;
	public int hp;
	public float timeInvincible;
	public float timerAddSuiveur;
	
	[Header("Camera Settings")]
	public float camDistanceZ;
	public float camDistanceY;
	public float camAdjustSpeed;
	[Tooltip("La distance par rapport au centre de l'arène en Z où commence la montée de la caméra pour ne pas clipper dans les murs")]
	public float distanceZStartOffsetWall;
	[Tooltip("Le pourcentage de la distance est attribué dans un offset en Y et en Z de la caméra")]
	public float multiplierDistanceZForOffset;
	public float smoothSpeedHorsArene;
	
	[Header("Followers Settings")]
	public float suiveursMoveSpeed;
	public float slerpSuiviDirectionMultiplier;
	public float cdDeathAlone;

	[Header("Formation Settings")] 
	public float distanceFormation;
	public float timeLerpSmoothMultiplier;
	public int suiveursLimit;
	public float packRotateSpeed;
	public float shootSpeed;
	
	[Header("ObstacleStatic Settings")] 
	public int hpS;
	
	[Header("Waves Settings")] 
	public spawnManager.SpawnMode spawn = spawnManager.SpawnMode.PressingButton;
	public float randomRadius;

	public int spawningPoolSize;
	public float spawnTimerLimit;
	public float spawnInterval;
	public int ennemyMin;
	public int nbrWaveRequiredForLife;
	public bool deathAloneIsDelayed;
	public float deathDelay;
	
	[Header("Ennemies Settings")] 
	public bool spawnSuiveurOnDeath;
	
	
	
	
	
	public GameObject[] followers;

	public GameObject[] spawns;

	public GameObject[] pointsFormations;
	
	public GameObject[] movingObstacles;
	
	public GameObject[] staticObstacles;

	public List<GameObject> allMurs;
	

	public GameObject player;

	public GameObject cam;

	public GameObject formation;



	public int skippedFrame;


	public bool mustReload;
	
	
	
	// variables components

	private CharacterControllerManager charaManager;

	


	void Awake ()
	{
		allMurs = new List<GameObject>();
		charaManager = player.GetComponent<CharacterControllerManager>();
		
		player.GetComponent<CharacterInputDetector> ().inputGaucheHorizontal = this.inputGaucheHorizontal;
		player.GetComponent<CharacterInputDetector> ().inputGaucheVertical = this.inputGaucheVertical;
		
		player.GetComponent<CharacterInputDetector> ().inputDroitHorizontal = this.inputDroitHorizontal;
		player.GetComponent<CharacterInputDetector> ().inputDroitVertical = this.inputDroitVertical;
		
		player.GetComponent<CharacterInputDetector> ().inputPadHorizontal = this.inputPadHorizontal;
		player.GetComponent<CharacterInputDetector> ().inputPadVertical = this.inputPadVertical;
		
		player.GetComponent<CharacterInputDetector> ().inputAccelerer = this.inputAccel;
		player.GetComponent<CharacterInputDetector> ().inputDecelerer = this.inputDecel;
		
		player.GetComponent<CharacterInputDetector> ().inputAbility = this.inputAbility;

		player.GetComponent<CharacterInputDetector>().inputPause = this.inputPause;

		player.GetComponent<CharacterInputDetector>().inputSuiveurUp = this.inputSuiveurUp;
		
		player.GetComponent<CharacterInputDetector>().inputShield = this.inputSuiveurBase;
		player.GetComponent<CharacterInputDetector>().inputLance = this.inputSuiveurAiguille;
		player.GetComponent<CharacterInputDetector>().inputDash = this.inputSuiveurRateau;

		player.GetComponent<CharacterInputDetector>().inputCristallisation = this.inputCristallisation;

		
		player.GetComponent<CharacterInputDetector>().isMouseActive = this.isMouseInputActive;
		
		player.GetComponent<CustomCharacterController>().distanceFormation = this.distanceFormation;
		player.GetComponent<CustomCharacterController>().moveSpeed = this.moveSpeed;
		player.GetComponent<CustomCharacterController>().hp = this.hp;
		player.GetComponent<CustomCharacterController>().previousHp = this.hp;
		player.GetComponent<CustomCharacterController>().packRotateSpeed = this.packRotateSpeed;
		player.GetComponent<CustomCharacterController>().timeInvincibility = this.timeInvincible;
		player.GetComponent<CustomCharacterController>().timerGimme = this.timerAddSuiveur;
		
		player.GetComponent<DetectionParcours>().suiveursLimit = this.suiveursLimit;

		GetComponent<spawnManager>().spawn = this.spawn;
		GetComponent<spawnManager>().spawnTimerLimit = this.spawnTimerLimit;
		GetComponent<spawnManager>().spawningPoolSize = this.spawningPoolSize;
		GetComponent<spawnManager>().ennemyMin = this.ennemyMin;
		GetComponent<spawnManager>().spawnTimerInterval = this.spawnInterval;
		GetComponent<spawnManager>().nbrWaveRegain = this.nbrWaveRequiredForLife;
		GetComponent<spawnManager>().deathAloneIsDelayed = this.deathAloneIsDelayed;
		GetComponent<spawnManager>().deathDelay = this.deathDelay;
		GetComponent<spawnManager>().Awaken();

		formation.GetComponent<FormationBehavior>().timeLerpSmoothMultiplier = this.timeLerpSmoothMultiplier;

       

		cam.GetComponent<Camera>().GetComponent<CameraInputDetector> ().inputDroitHorizontal = this.inputDroitHorizontal;
		cam.GetComponent<Camera>().GetComponent<CameraInputDetector> ().inputDroitVertical = this.inputDroitVertical;
		
		cam.GetComponent<Camera>().GetComponent<CameraController> ().camDistanceZ = this.camDistanceZ;
		cam.GetComponent<Camera>().GetComponent<CameraController> ().cameDistanceY = this.camDistanceY;
		cam.GetComponent<Camera>().GetComponent<CameraController> ().camAdjustSpeed = this.camAdjustSpeed;
		cam.GetComponent<Camera>().GetComponent<CameraController>().distanceZStartOffsetWall =
			this.distanceZStartOffsetWall;
		cam.GetComponent<Camera>().GetComponent<CameraController>().multiplierDistanceZForOffset =
			this.multiplierDistanceZForOffset;

		cam.GetComponent<Camera>().GetComponent<CameraController>().smoothSpeedHorsArene = this.smoothSpeedHorsArene;
		

		spawns = GameObject.FindGameObjectsWithTag("SpawnSuiveur");

		

	}


	public void Update()
	{
		
		
		if (skippedFrame >= 0)
		{
			pointsFormations = GameObject.FindGameObjectsWithTag("pointFormation");

		
			foreach (GameObject go in pointsFormations)
			{
				go.GetComponent<formationPointBehavior>().UpdatePositionFromGround();
			}

			skippedFrame = 0;
		}
		
		followers = GameObject.FindGameObjectsWithTag("Recoltable");
		
		

		for (int i = 0; i < followers.Length; i++)
		{
			followers[i].GetComponent<SuiveursManager>().shootSpeed = this.shootSpeed;
			followers[i].GetComponent<SuiveursManager>().slerpSuiviDirectionMultiplier =
				this.slerpSuiviDirectionMultiplier;
			followers[i].GetComponent<SuiveursManager>().cdDeathAlone = this.cdDeathAlone;
		}
		
		charaManager.CustomManagerControllerUpdate();
		
		/*spawns = GameObject.FindGameObjectsWithTag("SpawnSuiveur");

		for (int i = 0; i < spawns.Length; i++)
		{
			spawns[i].GetComponent<SpawnSuiveurBehavior>().CustomUpdateSpawnRecoltable();
		}*/
		
		
		//formation.GetComponent<FormationBehavior>().CustomFormationUpdate();

		skippedFrame++;

		if (mustReload)
		{
			Debug.Log("Reloading game...");
			Scene scene = SceneManager.GetActiveScene();
			SceneManager.LoadSceneAsync(scene.name);
			mustReload = false;
		}
	}

	private void FixedUpdate()
	{
		movingObstacles = GameObject.FindGameObjectsWithTag("MovingObstacle");
		
		for (int i = 0; i < movingObstacles.Length; i++)
		{
			movingObstacles[i].GetComponent<MovingPlatform>().CustomFixedUpdate();
		}
	}
}
