using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RainbowAssets.StateMachine.Editor
{
    [CustomEditor(typeof(State), true)]
    public class StateEditor : UnityEditor.Editor
    {
        VisualElement container;

        public override VisualElement CreateInspectorGUI()
        {
            container = new VisualElement();

            if (serializedObject.targetObject is ActionState)
            {
                DrawProperty("title");
                DrawProperty("onEnterActions");
                DrawProperty("onTickActions");
                DrawProperty("onExitActions");
                DrawTransitions();
            }

            if (serializedObject.targetObject is AnyState)
            {
                DrawTransitions();
            }

            return container;
        }

        void DrawProperty(string propertyName)
        {
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            PropertyField field = new(property);

            field.Bind(serializedObject);

            container.Add(field);
            container.Add(MakeEmptyLine());
        }

        void DrawTransitions()
        {
            ListView listView = new()
            {
                headerTitle = "Transitions",
                bindingPath = "transitions",
                showBorder = true,
                showFoldoutHeader = true,
                showBoundCollectionSize = false,
                horizontalScrollingEnabled = true,
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                makeItem = MakeTransitionItem,
                bindItem = BindTransitionItem
            };

            listView.Bind(serializedObject);
            container.Add(listView);
        }

        VisualElement MakeTransitionItem()
        {
            VisualElement itemContainer = new()
            {
                style =
                {
                    paddingLeft = 10,
                    paddingRight = 10,
                    paddingTop = 5,
                    paddingBottom = 5
                }
            };

            return itemContainer;
        }

        void BindTransitionItem(VisualElement element, int index)
        {
            SerializedProperty transitions = serializedObject.FindProperty("transitions");
            SerializedProperty itemProperty = transitions.GetArrayElementAtIndex(index);
            PropertyField field = new(itemProperty);

            field.Bind(serializedObject);

            element.Clear();
            element.Add(field);
        }

        VisualElement MakeEmptyLine()
        {
            VisualElement space = new()
            {
                style = { marginBottom = 10 }
            };

            return space;
        }
    }
}