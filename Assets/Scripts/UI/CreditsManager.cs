using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace GameJam2026UI.UI
{
    public class CreditsManager : MonoBehaviour
    {
        [Header("UI Panel References")]
        [Tooltip("Panel contenedor principal de los créditos")]
        [SerializeField] private GameObject creditsPanel;

        [Tooltip("ScrollRect que contiene la lista de créditos")]
        [SerializeField] private ScrollRect scrollRect;

        [Tooltip("Botón opcional para cerrar el panel de créditos")]
        [SerializeField] private Button backButton;

        [Header("Scene Config")]
        [Tooltip("Nombre de la escena del menú principal a cargar al presionar Volver")]
        [SerializeField] private string mainMenuSceneName = "MainMenu";

        [Header("Auto-Scroll Settings")]
        [Tooltip("Activar desplazamiento automático al abrir los créditos")]
        [SerializeField] private bool autoScrollEnabled = true;

        [Tooltip("Velocidad de desplazamiento automático hacia abajo")]
        [SerializeField] private float autoScrollSpeed = 0.05f;

        [Tooltip("Reiniciar la posición arriba al abrir el panel")]
        [SerializeField] private bool resetToTopOnOpen = true;

        private bool isAutoScrolling = false;
        private bool isProgrammaticScroll = false;

        private void Awake()
        {
            if (backButton != null)
            {
                backButton.onClick.AddListener(ReturnToMainMenu);
            }

            if (scrollRect != null)
            {
                scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
            }
        }

        private void Start()
        {
            // Si el panel está activo al iniciar la escena, inicializar estado
            if (creditsPanel != null && creditsPanel.activeSelf)
            {
                OnCreditsOpened();
            }
        }

        private void Update()
        {
            if (creditsPanel != null && creditsPanel.activeSelf && isAutoScrolling && scrollRect != null)
            {
                // Scroll suave hacia abajo (de 1.0 a 0.0)
                if (scrollRect.verticalNormalizedPosition > 0f)
                {
                    isProgrammaticScroll = true;
                    scrollRect.verticalNormalizedPosition -= autoScrollSpeed * Time.deltaTime;
                    if (scrollRect.verticalNormalizedPosition <= 0f)
                    {
                        scrollRect.verticalNormalizedPosition = 0f;
                        isAutoScrolling = false;
                    }
                }
            }
        }

        private void OnScrollValueChanged(Vector2 position)
        {
            // Si el scroll cambia y no fue provocado por el código de auto-scroll, es interacción manual
            if (!isProgrammaticScroll)
            {
                isAutoScrolling = false;
            }
            isProgrammaticScroll = false;
        }

        /// <summary>
        /// Abre el panel de créditos y reinicia el scroll si está configurado.
        /// </summary>
        public void OpenCredits()
        {
            if (creditsPanel != null)
            {
                creditsPanel.SetActive(true);
                OnCreditsOpened();
            }
            else
            {
                Debug.LogWarning("[CreditsManager] No se ha asignado el 'creditsPanel' en el Inspector.");
            }
        }

        /// <summary>
        /// Cierra el panel de créditos.
        /// </summary>
        public void CloseCredits()
        {
            isAutoScrolling = false;
            if (creditsPanel != null)
            {
                creditsPanel.SetActive(false);
            }
        }

        private void OnCreditsOpened()
        {
            if (scrollRect != null && resetToTopOnOpen)
            {
                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = 1f;
            }

            isAutoScrolling = autoScrollEnabled;
        }

        /// <summary>
        /// Reanuda o pausa el scroll automático manualmente.
        /// </summary>
        public void ToggleAutoScroll()
        {
            isAutoScrolling = !isAutoScrolling;
        }

        /// <summary>
        /// Carga la escena del Menú Principal.
        /// </summary>
        public void ReturnToMainMenu()
        {
            Debug.Log($"[CreditsManager] Cargando escena del menú principal: {mainMenuSceneName}");
            if (!string.IsNullOrEmpty(mainMenuSceneName))
            {
                SceneManager.LoadScene(mainMenuSceneName);
            }
        }
    }
}
