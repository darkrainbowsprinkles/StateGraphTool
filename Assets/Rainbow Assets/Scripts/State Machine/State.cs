using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RainbowAssets.StateMachine
{
    public abstract class State : ScriptableObject
    {
        [SerializeField] string title = "New State";
        [SerializeField] List<Transition> transitions = new();
        [HideInInspector, SerializeField] string uniqueID;
        [HideInInspector, SerializeField] Vector2 position;

        public string GetUniqueID()
        {
            return uniqueID;
        }

        public void SetUniqueID(string uniqueID)
        {
            this.uniqueID = uniqueID;
        }

        public string GetTitle()
        {
            return title;
        }

        public void SetTitle(string title)
        {
            this.title = title;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public IEnumerable<Transition> GetTransitions()
        {
            return transitions;
        }

 #if UNITY_EDITOR
        public void SetPosition(Vector2 position)
        {
            Undo.RecordObject(this, "State Moved");
            this.position = position;
            EditorUtility.SetDirty(this);
        }

        public void AddTransition(string trueStateID)
        {
            Undo.RecordObject(this, "Transition Added");
            transitions.Add(new Transition(uniqueID, trueStateID));
            EditorUtility.SetDirty(this);
        }

        public void RemoveTransition(string trueStateID)
        {
            Undo.RecordObject(this, "Transition Removed");
            transitions.Remove(GetTransition(trueStateID));
            EditorUtility.SetDirty(this);
        }
    #endif

        public virtual void Enter(StateMachineController controller) { }

        public virtual void Exit(StateMachineController controller) { }

        public virtual void Tick(StateMachineController controller)
        {
            CheckTransitions(controller);
        }

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
