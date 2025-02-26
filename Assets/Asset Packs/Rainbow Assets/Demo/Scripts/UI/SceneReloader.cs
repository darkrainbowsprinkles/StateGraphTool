
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RainbowAssets.Demo
{
    /// <summary>
    /// Reloads the current scene when the attached button is clicked.
    /// </summary>
    public class SceneReloader : MonoBehaviour
    {
        // LIFECYCLE METHODS

        void OnEnable()
        {
            GetComponent<Button>().onClick.AddListener(ReloadScene);
        }

        void OnDisable()
        {
            GetComponent<Button>().onClick.RemoveListener(ReloadScene);
        }

        /// <summary>
        /// Reloads the currently active scene.
        /// </summary>
        void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
