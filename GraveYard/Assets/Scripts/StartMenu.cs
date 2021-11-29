using UnityEngine;
using UnityEngine.SceneManagement;

namespace GraveYard
{
    public class StartMenu : MonoBehaviour
    {
        private void Update()
        {
            if (Input.anyKey)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}