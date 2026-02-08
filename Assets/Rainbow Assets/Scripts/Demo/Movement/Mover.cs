using RainbowAssets.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace RainbowAssets.Demo.Movement
{
    public class Mover : MonoBehaviour, IActionPerformer
    {
        [SerializeField] float maxSpeed = 6;
        [SerializeField] float destinationTolerance = 1;
        NavMeshAgent agent;
        RainbowAssets.Demo.Core.AnimationPlayer animationPlayer;

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            if(agent.isActiveAndEnabled)
            {
                agent.isStopped = false;
                agent.destination = destination;
                agent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            }
        }

        public bool AtDestination(Vector3 destination)
        {
            return Vector3.Distance(transform.position, destination) < destinationTolerance;
        }

        // LIFECYCLE METHODS

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();   
            animationPlayer = GetComponent<RainbowAssets.Demo.Core.AnimationPlayer>();
        }

        void Update()
        {
            animationPlayer.UpdateParameter("movementSpeed", GetMovementMagnitude());
        } 

        float GetMovementMagnitude()
        {
            return transform.InverseTransformDirection(agent.velocity).magnitude;
        }

        void IActionPerformer.PerformAction(EAction action, string[] parameters)
        {
            switch(action)
            {
                case EAction.CancelMovement:
                    agent.isStopped = true;
                    agent.ResetPath();
                    break;
            }
        }
    }
}
