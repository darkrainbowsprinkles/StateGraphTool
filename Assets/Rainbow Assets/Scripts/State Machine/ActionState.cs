using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.StateMachine
{
    public class ActionState : State
    {
        [SerializeField] StateAction[] onEnterActions;
        [SerializeField] StateAction[] onTickActions;
        [SerializeField] StateAction[] onExitActions;

        [System.Serializable]
        class StateAction
        {
            public EAction action;
            public string[] parameters;
        }

        public override void Enter(StateMachineController controller)
        {
            base.Enter(controller);
            PerformActions(onEnterActions, controller);
        }

        public override void Tick(StateMachineController controller)
        {
            base.Tick(controller);
            PerformActions(onTickActions, controller);
        }

        public override void Exit(StateMachineController controller)
        {
            base.Exit(controller);
            PerformActions(onExitActions, controller);
        }

        void PerformActions(StateAction[] actions, StateMachineController controller)
        {
            foreach (var action in controller.GetActionPerformers())
            {
                foreach (var actionData in actions)
                {
                    action.PerformAction(actionData.action, actionData.parameters);
                }
            }
        }
    }

}
