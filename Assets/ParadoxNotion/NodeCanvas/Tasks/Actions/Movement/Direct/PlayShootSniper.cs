using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions
{

   
    public class PlayShootSniper : ActionTask<Transform>
    {


        

        protected override void OnExecute() {
#if UNITY_5_4_OR_NEWER

           GameObject.Find("Manager").GetComponent<SoundManager>().PlayShootSniperSound(agent.gameObject);

#endif
          
            
            EndAction();
        }
    }
}