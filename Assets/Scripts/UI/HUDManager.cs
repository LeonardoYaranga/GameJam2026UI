using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace GameJamUI.HUD
{
    public class HUDManager : MonoBehaviour
    {
        public static HUDManager Instance { get; private set; }

        [Header("Status Bars")]
        [SerializeField] private UI_StatusBar healthBar;
        [SerializeField] private UI_StatusBar energyBar;

        [Header("Resource Counters (Fuego, Agua, Naturaleza)")]
        [SerializeField] private UI_ResourceCounter fireCounter;
        [SerializeField] private UI_ResourceCounter waterCounter;
        [SerializeField] private UI_ResourceCounter natureCounter;

        [Header("Stat Counters (Ataque, Defensa, Vida, Agilidad, Velocidad, Stamina)")]
        [SerializeField] private UI_ResourceCounter attackCounter;
        [SerializeField] private UI_ResourceCounter defenseCounter;
        [SerializeField] private UI_ResourceCounter healthStatCounter;
        [SerializeField] private UI_ResourceCounter agilityCounter;
        [SerializeField] private UI_ResourceCounter speedCounter;
        [SerializeField] private UI_ResourceCounter staminaCounter;

        [Header("Timer")]
        [SerializeField] private TextMeshProUGUI timerText;

        [Header("Pause Controls")]
        [SerializeField] private Button pauseButton;

        public event Action OnPauseRequested;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            if (pauseButton != null)
            {
                pauseButton.onClick.AddListener(TriggerPause);
            }
        }

        public void SetupStatusBars(float health, float maxHealth, float energy, float maxEnergy)
        {
            if (healthBar != null) healthBar.Initialize(health, maxHealth);
            if (energyBar != null) energyBar.Initialize(energy, maxEnergy);
        }

        public void UpdateHealth(float current, float max)
        {
            if (healthBar != null) healthBar.SetValues(current, max);
        }

        public void UpdateEnergy(float current, float max)
        {
            if (energyBar != null) energyBar.SetValues(current, max);
        }

        public void UpdateFireCount(int current, int max)
        {
            if (fireCounter != null) fireCounter.SetValues(current, max);
        }

        public void UpdateWaterCount(int current, int max)
        {
            if (waterCounter != null) waterCounter.SetValues(current, max);
        }

        public void UpdateNatureCount(int current, int max)
        {
            if (natureCounter != null) natureCounter.SetValues(current, max);
        }

        public void UpdateStats(int attack, int defense, int health, int agility, int speed, int stamina)
        {
            if (attackCounter != null) attackCounter.SetValues(attack, 99);
            if (defenseCounter != null) defenseCounter.SetValues(defense, 99);
            if (healthStatCounter != null) healthStatCounter.SetValues(health, 999);
            if (agilityCounter != null) agilityCounter.SetValues(agility, 99);
            if (speedCounter != null) speedCounter.SetValues(speed, 99);
            if (staminaCounter != null) staminaCounter.SetValues(stamina, 99);
        }

        public void UpdateTimer(float timeInSeconds)
        {
            if (timerText != null)
            {
                int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
                int seconds = Mathf.FloorToInt(timeInSeconds - minutes * 60);
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }

        public void TriggerPause()
        {
            Debug.Log("[HUDManager] Pause Button Pressed");
            OnPauseRequested?.Invoke();
        }
    }
}
