using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float camDistanceZ = 10f;
    public float cameDistanceY = 3f;
    public float camDistanceX = 0f;
    private float currentX;
    private float currentY;
    public float sensitivityX = 1f;
    public float sensitivityY = 1f;
    public float minY = -20f;
    public float maxY = 10f;
    public Transform target;
    public Transform player;

    public float camAdjustSpeed = 3f;

    public float smoothSpeedHorsArene;

    private float charaSpeed;

    private float camXOffSetLimit = 3f;

    public float distanceZStartOffsetWall;

    public float multiplierDistanceZForOffset;

    public float offsetTop;

    public Vector3 mousPosHit;

    private Vector3 targetPosOffseted, targetPos;

    private Vector3 targetPosDirection;

    public Vector3 areneCenterPoint;
    
    float offsetFromWall;

    public  bool isOffsetActif; // à déclencher quand on entre dans l'arène

    private RaycastHit hit;

    public LayerMask layerGround;

   
    public List<CamTrigger> allCamPoints;

    public static int currentCamPoint;

    private Vector3 eulerCam;
    // components

    private CameraInputDetector camInput;
    private CustomCharacterController customChara;

    private bool canLookAt;

    public static bool beforeArena;

    private XtraLifeManager xtraLife;




    [System.Serializable]
    public class CamTrigger
    {
        public Transform CameraPositionPoint;

        public bool isLookingAtPlayer;
        
    }


    private void Awake()
    {
        xtraLife = xtraLife = GameObject.Find("Manager").GetComponent<XtraLifeManager>();
        camInput = GetComponent<CameraInputDetector>();
        customChara = player.gameObject.GetComponent<CustomCharacterController>();
        targetPos = target.transform.position;

        eulerCam = transform.rotation.eulerAngles;
        beforeArena = false;
        currentCamPoint = 0;

    }

    public void UpdateHorsArena()
    {

        if (!beforeArena)
        {
            transform.position = Vector3.Lerp(transform.position, allCamPoints[currentCamPoint].CameraPositionPoint.position, Time.deltaTime * smoothSpeedHorsArene);
        
        
        
            if (allCamPoints[currentCamPoint].isLookingAtPlayer)
            {
                transform.LookAt(target);
            }
            else
            {
                eulerCam = Vector3.Lerp(eulerCam,  allCamPoints[currentCamPoint].CameraPositionPoint.rotation.eulerAngles, Time.deltaTime * smoothSpeedHorsArene);
        
                transform.rotation = Quaternion.Euler(eulerCam);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position + new Vector3(0,7,-15), Time.deltaTime * smoothSpeedHorsArene);
        }
        
        

    }

    public void putCamRightArena()
    {
        Vector3 dir = new Vector3(0, cameDistanceY , -camDistanceZ  );
        var tempV = target.position + dir;
        transform.position =  tempV;

        transform.LookAt(player);
        
        /* Ray rere = Camera.main.ScreenPointToRay(Input.mousePosition);
              
          Physics.Raycast(rere, out hit, 150f, layerGround);
          
          
          targetPosDirection = hit.point - player.transform.position;
          
          targetPosOffseted = player.transform.position + targetPosDirection + dir;
          
          transform.position =  targetPosOffseted;
        
        
        targetPos = Vector3.Lerp(targetPos,  player.transform.position + targetPosDirection, camAdjustSpeed * Time.deltaTime );
        
        transform.LookAt(targetPos);*/
        
        
        CameraControllerManager.isInArena = true;

        StartCoroutine(waitCam());


    }

      public void PressPlayCam()
      {

          PlayerSave ps = GameObject.Find("Manager").GetComponent<PlayerSave>();

        
            
        
               
            
            ps.panelSkip.gameObject.SetActive(XtraLifeManager.seenIntro);


          GetComponent<Fading>().StartLevel();
        
        
    }


    public IEnumerator waitCam()
    {
        for (float f = 0; f < 1f; f+= Time.deltaTime)
        {
            yield return 0;
        }

        canLookAt = true;
    }


    public void OrbitAround()
    {
        charaSpeed = player.gameObject.GetComponent<CustomCharacterController>().moveSpeed;
        currentY += camInput.yAxisCam * sensitivityY;
        currentY = Mathf.Clamp(currentY, minY, maxY);

        camDistanceX = currentX * -camXOffSetLimit;

        if(currentX < 360 && currentX > -360) {

            currentX += camInput.xAxisCam * sensitivityX;

        } else { currentX = 0; }

        if (areneCenterPoint.z-distanceZStartOffsetWall > target.position.z && isOffsetActif)
        {
            offsetFromWall = areneCenterPoint.z-distanceZStartOffsetWall - target.position.z;
        }
        else
        {
            offsetFromWall = 0;
        }
        

        Vector3 dir = new Vector3(0, cameDistanceY + (offsetFromWall * multiplierDistanceZForOffset), -camDistanceZ  + (offsetFromWall * multiplierDistanceZForOffset));
      // Quaternion rotation = player.transform.rotation;


         targetPosDirection = mousPosHit - player.transform.position;
        
        if (targetPosDirection.magnitude < 5f)
        {
            //targetPosDirection = Vector3.MoveTowards(targetPosDirection, Vector3.zero, Time.deltaTime);
            targetPosDirection = Vector3.zero;
        }
        else
        {
            targetPosDirection /= 4;
        }
        
       

        targetPosOffseted = player.transform.position + targetPosDirection + dir;

        //var targetPosDirection = ((player.transform.position + mousPosHit))/3 + dir;
        
        transform.position = Vector3.Lerp(transform.position, targetPosOffseted, camAdjustSpeed * Time.deltaTime );
 
    }


    public void SimpleLookAt()
    {
        
        targetPos = Vector3.Lerp(targetPos,  player.transform.position + targetPosDirection, camAdjustSpeed * Time.deltaTime );
        
        //targetPos = (mousPosHit + target.transform.position) / 3;


        if (!canLookAt)
        {
            Vector3 direction = targetPos - transform.position;
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime/2f);  
        }
        else
        {
            transform.LookAt(targetPos); 
        }
       

         

    }
    
    

}
