using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace RainbowAssets.StateMachine.Editor
{
    /// <summary>
    /// Editor window for managing and visualizing state machines within the Unity Editor.
    /// </summary>
    public class StateMachineEditor : EditorWindow
    {
        // <summary>
        /// The visual component used to display the StateMachine.
        /// </summary>
        StateMachineView stateMachineView;

        /// <summary>
        /// The currently selected StateMachineController for updating the view.
        /// </summary>
        StateMachineController currentController;

        /// <summary>
        /// Gets the directory path of this editor script dynamically.
        /// </summary>
        public static string GetPath()
        {
            string[] guids = AssetDatabase.FindAssets("StateMachineEditor t:Script");

            if (guids.Length > 0)
            {
                string scriptPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                return System.IO.Path.GetDirectoryName(scriptPath).Replace("\\", "/") + "/";
            }

            return "Assets/";
        }

        /// <summary>
        /// Displays the State Machine Editor window.
        /// </summary>
        [MenuItem("Tools/State Graph")]
        public static void ShowWindow()
        {
            GetWindow(typeof(StateMachineEditor), false, "State Graph");
        }

        /// <summary>
        /// Invoked when a StateMachine asset is opened in the editor.
        /// </summary>
        /// <returns>Returns true if the StateMachine was successfully opened in the editor.</returns>
        [OnOpenAsset]
        public static bool OnStateMachineOpened(int instanceID, int line)
        {
            StateMachine stateMachine = EditorUtility.InstanceIDToObject(instanceID) as StateMachine;

            if (stateMachine != null)
            {
                ShowWindow();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Initializes the editor window by loading the UXML layout and setting up the StateMachineView.
        /// </summary>
        void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(GetPath() + "StateMachineEditor.uxml");
            visualTree.CloneTree(root);

            stateMachineView = root.Q<StateMachineView>();

            OnSelectionChange();
        }

        /// <summary>
        /// Called when the selection in the editor changes. Updates the StateMachineView to reflect the selected StateMachine.
        /// </summary>
        void OnSelectionChange()
        {
            StateMachine stateMachine = Selection.activeObject as StateMachine;
            currentController = null;

            if (Selection.activeGameObject)
            {
                if (Selection.activeGameObject.TryGetComponent(out StateMachineController controller))
                {
                    currentController = controller;
                    stateMachine = controller.GetStateMachine();
                }
            }

            if (stateMachine != null)
            {
                stateMachineView.Refresh(stateMachine);
            }
        }

        /// <summary>
        /// Initializes the editor window by subscribing to the play mode state change event.
        /// </summary>
        void OnEnable()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        /// <summary>
        /// Unsubscribes from the play mode state change event when the editor window is disabled.
        /// </summary>
        void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        /// <summary>
        /// Responds to changes in the play mode state. Updates the state machine view when entering play or edit mode.
        /// </summary>
        /// <param name="change">The change in the play mode state.</param>
        void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            if (stateMachineView != null)
            {
                if (change == PlayModeStateChange.EnteredEditMode)
                {
                    OnSelectionChange();
                }

                if (change == PlayModeStateChange.EnteredPlayMode)
                {
                    OnSelectionChange();
                }
            }
        }

        /// <summary>
        /// Updates the state machine view during the editor's inspector update cycle.
        /// </summary>
        void OnInspectorUpdate()
        {
            if (stateMachineView != null && Application.isPlaying)
            {
                stateMachineView.UpdateStates(currentController);
            }
        }
    }
}
