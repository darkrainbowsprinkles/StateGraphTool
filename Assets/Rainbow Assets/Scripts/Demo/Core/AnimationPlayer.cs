using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.Demo.Core
{
    /// <summary>
    /// Handles smooth animation playback.
    /// </summary>
    public class AnimationPlayer : MonoBehaviour, IAction, IPredicateEvaluator
    {
        /// <summary>
        /// The duration for animation cross-fading.
        /// </summary>
        [SerializeField] float crossFadeTime = 0.1f;
        Animator animator;

        /// <summary>
        /// Updates the specified animation parameter with a smoothed transition.
        /// </summary>
        /// <param name="parameter">The name of the parameter.</param>
        /// <param name="value">The target value for the parameter.</param>
        public void UpdateParameter(string parameter, float value)
        {
            animator.SetFloat(parameter, value, crossFadeTime, Time.deltaTime);
        }

        // LIFECYCLE METHODS

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Checks if the current animation with the specified tag has finished playing.
        /// </summary>
        /// <param name="tag">The animation tag to check.</param>
        /// <returns>True if the animation has completed, otherwise false.</returns>
        bool AnimationOver(string tag)
        {
            var currentInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (currentInfo.IsTag(tag) && !animator.IsInTransition(0))
            {
                return currentInfo.normalizedTime >= 1;
            }

            return false;
        }

        /// <summary>
        /// Smoothly plays an animation using cross-fading.
        /// </summary>
        /// <param name="name">The name of the animation to play.</param>
        void PlaySmooth(string name)
        {
            animator.CrossFadeInFixedTime(name, crossFadeTime);
        }

        /// <summary>
        /// Executes an action related to animations.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="parameters">Additional parameters for the action.</param>
        void IAction.DoAction(EAction action, string[] parameters)
        {
            switch (action)
            {
                case EAction.PlayAnimation:

                    if (parameters.Length > 1)
                    {
                        int randomIndex = Random.Range(0, parameters.Length);
                        PlaySmooth(parameters[randomIndex]);
                    }
                    else
                    {
                        PlaySmooth(parameters[0]);
                    }

                    break;
            }
        }

        /// <summary>
        /// Evaluates a predicate related to animation state.
        /// </summary>
        /// <param name="predicate">The predicate to evaluate.</param>
        /// <param name="parameters">Additional parameters for evaluation.</param>
        /// <returns>True if the predicate condition is met, otherwise false. Returns null if the predicate is unsupported.</returns>
        public bool? Evaluate(EPredicate predicate, string[] parameters)
        {
            switch (predicate)
            {
                case EPredicate.AnimationOver:
                    return AnimationOver(parameters[0]);
            }

            return null;
        }
    }
}