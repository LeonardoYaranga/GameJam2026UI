using UnityEngine;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using TMPro;
using GameJam2026UI.UI;

namespace GameJam2026UI.Editor
{
    public class PauseMenuCanvasBuilder : EditorWindow
    {
        [MenuItem("GameJamUI/Build Pause Menu Prefab & Add to HUD Scene")]
        public static void BuildPauseMenu()
        {
            // 1. Asegurar escena Hud cargada o abierta
            string hudScenePath = "Assets/Scenes/Hud.unity";
            var hudScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            if (!Application.isPlaying && hudScene.path != hudScenePath)
            {
                hudScene = EditorSceneManager.OpenScene(hudScenePath, OpenSceneMode.Single);
            }

            // Eliminar versión previa si existe en la escena para evitar duplicados
            GameObject oldCanvas = GameObject.Find("PauseMenu_Canvas");
            if (oldCanvas != null) DestroyImmediate(oldCanvas);

            // 2. Crear Canvas Principal de Pausa
            GameObject canvasObj = new GameObject("PauseMenu_Canvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100; // Alto para superponerse a todo el HUD

            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;

            canvasObj.AddComponent<GraphicRaycaster>();

            // 3. Script Manager
            PauseMenuManager pauseManager = canvasObj.AddComponent<PauseMenuManager>();

            // 4. Panel de Fondo Semitransparente (Raíz del panel que se activa/desactiva)
            GameObject pausePanel = CreateUIElement("PauseMenuPanel", canvasObj.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            Image bgImg = pausePanel.AddComponent<Image>();
            bgImg.color = new Color(0.05f, 0.05f, 0.07f, 0.85f); // Oscuro semitransparente

            // Asignar el panel al serializedProperty de pauseMenuPanel en PauseMenuManager
            SerializedObject managerSo = new SerializedObject(pauseManager);
            SerializedProperty panelProp = managerSo.FindProperty("pauseMenuPanel");
            if (panelProp != null)
            {
                panelProp.objectReferenceValue = pausePanel;
                managerSo.ApplyModifiedProperties();
            }

            // 5. Contenedor Izquierdo (Título y Botones)
            GameObject leftContainer = CreateUIElement("LeftContainer", pausePanel.transform, new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(140, 0), new Vector2(500, 600));

            // Título "PAUSE"
            GameObject titleObj = CreateUIElement("PauseTitleText", leftContainer.transform, new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, 1), new Vector2(0, 0), new Vector2(500, 100));
            TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
            titleText.text = "PAUSE";
            titleText.fontSize = 76;
            titleText.fontStyle = FontStyles.Bold;
            titleText.color = new Color(0.85f, 0.12f, 0.12f, 1f); // Rojo destacado estilo Kena
            titleText.alignment = TextAlignmentOptions.Left;

            // Contenedor de Botones
            GameObject buttonsContainer = CreateUIElement("ButtonsContainer", leftContainer.transform, new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1), new Vector2(0, -120), new Vector2(500, 400));
            VerticalLayoutGroup vLayout = buttonsContainer.AddComponent<VerticalLayoutGroup>();
            vLayout.childAlignment = TextAnchor.UpperLeft;
            vLayout.spacing = 25;
            vLayout.childControlWidth = true;
            vLayout.childControlHeight = true;
            vLayout.childForceExpandWidth = false;
            vLayout.childForceExpandHeight = false;

            // Crear los 3 botones solicitados: CONTINUE, SETTINGS, QUIT
            CreatePauseButton("Btn_Continue", "CONTINUE", buttonsContainer.transform, pauseManager, () => pauseManager.ResumeGame());
            CreatePauseButton("Btn_Settings", "SETTINGS", buttonsContainer.transform, pauseManager, () => pauseManager.OpenSettings());
            CreatePauseButton("Btn_Quit", "QUIT", buttonsContainer.transform, pauseManager, () => pauseManager.QuitGame());

            // Forzar actualización inmediata del layout para calcular posiciones verticales
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(buttonsContainer.GetComponent<RectTransform>());

            // Al inicio el panel debe estar desactivado para que el juego comience normal
            pausePanel.SetActive(false);

            // 6. Guardar como Prefab
            if (!System.IO.Directory.Exists("Assets/Prefabs/UI"))
            {
                System.IO.Directory.CreateDirectory("Assets/Prefabs/UI");
            }

            string prefabPath = "Assets/Prefabs/UI/PauseMenuCanvas.prefab";
            PrefabUtility.SaveAsPrefabAssetAndConnect(canvasObj, prefabPath, InteractionMode.UserAction);

            // 7. Guardar la escena si no estamos en Play Mode
            if (!Application.isPlaying)
            {
                EditorSceneManager.SaveScene(hudScene, hudScenePath);
            }
            AssetDatabase.Refresh();

            Debug.Log("<color=green>[PauseMenuCanvasBuilder] Prefab guardado en " + prefabPath + " y añadido a la escena " + hudScenePath + "</color>");
        }

        private static GameObject CreatePauseButton(string name, string label, Transform parent, PauseMenuManager manager, System.Action action)
        {
            GameObject btnObj = CreateUIElement(name, parent, Vector2.zero, Vector2.zero, new Vector2(0, 0.5f), Vector2.zero, new Vector2(350, 50));
            LayoutElement le = btnObj.AddComponent<LayoutElement>();
            le.preferredWidth = 350;
            le.preferredHeight = 50;
            le.minWidth = 350;
            le.minHeight = 50;
            
            Image btnImg = btnObj.AddComponent<Image>();
            btnImg.color = new Color(0, 0, 0, 0.01f);

            Button button = btnObj.AddComponent<Button>();
            button.targetGraphic = btnImg;

            GameObject textObj = CreateUIElement("Label", btnObj.transform, Vector2.zero, Vector2.one, new Vector2(0, 0.5f), Vector2.zero, Vector2.zero);
            TextMeshProUGUI tmpText = textObj.AddComponent<TextMeshProUGUI>();
            tmpText.text = label;
            tmpText.fontSize = 36;
            tmpText.fontStyle = FontStyles.Bold;
            tmpText.color = new Color(0.9f, 0.9f, 0.9f, 1f);
            tmpText.alignment = TextAlignmentOptions.Left;

            // Añadir animación Hover
            btnObj.AddComponent<UI_ButtonHoverEffect>();

            // Enlazar evento Click
            if (manager != null)
            {
                if (label == "CONTINUE")
                    UnityEventTools.AddPersistentListener(button.onClick, manager.ResumeGame);
                else if (label == "SETTINGS")
                    UnityEventTools.AddPersistentListener(button.onClick, manager.OpenSettings);
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
