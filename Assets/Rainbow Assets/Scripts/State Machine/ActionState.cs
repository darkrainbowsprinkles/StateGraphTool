using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.StateMachine
{
    /// <summary>
    /// This state executes a set of predefined actions
    /// </summary>
    public class ActionState : State
    {
        /// <summary>
        /// Actions executed when the state is entered.
        /// </summary>
        [SerializeField] StateAction[] onEnterActions;

        /// <summary>
        /// Actions executed during the state's update cycle.
        /// </summary>
        [SerializeField] StateAction[] onTickActions;

        /// <summary>
        /// Actions executed when the state is exited.
        /// </summary>
        [SerializeField] StateAction[] onExitActions;

        /// <summary>
        /// Represents an action with parameters that can be executed by the state.
        /// </summary>
        [System.Serializable]
        class StateAction
        {
            /// <summary>
            /// The action to be executed.
            /// </summary>
            public EAction action;

            /// <summary>
            /// Parameters associated with the action.
            /// </summary>
            public string[] parameters;
        }

        /// <summary>
        /// Called when the state is entered. Executes all defined enter actions.
        /// </summary>
        public override void Enter(StateMachineController controller)
        {
            base.Enter(controller);
            PerformActions(onEnterActions, controller);
        }

        /// <summary>
        /// Called every frame while the state is active. Executes all defined tick actions.
        /// </summary>
        public override void Tick(StateMachineController controller)
        {
            base.Tick(controller);
            PerformActions(onTickActions, controller);
        }

        /// <summary>
        /// Called when the state is exited. Executes all defined exit actions.
        /// </summary>
        public override void Exit(StateMachineController controller)
        {
            base.Exit(controller);
            PerformActions(onExitActions, controller);
        }

        /// <summary>
        /// Executes a given set of actions by invoking them on all components implementing IAction.
        /// </summary>
        /// <param name="actions">The array of actions to execute.</param>
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
