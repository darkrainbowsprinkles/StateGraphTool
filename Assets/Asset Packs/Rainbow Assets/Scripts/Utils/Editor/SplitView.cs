using UnityEngine.UIElements;

namespace RainbowAssets.Utils.Editor
{
    /// <summary>
    /// A custom split view that extends TwoPaneSplitView.
    /// </summary>
    public class SplitView : TwoPaneSplitView
    {
        /// <summary>
        /// Factory class for creating SplitView instances from UXML.
        /// </summary>
        new class UxmlFactory : UxmlFactory<SplitView, UxmlTraits> { }
    }
}