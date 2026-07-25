using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CardSystem
{
    public class StatRowUI : MonoBehaviour
    {
        [SerializeField] private Image statIconImage;
        [SerializeField] private TextMeshProUGUI statTextLabel;

        public void Setup(Sprite iconSprite, string formattedText, Color textColor)
        {
            if (statIconImage != null && iconSprite != null)
            {
                statIconImage.sprite = iconSprite;
            }

            if (statTextLabel != null)
            {
                statTextLabel.text = formattedText;
                statTextLabel.color = textColor;
            }
        }
    }
}
