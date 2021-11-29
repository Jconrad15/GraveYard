using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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


    }
}