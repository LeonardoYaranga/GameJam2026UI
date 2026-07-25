using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    public static class StatGenerator
    {
        private static readonly string[] FireNames = { "Llama Llameante", "Ignis Breaker", "Calor Divino", "Espada Ígnea", "Furia de Fuego" };
        private static readonly string[] WaterNames = { "Marea Creciente", "Aqua Shield", "Escudo Acuático", "Cascada Bendita", "Torrente Vital" };
        private static readonly string[] NatureNames = { "Espíritu del Bosque", "Gaia's Grasp", "Abrazo de Gaia", "Raíces Vitales", "Manto Verde" };

        public static CardData GenerateRandomCard()
        {
            CardData card = new CardData();

            // 1. Elemento Aleatorio
            card.element = (ElementType)Random.Range(0, 3);

            // 2. Nombre según elemento
            switch (card.element)
            {
                case ElementType.Fire:
                    card.cardName = FireNames[Random.Range(0, FireNames.Length)];
                    break;
                case ElementType.Water:
                    card.cardName = WaterNames[Random.Range(0, WaterNames.Length)];
                    break;
                case ElementType.Nature:
                    card.cardName = NatureNames[Random.Range(0, NatureNames.Length)];
                    break;
            }

            // 3. Cantidad de stats: mínimo 2, máximo 3
            int rowCount = Random.Range(2, 4); // 2 o 3

            // 4. Garantizar mezcla de Buffs (+) y Nerfs (-)
            int buffCount = 1;
            int nerfCount = 1;

            if (rowCount == 3)
            {
                // Con 3 filas: o (2 buffs + 1 nerf) o (1 buff + 2 nerfs)
                if (Random.value < 0.5f)
                {
                    buffCount = 2;
                    nerfCount = 1;
                }
                else
                {
                    buffCount = 1;
                    nerfCount = 2;
                }
            }

            // Seleccionar tipos de stats únicos
            List<StatType> availableStats = new List<StatType>() 
            { 
                StatType.Attack, 
                StatType.Defense, 
                StatType.Health, 
                StatType.Speed 
            };
            Shuffle(availableStats);

            // Generar Buffs (Positivos)
            for (int i = 0; i < buffCount; i++)
            {
                StatType type = availableStats[0];
                availableStats.RemoveAt(0);
                int amount = GetBuffAmount(type);
                card.statModifiers.Add(new StatModifier(type, amount));
            }

            // Generar Nerfs (Negativos)
            for (int i = 0; i < nerfCount; i++)
            {
                StatType type = availableStats[0];
                availableStats.RemoveAt(0);
                int amount = GetNerfAmount(type);
                card.statModifiers.Add(new StatModifier(type, -amount));
            }

            // Mezclar orden para variedad visual
            Shuffle(card.statModifiers);

            return card;
        }

        private static int GetBuffAmount(StatType type)
        {
            switch (type)
            {
                case StatType.Attack: return Random.Range(2, 6);   // +2 a +5 Ataque
                case StatType.Defense: return Random.Range(2, 5);  // +2 a +4 Defensa
                case StatType.Health: return Random.Range(15, 36); // +15 a +35 Salud
                case StatType.Speed: return Random.Range(1, 3);    // +1 a +2 Velocidad
                default: return 2;
            }
        }

        private static int GetNerfAmount(StatType type)
        {
            switch (type)
            {
                case StatType.Attack: return Random.Range(1, 4);   // -1 a -3 Ataque
                case StatType.Defense: return Random.Range(1, 4);  // -1 a -3 Defensa
                case StatType.Health: return Random.Range(8, 21);  // -8 a -20 Salud
                case StatType.Speed: return 1;                     // -1 Velocidad
                default: return 1;
            }
        }

        private static void Shuffle<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int rnd = Random.Range(i, list.Count);
                T temp = list[i];
                list[i] = list[rnd];
                list[rnd] = temp;
            }
        }

        public static List<CardData> GenerateCardOptions(int count = 3)
        {
            List<CardData> options = new List<CardData>();
            for (int i = 0; i < count; i++)
            {
                options.Add(GenerateRandomCard());
            }
            return options;
        }
    }
}
