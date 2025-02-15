using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RainbowAssets.StateMachine.Editor
{
    /// <summary>
    /// A custom VisualElement for editing states in the state machine.
    /// </summary>
    public class StateEditor : VisualElement
    { 
        /// <summary>
        /// Factory class for creating StateEditor instances from UXML.
        /// </summary>
        new class UxmlFactory : UxmlFactory<StateEditor, UxmlTraits> { }

        /// <summary>
        /// Initializes a new instance of the StateEditor class.
        /// </summary>
        public StateEditor() { }

        /// <summary>
        /// Refreshes the editor UI to reflect the given state.
        /// </summary>
        /// <param name="state">The state to be displayed and edited.</param>
        public void Refresh(State state)
        {
            Clear();

            SerializedObject serializedState = new(state);

            DrawProperty(serializedState, "title");
            DrawProperty(serializedState, "onEnterActions");
            DrawProperty(serializedState, "onTickActions");
            DrawProperty(serializedState, "onExitActions");

            DrawTransitions(serializedState);
        }


        /// <summary>
        /// Draws serialized state properties in the editor view.
        /// </summary>
        void DrawProperty(SerializedObject serializedState, string propertyName)
        {
            SerializedProperty property = serializedState.FindProperty(propertyName);

            PropertyField field = new(property);

            field.Bind(serializedState);

            Add(field);
        }

        /// <summary>
        /// Draws the transitions associated with the state as a ListView.
        /// </summary>
        void DrawTransitions(SerializedObject serializedState)
        {
            SerializedProperty transitions = serializedState.FindProperty("transitions");

            ListView listView = new();

            for(int i = 0; i < transitions.arraySize; i++)
            {
                SerializedProperty transition = transitions.GetArrayElementAtIndex(i);

                listView.hierarchy.Add(new PropertyField(transition));
            }

            Add(listView);
        }
    }
}