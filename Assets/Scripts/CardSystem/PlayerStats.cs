using UnityEngine;

namespace CardSystem
{
    public class PlayerStats : MonoBehaviour
    {
        [Header("Estadísticas del Jugador")]
        public int attack = 10;
        public int defense = 5;
        public int health = 100;
        public int speed = 5;

        private void Start()
        {
            PrintCurrentStats();
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
            }

            string sign = amount >= 0 ? "+" : "";
            Debug.Log($"<color=#55FF55><b>[PlayerStats]</b> Modificado {type}: {sign}{amount}</color>");
        }

        public void PrintCurrentStats()
        {
            Debug.Log($"<color=#FFFF55><b>==========================================</b>\n" +
                      $"<b>[ESTADÍSTICAS ACTUALES DEL JUGADOR]</b>\n" +
                      $"⚔️ <b>Ataque:</b> {attack}\n" +
                      $"🛡️ <b>Defensa:</b> {defense}\n" +
                      $"❤️ <b>Salud:</b> {health}\n" +
                      $"⚡ <b>Velocidad:</b> {speed}\n" +
                      $"<b>==========================================</b></color>");
        }
    }
}
