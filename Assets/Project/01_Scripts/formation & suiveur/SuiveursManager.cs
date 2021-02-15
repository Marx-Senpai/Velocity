using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using FMOD;
using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Debug = FMOD.Debug;

public class SuiveursManager : MonoBehaviour {
	
    public bool isPicked = false;
    public GameObject player;
    public float charaSpeed;

	private float distanceFromTarget;

	public bool[] pointsFormation;

	public System.Collections.Generic.List<GameObject> suiveurs;

	public GameObject targetPoint;

	public int indexPoint;

	public GameObject spawnAttached;

	public GameObject formation;

	private int numbSuiveurs;

	public float slerpSuiviDirectionMultiplier;
	public float suiveurMoveSpeed = 5f;

	public bool isShooted;

	public Vector3 shootDirection;
	public float shootSpeed = 10f;

	public bool isFromSpawn, isFromEnnemy;

	public float cdDeathAlone;
	public float timerDeathAlone;


	private GameObject mesh_base;
	private GameObject mesh_stance;
	
	[SerializeField] private GameObject ps_impact;
	[SerializeField] private GameObject ps_death;
	
	// components 

	private DetectionParcours detectP;
	private CustomCharacterController customChara;
	private CharacterInputDetector charaInput;
	private FormationBehavior formationBe;
	private Rigidbody rb;
	private ParticleSystem ps;
	private GameObject manager;
	private SoundManager sd;
	private BoxCollider boxCol;
	private SphereCollider sphereCol;
	
	private System.Collections.Generic.List<Transform> childs;


	private BossManager bossM;

	public GameObject boss;

	private void Awake()
	{
		manager = GameObject.Find("Manager");
	    
	    rb = GetComponent<Rigidbody>();
	    player = GameObject.Find("Player");
	    formation = GameObject.Find("Formation");
		boss = GameObject.Find("Boss");
	  
	    detectP = player.GetComponent<DetectionParcours>();
	    customChara = player.GetComponent<CustomCharacterController>();
	    charaInput = player.GetComponent<CharacterInputDetector>();
	    formationBe = formation.GetComponent<FormationBehavior>();
		bossM = boss.GetComponent<BossManager>();
	    ps = this.GetComponent<ParticleSystem>();
	    sd = manager.GetComponent<SoundManager>();
		sphereCol = GetComponent<SphereCollider>();
		boxCol = GetComponent<BoxCollider>();

		suiveurMoveSpeed = GameObject.Find("Manager").GetComponent<GameManager>().suiveursMoveSpeed;
	    
	    mesh_base = this.transform.GetChild(0).gameObject;
	    
	    childs = new System.Collections.Generic.List<Transform>();
	    
	    int count = transform.childCount-1;
			
	    childs.Clear();

		

	    for (int i = 0; i <= count; i++)
	    {
		    childs.Add(transform.GetChild(i));
	    }
    }

