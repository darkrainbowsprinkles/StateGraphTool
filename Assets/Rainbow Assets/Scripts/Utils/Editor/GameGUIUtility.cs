using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace RainbowAssets.Utils.Editor
{
    public static class GameGUIUtlity
    {
        const string inputActionsAssetPath = "Assets/Rainbow Assets/Demo/Assets/State Machine Input Actions.inputactions";

        public static void DrawTextField(VisualElement container, SerializedProperty property, string label)
        {
            TextField messageField = new(label);
            messageField.BindProperty(property);

            messageField.RegisterValueChangedCallback(evt =>
            {
                property.stringValue = messageField.value;
                property.serializedObject.ApplyModifiedProperties();
            });

            container.Add(messageField);
        }

        public static void DrawInputActions(VisualElement container, SerializedProperty property)
        {
            List<string> inputActionNames = GetInputActionNames();

            if (inputActionNames == null || inputActionNames.Count == 0)
            {
                DrawTextField(container, property, "Input Action");
                container.Add(new HelpBox($"InputActionAsset not found at '{inputActionsAssetPath}", HelpBoxMessageType.Info));
                return;
            }

            string currentValue = property.stringValue;

            if (string.IsNullOrEmpty(currentValue) || !inputActionNames.Contains(currentValue))
            {
                currentValue = inputActionNames[0];
                property.stringValue = currentValue;
                property.serializedObject.ApplyModifiedProperties();
            }

            PopupField<string> popup = new("Input Action", inputActionNames, currentValue);

            popup.RegisterValueChangedCallback(evt =>
            {
                property.stringValue = evt.newValue;
                property.serializedObject.ApplyModifiedProperties();
            });

            container.Add(popup);
        }

        static List<string> GetInputActionNames()
        {
            InputActionAsset inputActions = AssetDatabase.LoadAssetAtPath<InputActionAsset>(inputActionsAssetPath);

            if (inputActions == null)
            {
                return null;
            }

            List<string> inputActionNames = new();

            foreach (InputAction action in inputActions)
            {
                inputActionNames.Add(action.name);
            }

            return inputActionNames;
        }
    }
}