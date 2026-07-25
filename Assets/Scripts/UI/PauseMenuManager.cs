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

        private bool isPaused = false;

        private void Start()
        {
            // Asegurarnos de que el menú está cerrado al iniciar
            if (pauseMenuPanel != null)
            {
                pauseMenuPanel.SetActive(false);
            }
            ResumeGame();
        }

        private void Update()
        {
            // Fallback al sistema de input antiguo por si acaso
            // Si usas el nuevo Input System a través de eventos, puedes borrar este método Update
            // y llamar directamente al método TogglePause() desde tu PlayerInput.
            #if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                TogglePause();
            }
            #endif
        }

        /// <summary>
        /// Alterna entre pausar y reanudar el juego. Útil para enlazar con eventos del Input System.
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
            Time.timeScale = 0f; // Detiene el tiempo
            
            if (pauseMenuPanel != null)
                pauseMenuPanel.SetActive(true);
                
            // Mostrar y desbloquear el cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void ResumeGame()
        {
            isPaused = false;
            Time.timeScale = 1f; // Reanuda el tiempo
            
            if (pauseMenuPanel != null)
                pauseMenuPanel.SetActive(false);
                
            // Ocultar y bloquear el cursor (Ajusta esto según el tipo de juego, ej. si es un FPS)
            // Cursor.lockState = CursorLockMode.Locked;
            // Cursor.visible = false;
        }

        public void SaveGame()
        {
            Debug.Log("Guardar partida accionado.");
            // Aquí iría la lógica para guardar el progreso
        }

        public void OpenSettings()
        {
            Debug.Log("Abrir Ajustes accionado.");
            // Aquí iría la lógica para mostrar el menú de opciones (desactivar pauseMenuPanel y activar panel de ajustes)
        }

        public void QuitGame()
        {
            Debug.Log("Salir del juego accionado.");
            
            // Reanudar el tiempo antes de salir (buena práctica por si cambias de escena)
            Time.timeScale = 1f;

            // Ejemplo: Cargar la escena del Menú Principal
            // SceneManager.LoadScene("MainMenu");
            
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
