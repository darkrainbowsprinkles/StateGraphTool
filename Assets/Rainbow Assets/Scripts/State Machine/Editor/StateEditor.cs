using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RainbowAssets.StateMachine.Editor
{
    /// <summary>
    /// A custom state editor for easy state visualization in the Unity Inspector.
    /// </summary>
    [CustomEditor(typeof(State), true)]
    public class StateEditor : UnityEditor.Editor
    {
        /// <summary>
        /// The main container that holds all UI elements in the custom editor.
        /// </summary>
        VisualElement container;

        /// <summary>
        /// Creates and returns the custom Inspector UI for the state editor.
        /// </summary>
        /// <returns>A <see cref="VisualElement"/> representing the custom Inspector UI.</returns>
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

        /// <summary>
        /// Draws a property field in the Inspector UI.
        /// </summary>
        /// <param name="propertyName">The name of the serialized property to draw.</param>
        void DrawProperty(string propertyName)
        {
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            PropertyField field = new PropertyField(property);

            field.Bind(serializedObject);

            container.Add(field);
            container.Add(MakeEmptyLine());
        }

        /// <summary>
        /// Draws the transitions list in the Inspector UI.
        /// </summary>
        void DrawTransitions()
        {
            ListView listView = new ListView
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

        /// <summary>
        /// Creates a container for an individual transition item in the list.
        /// </summary>
        /// <returns>A <see cref="VisualElement"/> representing a transition item.</returns>
        VisualElement MakeTransitionItem()
        {
            VisualElement itemContainer = new VisualElement
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

        /// <summary>
        /// Binds a transition item to the corresponding serialized property.
        /// </summary>
        /// <param name="element">The UI element representing the transition item.</param>
        /// <param name="index">The index of the transition in the list.</param>
        void BindTransitionItem(VisualElement element, int index)
        {
            SerializedProperty transitions = serializedObject.FindProperty("transitions");
            SerializedProperty itemProperty = transitions.GetArrayElementAtIndex(index);
            PropertyField field = new PropertyField(itemProperty);

            field.Bind(serializedObject);

            element.Clear();
            element.Add(field);
        }

        /// <summary>
        /// Creates an empty visual element to add spacing between UI elements.
        /// </summary>
        /// <returns>A <see cref="VisualElement"/> representing an empty line.</returns>
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