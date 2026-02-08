using System.Collections.Generic;
using UnityEngine;

namespace RainbowAssets.Utils
{
    [System.Serializable]
    public class Action
    {
        [SerializeField] EAction action;
        [SerializeField] string[] parameters;

        public void PerformAction(IEnumerable<IActionPerformer> performers)
        {   
            foreach(var performer in performers)
            {
                performer.PerformAction(action, parameters);
            }
        }
    }
}
