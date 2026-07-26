using UnityEngine;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using TMPro;
using GameJam2026UI.UI;

namespace GameJam2026UI.Editor
{
    public class MainMenuCanvasBuilder : EditorWindow
    {
        [MenuItem("GameJamUI/Build Main Menu Scene")]
        public static void BuildMainMenuScene()
        {
            // 1. Crear o abrir la escena MainMenu.unity
            string scenePath = "Assets/Scenes/MainMenu.unity";
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            if (!Application.isPlaying && scene.path != scenePath)
            {
                scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            }

            // Eliminar Canvas antiguo si existe para reconstrucción limpia
            GameObject oldCanvas = GameObject.Find("MainMenu_Canvas");
            if (oldCanvas != null) DestroyImmediate(oldCanvas);

            // 2. Crear Canvas Principal
            GameObject canvasObj = new GameObject("MainMenu_Canvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 0;

            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;

            canvasObj.AddComponent<GraphicRaycaster>();

            // EventSystem (InputSystem Package)
            GameObject existingES = GameObject.Find("EventSystem");
            if (existingES == null)
            {
                GameObject esObj = new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem));
#if ENABLE_INPUT_SYSTEM
                esObj.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
#else
                esObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
#endif
            }

            // 3. Crear Manager
            GameObject managerObj = new GameObject("MainMenuManager");
            managerObj.transform.SetParent(canvasObj.transform, false);
            MainMenuManager menuManager = managerObj.AddComponent<MainMenuManager>();

            // 4. Panel de Fondo Oscuro/Elegante
            GameObject bgPanel = CreateUIElement("BackgroundOverlay", canvasObj.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            Image bgImg = bgPanel.AddComponent<Image>();
            bgImg.color = new Color(0.08f, 0.09f, 0.12f, 0.95f); // Dark elegant slate background

            // 5. Título Principal (Arriba a la Izquierda, estilo Kena)
            GameObject titleContainer = CreateUIElement("TitleContainer", canvasObj.transform, new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1), new Vector2(120, -100), new Vector2(600, 150));
            
            GameObject titleTextObj = CreateUIElement("TitleText", titleContainer.transform, Vector2.zero, Vector2.one, new Vector2(0, 1), Vector2.zero, Vector2.zero);
            TextMeshProUGUI titleText = titleTextObj.AddComponent<TextMeshProUGUI>();
            titleText.text = "GAME TITLE";
            titleText.fontSize = 72;
            titleText.fontStyle = FontStyles.Bold;
            titleText.color = new Color(0.95f, 0.95f, 0.95f, 1f);
            titleText.alignment = TextAlignmentOptions.Left;

            GameObject subtitleTextObj = CreateUIElement("SubtitleText", titleContainer.transform, new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, -75), new Vector2(600, 40));
            TextMeshProUGUI subtitleText = subtitleTextObj.AddComponent<TextMeshProUGUI>();
            subtitleText.text = "SUBTITLE / SLOGAN HERE";
            subtitleText.fontSize = 24;
            subtitleText.fontStyle = FontStyles.Italic;
            subtitleText.color = new Color(0.7f, 0.75f, 0.8f, 0.8f);
            subtitleText.alignment = TextAlignmentOptions.Left;

            // 6. Contenedor de Botones (Abajo a la Izquierda)
            GameObject buttonsContainer = CreateUIElement("ButtonsContainer", canvasObj.transform, new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(120, 150), new Vector2(350, 300));
            VerticalLayoutGroup vLayout = buttonsContainer.AddComponent<VerticalLayoutGroup>();
            vLayout.childAlignment = TextAnchor.LowerLeft;
            vLayout.spacing = 18;
            vLayout.childControlWidth = true;
            vLayout.childControlHeight = true;
            vLayout.childForceExpandWidth = false;
            vLayout.childForceExpandHeight = false;

