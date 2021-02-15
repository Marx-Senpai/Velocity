using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions
{

    [Category("GameObject")]
    public class SetLineRendererPosition : ActionTask<Transform>
    {

        [RequiredField]
        public BBParameter<GameObject> LineOwner;

        public BBParameter<GameObject> Child;
        
        public bool repeat = false;


        private Vector3 posFinal;
        

        protected override void OnExecute() { DoLook(); }
        //protected override void OnUpdate() {  }

        void DoLook() {
            
            
            LineRenderer lr = LineOwner.value.GetComponent<LineRenderer>();
            
            lr.enabled = true;
         

            posFinal = ownerAgent.gameObject.transform.position + (Child.value.transform.forward * 300f);
            
            lr.SetPosition(0, ownerAgent.gameObject.transform.position);
            lr.SetPosition(1, posFinal); 
            
            
            if ( !repeat )
                EndAction(true);
        }
        
        
      
    }
}