
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RainbowAssets.Demo
{
    public class SceneReloader : MonoBehaviour
    {
        void OnEnable()
        {
            GetComponent<Button>().onClick.AddListener(ReloadScene);
        }

        void OnDisable()
        {
            GetComponent<Button>().onClick.RemoveListener(ReloadScene);
        }

        void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