    void Update ()
    {

	    if (isFromEnnemy)
	    {
		    timerDeathAlone += Time.deltaTime;
		    var main = transform.GetComponent<ParticleSystem>().main;
		    main.loop = false;
		    main.simulationSpeed = 2;
		    
		    if (timerDeathAlone > 1)
		    {
			    transform.GetChild(0).GetComponent<Animator>().enabled = true;
			    transform.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
		    }

		    if (timerDeathAlone >= cdDeathAlone)
		    {
			    DieButNow();
		    }
	    }
	    
	    if (isShooted)
	    {
		    isPicked = false;
	    }

	    if (!isShooted && !isPicked && !isFromSpawn)
	    {
		   // timerDeathAlone += Time.deltaTime;
		    if (!transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying && !isFromEnnemy)
		    {
			    transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>().speed = 2;
		    }
		    
		    if (!transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying && isFromEnnemy)
		    {
			    transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>().speed = 2;
			    transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystem>().Play();
		    }
	    }
	  

	   /* if (timerDeathAlone >= cdDeathAlone)
	    {
		   Destroy(gameObject);
	    }*/
	    
	    if (isShooted)
	    {
		    ShootFly(shootDirection);
	    }
	    
	    numbSuiveurs = 0;
	    GameObject[] gosParent = GameObject.FindGameObjectsWithTag("Recoltable");

	    for (int i = 0; i < gosParent.Length; i++)
	    {
		    if (gosParent[i].gameObject.GetComponent<SuiveursManager>().isPicked)
		    {
			    numbSuiveurs++;
		    }
	    }

	 

		pointsFormation = player.GetComponent<CustomCharacterController>().formation;

	   // suiveurs = player.GetComponent<DetectionParcours>().suiveurs;

		if (isPicked && targetPoint == null /*&& !isFollowingIndienne*/)
		{
			StopAllCoroutines();
			transform.localScale = new Vector3(1,1,1);
			for (int i = 0; i < pointsFormation.Length; i++)
			{
				if ( pointsFormation[i] == false)
				{
					targetPoint = GameObject.Find("point" + i);
					transform.parent = null;
					gameObject.name = "suiveur" + i;
					pointsFormation[i] = true;
					indexPoint = i;
					
					if (charaInput.stance == CharacterInputDetector.stanceFormation.Shield)
					{
						gameObject.layer = 21;
					}
					else if (charaInput.stance == CharacterInputDetector.stanceFormation.Lance)
					{
						gameObject.layer = 22;
					}
					else if (charaInput.stance == CharacterInputDetector.stanceFormation.Dash)
					{
						gameObject.layer = 23;
					}
					
					break;
				}
			}
		}
		else if (isPicked && targetPoint != null)
		{
			if (!rb.IsSleeping())
			{
				rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * 2f);
			}
				float dottedDistance = Vector3.Dot(transform.forward.normalized,
					(targetPoint.transform.position - transform.position).normalized);

				distanceFromTarget = Vector3.Distance(targetPoint.transform.position, transform.position);

				transform.forward = Vector3.Slerp(transform.forward, (targetPoint.transform.position - transform.position),    Time.deltaTime * slerpSuiviDirectionMultiplier );

				Vector3 motion = transform.forward;

				motion = Vector3.ClampMagnitude(motion, distanceFromTarget);
				//GetComponent<CharacterController>().Move(motion);

			//transform.position = Vector3.MoveTowards(transform.position, (transform.position + motion), Time.deltaTime * suiveurMoveSpeed);

			transform.position = Vector3.MoveTowards(transform.position, targetPoint.transform.position, Time.deltaTime * suiveurMoveSpeed);
			
				transform.forward = (transform.position - player.transform.position).normalized;
		

		}

