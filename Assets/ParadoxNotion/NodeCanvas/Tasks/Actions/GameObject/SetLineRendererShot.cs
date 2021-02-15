using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions
{

    [Category("GameObject")]
    public class SetLineRendererShot : ActionTask<Transform>
    {

        [RequiredField]
        public BBParameter<GameObject> LineOwner;

        public BBParameter<Vector3> TargetPosition;
        
        public bool repeat = false;

        

        protected override void OnExecute() { 
       
#if UNITY_5_4_OR_NEWER
            /*var lookPos = lookTarget.value.transform.position;
            lookPos.y = agent.position.y;
            agent.LookAt(lookPos);*/


            LineRenderer lr = LineOwner.value.GetComponent<LineRenderer>();
            
            lr.enabled = true;
            lr.SetPosition(0, ownerAgent.gameObject.transform.position);
            lr.SetPosition(1, TargetPosition.value );
            
#endif
          
            
            EndAction();
        }
    }
}