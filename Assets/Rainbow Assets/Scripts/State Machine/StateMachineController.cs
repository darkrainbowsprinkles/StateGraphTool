using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.StateMachine
{
    public class StateMachineController : MonoBehaviour
    {
        [SerializeField] StateMachine stateMachine;
        IPredicateEvaluator[] predicateEvaluators;
        IActionPerformer[] actionPerformers;
        State currentState;

        public IPredicateEvaluator[] GetPredicateEvaluators()
        {
            return predicateEvaluators;
        }

        public IActionPerformer[] GetActionPerformers()
        {
            return actionPerformers;
        }

        public State GetCurrentState()
        {
            return currentState;
        }

        public StateMachine GetStateMachine()
        {
            return stateMachine;
        }

        public void SwitchState(string newStateID)
        {
            currentState?.Exit(this);
            currentState = stateMachine.GetState(newStateID);
            currentState.Enter(this);
        }

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
