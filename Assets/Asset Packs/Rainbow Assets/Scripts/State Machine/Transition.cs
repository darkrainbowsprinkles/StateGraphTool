using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.StateMachine
{
    [System.Serializable]
    public class Transition
    {
        [SerializeField] Condition condition;
        [SerializeField, HideInInspector] string rootStateID;
        [SerializeField, HideInInspector] string trueStateID;
        StateMachineController controller;

        public Transition(string rootStateID, string trueStateID)
        {
            this.rootStateID = rootStateID;
            this.trueStateID = trueStateID;
        }

        public void Bind(StateMachineController controller)
        {
            this.controller = controller;
        }

        public string GetRootStateID()
        {
            return rootStateID;
        }

        public string GetTrueStateID()
        {
            return trueStateID;
        }

        public bool Check()
        {
            return condition.Check(controller.GetComponents<IPredicateEvaluator>());
        }
    }
}
