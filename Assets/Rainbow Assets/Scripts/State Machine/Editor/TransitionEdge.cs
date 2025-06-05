using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace RainbowAssets.StateMachine.Editor
{
    /// <summary>
    /// Represents a transition edge in the state machine graph.
    /// </summary>
    public class TransitionEdge : Edge
    {
        /// <summary>
        /// The offset for the edge when positioning points horizontally.
        /// </summary>
        const float edgeOffset = 4;

        /// <summary>
        /// The width of the transition arrow.
        /// </summary>
        const float arrowWidth = 12;

        /// <summary>
        /// The vertical offset used when drawing a self-transition arrow.
        /// </summary>
        const float selfArrowOffset = 35;

        /// <summary>
        /// Initializes the transition edge by registering callbacks for geometry changes and visual content drawing.
        /// </summary>
        public TransitionEdge()
        {
            edgeControl.RegisterCallback<GeometryChangedEvent>(OnEdgeGeometryChanged);
            generateVisualContent += DrawArrow;
        }

        /// <summary>
        /// Determines whether a given point is within the bounds of the transition edge
        /// </summary>
        public override bool ContainsPoint(Vector2 localPoint)
        {
            if(base.ContainsPoint(localPoint))
            {
                return true; 
            }

            Vector2 start = PointsAndTangents[PointsAndTangents.Length / 2 - 1];
            Vector2 end = PointsAndTangents[PointsAndTangents.Length / 2];
            Vector2 mid = (start + end) / 2;

            if(IsSelfTransition())
            {
                mid = PointsAndTangents[0] + Vector2.up * selfArrowOffset;
            }

            return (localPoint - mid).sqrMagnitude <= (arrowWidth * arrowWidth);
        }


        /// <summary>
        /// Callback method for handling geometry changes in the edge's layout.
        /// </summary>
        /// <param name="evt">The event data containing information about the geometry change.</param>
        void OnEdgeGeometryChanged(GeometryChangedEvent evt)
        {
            PointsAndTangents[1] = PointsAndTangents[0];
            PointsAndTangents[2] = PointsAndTangents[3];

            if(input != null && output != null)
            {   
                AddHorizontalOffset();
                AddVerticalOffset();
            }

            MarkDirtyRepaint();
        }

        /// <summary>
        /// Adds a horizontal offset to the edge points based on the relative positions of the input and output nodes.
        /// </summary>
        void AddHorizontalOffset()
        {
            if(input.node.GetPosition().x > output.node.GetPosition().x)
            {
                PointsAndTangents[1].y -= edgeOffset;
                PointsAndTangents[2].y -= edgeOffset;
            }
            else if(input.node.GetPosition().x < output.node.GetPosition().x)
            {
                PointsAndTangents[1].y += edgeOffset;
                PointsAndTangents[2].y += edgeOffset;
            }
        }

        /// <summary>
        /// Adds a vertical offset to the edge points based on the relative positions of the input and output nodes.
        /// </summary>
        void AddVerticalOffset()
        {
            if(input.node.GetPosition().y > output.node.GetPosition().y)
            {
                PointsAndTangents[1].x += edgeOffset;
                PointsAndTangents[2].x += edgeOffset;
            }
            else if(input.node.GetPosition().y < output.node.GetPosition().y)
            {
                PointsAndTangents[1].x -= edgeOffset;
                PointsAndTangents[2].x -= edgeOffset;
            }
        }

        /// <summary>
        /// Draws the arrow for the transition edge based on its points and tangents.
        /// </summary>
        /// <param name="context">The mesh generation context used to allocate vertices and set visual data.</param>
        void DrawArrow(MeshGenerationContext context)
        {
            Vector2 start = PointsAndTangents[PointsAndTangents.Length / 2 - 1];
            Vector2 end = PointsAndTangents[PointsAndTangents.Length / 2];
            Vector2 mid = (start + end) / 2;
            Vector2 direction = end - start;

            if(IsSelfTransition())
            {
                mid = PointsAndTangents[0] + Vector2.up * selfArrowOffset;
                direction = Vector2.down;
            }

            float distanceFromMid = arrowWidth * Mathf.Sqrt(3) / 4;
            float angle = Vector2.SignedAngle(Vector2.right, direction);
            float perpendicularLength = GetPerpendicularLength(angle);

            Vector2 perpendicular = new Vector2(-direction.y, direction.x).normalized * perpendicularLength;

            if(IsSelfTransition())
            {
                perpendicular = Vector2.right * perpendicularLength;
            }

            MeshWriteData mesh = context.Allocate(3, 3);
            Vertex[] vertices = new Vertex[3];

            vertices[0].position = mid + (direction.normalized * distanceFromMid);
            vertices[1].position = mid + (-direction.normalized * distanceFromMid) + (perpendicular.normalized * arrowWidth / 2);
            vertices[2].position = mid + (-direction.normalized * distanceFromMid) + (-perpendicular.normalized * arrowWidth / 2);

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].position += Vector3.forward * Vertex.nearZ;
                vertices[i].tint = GetColor();
            }

            mesh.SetAllVertices(vertices);
            mesh.SetAllIndices(new ushort[] { 0, 1, 2 });
        }

        /// <summary>
        /// Determines if the transition is a self-transition (where the input and output nodes are the same).
        /// </summary>
        /// <returns>True if the transition is a self-transition; otherwise, false.</returns>
        bool IsSelfTransition()
        {   
            if(input != null && output != null)
            {
                return input.node == output.node;
            }

            return false;
        }

        /// <summary>
        /// Calculates the length of the perpendicular line for the arrow's direction based on the angle.
        /// </summary>
        /// <param name="angle">The angle of the arrow in degrees.</param>
        /// <returns>The perpendicular length used for arrow drawing.</returns>
        float GetPerpendicularLength(float angle)
        {
            if(angle < 60 && angle > 0)
            {
                return arrowWidth / (Mathf.Sin(Mathf.Deg2Rad * (angle + 120)) * 2);
            }
            else if(angle > -120 && angle < -60)
            {
                return arrowWidth / (Mathf.Sin(Mathf.Deg2Rad * (angle - 120)) * 2);
            }
            else if(angle > -60 && angle < 0)
            {
                return arrowWidth / (Mathf.Sin(Mathf.Deg2Rad * (angle + 60)) * 2);
            }

            return arrowWidth / (Mathf.Sin(Mathf.Deg2Rad * (angle - 60)) * 2);
        }

        /// <summary>
        /// Retrieves the color used for the transition edge based on its state.
        /// </summary>
        /// <returns>The color of the transition edge.</returns>
        Color GetColor()
        {
            Color color = defaultColor;

            if(output != null)
            {
                color = output.portColor;
            }

            if(selected)
            {
                color = selectedColor;
            }
            
            if(isGhostEdge)
            {
                color = ghostColor;
            }

            return color;
        }
    }
}
