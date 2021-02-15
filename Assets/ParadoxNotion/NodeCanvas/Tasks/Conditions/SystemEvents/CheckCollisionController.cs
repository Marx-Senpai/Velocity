using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions
{

    [Category("System Events")]
    [EventReceiver("OnCollisionEnter", "OnCollisionExit")]
    public class CheckCollisionController : ConditionTask<ControllerColliderHit>
    {

        public bool specifiedTagOnly;
        [TagField]
        public string objectTag = "Untagged";
        [BlackboardOnly]
        public BBParameter<GameObject> saveGameObjectAs;

        private bool stay;

        protected override string info {
            get { return  ( specifiedTagOnly ? ( " '" + objectTag + "' tag" ) : "" ); }
        }

     

        public void OnControllerColliderHit(ControllerColliderHit info) {

            if ( !specifiedTagOnly || info.gameObject.tag == objectTag ) {
                stay = true;
               
                    saveGameObjectAs.value = info.gameObject;
                    YieldReturn(true); 
            }
        }

       
    }
}