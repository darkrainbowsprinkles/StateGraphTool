using RainbowAssets.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace RainbowAssets.Demo
{
    /// <summary>
    /// Handles character movement.
    /// </summary>
    public class Mover : MonoBehaviour, IAction
    {
        /// <summary>
        /// Maximum movement speed of the character.
        /// </summary>
        [SerializeField] float maxSpeed = 6;

        /// <summary>
        /// The tolerance distance to consider the destination reached.
        /// </summary>
        [SerializeField] float destinationTolerance = 1;

        NavMeshAgent agent;
        AnimationPlayer animationPlayer;

        /// <summary>
        /// Moves the character to the specified destination at a given speed fraction.
        /// </summary>
        /// <param name="destination">The target position to move towards.</param>
        /// <param name="speedFraction">Multiplier for movement speed (0 to 1).</param>
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            if(agent.isActiveAndEnabled)
            {
                agent.isStopped = false;
                agent.destination = destination;
                agent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            }
        }

        /// <summary>
        /// Checks if the character has reached the destination.
        /// </summary>
        /// <param name="destination">The target position.</param>
        /// <returns>True if within the destination tolerance, otherwise false.</returns>
        public bool AtDestination(Vector3 destination)
        {
            return Vector3.Distance(transform.position, destination) < destinationTolerance;
        }

        // LIFECYCLE METHODS

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();   
            animationPlayer = GetComponent<AnimationPlayer>();
        }

        void Update()
        {
            animationPlayer.UpdateParameter("movementSpeed", GetMovementMagnitude());
        } 

        /// <summary>
        /// Gets the movement speed magnitude of the character relative to its local space.
        /// </summary>
        /// <returns>The movement speed magnitude.</returns>
        float GetMovementMagnitude()
        {
            return transform.InverseTransformDirection(agent.velocity).magnitude;
        }

        /// <summary>
        /// Performs an action defined by the IAction interface.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="parameters">Additional parameters for the action.</param>
        void IAction.DoAction(EAction action, string[] parameters)
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
