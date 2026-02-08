using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;

namespace RainbowAssets.StateMachine.Editor
{
    public class StateMachineView : GraphView
    {
        new class UxmlFactory : UxmlFactory<StateMachineView, UxmlTraits> { }

        StateMachine stateMachine;
        StateMachineController controller;

        public StateMachineView()
        {
            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(StateMachineEditor.GetPath() + "StateMachineEditor.uss");
            styleSheets.Add(styleSheet);

            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());

            Undo.undoRedoPerformed += () => Refresh(stateMachine, controller);
            EditorApplication.update += UpdateRunningState;
        }

        public void Refresh(StateMachine stateMachine, StateMachineController controller)
        {
            this.stateMachine = stateMachine;
            this.controller = controller;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            if (stateMachine == null)
            {
                return;
            }
        
            foreach (State state in stateMachine.GetStates())
            {
                CreateStateView(state);
            }

            foreach (State state in stateMachine.GetStates())
            {
                foreach (Transition transition in state.GetTransitions())
                {
                    CreateTransitionEdge(transition);
                }
            }
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if (!Application.isPlaying)
            {
                base.BuildContextualMenu(evt);
                Vector2 mousePosition = viewTransform.matrix.inverse.MultiplyPoint(evt.localMousePosition);
                evt.menu.AppendAction($"Create State", _ => CreateState(typeof(ActionState), mousePosition));
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new();

            foreach (Port endPort in ports)
            {
                if (endPort.direction == startPort.direction)
                {
                    continue;
                }

                if (PortsConnected(startPort, endPort))
                {
                    continue;
                }

                compatiblePorts.Add(endPort);
            }

            return compatiblePorts;
        }

        bool PortsConnected(Port startPort, Port endPort)
        {
            foreach (Edge connection in startPort.connections)
            {
                if (connection.input == endPort || connection.output == endPort)
                {
                    return true;
                }
            }

            return false;
        }

        void CreateStateView(State state)
        {
            StateView newStateView = new(state);
            AddElement(newStateView);
        }

        void CreateState(Type type, Vector2 mousePosition)
        {
            State newState = stateMachine.CreateState(type, mousePosition);
            CreateStateView(newState);
        }

        void RemoveState(StateView stateView)
        {
            stateMachine.RemoveState(stateView.GetState());
        }

        void CreateTransitionEdge(Transition transition)
        {
            StateView rootStateView = GetNodeByGuid(transition.GetRootStateID()) as StateView;
            StateView trueStateView = GetNodeByGuid(transition.GetTrueStateID()) as StateView;
            AddElement(rootStateView.ConnectTo(trueStateView));
        }

        void CreateTransition(TransitionEdge edge)
        {
            State rootState = stateMachine.GetState(edge.output.node.viewDataKey);
            rootState.AddTransition(edge.input.node.viewDataKey);
        }

        void RemoveTransition(TransitionEdge edge)
        {
            State rootState = stateMachine.GetState(edge.output.node.viewDataKey);
            rootState.RemoveTransition(edge.input.node.viewDataKey);
        }

        GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            List<Edge> edgesToCreate = graphViewChange.edgesToCreate;

            if (edgesToCreate != null)
            {
                foreach (Edge edge in edgesToCreate)
                {
                    CreateTransition(edge as TransitionEdge);
                }
            }

            List<GraphElement> elementsToRemove = graphViewChange.elementsToRemove;

            if (elementsToRemove != null)
            {
                foreach (GraphElement element in elementsToRemove)
                {
                    if (element is StateView stateView)
                    {
                        RemoveState(stateView);
                    }

                    if (element is TransitionEdge transitionEdge)
                    {
                        RemoveTransition(transitionEdge);
                    }
                }
            }

            return graphViewChange;
        }

        void UpdateRunningState()
        {
            if (!Application.isPlaying || controller == null)
            {
                ClearRunningStates();
                return;
            }

            State currentState = controller.GetCurrentState();

            foreach (Node node in nodes)
            {
                if (node is StateView stateView)
                {
                    bool isRunning = stateView.GetState() == currentState;
                    stateView.SetRunning(isRunning);
                }
            }
        }

        void ClearRunningStates()
        {
            foreach (Node node in nodes)
            {
                if (node is StateView stateView)
                {
                    stateView.SetRunning(false);
                }
            }
        }
    }
}