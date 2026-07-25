using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    public enum ElementType
    {
        Fire,
        Water,
        Nature
    }

    public enum StatType
    {
        Attack,
        Defense,
        Health,
        Speed
    }

    [System.Serializable]
    public class StatModifier
    {
        public StatType statType;
        public int amount; // Positivo = Buff, Negativo = Nerf

        public StatModifier(StatType statType, int amount)
        {
            this.statType = statType;
            this.amount = amount;
        }

        public string GetFormattedText()
        {
            string sign = amount >= 0 ? "+" : "";
            string statName = GetStatNameSpanish(statType);
            return $"{sign}{amount} {statName}";
        }

        private string GetStatNameSpanish(StatType type)
        {
            switch (type)
            {
                case StatType.Attack: return "Ataque";
                case StatType.Defense: return "Defensa";
                case StatType.Health: return "Salud";
                case StatType.Speed: return "Velocidad";
                default: return type.ToString();
            }
        }
    }

    [System.Serializable]
    public class CardData
    {
        public string cardName;
        public ElementType element;
        public List<StatModifier> statModifiers = new List<StatModifier>();
    }
}
