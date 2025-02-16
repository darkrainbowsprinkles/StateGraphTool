using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RainbowAssets.StateMachine.Editor
{
    /// <summary>
    /// A custom state editor for easy state visualization.
    /// </summary>
    [CustomEditor(typeof(State), true)]
    public class StateEditor : UnityEditor.Editor
    {
        VisualElement container;

        public override VisualElement CreateInspectorGUI()
        {
            container = new();

            ActionState actionState = serializedObject.targetObject as ActionState;

            if(actionState != null)
            {
                DrawProperty("title");
                DrawProperty("onEnterActions");
                DrawProperty("onTickActions");
                DrawProperty("onExitActions");
                DrawTransitions();
            }

            AnyState anyState = serializedObject.targetObject as AnyState;

            if(anyState != null)
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
            container.Add(GetSpace());
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
            };

            listView.Bind(serializedObject);
            container.Add(listView);
        }

        VisualElement GetSpace()
        {
            PropertyField space = new();
            space.style.marginBottom = 10;
            return space;
        }
    }
}