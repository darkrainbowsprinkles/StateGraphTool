using System.Linq;

namespace RainbowAssets.StateMachine
{
    /// <summary>
    /// Represents the entry point of the state machine.
    /// This state automatically transitions to the first defined state.
    /// </summary>
    public class EntryState : State
    {
        /// <summary>
        /// Retrieves the unique identifier of the state that this entry state transitions to.
        /// </summary>
        /// <returns>The ID of the first transition's destination state.</returns>
        public string GetEntryStateID()
        {
            return GetTransitions().ToList()[0].GetTrueStateID();
        }
    }
}