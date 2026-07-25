using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameJamUI.HUD
{
    public class UI_StatusBar : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private Image fillImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TextMeshProUGUI valueText;

        [Header("Colors")]
        [SerializeField] private Color barColor = Color.red;
        [SerializeField] private Color backgroundColor = new Color(0.15f, 0.15f, 0.15f, 0.9f);

        [Header("Animation Settings")]
        [SerializeField] private float smoothSpeed = 8f;

        private float currentAmount = 100f;
        private float maxAmount = 100f;
        private float targetFillAmount = 1f;

        private void Awake()
        {
            if (fillImage != null)
            {
                fillImage.color = barColor;
                fillImage.type = Image.Type.Filled;
                fillImage.fillMethod = Image.FillMethod.Horizontal;
            }
            if (backgroundImage != null)
            {
                backgroundImage.color = backgroundColor;
            }
        }

        private void Update()
        {
            if (fillImage != null)
            {
                fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFillAmount, Time.deltaTime * smoothSpeed);
            }
        }

        public void Initialize(float current, float max)
        {
            maxAmount = max;
            SetValues(current, max);
            if (fillImage != null)
            {
                fillImage.fillAmount = targetFillAmount;
            }
        }

        public void SetValues(float current, float max)
        {
            currentAmount = Mathf.Clamp(current, 0f, max);
            maxAmount = max;
            targetFillAmount = maxAmount > 0 ? currentAmount / maxAmount : 0f;

            UpdateText();
        }

        public void SetColor(Color color)
        {
            barColor = color;
            if (fillImage != null)
            {
                fillImage.color = barColor;
            }
        }

        private void UpdateText()
        {
            if (valueText != null)
            {
                valueText.text = $"{Mathf.RoundToInt(currentAmount)} / {Mathf.RoundToInt(maxAmount)}";
            }
        }
    }
}
