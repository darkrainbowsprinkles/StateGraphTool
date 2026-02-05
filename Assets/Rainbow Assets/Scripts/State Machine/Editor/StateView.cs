using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace RainbowAssets.StateMachine.Editor
{
    /// <summary>
    /// Represents a visual representation of a state in the state machine graph.
    /// </summary>
    public class StateView : Node
    {
        /// <summary>
        /// The state that this StateView represents.
        /// </summary>
        State state;

        /// <summary>
        /// The input port for connecting other StateViews.
        /// </summary>
        Port inputPort;

        /// <summary>
        /// The output port for connecting to other StateViews.
        /// </summary>
        Port outputPort;

        /// <summary>
        /// The container for the state node's border.
        /// </summary>
        VisualElement borderContainer;

        /// <summary>
        /// The container for the state update container.
        /// </summary>
        VisualElement updateContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateView"/> class.
        /// </summary>
        /// <param name="state">The state associated with this view.</param>
        public StateView(State state) : base(StateMachineEditor.GetPath() + "StateView.uxml")
        {
            this.state = state;

            viewDataKey = state.GetUniqueID();

            style.left = state.GetPosition().x;
            style.top = state.GetPosition().y;

            borderContainer = this.Q<VisualElement>("node-border");
            updateContainer = this.Q<VisualElement>("state-update");

            CreatePorts();
            SetTitle();
            SetStyle();
            SetCapabilites();
        }

        /// <summary>
        /// Gets the <see cref="State"/> associated with this <see cref="StateView"/>.
        /// </summary>
        /// <returns>The state associated with this view.</returns>
        public State GetState()
        {
            return state;
        }

        /// <summary>
        /// Connects this StateView's output port to another StateView's input port.
        /// </summary>
        /// <param name="stateView">The destination StateView to connect to.</param>
        /// <returns>The created <see cref="TransitionEdge"/> between the two StateViews.</returns>
        public TransitionEdge ConnectTo(StateView stateView)
        {
            return outputPort.ConnectTo<TransitionEdge>(stateView.inputPort);
        }

        /// <summary>
        /// Updates the state in the view if the application is running.
        /// </summary>
        public void SetRunning(bool running)
        {
            if (Application.isPlaying)
            {
                if (running)
                {
                    updateContainer.AddToClassList("runningState");
                }
                else
                {   
                    updateContainer.RemoveFromClassList("runningState");
                }
            }
        }

        /// <summary>
        /// Sets the position of the StateView and updates the position of the associated state.
        /// </summary>
        /// <param name="newPos">The new position for the StateView.</param>
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            state.SetPosition(new Vector2(newPos.x, newPos.y));
        }

        /// <summary>
        /// Called when the StateView is selected.
        /// </summary>
        public override void OnSelected()
        {
            base.OnSelected();
            Selection.SetActiveObjectWithContext(state, null);
        }

        /// <summary>
        /// Builds a contextual menu for the StateView. 
        /// </summary>
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if (!Application.isPlaying)
            {
                evt.menu.AppendAction("Add Transition", a => DragTransitionEdge());
            }
        }

        /// <summary>
        /// Collects all elements related to this StateView into a set.
        /// </summary>
        public override void CollectElements(HashSet<GraphElement> collectedElementSet, Func<GraphElement, bool> conditionFunc)
        {
            base.CollectElements(collectedElementSet, conditionFunc);

            if (inputPort != null)
            {
                foreach (var connection in inputPort.connections)
                {
                    collectedElementSet.Add(connection);
                }
            }

            if (outputPort != null)
            {
                foreach (var connection in outputPort.connections)
                {
                    collectedElementSet.Add(connection);
                }
            }
        }

        /// <summary>
        /// Creates the input and output ports for the StateView, based on the type of state.
        /// </summary>
        void CreatePorts()
        {
            if (state is ActionState)
            {
                inputPort = CreatePort(Direction.Input, Port.Capacity.Multi);
                outputPort = CreatePort(Direction.Output, Port.Capacity.Multi);
            }

            if (state is EntryState)
            {
                outputPort = CreatePort(Direction.Output, Port.Capacity.Single);
            }

            if (state is AnyState)
            {
                outputPort = CreatePort(Direction.Output, Port.Capacity.Multi);
            }
        }

        /// <summary>
        /// Creates a port with the specified direction and capacity.
        /// </summary>
        /// <param name="direction">The direction of the port (input or output).</param>
        /// <param name="capacity">The capacity of the port (single or multi).</param>
        /// <returns>The created port.</returns>
        Port CreatePort(Direction direction, Port.Capacity capacity)
        {
            Port port = Port.Create<TransitionEdge>(Orientation.Vertical, direction, capacity, typeof(bool));
            Insert(0, port);
            return port;
        }

        /// <summary>
        /// Sets the title for the state.
        /// </summary>
        void SetTitle()
        {
            if (state is ActionState)
            {
                BindTitle();
            }
            else
            {
                title = state.GetTitle();
            }
        }

        /// <summary>
        /// Binds the title of the state dynamically to the UI label.
        /// </summary>
        void BindTitle()
        {
            Label titleLabel = this.Q<Label>("title-label");
            titleLabel.bindingPath = "title";
            titleLabel.Bind(new SerializedObject(state));
        }

        /// <summary>
        /// Sets the style for the StateView based on the type of state.
        /// </summary>
        void SetStyle()
        {
            if (state is ActionState)
            {
                borderContainer.AddToClassList("actionState");
            }

            if (state is EntryState)
            {
                borderContainer.AddToClassList("entryState");
            }

            if (state is AnyState)
            {
                borderContainer.AddToClassList("anyState");
            }
        }

        /// <summary>
        /// Sets the capabilities of the StateView based on the type of state.
        /// </summary>
        void SetCapabilites()
        {
            if (state is EntryState || state is AnyState)
            {
                capabilities -= Capabilities.Deletable;
            }
        }

        /// <summary>
        /// Represents a mouse event triggered by dragging.
        /// </summary>
        class DragEvent : MouseDownEvent
        {
            public DragEvent(Vector2 mousePosition, VisualElement target)
            {
                this.mousePosition = mousePosition;
                this.target = target;
            }
        }

        /// <summary>
        /// Initiates the process of creating a transition edge by sending a drag event to the output port.
        /// </summary>
        void DragTransitionEdge()
        {
            outputPort.SendEvent(new DragEvent(outputPort.GetGlobalCenter(), outputPort));
        }
    }
}
