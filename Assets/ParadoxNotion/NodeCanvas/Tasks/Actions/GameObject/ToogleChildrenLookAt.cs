using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
    [Category("GameObject")]
    public class ToogleChildrenLookAt : ActionTask<GameObject>
    {
        public BBParameter<GameObject> Children;
        

       

        protected override void OnExecute() {
#if UNITY_5_4_OR_NEWER

            if (Children.value.GetComponent<LookAtPlayer>().enabled)
            {
                Children.value.GetComponent<LookAtPlayer>().enabled = false;
            }
            else
            {
                Children.value.GetComponent<LookAtPlayer>().enabled = true;
            }

      
#else



#endif
          
            
            EndAction();
        }
    }
}