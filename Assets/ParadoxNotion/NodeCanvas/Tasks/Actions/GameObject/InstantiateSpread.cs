using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
    [Category("GameObject")]
    public class InstantiateSpread : ActionTask<GameObject>
    {
        public BBParameter<GameObject> go;
        public BBParameter<GameObject> owner;
        public BBParameter<int> numberOfBullet;
        public BBParameter<float> angleOfSpread;

        protected override string info
        {
            get
            {
                return "Instantiate " + agentInfo + " under " + (go.value ? go.ToString() : "World");
            }
        }

        protected override void OnExecute() {
#if UNITY_5_4_OR_NEWER

            float angleRef = angleOfSpread.value / numberOfBullet.value;

            for (int i = 0; i < numberOfBullet.value; i++)
            {
               // float rdAngle = Random.Range(-angleOfSpread.value / 2, angleOfSpread.value / 2);
                float rdAngle = -angleOfSpread.value/2 + angleRef * i;
                Vector3 eulerRotation = owner.value.transform.rotation.eulerAngles + new Vector3(0, rdAngle, 0);
                var clone = (GameObject)Object.Instantiate(go.value, owner.value.transform.position , Quaternion.Euler(eulerRotation));
               // clone.transform.position += new Vector3(0, 0, 0.5f);
            }
            
#else

            var clone = (GameObject)Object.Instantiate(agent.gameObject);
            clone.transform.SetParent(parent.value);

#endif
          
            
            EndAction();
        }
    }
}