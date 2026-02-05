using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.StateMachine
{
    /// <summary>
    /// Controls the execution of a StateMachine within a MonoBehaviour.
    /// Handles state switching and updates the StateMachine lifecycle.
    /// </summary>
    public class StateMachineController : MonoBehaviour
    {
        /// <summary>
        /// The StateMachine instance controlled by this component.
        /// </summary>
        [SerializeField] StateMachine stateMachine;

        /// <summary>
        /// Cached array of IPredicateEvaluator components on this GameObject.
        /// </summary>
        IPredicateEvaluator[] predicateEvaluators;

        /// <summary>
        /// Cached array of IActionPerformer components on this GameObject.
        /// </summary>
        IActionPerformer[] actionPerformers;
        
        /// <summary>
        /// The currently active state in the state machine.
        /// </summary>
        State currentState;

        /// <summary>
        /// Gets the cached predicate evaluators on this GameObject.
        /// </summary>
        /// <returns>An array of IPredicateEvaluator components.</returns>
        public IPredicateEvaluator[] GetPredicateEvaluators()
        {
            return predicateEvaluators;
        }

        /// <summary>
        /// Gets the cached action performers on this GameObject.
        /// </summary>
        /// <returns>An array of IActionPerformer components.</returns>
        public IActionPerformer[] GetActionPerformers()
        {
            return actionPerformers;
        }

        /// <summary>
        /// Gets the currently active state.
        /// </summary>
        /// <returns>The current State.</returns>
        public State GetCurrentState()
        {
            return currentState;
        }

        /// <summary>
        /// Gets the current StateMachine instance.
        /// </summary>
        /// <returns>The controlled StateMachine.</returns>
        public StateMachine GetStateMachine()
        {
            return stateMachine;
        }

        /// <summary>
        /// Switches the active state of the state machine.
        /// </summary>
        /// <param name="newState">Tthe state to transition to.</param>
        public void SwitchState(string newStateID)
        {
            currentState?.Exit(this);
            currentState = stateMachine.GetState(newStateID);
            currentState.Enter(this);
        }

        // LIFECYCLE METHODS

        void Awake()
        {
            predicateEvaluators = GetComponents<IPredicateEvaluator>();
            actionPerformers = GetComponents<IActionPerformer>();
        }

        void Start()
        {
            SwitchState(stateMachine.GetEntryState().GetUniqueID());
        }

        void Update()
        {
            currentState.Tick(this);
            stateMachine.GetAnyState().Tick(this);
        }
    }
}
