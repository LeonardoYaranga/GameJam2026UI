using UnityEngine;
using TMPro;

namespace GameJamUI.HUD
{
    public class UI_ResourceCounter : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private string resourceName = "Recurso";

        private int currentCount = 0;
        private int maxCapacity = 30;

        public void Initialize(int current, int max)
        {
            SetValues(current, max);
        }

        public void SetValues(int current, int max)
        {
            currentCount = Mathf.Clamp(current, 0, max);
            maxCapacity = max;
            UpdateText();
        }

        public void SetCount(int current)
        {
            SetValues(current, maxCapacity);
        }

        public int GetCount() => currentCount;
        public int GetMax() => maxCapacity;

        private void UpdateText()
        {
            if (countText != null)
            {
                countText.text = $"{currentCount} / {maxCapacity}";
            }
        }
    }
}
