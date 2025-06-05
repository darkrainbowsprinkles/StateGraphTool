namespace RainbowAssets.Utils
{
    /// <summary>
    /// Interface that defines a contract for evaluating predicates.
    /// </summary>
    public interface IPredicateEvaluator
    {
        /// <summary>
        /// Evaluates a predicate with the given parameters to determine if the condition is met.
        /// </summary>
        /// <param name="predicate">The predicate to evaluate</param>
        /// <param name="parameters">An array of parameters used in evaluating the predicate.</param>
        /// <returns>Returns a nullable boolean: true if the predicate is satisfied, false if not, or null for undefined conditions.</returns>
        bool? Evaluate(EPredicate predicate, string[] parameters);
    }
}