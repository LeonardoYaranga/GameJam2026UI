using UnityEngine;

namespace CardSystem
{
    public class PlayerStats : MonoBehaviour
    {
        [Header("Estadísticas del Jugador")]
        public int attack = 10;
        public int defense = 5;
        public int health = 100;
        public int maxHealth = 100;
        public int speed = 5;
        public int agility = 8;
        public int stamina = 50;

        [Header("Contadores de Tarjetas de Elemento")]
        public int fireCards = 0;
        public int waterCards = 0;
        public int natureCards = 0;

        private void Start()
        {
            UpdateHUD();
            PrintCurrentStats();
        }

        public void AddElementCard(ElementType element)
        {
            switch (element)
            {
                case ElementType.Fire:
                    fireCards++;
                    break;
                case ElementType.Water:
                    waterCards++;
                    break;
                case ElementType.Nature:
                    natureCards++;
                    break;
            }
            UpdateHUD();
        }

        public void ApplyModifier(StatType type, int amount)
        {
            switch (type)
            {
                case StatType.Attack:
                    attack += amount;
                    break;
                case StatType.Defense:
                    defense += amount;
                    break;
                case StatType.Health:
                    health += amount;
                    break;
                case StatType.Speed:
                    speed += amount;
                    break;
                case StatType.Agility:
                    agility += amount;
                    break;
                case StatType.Stamina:
                    stamina += amount;
                    break;
            }

            string sign = amount >= 0 ? "+" : "";
            Debug.Log($"<color=#55FF55><b>[PlayerStats]</b> Modificado {type}: {sign}{amount}</color>");
            
            UpdateHUD();
        }

        public void UpdateHUD()
        {
            if (GameJamUI.HUD.HUDManager.Instance != null)
            {
                GameJamUI.HUD.HUDManager.Instance.UpdateFireCount(fireCards, 30);
                GameJamUI.HUD.HUDManager.Instance.UpdateWaterCount(waterCards, 30);
                GameJamUI.HUD.HUDManager.Instance.UpdateNatureCount(natureCards, 30);
                GameJamUI.HUD.HUDManager.Instance.UpdateStats(attack, defense, health, agility, speed, stamina);
                GameJamUI.HUD.HUDManager.Instance.UpdateHealth(health, maxHealth);
            }
        }

        public void PrintCurrentStats()
        {
            Debug.Log($"<color=#FFFF55><b>==========================================</b>\n" +
                      $"<b>[ESTADÍSTICAS ACTUALES DEL JUGADOR]</b>\n" +
                      $"⚔️ <b>Ataque:</b> {attack}\n" +
                      $"🛡️ <b>Defensa:</b> {defense}\n" +
                      $"❤️ <b>Salud:</b> {health}\n" +
                      $"⚡ <b>Velocidad:</b> {speed}\n" +
                      $"🤸 <b>Agilidad:</b> {agility}\n" +
                      $"🔋 <b>Estamina:</b> {stamina}\n" +
                      $"<b>==========================================</b></color>");
        }
    }
}
