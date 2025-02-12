using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RainbowAssets.StateMachine.Editor
{
    [CustomPropertyDrawer(typeof(Transition))]
    public class TransitionDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new();

            SerializedObject serializedState = property.serializedObject;

            if(serializedState.targetObject is State state)
            {
                DrawTransitionLabel(property, container, state);
                DrawConditionArray(property, container);
            }

            return container;
        }

        void DrawTransitionLabel(SerializedProperty property, VisualElement container, State state)
        {
            StateMachine stateMachine = AssetDatabase.LoadAssetAtPath<StateMachine>(AssetDatabase.GetAssetPath(state));

            if(stateMachine != null)
            {
                SerializedProperty rootStateID = property.FindPropertyRelative("rootStateID");
                SerializedProperty trueStateID = property.FindPropertyRelative("trueStateID");

                State rootState = stateMachine.GetState(rootStateID.stringValue);
                State trueState = stateMachine.GetState(trueStateID.stringValue);

                Label transitionLabel = new($"{rootState.GetTitle()} → {trueState.GetTitle()}");

                container.Add(transitionLabel);
            }
        }

        void DrawConditionArray(SerializedProperty property, VisualElement container)
        {
            SerializedProperty condition = property.FindPropertyRelative("condition");
            PropertyField conditionField = new(condition);
            container.Add(conditionField);
        }
    }
}