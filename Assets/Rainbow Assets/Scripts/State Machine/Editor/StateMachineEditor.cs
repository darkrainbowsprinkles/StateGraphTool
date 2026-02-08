using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace RainbowAssets.StateMachine.Editor
{
    public class StateMachineEditor : EditorWindow
    {
        StateMachineView stateMachineView;

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

        [MenuItem("Tools/State Graph")]
        static void ShowWindow()
        {
            StateMachineEditor window = GetWindow<StateMachineEditor>();
            window.titleContent = new GUIContent("State Machine Editor");
        }

        [OnOpenAsset]
        static bool OnStateMachineOpened(int instanceID, int line)
        {
            StateMachine stateMachine = EditorUtility.InstanceIDToObject(instanceID) as StateMachine;

            if (stateMachine != null)
            {
                ShowWindow();
                return true;
            }

            return false;
        }

        void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(GetPath() + "StateMachineEditor.uxml");
            visualTree.CloneTree(root);

            stateMachineView = root.Q<StateMachineView>();

            OnSelectionChange();
        }

        void OnSelectionChange()
        {
            StateMachine stateMachine = Selection.activeObject as StateMachine;
            StateMachineController controller = null;

            if (Selection.activeGameObject != null && Selection.activeGameObject.TryGetComponent(out controller))
            {
                stateMachine = controller.GetStateMachine();
            }

            if (stateMachine != null)
            {
                stateMachineView.Refresh(stateMachine, controller);
            }
        }
    }
}
