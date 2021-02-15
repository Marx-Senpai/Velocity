using NodeCanvas.Framework;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    private float speed = 50f;
    private Vector3 dir;
    private bool isSeekingBullet = false;
	public bool isReflected;

	public Material matReflect;
	public Material matReflectTrail;

    private GameObject player;

    private CharacterInputDetector charaInput;
    
    [SerializeField] private GameObject ps_impact;

	private Rigidbody rb;

	public float moveSpeed;

	public float moveSpeedDeflected;

	public float scaleMultiplier;

	private Vector3 baseScale;


	private BossManager bossM;

    private void Awake()
    {
	    rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        charaInput = player.GetComponent<CharacterInputDetector>();
	    BulletGo();

	    baseScale = transform.localScale;

	    bossM = GameObject.Find("Boss").GetComponent<BossManager>();

    }

	

	private void OnCollisionEnter(Collision other)
    {
	    if (other.gameObject.CompareTag("Bullet") && !other.gameObject.GetComponent<Bullet>().isReflected)
	    {
		    Destroy(other.gameObject);
		 
	    }
	    
	    if (other.gameObject.tag == "Player")
	    {
		   
		    Destroy(gameObject);
		    other.gameObject.GetComponent<CustomCharacterController>().LooseHp();
			
	    }

	    if (other.gameObject.tag != "Recoltable")
        {
            Instantiate(ps_impact, other.contacts[0].point, Quaternion.identity);
        }

	    if (other.gameObject.tag == "Obstacle")
	    {
		    if (other.gameObject.GetComponent<MurSuiveurBehavior>())
		    {
			    other.gameObject.GetComponent<MurSuiveurBehavior>().NbrShoot--;
		    }
		    
		    Destroy(gameObject);
	    }



	    if (other.gameObject.CompareTag("WidowMaker"))
        {
	        
                //GameObject.Find("Manager").GetComponent<spawnManager>().ennemyCount--;
					
                other.gameObject.GetComponent<DestroyBehavior>().SelfDestruct(other.contacts[0].point);
                Destroy(this.gameObject);
	  
        }
        
        if (other.gameObject.CompareTag("Tracer"))
			{
				
					var i = other.gameObject.GetComponent<Blackboard>().GetVariable("Pv", typeof(int));
					int ii = (int) i.value;
					other.gameObject.GetComponent<Blackboard>().SetValue("Pv", ii - 1);

					if ((int) other.gameObject.GetComponent<Blackboard>().GetVariable("Pv", typeof(int)).value <= 0)
					{
						//GameObject.Find("Manager").GetComponent<spawnManager>().ennemyCount--;
						other.gameObject.GetComponent<DestroyBehavior>().SelfDestruct(other.contacts[0].point);
					}

				Destroy(this.gameObject);

			}

			if (other.gameObject.CompareTag("Fonceur"))
			{
				
				
					var i = other.gameObject.GetComponent<Blackboard>().GetVariable("Pv", typeof(int));
					int ii = (int) i.value;
					other.gameObject.GetComponent<Blackboard>().SetValue("Pv", ii - 1);

					if ((int) other.gameObject.GetComponent<Blackboard>().GetVariable("Pv", typeof(int)).value <= 0)
					{
						//	GameObject.Find("Manager").GetComponent<spawnManager>().ennemyCount--;
						other.gameObject.GetComponent<DestroyBehavior>().SelfDestruct(other.contacts[0].point);
					}

					Destroy(this.gameObject);
				
				
			}
	    
	    
	    if (other.gameObject.CompareTag("BossMur"))
		{
				
			
				Destroy(this.gameObject);
				
			

		}
		
		if ( other.gameObject.CompareTag("BossSpread") || other.gameObject.CompareTag("BossStandard"))
		{
				
			
				other.gameObject.SetActive(false);
				other.gameObject.transform.parent.gameObject.SetActive(false);
				bossM.PartiesHitted.Add(other.gameObject);
				bossM.PartiesHitted.Add(other.gameObject.transform.parent.gameObject);
				Destroy(this.gameObject);
				
			

		}
		
		if (other.gameObject.CompareTag("BossBlindage") )
		{
			
				other.gameObject.GetComponent<ShieldBossBehavior>().SelfDestruct(other.contacts[0].point);
				bossM.PartiesHitted.Add(other.gameObject);
				Destroy(this.gameObject);
				
			

		}
		
		if (other.gameObject.CompareTag("BossHeart") )
		{
				
			
				other.gameObject.SetActive(false);
				bossM.PartiesHitted.Add(other.gameObject);
				
				bossM.SingleHeartDestroy();
				Destroy(this.gameObject);
			

		}
	    
	    if (other.gameObject.CompareTag("Infanterie"))
	    {
		   
					
					
			    //GameObject.Find("Manager").GetComponent<spawnManager>().ennemyCount--;
					
			    other.gameObject.GetComponent<DestroyBehavior>().SelfDestruct(other.contacts[0].point);
			    Destroy(this.gameObject);
					
	    }
        
        
    }


	public void BulletGo()
	{
		Vector3 vel = new Vector3();
		
		if (isReflected)
		{
			transform.forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
			transform.localScale = baseScale * scaleMultiplier;
			vel = transform.forward * moveSpeedDeflected;
		}
		else
		{
			 vel = transform.forward * moveSpeed;
		}
		

		rb.velocity = vel;
		
	}


	private void Update()
	{
		if (transform.position.y >= 5f)
		{
			Destroy(gameObject);
		}

		if (!isReflected)
		{
			if (rb.velocity.magnitude < moveSpeed)
			{
				Vector3 vel = new Vector3();
				transform.forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
			
				vel = transform.forward * moveSpeed;
				
				rb.velocity = vel;
			
			}
		}
		else
		{
			if (rb.velocity.magnitude < moveSpeedDeflected)
			{
				Vector3 vel = new Vector3();
				transform.forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
			
				vel = transform.forward * moveSpeedDeflected;
				
				rb.velocity = vel;
			
			}
		}
		
	}


	public void Seek(Transform target, bool isSeekingBullet, float speed)
    {
        this.target = target;
        this.isSeekingBullet = isSeekingBullet;
        this.speed = speed;
        
        dir = target.position - transform.position;
    }


    /*private void HitTarget()
    {
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }*/
}
