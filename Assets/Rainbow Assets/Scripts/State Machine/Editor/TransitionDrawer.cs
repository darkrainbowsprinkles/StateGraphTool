using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RainbowAssets.StateMachine.Editor
{
    /// <summary>
    /// Custom PropertyDrawer for handling the display and editing of a Transition object in the Unity Editor.
    /// </summary>
    [CustomPropertyDrawer(typeof(Transition))]
    public class TransitionDrawer : PropertyDrawer
    {
        /// <summary>
        /// Creates and returns the property GUI for displaying and editing a Transition object in the Unity Inspector.
        /// </summary>
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new();

            SerializedObject serializedState = property.serializedObject;

            if (serializedState.targetObject is State state)
            {
                DrawTransitionLabel(property, container, state);
                DrawConditionArray(property, container);
            }

            return container;
        }

        /// <summary>
        /// Draws the transition labelbased on the current Transition.
        /// </summary>
        /// <param name="property">The SerializedProperty representing the Transition object.</param>
        /// <param name="container">The container VisualElement to which the transition label is added.</param>
        /// <param name="state">The State object that contains the transition data.</param>
        void DrawTransitionLabel(SerializedProperty property, VisualElement container, State state)
        {
            StateMachine stateMachine = AssetDatabase.LoadAssetAtPath<StateMachine>(AssetDatabase.GetAssetPath(state));

            if (stateMachine != null)
            {
                SerializedProperty rootStateID = property.FindPropertyRelative("rootStateID");
                SerializedProperty trueStateID = property.FindPropertyRelative("trueStateID");

                State rootState = stateMachine.GetState(rootStateID.stringValue);
                State trueState = stateMachine.GetState(trueStateID.stringValue);

                Label transitionLabel = new($"{rootState.GetTitle()} → {trueState.GetTitle()}");
                container.Add(transitionLabel);
            }
        }

        /// <summary>
        /// Draws the condition array UI field for the Transition object.
        /// </summary>
        /// <param name="property">The SerializedProperty representing the Transition object.</param>
        /// <param name="container">The container VisualElement to which the condition field is added.</param>
        void DrawConditionArray(SerializedProperty property, VisualElement container)
        {
            SerializedProperty condition = property.FindPropertyRelative("condition");
            PropertyField conditionField = new(condition);
            container.Add(conditionField);
        }
    }
}