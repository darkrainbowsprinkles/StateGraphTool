using UnityEngine;
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
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement container = new();

            DrawProperty(container, "title");
            DrawProperty(container, "onEnterActions");
            DrawProperty(container, "onTickActions");
            DrawProperty(container, "onExitActions");

            DrawTransitions(container);

            return container;
        }

        void DrawProperty(VisualElement container, string propertyName)
        {
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            PropertyField field = new(property);

            field.Bind(serializedObject);

            container.Add(field);
            container.Add(GetSpace());
        }

        void DrawTransitions(VisualElement container)
        {
            ListView listView = new();

            container.Add(new Label("Transitions:"));
            container.Add(GetSpace());

            SetListPadding(listView);
            SetListBorder(listView);
            SetListBorderRadius(listView);
            SetListBorderColor(listView);

            SerializedProperty transitions = serializedObject.FindProperty("transitions");

            // TODO: Recieve transitions change callback to redraw transitions listView 
            // Or bind list to transitions list to ListView

            if(transitions.arraySize == 0)
            {
                listView.hierarchy.Add(new Label("List is Empty"));
            }
            else
            {
                FillTransitionList(transitions, listView);
            }

            container.Add(listView);
        }

        void SetListBorderColor(ListView listView)
        {
            listView.style.borderBottomColor = new StyleColor(Color.black);
            listView.style.borderLeftColor = new StyleColor(Color.black);
            listView.style.borderRightColor = new StyleColor(Color.black);
            listView.style.borderTopColor = new StyleColor(Color.black);
        }

        void SetListBorderRadius(ListView listView)
        {
            listView.style.borderTopLeftRadius = 5;
            listView.style.borderTopRightRadius = 5;
            listView.style.borderBottomLeftRadius = 5;
            listView.style.borderBottomRightRadius = 5;
        }

        void SetListBorder(ListView listView)
        {
            listView.style.borderTopWidth = 1;
            listView.style.borderBottomWidth = 1;
            listView.style.borderLeftWidth = 1;
            listView.style.borderRightWidth = 1;
        }

        void SetListPadding(ListView listView)
        {
            listView.style.paddingBottom = 5;
            listView.style.paddingLeft = 20;
            listView.style.paddingRight = 20;
            listView.style.paddingTop = 5;
        }

        void FillTransitionList(SerializedProperty transitions, ListView listView)
        {
            for(int i = 0; i < transitions.arraySize; i++)
            {
                SerializedProperty transition = transitions.GetArrayElementAtIndex(i);
                listView.hierarchy.Add(new PropertyField(transition));
                listView.hierarchy.Add(GetSpace());
            }
        }

        VisualElement GetSpace()
        {
            PropertyField space = new();
            space.style.marginBottom = 10;
            return space;
        }
    }
}