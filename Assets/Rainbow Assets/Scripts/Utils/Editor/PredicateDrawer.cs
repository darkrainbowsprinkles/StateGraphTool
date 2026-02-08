using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RainbowAssets.Utils.Editor
{
    [CustomPropertyDrawer(typeof(Condition.Predicate))]
    public class PredicateDrawer : PropertyDrawer
    {
        const int maxParameters = 2;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();

            SerializedProperty predicate = property.FindPropertyRelative("predicate");
            SerializedProperty parameters = property.FindPropertyRelative("parameters");
            SerializedProperty negate = property.FindPropertyRelative("negate");

            PropertyField predicateField = new(predicate);
            root.Add(predicateField);

            VisualElement parametersContainer = new();
            root.Add(parametersContainer);

            DrawParameters(parametersContainer, predicate, parameters);

            root.TrackPropertyValue(predicate, _ => DrawParameters(parametersContainer, predicate, parameters));

            PropertyField negateField = new(negate);
            root.Add(negateField);

            return root;
        }

        void DrawParameters(VisualElement container, SerializedProperty predicate, SerializedProperty parameters)
        {
            container.Clear();

            if (parameters.arraySize < maxParameters)
            {
                parameters.arraySize = maxParameters;
            }

            SerializedProperty parameterZero = parameters.GetArrayElementAtIndex(0);
            EPredicate selectedPredicate = (EPredicate)predicate.enumValueIndex;

            if (selectedPredicate == EPredicate.InputActionPressed)
            {
                GameGUIUtlity.DrawInputActions(container, parameterZero);
            }

            if (selectedPredicate == EPredicate.AnimationOver)
            {
                GameGUIUtlity.DrawTextField(container, parameterZero, "Animation Tag");
            }
        }


    }
}