	    if (isPicked)
	    {
		    ps.Stop();
		    //this.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
		    this.transform.GetChild(4).gameObject.GetComponent<ParticleSystem>().Stop();
		    
		    charaInput = player.GetComponent<CharacterInputDetector>();
		    mesh_base.SetActive(false);
		    
		    if (charaInput.stance == CharacterInputDetector.stanceFormation.Shield)
		    {
			    mesh_stance = this.transform.GetChild(3).gameObject;
		    }
		    else if (charaInput.stance == CharacterInputDetector.stanceFormation.Lance)
		    {
			    mesh_stance = this.transform.GetChild(1).gameObject;
		    }
		    else if (charaInput.stance == CharacterInputDetector.stanceFormation.Dash)
		    {
			    mesh_stance = this.transform.GetChild(2).gameObject;
		    }

		    for (int i = 0; i < this.transform.childCount; i++)
		    {
			    this.transform.GetChild(i).gameObject.SetActive(false);
		    }
		    mesh_stance.SetActive(true);
		    
		    if (charaInput.stance == CharacterInputDetector.stanceFormation.Shield)
		    {
			   // sphereCol.enabled = true;
			   // boxCol.enabled = true;
			    mesh_stance = this.transform.GetChild(3).gameObject;
			    
		    }
		    else if (charaInput.stance == CharacterInputDetector.stanceFormation.Lance)
		    {
			   // sphereCol.enabled = false;
			  //  boxCol.enabled = false;
			    mesh_stance = this.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
			   
		    }
		    else if (charaInput.stance == CharacterInputDetector.stanceFormation.Dash)
		    {
			  //sphereCol.enabled = false;
			  //  boxCol.enabled = false;
			    mesh_stance = this.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
			
		    }
		    
		    mesh_stance.gameObject.GetComponent<Renderer>().material.color = charaInput.stance_color;
	    }	   
    }

    private void OnDrawGizmos()
	{
		Gizmos.color  = Color.green;
		if (targetPoint != null)
		{
			Gizmos.DrawLine(transform.position, targetPoint.transform.position);
		}

		/*Gizmos.color = Color.black;
		
		Gizmos.DrawWireSphere(transform.position, 1f);*/
	}

	public void CustomCollision(Collision other)
	{
		if (other.gameObject.CompareTag("Recoltable") )
		{
			
			player.GetComponent<DetectionParcours>().testCollision(other.collider);
		
		}
	
	}

	public void DetachAndDestroy()
	{
		
		player.GetComponent<CustomCharacterController>().formation[indexPoint] = false;
		pointsFormation[indexPoint] = false;
		player.GetComponent<DetectionParcours>().suiveurs.Remove(this.gameObject);
		
		
		if (!formation.GetComponent<FormationBehavior>().isHitted)
		{
			formation.GetComponent<FormationBehavior>().isHitted = true;
			StartCoroutine(formation.GetComponent<FormationBehavior>().Reforme());
		}
		else
		{
			formation.GetComponent<FormationBehavior>().isHittedRefresh = true;
		}

		Instantiate(ps_death, this.gameObject.transform.position, Quaternion.identity);
		Destroy(this.gameObject);
	}
	
	
	

	public void ShootFly(Vector3 direction)
	{
		gameObject.layer = 14;
		transform.position = Vector3.MoveTowards(transform.position, (transform.position + direction), Time.deltaTime * shootSpeed);
	//	GetComponent<CharacterController>().Move(direction * shootSpeed);
	}

	

	private void OnCollisionEnter(Collision other)
	{

		if (isShooted && !other.gameObject.CompareTag("Recoltable"))
		{
			Instantiate(ps_impact, other.contacts[0].point, Quaternion.identity);
		}
		
			if (other.gameObject.CompareTag("Recoltable") && !other.gameObject.GetComponent<SuiveursManager>().isPicked)
			{
				if (isShooted)
				{
					transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
					//isShooted = false;
					isPicked = false;
				}
				
				player.GetComponent<DetectionParcours>().testCollision(other.collider);
			}

			if (other.gameObject.tag == "MovingObstacle")
			{
				other.gameObject.GetComponent<MovingPlatform>().pv--;
				DetachAndDestroy();
			}

			if (other.gameObject.tag == "Obstacle" && isShooted)
			{
				DetachAndDestroy();
			}

			if (other.gameObject.CompareTag("Bullet"))
			{

				if (charaInput.stance == CharacterInputDetector.stanceFormation.Shield )
				{
					//Destroy(other.gameObject);
					sd.PlayReflectSound(this.gameObject);
					other.transform.forward = transform.GetChild(4).transform.forward;
					
					Vector3 tempVY = transform.position;
					tempVY.x = other.transform.position.x;
					tempVY.z = other.transform.position.z;

					other.transform.position = tempVY;

					if (other.gameObject.GetComponent<Bullet>().matReflectTrail != null)
					{
						other.gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystemRenderer>().material =
							other.gameObject.GetComponent<Bullet>().matReflect;
					
						
						other.gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystemRenderer>().trailMaterial = other.gameObject.GetComponent<Bullet>().matReflectTrail;
						
					}
					
					
					Vector3 tempVEuler = other.transform.rotation.eulerAngles;
					tempVEuler.x = 0;
					other.transform.rotation = Quaternion.Euler(tempVEuler);
					
					other.gameObject.layer = 14; // tirOut
					other.gameObject.GetComponent<Bullet>().isReflected = true;
					other.gameObject.GetComponent<Bullet>().BulletGo();
					DetachAndDestroy();
				}
				else
				{
					DetachAndDestroy();
				}

			}


			if (other.gameObject.CompareTag("Infanterie"))
			{
				
			
				if (charaInput.stance == CharacterInputDetector.stanceFormation.Shield )
				{
					
					Vector3 vectorForce = other.contacts[0].normal * 10f;
					//other.gameObject.GetComponent<Rigidbody>().AddForce(-vectorForce, ForceMode.VelocityChange);
					other.gameObject.transform.parent.gameObject.GetComponent<NavMeshAgent>().velocity = -vectorForce/2;
					player.GetComponent<Rigidbody>().AddForce(vectorForce, ForceMode.VelocityChange);
					//DetachAndDestroy();
				}
				else
				{
					
					if (charaInput.stance == CharacterInputDetector.stanceFormation.Lance)
					{
						if (!isShooted)
						{
							DetachAndDestroy();
						}
						else
						{
							other.gameObject.GetComponent<DestroyBehavior>().SelfDestruct(other.contacts[0].point);
							DetachAndDestroy();
						}
					}
					
						//GameObject.Find("Manager").GetComponent<spawnManager>().ennemyCount--;
			
				}
			}
		
		if (other.gameObject.CompareTag("WidowMaker"))
		{
			//Debug.Log(other.gameObject.name);
			if (charaInput.stance == CharacterInputDetector.stanceFormation.Shield )
			{
					
				Vector3 vectorForce = other.contacts[0].normal * 10f;
				//other.gameObject.GetComponent<Rigidbody>().AddForce(-vectorForce, ForceMode.VelocityChange);
				other.gameObject.transform.parent.transform.parent.gameObject.GetComponent<NavMeshAgent>().velocity = -vectorForce/2;
				player.GetComponent<Rigidbody>().AddForce(vectorForce, ForceMode.VelocityChange);
				//DetachAndDestroy();
			}
			else
			{
					
					
				//GameObject.Find("Manager").GetComponent<spawnManager>().ennemyCount--;
				
				if (charaInput.stance == CharacterInputDetector.stanceFormation.Lance)
				{
					if (!isShooted)
					{
						DetachAndDestroy();
					}
					else
					{
						other.gameObject.GetComponent<DestroyBehavior>().SelfDestruct(other.contacts[0].point);
						DetachAndDestroy();
					}
				}
					
					
			}
		}



			if (other.gameObject.CompareTag("Tracer"))
			{
				
				if (charaInput.stance == CharacterInputDetector.stanceFormation.Shield )
				{
					
					Vector3 vectorForce = other.contacts[0].normal * 10f;
					//other.gameObject.GetComponent<Rigidbody>().AddForce(-vectorForce, ForceMode.VelocityChange);
					other.gameObject.GetComponent<NavMeshAgent>().velocity = -vectorForce/2;
					player.GetComponent<Rigidbody>().AddForce(vectorForce, ForceMode.VelocityChange);
					//DetachAndDestroy();
				}
				else
				{
					/*var i = other.gameObject.GetComponent<Blackboard>().GetVariable("Pv", typeof(int));
					int ii = (int) i.value;
					other.gameObject.GetComponent<Blackboard>().SetValue("Pv", ii - 1);

					if ((int) other.gameObject.GetComponent<Blackboard>().GetVariable("Pv", typeof(int)).value <= 0)
					{
						//GameObject.Find("Manager").GetComponent<spawnManager>().ennemyCount--;
						other.gameObject.GetComponent<DestroyBehavior>().SelfDestruct(other.contacts[0].point);
					}*/
					
					
					if (charaInput.stance == CharacterInputDetector.stanceFormation.Lance)
					{
						if (!isShooted)
						{
							DetachAndDestroy();
						}
						else
						{
							other.gameObject.GetComponent<DestroyBehavior>().SelfDestruct(other.contacts[0].point);
							DetachAndDestroy();
						}
					}

					
				}

			}
		
		if (other.gameObject.CompareTag("BossMur"))
		{
				
			if (charaInput.stance == CharacterInputDetector.stanceFormation.Shield )
			{
					
				Vector3 vectorForce = other.contacts[0].normal * 10f;
				//other.gameObject.GetComponent<Rigidbody>().AddForce(-vectorForce, ForceMode.VelocityChange);
			//	other.gameObject.GetComponent<NavMeshAgent>().velocity = -vectorForce/2;
				player.GetComponent<Rigidbody>().AddForce(vectorForce, ForceMode.VelocityChange);
				DetachAndDestroy();
			}
			else
			{
				DetachAndDestroy();
				
			}

		}
		
		if ( other.gameObject.CompareTag("BossSpread") || other.gameObject.CompareTag("BossStandard"))
		{
				
			/*if (charaInput.stance == CharacterInputDetector.stanceFormation.Shield )
			{
					
				Vector3 vectorForce = other.contacts[0].normal * 10f;
				//other.gameObject.GetComponent<Rigidbody>().AddForce(-vectorForce, ForceMode.VelocityChange);
					//other.gameObject.GetComponent<NavMeshAgent>().velocity = -vectorForce/2;
				player.GetComponent<Rigidbody>().AddForce(vectorForce, ForceMode.VelocityChange);
				DetachAndDestroy();
			}
			else
			{*/
				other.gameObject.SetActive(false);
				other.gameObject.transform.parent.gameObject.SetActive(false);
				bossM.PartiesHitted.Add(other.gameObject);
				bossM.PartiesHitted.Add(other.gameObject.transform.parent.gameObject);
				DetachAndDestroy();
				
			//}

		}
		
		if (other.gameObject.CompareTag("BossBlindage") )
		{
				
			/*if (charaInput.stance == CharacterInputDetector.stanceFormation.Shield )
			{
					
				Vector3 vectorForce = other.contacts[0].normal * 10f;
				//other.gameObject.GetComponent<Rigidbody>().AddForce(-vectorForce, ForceMode.VelocityChange);
				//other.gameObject.GetComponent<NavMeshAgent>().velocity = -vectorForce/2;
				player.GetComponent<Rigidbody>().AddForce(vectorForce, ForceMode.VelocityChange);
				DetachAndDestroy();
			}
			else
			{*/
				other.gameObject.GetComponent<ShieldBossBehavior>().SelfDestruct(other.contacts[0].point);
				bossM.PartiesHitted.Add(other.gameObject);
				DetachAndDestroy();
				
		//	}

		}
		
		if (other.gameObject.CompareTag("BossHeart") )
		{
				
			/*if (charaInput.stance == CharacterInputDetector.stanceFormation.Shield )
			{
					
				Vector3 vectorForce = other.contacts[0].normal * 10f;
				//other.gameObject.GetComponent<Rigidbody>().AddForce(-vectorForce, ForceMode.VelocityChange);
				//other.gameObject.GetComponent<NavMeshAgent>().velocity = -vectorForce/2;
				player.GetComponent<Rigidbody>().AddForce(vectorForce, ForceMode.VelocityChange);
				DetachAndDestroy();
			}
			else
			{*/
				other.gameObject.SetActive(false);
				//bossM.PartiesHitted.Add(other.gameObject);
				
				bossM.SingleHeartDestroy();
				DetachAndDestroy();	
			//}

		}

			if (other.gameObject.CompareTag("Fonceur"))
			{
				
				if (charaInput.stance == CharacterInputDetector.stanceFormation.Shield )
				{
					
					Vector3 vectorForce = other.contacts[0].normal * 10f;
					//other.gameObject.GetComponent<Rigidbody>().AddForce(-vectorForce, ForceMode.VelocityChange);
					other.gameObject.GetComponent<NavMeshAgent>().velocity = -vectorForce/2;
					player.GetComponent<Rigidbody>().AddForce(vectorForce, ForceMode.VelocityChange);
					//DetachAndDestroy();
				}
				else
				{
					/*var i = other.gameObject.GetComponent<Blackboard>().GetVariable("Pv", typeof(int));
					int ii = (int) i.value;
					other.gameObject.GetComponent<Blackboard>().SetValue("Pv", ii - 1);

					if ((int) other.gameObject.GetComponent<Blackboard>().GetVariable("Pv", typeof(int)).value <= 0)
					{
						//	GameObject.Find("Manager").GetComponent<spawnManager>().ennemyCount--;
						other.gameObject.GetComponent<DestroyBehavior>().SelfDestruct(other.contacts[0].point);
					}

					DetachAndDestroy();*/
					
					if (charaInput.stance == CharacterInputDetector.stanceFormation.Lance)
					{
						if (!isShooted)
						{
							DetachAndDestroy();
						}
						else
						{
							other.gameObject.GetComponent<DestroyBehavior>().SelfDestruct(other.contacts[0].point);
							DetachAndDestroy();
						}
					}
				}
				
			}
	}

	public void ModulesFalling()
	{
		rb.constraints = RigidbodyConstraints.None;
		rb.useGravity = true;
	}

	public IEnumerator DieButWithADelay( float delay)
	{
		transform.GetChild(0).GetComponent<Animator>().enabled = true;
		transform.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
		
		for (float f = 0; f < delay; f += Time.deltaTime)
		{
			yield return 0;
		}

		if (!isPicked)
		{
			Destroy(gameObject);
		}
		else
		{
			yield break;
		}
		
		
		
	}


	public void DieButNow()
	{
		Destroy(gameObject);
	}
}
