using System.Collections.Generic;
using UnityEngine;

namespace RainbowAssets.Utils
{
    /// <summary>
    /// Represents a condition that can be checked using a series of predicates and disjunctions.
    /// </summary>
    [System.Serializable]
    public class Condition
    {
        /// <summary>
        /// An array of disjunctions (AND conditions) that must all be satisfied for the condition to be true.
        /// </summary>
        [SerializeField] Disjunction[] and;

        /// <summary>
        /// Checks whether the condition evaluates to true based on a collection of evaluators.
        /// </summary>
        /// <param name="evaluators">The evaluators used to evaluate the predicates.</param>
        /// <returns>True if all disjunctions are satisfied, otherwise false.</returns>
        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach(var disjunction in and)
            {
                if(!disjunction.Check(evaluators))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Represents a disjunction of predicates (OR conditions) in the condition.
        /// </summary>
        [System.Serializable]
        class Disjunction
        {
            /// <summary>
            /// An array of predicates that must be evaluated as true for this disjunction to be true.
            /// </summary>
            [SerializeField] Predicate[] or;

            /// <summary>
            /// Checks whether any of the predicates in the disjunction evaluates to true.
            /// </summary>
            /// <param name="evaluators">The evaluators used to evaluate the predicates.</param>
            /// <returns>True if at least one predicate is satisfied, otherwise false.</returns>
            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach(var predicate in or)
                {
                    if(predicate.Check(evaluators))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Represents an individual predicate with a specific evaluation and optional negation.
        /// </summary>
        [System.Serializable]
        class Predicate
        {
            /// <summary>
            /// The type of predicate to evaluate.
            /// </summary>
            [SerializeField] EPredicate predicate;

            /// <summary>
            /// Parameters required for evaluating the predicate.
            /// </summary>
            [SerializeField] string[] parameters;

            /// <summary>
            /// A flag indicating whether the predicate evaluation should be negated.
            /// </summary>
            [SerializeField] bool negate = false;

            /// <summary>
            /// Evaluates the predicate using a set of evaluators and checks if the result matches the condition.
            /// </summary>
            /// <param name="evaluators">The evaluators used to evaluate the predicate.</param>
            /// <returns>True if the predicate is satisfied, false if negated or not satisfied.</returns>
            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach(var evaluator in evaluators)
                {
                    bool? result = evaluator.Evaluate(predicate, parameters);

                    if(result == null)
                    {
                        continue;
                    }

                    if(result == negate)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
