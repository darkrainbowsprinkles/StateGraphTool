using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine.UIElements;

namespace RainbowAssets.StateMachine.Editor
{
    /// <summary>
    /// Editor window for managing and visualizing state machines within the Unity Editor.
    /// </summary>
    public class StateMachineEditor : EditorWindow
    {
        /// <summary>
        /// The path to the resources for the StateMachineEditor.
        /// </summary>
        public const string path = "Assets/Asset Packs/Rainbow Assets/Scripts/State Machine/Editor/";

        /// <summary>
        /// The visual component used to display the StateMachine.
        /// </summary>
        StateMachineView stateMachineView;

        /// <summary>
        /// The editor to display data of each state.
        /// </summary>
        StateEditor stateEditor;

        /// <summary>
        /// Displays the State Machine Editor window.
        /// </summary>
        [MenuItem("Rainbow Assets/State Machine Editor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(StateMachineEditor), false, "State Machine Editor");
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

            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path + "StateMachineEditor.uxml");
            visualTree.CloneTree(root);

            stateMachineView = root.Q<StateMachineView>();
            stateEditor = root.Q<StateEditor>();

            stateMachineView.onStateSelected = OnStateSelected;

            OnSelectionChange();
        }

        /// <summary>
        /// Called when the selection in the editor changes. Updates the StateMachineView to reflect the selected StateMachine.
        /// </summary>
        void OnSelectionChange()
        {
            StateMachine stateMachine = Selection.activeObject as StateMachine;

            if (Selection.activeGameObject)
            {
                StateMachineController controller = Selection.activeGameObject.GetComponent<StateMachineController>();

                if (controller != null)
                {
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
        /// Refreshes the state editor every time a state is selected.
        /// </summary>
        void OnStateSelected(State state)
        {
            stateEditor.Refresh(state);
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
            if (stateMachineView != null)
            {
                stateMachineView.UpdateStates();
            }
        }
    }
}
