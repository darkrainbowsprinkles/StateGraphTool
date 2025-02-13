using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RainbowAssets.StateMachine
{
    /// <summary>
    /// Represents a state machine as a Scriptable Object.
    /// </summary>
    [CreateAssetMenu(menuName = "Rainbow Assets/New State Machine")]
    public class StateMachine : ScriptableObject, ISerializationCallbackReceiver
    {
        /// <summary>
        /// The entry state of the state machine.
        /// </summary>
        [SerializeField, HideInInspector] EntryState entryState;

        /// <summary>
        /// A special state that is always active and listens for global conditions.
        /// </summary>
        [SerializeField, HideInInspector] AnyState anyState;

        /// <summary>
        /// The list of all states in the state machine.
        /// </summary>
        [SerializeField, HideInInspector] List<State> states = new();

        /// <summary>
        /// The default position offset for the entry state in the editor.
        /// </summary>
        Vector2 entryStateOffset = new(250, 0);

        /// <summary>
        /// The default position offset for the any state in the editor.
        /// </summary>
        Vector2 anyStateOffset = new(250, 50);

        /// <summary>
        /// A lookup dictionary to quickly find states by their unique ID.
        /// </summary>
        Dictionary<string, State> stateLookup = new();

        /// <summary>
        /// The currently active state in the state machine.
        /// </summary>
        State currentState;

        /// <summary>
        /// Binds the state machine to a controller and to all its states.
        /// </summary>
        /// <param name="controller">The state machine controller.</param>
        public void Bind(StateMachineController controller)
        {
            foreach (var state in states)
            {
                state.Bind(controller);
            }
        }

        /// <summary>
        /// Creates a new instance of the state machine.
        /// </summary>
        /// <returns>A cloned instance of the state machine.</returns>
        public StateMachine Clone()
        {
            StateMachine clone = Instantiate(this);

            clone.states.Clear();
            clone.stateLookup.Clear();

            foreach (var state in states)
            {
                clone.AddState(state.Clone());
            }

            clone.entryState = clone.GetState(entryState.GetUniqueID()) as EntryState;
            clone.anyState = clone.GetState(anyState.GetUniqueID()) as AnyState;

            return clone;
        }

        /// <summary>
        /// Retrieves a state by its unique identifier.
        /// </summary>
        /// <param name="stateID">The unique identifier of the state.</param>
        /// <returns>The corresponding state, or null if not found.</returns>
        public State GetState(string stateID)
        {
            if (!stateLookup.ContainsKey(stateID))
            {
                Debug.LogError($"State with ID {stateID} not found");
                return null;
            }

            return stateLookup[stateID];
        }

        /// <summary>
        /// Gets a collection of all states in the state machine.
        /// </summary>
        /// <returns>A collection of states.</returns>
        public IEnumerable<State> GetStates()
        {
            return states;
        }

        /// <summary>
        /// Starts the state machine by transitioning to the entry state.
        /// </summary>
        public void Enter()
        {
            SwitchState(entryState.GetEntryStateID());
        }

        /// <summary>
        /// Updates the current state and the any state each frame.
        /// </summary>
        public void Tick()
        {
            currentState.Tick();
            anyState.Tick();
        }

        /// <summary>
        /// Switches the active state of the state machine.
        /// </summary>
        /// <param name="newStateID">The ID of the state to transition to.</param>
        public void SwitchState(string newStateID)
        {
            currentState?.Exit();
            currentState = GetState(newStateID);
            currentState.Enter();
        }

    #if UNITY_EDITOR
        /// <summary>
        /// Creates a new state of a given type at a specified position.
        /// </summary>
        /// <param name="type">The type of the state to create.</param>
        /// <param name="position">The position of the state in the editor.</param>
        /// <returns>The newly created state.</returns>
        public State CreateState(Type type, Vector2 position)
        {
            State newState = MakeState(type, position);
            Undo.RegisterCreatedObjectUndo(newState, "State Created");
            Undo.RecordObject(this, "State Added");
            AddState(newState);
            return newState;
        }

        /// <summary>
        /// Removes a state from the state machine.
        /// </summary>
        /// <param name="stateToRemove">The state to be removed.</param>
        public void RemoveState(State stateToRemove)
        {
            Undo.RecordObject(this, "State Removed");
            states.Remove(stateToRemove);
            OnValidate();
            Undo.DestroyObjectImmediate(stateToRemove);
        }

        /// <summary>
        /// Instantiates a new state of a given type and initializes its properties.
        /// </summary>
        /// <param name="type">The type of the state.</param>
        /// <param name="position">The position of the state in the editor.</param>
        /// <returns>The newly created state.</returns>
        State MakeState(Type type, Vector2 position)
        {
            State newState = CreateInstance(type) as State;
            newState.name = type.Name;
            newState.SetUniqueID(Guid.NewGuid().ToString());
            newState.SetPosition(position);
            return newState;
        }
    #endif

        /// <summary>
        /// Called when the object is enabled, resetting the current state.
        /// </summary>
        void OnEnable()
        {
            currentState = null;
        }

        /// <summary>
        /// Called when the object is first loaded, validating states.
        /// </summary>
        void Awake()
        {
            OnValidate();
        }

        /// <summary>
        /// Ensures the state lookup dictionary is updated when the object is modified.
        /// </summary>
        void OnValidate()
        {
            stateLookup.Clear();

            foreach (var state in states)
            {
                if (state != null)
                {
                    stateLookup[state.GetUniqueID()] = state;
                }
            }
        }

        /// <summary>
        /// Adds a new state to the state machine and updates its validation.
        /// </summary>
        /// <param name="newState">The new state to add.</param>
        void AddState(State newState)
        {
            states.Add(newState);
            OnValidate();
        }

        /// <summary>
        /// Handles serialization before the object is saved, ensuring all states are assigned properly.
        /// </summary>
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
    #if UNITY_EDITOR
            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (var state in states)
                {
                    if (AssetDatabase.GetAssetPath(state) == "")
                    {
                        AssetDatabase.AddObjectToAsset(state, this);
                    }
                }

                if (entryState == null)
                {
                    entryState = MakeState(typeof(EntryState), entryStateOffset) as EntryState;
                    entryState.SetTitle("Entry");
                    AddState(entryState);
                }

                if (anyState == null)
                {
                    anyState = MakeState(typeof(AnyState), anyStateOffset) as AnyState;
                    anyState.SetTitle("Any");
                    AddState(anyState);
                }
            }
    #endif
        }

        /// <summary>
        /// Handles deserialization after the object is loaded.
        /// </summary>
        void ISerializationCallbackReceiver.OnAfterDeserialize() { }
    }

}
