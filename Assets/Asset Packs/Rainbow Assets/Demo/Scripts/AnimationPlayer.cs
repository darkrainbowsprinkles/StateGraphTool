using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.Demo
{
    public class AnimationPlayer : MonoBehaviour, IAction, IPredicateEvaluator
    {
        [SerializeField] float crossFadeTime = 0.1f;
        Animator animator;

        public void UpdateParameter(string parameter, float value)
        {
            animator.SetFloat(parameter, value, crossFadeTime, Time.deltaTime);
        }

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        bool AnimationOver(string tag)
        {
            var currentInfo = animator.GetCurrentAnimatorStateInfo(0);

            if(currentInfo.IsTag(tag) && !animator.IsInTransition(0))
            {
                return currentInfo.normalizedTime >= 1;
            }

            return false;
        }

        void PlaySmooth(string name)
        {
            animator.CrossFadeInFixedTime(name, crossFadeTime);
        }

        void IAction.DoAction(EAction action, string[] parameters)
        {
            switch(action)
            {
                case EAction.PlayAnimation:
                
                    if(parameters.Length > 1)
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

        public bool? Evaluate(EPredicate predicate, string[] parameters)
        {
            switch(predicate)
            {
                case EPredicate.AnimationOver:
                    return AnimationOver(parameters[0]);
            }

            return null;
        }
    }
}