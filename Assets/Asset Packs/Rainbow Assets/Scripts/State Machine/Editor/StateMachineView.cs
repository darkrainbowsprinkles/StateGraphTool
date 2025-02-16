using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;

namespace RainbowAssets.StateMachine.Editor
{
    /// <summary>
    /// A custom GraphView used for displaying and interacting with a StateMachine in the Unity Editor.
    /// </summary>
    public class StateMachineView : GraphView
    {
        /// <summary>
        /// A factory for creating instances of the StateMachineView from UXML.
        /// </summary>
        new class UxmlFactory : UxmlFactory<StateMachineView, UxmlTraits> { }

        /// <summary>
        /// The state machine being visualized in this view.
        /// </summary>
        StateMachine stateMachine;

        /// <summary>
        /// Initializes the StateMachineView, loading the necessary stylesheet and setting up manipulators for interaction.
        /// </summary>
        public StateMachineView()
        {
            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(StateMachineEditor.path + "StateMachineEditor.uss");
            styleSheets.Add(styleSheet);

            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());

            Undo.undoRedoPerformed += OnUndoRedo;
        }

        /// <summary>
        /// Refreshes the StateMachineView to reflect the current StateMachine states and transitions.
        /// </summary>
        /// <param name="stateMachine">The StateMachine to display in the view.</param>
        public void Refresh(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;

            graphViewChanged -= OnGraphViewChanged;

            DeleteElements(graphElements);

            graphViewChanged += OnGraphViewChanged;

            if (stateMachine != null)
            {
                foreach (var state in stateMachine.GetStates())
                {
                    CreateStateView(state);
                }

                foreach (var state in stateMachine.GetStates())
                {
                    foreach (var transition in state.GetTransitions())
                    {
                        CreateTransitionEdge(transition);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the visual representation of the states in the graph view.
        /// </summary>
        public void UpdateStates()
        {
            foreach (var node in nodes)
            {
                StateView stateView = node as StateView;

                if (stateView != null)
                {
                    stateView.UpdateState();
                }
            }
        }

        /// <summary>
        /// Builds the contextual menu for the state machine view.
        /// </summary>
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if (!Application.isPlaying)
            {
                base.BuildContextualMenu(evt);
                Vector2 mousePosition = viewTransform.matrix.inverse.MultiplyPoint(evt.localMousePosition);
                evt.menu.AppendAction($"Create State", a => CreateState(typeof(ActionState), mousePosition));
            }
        }

        /// <summary>
        /// Retrieves a list of compatible ports for connecting nodes in the graph view.
        /// </summary>
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            foreach (var endPort in ports)
            {
                if (endPort.direction == startPort.direction)
                {
                    continue;
                }

                if (AreConnected(startPort, endPort))
                {
                    continue;
                }

                compatiblePorts.Add(endPort);
            }

            return compatiblePorts;
        }

        /// <summary>
        /// Checks if two ports are already connected.
        /// </summary>
        /// <param name="startPort">The starting port to check.</param>
        /// <param name="endPort">The ending port to check.</param>
        /// <returns>True if the ports are connected, false otherwise.</returns>
        bool AreConnected(Port startPort, Port endPort)
        {
            foreach (var connection in startPort.connections)
            {
                if (connection.input == endPort || connection.output == endPort)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates a visual representation of a state in the graph view.
        /// </summary>
        /// <param name="state">The state to create a view for.</param>
        void CreateStateView(State state)
        {
            StateView newStateView = new(state);
            AddElement(newStateView);
        }

        /// <summary>
        /// Creates a new state at the given position in the graph view.
        /// </summary>
        /// <param name="type">The type of state to create.</param>
        /// <param name="mousePosition">The position in the graph to create the state.</param>
        void CreateState(Type type, Vector2 mousePosition)
        {
            State newState = stateMachine.CreateState(type, mousePosition);
            CreateStateView(newState);
        }

        /// <summary>
        /// Removes a state from the StateMachine and its visual representation.
        /// </summary>
        /// <param name="stateView">The StateView to remove.</param>
        void RemoveState(StateView stateView)
        {
            stateMachine.RemoveState(stateView.GetState());
        }

        /// <summary>
        /// Retrieves a StateView by its state ID.
        /// </summary>
        /// <param name="stateID">The ID of the state to retrieve.</param>
        /// <returns>The StateView corresponding to the given ID.</returns>
        StateView GetStateView(string stateID)
        {
            return GetNodeByGuid(stateID) as StateView;
        }

        /// <summary>
        /// Creates a TransitionEdge between two states in the graph view.
        /// </summary>
        /// <param name="transition">The transition to create an edge for.</param>
        void CreateTransitionEdge(Transition transition)
        {
            StateView rootStateView = GetStateView(transition.GetRootStateID());
            StateView trueStateView = GetStateView(transition.GetTrueStateID());
            AddElement(rootStateView.ConnectTo(trueStateView));
        }

        /// <summary>
        /// Creates a new transition between two states based on the provided edge.
        /// </summary>
        /// <param name="edge">The TransitionEdge to create.</param>
        void CreateTransition(TransitionEdge edge)
        {
            State rootState = stateMachine.GetState(edge.output.node.viewDataKey);
            rootState.AddTransition(edge.input.node.viewDataKey);
        }

        /// <summary>
        /// Removes a transition from the StateMachine based on the provided edge.
        /// </summary>
        /// <param name="edge">The TransitionEdge to remove.</param>
        void RemoveTransition(TransitionEdge edge)
        {
            State rootState = stateMachine.GetState(edge.output.node.viewDataKey);
            rootState.RemoveTransition(edge.input.node.viewDataKey);
        }

        /// <summary>
        /// Handles changes in the graph view.
        /// </summary>
        GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            var edgesToCreate = graphViewChange.edgesToCreate;

            if (edgesToCreate != null)
            {
                foreach (var edge in edgesToCreate)
                {
                    CreateTransition(edge as TransitionEdge);
                }
            }

            var elementsToRemove = graphViewChange.elementsToRemove;

            if (elementsToRemove != null)
            {
                foreach (var element in elementsToRemove)
                {
                    StateView stateView = element as StateView;

                    if (stateView != null)
                    {
                        RemoveState(stateView);
                    }

                    TransitionEdge transitionEdge = element as TransitionEdge;

                    if (transitionEdge != null)
                    {
                        RemoveTransition(transitionEdge);
                    }
                }
            }

            return graphViewChange;
        }

        /// <summary>
        /// Refreshes the StateMachineView after Undo/Redo actions.
        /// </summary>
        void OnUndoRedo()
        {
            Refresh(stateMachine);
        }
    }
}