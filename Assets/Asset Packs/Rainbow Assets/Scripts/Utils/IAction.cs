namespace RainbowAssets.Utils
{
    /// <summary>
    /// Interface that defines a contract for actions to be performed.
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// Executes an action with the given parameters.
        /// </summary>
        /// <param name="action">The action to perform</param>
        /// <param name="parameters">An array of parameters to customize the behavior of the action.</param>
        void DoAction(EAction action, string[] parameters);
    }
}