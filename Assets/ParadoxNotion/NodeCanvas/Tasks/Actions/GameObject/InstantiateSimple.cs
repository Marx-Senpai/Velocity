using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
    [Category("GameObject")]
    public class InstantiateSimple : ActionTask<GameObject>
    {
        public BBParameter<GameObject> go;
        public BBParameter<GameObject> owner;
        public BBParameter<float> Zoffset;

        protected override string info
        {
            get
            {
                return "Instantiate " + agentInfo + " under " + (go.value ? go.ToString() : "World");
            }
        }

        protected override void OnExecute() {


            var clone = (GameObject)Object.Instantiate(go.value, owner.value.transform.position , owner.value.transform.rotation);
            clone.transform.position += owner.value.transform.forward * Zoffset.value;

          
            
            EndAction();
        }
    }
}