            // Crear los 4 botones solicitados: NEW GAME, OPTIONS, CREDITS, QUIT
            CreateMenuButton("Btn_NewGame", "NEW GAME", buttonsContainer.transform, menuManager, () => menuManager.NewGame());
            CreateMenuButton("Btn_Options", "OPTIONS", buttonsContainer.transform, menuManager, () => menuManager.OpenSettings());
            CreateMenuButton("Btn_Credits", "CREDITS", buttonsContainer.transform, menuManager, () => menuManager.Credits());
            CreateMenuButton("Btn_Quit", "QUIT", buttonsContainer.transform, menuManager, () => menuManager.QuitGame());

            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(buttonsContainer.GetComponent<RectTransform>());

            // 7. Texto de Versión (Abajo a la Derecha)
            GameObject versionObj = CreateUIElement("VersionText", canvasObj.transform, new Vector2(1, 0), new Vector2(1, 0), new Vector2(1, 0), new Vector2(-60, 40), new Vector2(250, 40));
            TextMeshProUGUI versionText = versionObj.AddComponent<TextMeshProUGUI>();
            versionText.text = "Version 1.0.0";
            versionText.fontSize = 18;
            versionText.color = new Color(0.5f, 0.55f, 0.6f, 0.7f);
            versionText.alignment = TextAlignmentOptions.Right;

            // 8. Guardar Escena
            if (!Application.isPlaying)
            {
                EditorSceneManager.SaveScene(scene, scenePath);
            }
            AssetDatabase.Refresh();

            Debug.Log("<color=green>[MainMenuCanvasBuilder] Escena 'MainMenu.unity' creada y guardada con éxito en Assets/Scenes/MainMenu.unity</color>");
        }

        private static GameObject CreateMenuButton(string name, string label, Transform parent, MainMenuManager manager, System.Action onClickAction)
        {
            GameObject btnObj = CreateUIElement(name, parent, Vector2.zero, Vector2.zero, new Vector2(0, 0.5f), Vector2.zero, new Vector2(350, 50));
            LayoutElement le = btnObj.AddComponent<LayoutElement>();
            le.preferredWidth = 350;
            le.preferredHeight = 50;
            le.minWidth = 350;
            le.minHeight = 50;
            
            // Imagen transparente para raycast
            Image btnImg = btnObj.AddComponent<Image>();
            btnImg.color = new Color(0, 0, 0, 0.01f); // Casi invisible para captura de raycast
            btnImg.raycastTarget = true;

            Button button = btnObj.AddComponent<Button>();
            button.targetGraphic = btnImg;

            // Texto del botón
            GameObject textObj = CreateUIElement("Label", btnObj.transform, Vector2.zero, Vector2.one, new Vector2(0, 0.5f), Vector2.zero, Vector2.zero);
            TextMeshProUGUI tmpText = textObj.AddComponent<TextMeshProUGUI>();
            tmpText.text = label;
            tmpText.fontSize = 32;
            tmpText.fontStyle = FontStyles.Bold;
            tmpText.color = new Color(0.9f, 0.9f, 0.9f, 1f);
            tmpText.alignment = TextAlignmentOptions.Left;
            tmpText.raycastTarget = false;

            // Añadir script de efecto Hover
            btnObj.AddComponent<UI_ButtonHoverEffect>();

            // Enlazar evento Click en Editor
            if (manager != null)
            {
                if (label == "NEW GAME")
                    UnityEventTools.AddPersistentListener(button.onClick, manager.NewGame);
                else if (label == "OPTIONS")
                    UnityEventTools.AddPersistentListener(button.onClick, manager.OpenSettings);
                else if (label == "CREDITS")
                    UnityEventTools.AddPersistentListener(button.onClick, manager.Credits);
                else if (label == "QUIT")
                    UnityEventTools.AddPersistentListener(button.onClick, manager.QuitGame);
            }

            return btnObj;
        }

        private static GameObject CreateUIElement(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 anchoredPosition, Vector2 sizeDelta)
        {
            GameObject obj = new GameObject(name);
            RectTransform rect = obj.AddComponent<RectTransform>();
            rect.SetParent(parent, false);
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            rect.anchoredPosition = anchoredPosition;
            if (sizeDelta != Vector2.zero)
                rect.sizeDelta = sizeDelta;
            return obj;
        }
    }
}
