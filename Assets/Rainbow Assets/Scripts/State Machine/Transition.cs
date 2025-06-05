using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.StateMachine
{
    /// <summary>
    /// Represents a transition between states in the state machine.
    /// </summary>
    [System.Serializable]
    public class Transition
    {
        /// <summary>
        /// The condition that must be met for the transition to occur.
        /// </summary>
        [SerializeField] Condition condition;

        /// <summary>
        /// The unique identifier of the root state from which this transition originates.
        /// </summary>
        [SerializeField, HideInInspector] string rootStateID;

        /// <summary>
        /// The unique identifier of the target state to which this transition leads.
        /// </summary>
        [SerializeField, HideInInspector] string trueStateID;

        /// <summary>
        /// The controller managing the StateMachine that this transition belongs to.
        /// </summary>
        StateMachineController controller;

        /// <summary>
        /// Initializes a new instance of a Transition with specified root and target state IDs.
        /// </summary>
        /// <param name="rootStateID">The ID of the root state.</param>
        /// <param name="trueStateID">The ID of the target state.</param>
        public Transition(string rootStateID, string trueStateID)
        {
            this.rootStateID = rootStateID;
            this.trueStateID = trueStateID;
        }

        /// <summary>
        /// Binds the transition to a specific StateMachineController.
        /// </summary>
        /// <param name="controller">The StateMachineController managing this transition.</param>
        public void Bind(StateMachineController controller)
        {
            this.controller = controller;
        }

        /// <summary>
        /// Gets the unique identifier of the root state.
        /// </summary>
        /// <returns>The root state ID.</returns>
        public string GetRootStateID()
        {
            return rootStateID;
        }

        /// <summary>
        /// Gets the unique identifier of the target state.
        /// </summary>
        /// <returns>The target state ID.</returns>
        public string GetTrueStateID()
        {
            return trueStateID;
        }

        /// <summary>
        /// Evaluates the transition condition to determine if the transition should occur.
        /// </summary>
        /// <returns>True if the condition is met, otherwise false.</returns>
        public bool Check()
        {
            return condition.Check(controller.GetComponents<IPredicateEvaluator>());
        }
    }
}
