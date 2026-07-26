using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace CardSystem
{
    public class CardSelectionOverlayManager : MonoBehaviour
    {
        [Header("Referencias de UI")]
        [SerializeField] private GameObject overlayPanel; // El panel hijo oscuro que se muestra/oculta
        [SerializeField] private Transform cardsContainer;
        [SerializeField] private Button generateCardsButton;

        [Header("Prefabs & Componentes")]
        [SerializeField] private CardDisplayUI cardPrefab;
        [SerializeField] private PlayerStats playerStats;

        [Header("Sprites de Elementos")]
        public Sprite fireSprite;
        public Sprite waterSprite;
        public Sprite natureSprite;

        [Header("Sprites de Fondos de Tarjetas")]
        public Sprite fireCardBgSprite;
        public Sprite waterCardBgSprite;
        public Sprite natureCardBgSprite;

        [Header("Sprites de Stats")]
        public Sprite attackSprite;
        public Sprite defenseSprite;
        public Sprite healthSprite;
        public Sprite speedSprite;
        public Sprite agilitySprite;
        public Sprite staminaSprite;

        private void Start()
        {
            if (playerStats == null)
            {
                playerStats = FindFirstObjectByType<PlayerStats>();
                if (playerStats == null)
                {
                    GameObject psObj = new GameObject("PlayerStatsSystem");
                    playerStats = psObj.AddComponent<PlayerStats>();
                }
            }

            if (generateCardsButton != null)
            {
                generateCardsButton.onClick.RemoveAllListeners();
                generateCardsButton.onClick.AddListener(OnGenerateCardsButtonPressed);
                generateCardsButton.interactable = true;
            }

            if (overlayPanel != null)
            {
                overlayPanel.SetActive(false);
            }

            EnsureInputModule();
        }

        private void Update()
        {
            // La tecla 'G' abre/genera tarjetas si el panel está cerrado y el botón está activo
            bool canOpenWithKey = (overlayPanel == null || !overlayPanel.activeSelf) && 
                                 (generateCardsButton == null || generateCardsButton.interactable);

            if (canOpenWithKey)
            {
#if ENABLE_INPUT_SYSTEM
                if (Keyboard.current != null && Keyboard.current.gKey.wasPressedThisFrame)
                {
                    OnGenerateCardsButtonPressed();
                }
#else
                try
                {
                    if (Input.GetKeyDown(KeyCode.G))
                    {
                        OnGenerateCardsButtonPressed();
                    }
                }
                catch { }
#endif
            }
        }

        private void EnsureInputModule()
        {
            var eventSystem = UnityEngine.EventSystems.EventSystem.current;
            if (eventSystem != null)
            {
                var standalone = eventSystem.GetComponent<UnityEngine.EventSystems.StandaloneInputModule>();
                if (standalone != null)
                {
                    Destroy(standalone);
#if ENABLE_INPUT_SYSTEM
                    if (eventSystem.GetComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>() == null)
                    {
                        eventSystem.gameObject.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
                    }
#endif
                }
            }
        }

        public void OnGenerateCardsButtonPressed()
        {
            Debug.Log("<color=#00FFFF><b>[CardSelectionManager]</b> ¡Generando tarjetas!</color>");

            if (generateCardsButton != null)
            {
                generateCardsButton.interactable = false; // Deshabilitar hasta seleccionar una tarjeta
            }

            if (overlayPanel != null)
            {
                overlayPanel.SetActive(true);
            }

            GenerateThreeCards();
        }

        private void GenerateThreeCards()
        {
            if (cardsContainer != null)
            {
                foreach (Transform child in cardsContainer)
                {
                    Destroy(child.gameObject);
                }
            }

            List<CardData> cardOptions = StatGenerator.GenerateCardOptions(3);

            foreach (CardData data in cardOptions)
            {
                if (cardPrefab != null && cardsContainer != null)
                {
                    CardDisplayUI cardInstance = Instantiate(cardPrefab, cardsContainer);
                    cardInstance.SetupCard(
                        data, 
                        OnCardSelected, 
                        fireSprite, waterSprite, natureSprite, 
                        fireCardBgSprite, waterCardBgSprite, natureCardBgSprite,
                        attackSprite, defenseSprite, healthSprite, speedSprite, agilitySprite, staminaSprite
                    );
                }
            }

            Debug.Log("<color=#55FFFF><b>[CardSelection]</b> Se han generado 3 tarjetas aleatorias. Haz clic en una para elegirla.</color>");
        }

        private void OnCardSelected(CardData selectedData)
        {
            Debug.Log($"<color=#FFAA00><b>[CardSelection]</b> ¡Has seleccionado la tarjeta: <b>{selectedData.cardName}</b> ({selectedData.element})!</color>");

            if (playerStats != null)
            {
                playerStats.AddElementCard(selectedData.element);

                foreach (var modifier in selectedData.statModifiers)
                {
                    playerStats.ApplyModifier(modifier.statType, modifier.amount);
                }

                playerStats.PrintCurrentStats();
            }

            if (overlayPanel != null)
            {
                overlayPanel.SetActive(false);
            }

            if (generateCardsButton != null)
            {
                generateCardsButton.interactable = true;
            }
        }
    }
}
