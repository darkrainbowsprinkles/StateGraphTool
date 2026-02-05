using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RainbowAssets.StateMachine
{
    /// <summary>
    /// Represents an abstract base class for states in a state machine.
    /// </summary>
    public abstract class State : ScriptableObject
    {
        /// <summary>
        /// The title of the state.
        /// </summary>
        [SerializeField] string title = "New State";

        /// <summary>
        /// List of transitions associated with this state.
        /// </summary>
        [SerializeField] List<Transition> transitions = new();

        /// <summary>
        /// Unique identifier for this state.
        /// </summary>
        [HideInInspector, SerializeField] string uniqueID;

        /// <summary>
        /// The position of the state in the editor.
        /// </summary>
        [HideInInspector, SerializeField] Vector2 position;

        /// <summary>
        /// Gets the unique ID of the state.
        /// </summary>
        /// <returns>The unique ID of the state.</returns>
        public string GetUniqueID()
        {
            return uniqueID;
        }

        /// <summary>
        /// Sets the unique ID of the state.
        /// </summary>
        /// <param name="uniqueID">The unique ID to assign.</param>
        public void SetUniqueID(string uniqueID)
        {
            this.uniqueID = uniqueID;
        }

        /// <summary>
        /// Gets the title of the state.
        /// </summary>
        /// <returns>The title of the state.</returns>
        public string GetTitle()
        {
            return title;
        }

        /// <summary>
        /// Sets the title of the state.
        /// </summary>
        /// <param name="title">The new title to assign.</param>
        public void SetTitle(string title)
        {
            this.title = title;
        }

        /// <summary>
        /// Gets the position of the state in the editor.
        /// </summary>
        /// <returns>The position of the state.</returns>
        public Vector2 GetPosition()
        {
            return position;
        }

        /// <summary>
        /// Gets the transitions associated with this state.
        /// </summary>
        /// <returns>An enumerable collection of transitions.</returns>
        public IEnumerable<Transition> GetTransitions()
        {
            return transitions;
        }

 #if UNITY_EDITOR
        /// <summary>
        /// Sets the position of the state in the editor.
        /// </summary>
        /// <param name="position">The new position to assign.</param>
        public void SetPosition(Vector2 position)
        {
            Undo.RecordObject(this, "State Moved");
            this.position = position;
            EditorUtility.SetDirty(this);
        }

        /// <summary>
        /// Adds a transition to another state.
        /// </summary>
        /// <param name="trueStateID">The ID of the target state.</param>
        public void AddTransition(string trueStateID)
        {
            Undo.RecordObject(this, "Transition Added");
            transitions.Add(new Transition(uniqueID, trueStateID));
            EditorUtility.SetDirty(this);
        }

        /// <summary>
        /// Removes a transition to another state.
        /// </summary>
        /// <param name="trueStateID">The ID of the target state to remove.</param>
        public void RemoveTransition(string trueStateID)
        {
            Undo.RecordObject(this, "Transition Removed");
            transitions.Remove(GetTransition(trueStateID));
            EditorUtility.SetDirty(this);
        }
    #endif

        /// <summary>
        /// Called when the state is entered.
        /// </summary>
        public virtual void Enter(StateMachineController controller) { }

        /// <summary>
        /// Called when the state is exited.
        /// </summary>
        public virtual void Exit(StateMachineController controller) { }

        /// <summary>
        /// Called every frame while the state is active.
        /// </summary>
        public virtual void Tick(StateMachineController controller)
        {
            CheckTransitions(controller);
        }

        /// <summary>
        /// Retrieves a transition by its target state ID.
        /// </summary>
        /// <param name="trueStateID">The target state ID.</param>
        /// <returns>The corresponding transition, or null if not found.</returns>
        Transition GetTransition(string trueStateID)
        {
            foreach (var transition in transitions)
            {
                if (transition.GetTrueStateID() == trueStateID)
                {
                    return transition;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks all transitions and switches the state if a condition is met.
        /// </summary>
        void CheckTransitions(StateMachineController controller)
        {
            foreach (var transition in transitions)
            {
                if (transition.Check(controller))
                {
                    string trueStateID = transition.GetTrueStateID();
                    controller.SwitchState(trueStateID);
                }
            }
        }
    }

}
