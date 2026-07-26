using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace GameJam2026UI.UI
{
    public class PauseMenuManager : MonoBehaviour
    {
        [Header("UI References")]
        [Tooltip("Asigna aquí el GameObject raíz del Menú de Pausa (ej. el Panel principal oscuro)")]
        [SerializeField] private GameObject pauseMenuPanel;

        [Header("Scene Navigation")]
        [SerializeField] private string mainMenuSceneName = "MainMenu";

        private bool isPaused = false;

        public bool IsPaused => isPaused;

        private void Start()
        {
            // Asegurarnos de que el tiempo esté normal y el menú cerrado al iniciar la escena
            Time.timeScale = 1f;
            if (pauseMenuPanel != null)
            {
                pauseMenuPanel.SetActive(false);
            }
            isPaused = false;

            // Escuchar el botón "Menu" del HUD
            if (GameJamUI.HUD.HUDManager.Instance != null)
            {
                GameJamUI.HUD.HUDManager.Instance.OnPauseRequested += TogglePause;
            }
        }

        private void OnDestroy()
        {
            if (GameJamUI.HUD.HUDManager.Instance != null)
            {
                GameJamUI.HUD.HUDManager.Instance.OnPauseRequested -= TogglePause;
            }
        }

        private void Update()
        {
            // Atajo de teclado para pausar / despausar con Escape o P (Compatible con el nuevo Input System y Legacy)
#if ENABLE_INPUT_SYSTEM
            var kbd = Keyboard.current;
            if (kbd != null)
            {
                if (kbd.escapeKey.wasPressedThisFrame || kbd.pKey.wasPressedThisFrame)
                {
                    TogglePause();
                }
            }
#else
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                TogglePause();
            }
#endif
        }

        /// <summary>
        /// Alterna entre pausar y reanudar el juego.
        /// </summary>
        public void TogglePause()
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        public void PauseGame()
        {
            isPaused = true;
            Time.timeScale = 0f; // Congela el tiempo del juego
            
            if (pauseMenuPanel != null)
                pauseMenuPanel.SetActive(true);
                
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void ResumeGame()
        {
            isPaused = false;
            Time.timeScale = 1f; // Reanuda el tiempo del juego
            
            if (pauseMenuPanel != null)
                pauseMenuPanel.SetActive(false);
        }

        public void OpenSettings()
        {
            Debug.Log("[PauseMenuManager] Abrir Opciones / Ajustes.");
        }

        public void QuitGame()
        {
            Debug.Log("[PauseMenuManager] Salir al Menú Principal. Cargando escena: " + mainMenuSceneName);
            
            // Reanudar el tiempo antes de cambiar de escena para evitar que el MainMenu empiece pausado
            Time.timeScale = 1f;
            isPaused = false;

            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}
