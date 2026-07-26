using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameJam2026UI.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        [Header("Scene Config")]
        [Tooltip("Nombre de la escena a cargar al presionar NEW GAME")]
        [SerializeField] private string gameplaySceneName = "Hud";

        [Tooltip("Nombre de la escena a cargar al presionar CREDITS (si loadCreditsAsScene es true)")]
        [SerializeField] private string creditsSceneName = "Credits";

        [Tooltip("Si es true, se cargará la escena de Créditos. Si es false, se activará el panel overlay.")]
        [SerializeField] private bool loadCreditsAsScene = true;

        [Header("UI Panels (Opcional)")]
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject creditsPanel;
        [SerializeField] private CreditsManager creditsManager;

        private void Start()
        {
            // Asegurarse que el tiempo transcurre normalmente
            Time.timeScale = 1f;

            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false);
            }

            if (creditsPanel != null)
            {
                creditsPanel.SetActive(false);
            }
        }

        public void NewGame()
        {
            Debug.Log("<color=#00FFFF><b>[MainMenuManager]</b> Clic en NEW GAME (Debug Mode - No redirige).</color>");
        }

        public void LoadGame()
        {
            Debug.Log("<color=#00FFFF><b>[MainMenuManager]</b> Clic en LOAD GAME (Debug Mode).</color>");
        }

        public void OpenSettings()
        {
            Debug.Log("<color=#00FFFF><b>[MainMenuManager]</b> Clic en OPTIONS (Debug Mode).</color>");
        }

        public void Credits()
        {
            Debug.Log("<color=#00FFFF><b>[MainMenuManager]</b> Clic en CREDITS.</color>");

            if (loadCreditsAsScene && !string.IsNullOrEmpty(creditsSceneName))
            {
                SceneManager.LoadScene(creditsSceneName);
                return;
            }

            if (creditsManager != null)
            {
                creditsManager.OpenCredits();
            }
            else if (creditsPanel != null)
            {
                creditsPanel.SetActive(true);
            }
        }

        public void CloseCredits()
        {
            if (creditsManager != null)
            {
                creditsManager.CloseCredits();
            }
            else if (creditsPanel != null)
            {
                creditsPanel.SetActive(false);
            }
        }

        public void CloseSettings()
        {
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false);
            }
        }

        public void QuitGame()
        {
            Debug.Log("<color=#FF5555><b>[MainMenuManager]</b> Salir del juego (Cerrando juego / Deteniendo Play Mode).</color>");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
