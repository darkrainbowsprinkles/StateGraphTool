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
        [SerializeField] ActionData[] onEnterActions;

        /// <summary>
        /// Actions executed during the state's update cycle.
        /// </summary>
        [SerializeField] ActionData[] onTickActions;

        /// <summary>
        /// Actions executed when the state is exited.
        /// </summary>
        [SerializeField] ActionData[] onExitActions;

        /// <summary>
        /// Represents an action with parameters that can be executed by the state.
        /// </summary>
        [System.Serializable]
        class ActionData
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
        public override void Enter()
        {
            base.Enter();
            DoActions(onEnterActions);
        }

        /// <summary>
        /// Called every frame while the state is active. Executes all defined tick actions.
        /// </summary>
        public override void Tick()
        {
            base.Tick();
            DoActions(onTickActions);
        }

        /// <summary>
        /// Called when the state is exited. Executes all defined exit actions.
        /// </summary>
        public override void Exit()
        {
            base.Exit();
            DoActions(onExitActions);
        }

        /// <summary>
        /// Executes a given set of actions by invoking them on all components implementing IAction.
        /// </summary>
        /// <param name="actions">The array of actions to execute.</param>
        void DoActions(ActionData[] actions)
        {
            foreach (var action in controller.GetComponents<IAction>())
            {
                foreach (var actionData in actions)
                {
                    action.DoAction(actionData.action, actionData.parameters);
                }
            }
        }
    }

}
