using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions
{

    [Category("GameObject")]
    public class SetLineRendererNonVisible : ActionTask<Transform>
    {

        [RequiredField]
        public BBParameter<GameObject> LineOwner;
        public bool repeat = false;

        

        protected override void OnExecute() { DoLook(); }
        protected override void OnUpdate() { DoLook(); }

        void DoLook() {
           


            LineRenderer lr = LineOwner.value.GetComponent<LineRenderer>();
            
            lr.enabled = false;
            
         
            
            

            if ( !repeat )
                EndAction(true);
        }
    }
}