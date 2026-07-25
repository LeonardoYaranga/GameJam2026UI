using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace GameJamUI.HUD
{
    public class HUDTester : MonoBehaviour
    {
        [Header("Testing Values")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth = 100f;

        [SerializeField] private float maxEnergy = 100f;
        [SerializeField] private float currentEnergy = 80f;

        [Header("Resources (Fuego, Agua, Naturaleza)")]
        [SerializeField] private int fireCount = 30;
        [SerializeField] private int waterCount = 30;
        [SerializeField] private int natureCount = 30;
        [SerializeField] private int maxPerResource = 30;

        [Header("Stats (Ataque, Defensa, Vida, Agilidad, Velocidad, Stamina)")]
        [SerializeField] private int attack = 10;
        [SerializeField] private int defense = 5;
        [SerializeField] private int healthStat = 100;
        [SerializeField] private int agility = 8;
        [SerializeField] private int speed = 12;
        [SerializeField] private int staminaStat = 50;

        [Header("Timer")]
        [SerializeField] private float timeRemaining = 1200f; // 20:00


        private void Start()
        {
            if (HUDManager.Instance != null)
            {
                HUDManager.Instance.SetupStatusBars(currentHealth, maxHealth, currentEnergy, maxEnergy);
                HUDManager.Instance.UpdateFireCount(fireCount, maxPerResource);
                HUDManager.Instance.UpdateWaterCount(waterCount, maxPerResource);
                HUDManager.Instance.UpdateNatureCount(natureCount, maxPerResource);
                HUDManager.Instance.UpdateStats(attack, defense, healthStat, agility, speed, staminaStat);
                HUDManager.Instance.UpdateTimer(timeRemaining);
            }
        }

        private void Update()
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                if (timeRemaining < 0) timeRemaining = 0;
                HUDManager.Instance?.UpdateTimer(timeRemaining);
            }

#if ENABLE_INPUT_SYSTEM
            var kbd = Keyboard.current;
            if (kbd == null) return;

            // Tecla H: Recibir daño / Curar
            if (kbd.hKey.wasPressedThisFrame)
            {
                currentHealth -= 20f;
                if (currentHealth <= 0f) currentHealth = maxHealth;
                HUDManager.Instance?.UpdateHealth(currentHealth, maxHealth);
                Debug.Log($"[HUDTester] Health changed: {currentHealth}/{maxHealth}");
            }

            // Tecla J: Gastar energía
            if (kbd.jKey.wasPressedThisFrame)
            {
                currentEnergy -= 15f;
                if (currentEnergy <= 0f) currentEnergy = maxEnergy;
                HUDManager.Instance?.UpdateEnergy(currentEnergy, maxEnergy);
                Debug.Log($"[HUDTester] Energy changed: {currentEnergy}/{maxEnergy}");
            }

            // Teclas 1, 2, 3: Modificar recursos
            if (kbd.digit1Key.wasPressedThisFrame)
            {
                fireCount = (fireCount + 5) % (maxPerResource + 1);
                HUDManager.Instance?.UpdateFireCount(fireCount, maxPerResource);
            }
            if (kbd.digit2Key.wasPressedThisFrame)
            {
                waterCount = (waterCount + 5) % (maxPerResource + 1);
                HUDManager.Instance?.UpdateWaterCount(waterCount, maxPerResource);
            }
            if (kbd.digit3Key.wasPressedThisFrame)
            {
                natureCount = (natureCount + 5) % (maxPerResource + 1);
                HUDManager.Instance?.UpdateNatureCount(natureCount, maxPerResource);
            }
#else
            // Legacy Input System Fallback
            if (Input.GetKeyDown(KeyCode.H))
            {
                currentHealth -= 20f;
                if (currentHealth <= 0f) currentHealth = maxHealth;
                HUDManager.Instance?.UpdateHealth(currentHealth, maxHealth);
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                currentEnergy -= 15f;
                if (currentEnergy <= 0f) currentEnergy = maxEnergy;
                HUDManager.Instance?.UpdateEnergy(currentEnergy, maxEnergy);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                fireCount = (fireCount + 5) % (maxPerResource + 1);
                HUDManager.Instance?.UpdateFireCount(fireCount, maxPerResource);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                waterCount = (waterCount + 5) % (maxPerResource + 1);
                HUDManager.Instance?.UpdateWaterCount(waterCount, maxPerResource);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                natureCount = (natureCount + 5) % (maxPerResource + 1);
                HUDManager.Instance?.UpdateNatureCount(natureCount, maxPerResource);
            }
#endif
        }
    }
}
