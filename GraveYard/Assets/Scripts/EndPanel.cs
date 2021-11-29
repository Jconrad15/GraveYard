using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GraveYard
{
    public class EndPanel : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private Color winColor;

        [SerializeField]
        private Color loseColor;

        public void Display(bool win)
        {
            gameObject.SetActive(true);

            if (win)
            {
                gameObject.GetComponent<Image>().color = winColor;
                text.SetText("Victory");
            }
            else
            {
                gameObject.GetComponent<Image>().color = loseColor;
                text.SetText("Defeat");
            }

        }

        public void ReturnStartMenu()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }


    }
}