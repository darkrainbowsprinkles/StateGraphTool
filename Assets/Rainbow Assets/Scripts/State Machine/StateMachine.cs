using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RainbowAssets.StateMachine
{
    [CreateAssetMenu(menuName = "Rainbow Assets/New State Machine")]
    public class StateMachine : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector] EntryState entryState;
        [SerializeField, HideInInspector] AnyState anyState;
        [SerializeField, HideInInspector] List<State> states = new();
        readonly Vector2 entryStateOffset = new(250, 0);
        readonly Vector2 anyStateOffset = new(250, 50);

        public EntryState GetEntryState()
        {
            return entryState;
        }

        public AnyState GetAnyState()
        {
            return anyState;
        }

        public State GetState(string stateID)
        {
            foreach (var state in states)
            {
                if (state.GetUniqueID() == stateID)
                {
                    return state;
                }
            }

            return null;
        }

        public IEnumerable<State> GetStates()
        {
            return states;
        }

    #if UNITY_EDITOR
        public State CreateState(Type type, Vector2 position)
        {
            State newState = CreateInstance(type) as State;
            newState.SetUniqueID(Guid.NewGuid().ToString());
            newState.SetPosition(position);
            newState.name = type.Name;

            if (newState is ActionState)
            {
                Undo.RegisterCreatedObjectUndo(newState, "State Created");
                Undo.RecordObject(this, "State Added");
            }

            states.Add(newState);
            return newState;
        }

        public void RemoveState(State stateToRemove)
        {
            Undo.RecordObject(this, "State Removed");
            states.Remove(stateToRemove);
            Undo.DestroyObjectImmediate(stateToRemove);
        }
    #endif

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
    #if UNITY_EDITOR
            if (AssetDatabase.GetAssetPath(this) == "")
            {
                return;
            }

            if (AssetDatabase.IsAssetImportWorkerProcess())
            {
                return;
            }

            CreateDefaultStates();
            NestStates();
    #endif
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize() { }

   #if UNITY_EDITOR
        void CreateDefaultStates()
        {
            if (entryState == null)
            {
                entryState = CreateState(typeof(EntryState), entryStateOffset) as EntryState;
                entryState.SetTitle("Entry State");
            }

            if (anyState == null)
            {
                anyState = CreateState(typeof(AnyState), anyStateOffset) as AnyState;
                anyState.SetTitle("Any State");
            }
        }

        void NestStates()
        {
            foreach (State state in states)
            {
                if (state == null)
                {
                    continue;
                }

                if (AssetDatabase.GetAssetPath(state) != "")
                {
                    continue;
                }

                AssetDatabase.AddObjectToAsset(state, this);
            }
        }
    }
    #endif
}
