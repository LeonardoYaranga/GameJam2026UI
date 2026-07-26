using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CardSystem
{
    public class CardDisplayUI : MonoBehaviour
    {
        [Header("UI Componentes")]
        [SerializeField] private Image elementHeaderIcon;
        [SerializeField] private Image cardGlowFrame;
        [SerializeField] private Image cardBackgroundImage;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI elementLabelText;
        [SerializeField] private Transform statsContainer;
        [SerializeField] private Button cardButton;

        [Header("Prefab de Fila de Stat")]
        [SerializeField] private StatRowUI statRowPrefab;

        [Header("Sprites de Elementos")]
        [SerializeField] private Sprite fireSprite;
        [SerializeField] private Sprite waterSprite;
        [SerializeField] private Sprite natureSprite;

        [Header("Sprites de Fondos de Tarjetas")]
        [SerializeField] private Sprite fireCardBgSprite;
        [SerializeField] private Sprite waterCardBgSprite;
        [SerializeField] private Sprite natureCardBgSprite;

        [Header("Sprites de Stats")]
        [SerializeField] private Sprite attackIcon;
        [SerializeField] private Sprite defenseIcon;
        [SerializeField] private Sprite healthIcon;
        [SerializeField] private Sprite speedIcon;
        [SerializeField] private Sprite agilityIcon;
        [SerializeField] private Sprite staminaIcon;

        [Header("Colores por Elemento")]
        [SerializeField] private Color fireGlowColor = new Color(1f, 0.45f, 0.1f, 1f);
        [SerializeField] private Color waterGlowColor = new Color(0.1f, 0.75f, 1f, 1f);
        [SerializeField] private Color natureGlowColor = new Color(0.3f, 0.9f, 0.3f, 1f);

        private Action<CardData> onCardSelectedCallback;
        private CardData currentCardData;

        public void SetupCard(
            CardData data, 
            Action<CardData> onSelect, 
            Sprite fireSp, Sprite waterSp, Sprite natureSp, 
            Sprite fireBgSp, Sprite waterBgSp, Sprite natureBgSp,
            Sprite attSp, Sprite defSp, Sprite hpSp, Sprite spdSp, Sprite agiSp, Sprite staSp)
        {
            currentCardData = data;
            onCardSelectedCallback = onSelect;

            if (fireSp != null) fireSprite = fireSp;
            if (waterSp != null) waterSprite = waterSp;
            if (natureSp != null) natureSprite = natureSp;

            if (fireBgSp != null) fireCardBgSprite = fireBgSp;
            if (waterBgSp != null) waterCardBgSprite = waterBgSp;
            if (natureBgSp != null) natureCardBgSprite = natureBgSp;

            if (attSp != null) attackIcon = attSp;
            if (defSp != null) defenseIcon = defSp;
            if (hpSp != null) healthIcon = hpSp;
            if (spdSp != null) speedIcon = spdSp;
            if (agiSp != null) agilityIcon = agiSp;
            if (staSp != null) staminaIcon = staSp;

            // 1. Configurar Título y Elemento
            if (titleText != null) titleText.text = data.cardName;

            Sprite elemSprite = null;
            Sprite cardBgSprite = null;
            Color glowColor = fireGlowColor;
            string elemName = "";

            switch (data.element)
            {
                case ElementType.Fire:
                    elemSprite = fireSprite;
                    cardBgSprite = fireCardBgSprite;
                    glowColor = fireGlowColor;
                    elemName = "FUEGO";
                    break;
                case ElementType.Water:
                    elemSprite = waterSprite;
                    cardBgSprite = waterCardBgSprite;
                    glowColor = waterGlowColor;
                    elemName = "AGUA";
                    break;
                case ElementType.Nature:
                    elemSprite = natureSprite;
                    cardBgSprite = natureCardBgSprite;
                    glowColor = natureGlowColor;
                    elemName = "NATURALEZA";
                    break;
            }

            if (elementHeaderIcon != null && elemSprite != null)
            {
                elementHeaderIcon.sprite = elemSprite;
                elementHeaderIcon.color = Color.white;
            }

            // Aplicar fondo de tarjeta si existe la referencia
            Image targetBgImage = cardBackgroundImage != null ? cardBackgroundImage : cardGlowFrame;
            if (targetBgImage != null && cardBgSprite != null)
            {
                targetBgImage.sprite = cardBgSprite;
                targetBgImage.color = Color.white;
            }
            else if (cardGlowFrame != null)
            {
                cardGlowFrame.color = glowColor;
            }

            if (elementLabelText != null)
            {
                elementLabelText.text = elemName;
                elementLabelText.color = glowColor;
            }

            // 2. Limpiar filas anteriores
            if (statsContainer != null)
            {
                foreach (Transform child in statsContainer)
                {
                    Destroy(child.gameObject);
                }

                // 3. Crear filas de stats
                foreach (var modifier in data.statModifiers)
                {
                    if (statRowPrefab != null)
                    {
                        StatRowUI row = Instantiate(statRowPrefab, statsContainer);
                        Sprite statIcon = GetStatIcon(modifier.statType);
                        
                        // Verde brillante para Buff (+), Rojo vibrante para Nerf (-)
                        Color txtColor = modifier.amount >= 0 ? new Color(0.4f, 1f, 0.4f, 1f) : new Color(1f, 0.35f, 0.35f, 1f);
                        row.Setup(statIcon, modifier.GetFormattedText(), txtColor);
                    }
                }
            }

            // 4. Asignar Callback al Botón
            if (cardButton != null)
            {
                cardButton.onClick.RemoveAllListeners();
                cardButton.onClick.AddListener(() =>
                {
                    onCardSelectedCallback?.Invoke(currentCardData);
                });
            }
        }

        private Sprite GetStatIcon(StatType type)
        {
            switch (type)
            {
                case StatType.Attack: return attackIcon;
                case StatType.Defense: return defenseIcon;
                case StatType.Health: return healthIcon;
                case StatType.Speed: return speedIcon;
                case StatType.Agility: return agilityIcon;
                case StatType.Stamina: return staminaIcon;
                default: return attackIcon;
            }
        }
    }
}
