using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.StateMachine
{
    public class ActionState : State
    {
        [SerializeField] Action[] onEnterActions;
        [SerializeField] Action[] onTickActions;
        [SerializeField] Action[] onExitActions;

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

        void PerformActions(Action[] actions, StateMachineController controller)
        {
            foreach (Action action in actions)
            {
                action.PerformAction(controller.GetActionPerformers());
            }
        }
    }

}
