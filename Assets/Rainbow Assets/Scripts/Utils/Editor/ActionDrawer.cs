using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RainbowAssets.Utils.Editor
{
    [CustomPropertyDrawer(typeof(Action))]
    public class ActionDrawer : PropertyDrawer
    {
        const int maxParameters = 2;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();

            SerializedProperty action = property.FindPropertyRelative("action");
            SerializedProperty parameters = property.FindPropertyRelative("parameters");

            PropertyField actionField = new(action);
            root.Add(actionField);

            VisualElement parametersContainer = new();
            root.Add(parametersContainer);

            DrawParameters(parametersContainer, action, parameters);

            root.TrackPropertyValue(action, _ => DrawParameters(parametersContainer, action, parameters));

            return root;
        }

        void DrawParameters(VisualElement container, SerializedProperty action, SerializedProperty parameters)
        {
            container.Clear();

            if (parameters.arraySize < maxParameters)
            {
                parameters.arraySize = maxParameters;
            }

            SerializedProperty parameterZero = parameters.GetArrayElementAtIndex(0);
            EAction selectedAction = (EAction)action.enumValueIndex;

            if (selectedAction == EAction.PrintMessage)
            {
                GameGUIUtlity.DrawTextField(container, parameterZero, "Message");
            }

            if (selectedAction == EAction.PlayAnimation)
            {
                GameGUIUtlity.DrawTextField(container, parameterZero, "Animation Name");
            }
        }
    }
}