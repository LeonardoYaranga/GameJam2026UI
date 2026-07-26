using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using GameJamUI.HUD;

namespace GameJamUI.Editor
{
    public class HUDCanvasBuilder : EditorWindow
    {
        [MenuItem("GameJamUI/Build HUD Layout (Redesign)")]
        public static void BuildHUD()
        {
            // 0. Clean ALL old HUD objects and canvases in the scene
            CleanAllOldHUDObjects();

            GameObject canvasObj = new GameObject("HUD_Canvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 0; // Layer base para el HUD
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            canvasObj.AddComponent<GraphicRaycaster>();

            GameObject hudManagerObj = new GameObject("HUDManager");
            hudManagerObj.transform.SetParent(canvas.transform, false);
            HUDManager hudManager = hudManagerObj.AddComponent<HUDManager>();
            HUDTester hudTester = hudManagerObj.AddComponent<HUDTester>();

            // 1. Top-Left: Profile Pic and Bars (Keep as is, user didn't ask to remove it)
            GameObject topLeftPanel = CreateUIElement("TopLeft_ProfilePanel", canvas.transform, new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1), new Vector2(20, -20));
            // Profile Pic
            GameObject profilePic = CreateUIElement("ProfilePic", topLeftPanel.transform, new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1), Vector2.zero, new Vector2(120, 120));
            Image profileImg = profilePic.AddComponent<Image>();
            Sprite boobySprite = LoadOrFixSprite("Assets/Sprites/UI/profile_booby.jpg");
            if (boobySprite != null) profileImg.sprite = boobySprite;
            else profileImg.color = Color.gray;

            GameObject barsContainer = CreateUIElement("BarsContainer", topLeftPanel.transform, new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1), new Vector2(130, -10), new Vector2(300, 120));
            VerticalLayoutGroup barsLayout = barsContainer.AddComponent<VerticalLayoutGroup>();
            barsLayout.childForceExpandHeight = false;
            barsLayout.spacing = 10;

            // Health Bar
            GameObject healthBarObj = CreateUIElement("HealthBar", barsContainer.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(300, 30));
            Image healthBg = healthBarObj.AddComponent<Image>();
            Sprite healthFrameSprite = LoadOrFixSprite("Assets/Sprites/UI/frame_health.jpg");
            if (healthFrameSprite != null) healthBg.sprite = healthFrameSprite;
            else healthBg.color = new Color(0.2f, 0.2f, 0.2f);
            
            GameObject healthFillObj = CreateUIElement("Fill", healthBarObj.transform, Vector2.zero, Vector2.one, new Vector2(0, 0.5f), Vector2.zero);
            healthFillObj.AddComponent<Image>().color = Color.red;
            UI_StatusBar healthStatusBar = healthBarObj.AddComponent<UI_StatusBar>();

            // Energy Bar
            GameObject energyBarObj = CreateUIElement("EnergyBar", barsContainer.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(300, 20));
            Image energyBg = energyBarObj.AddComponent<Image>();
            Sprite energyFrameSprite = LoadOrFixSprite("Assets/Sprites/UI/frame_stamina.jpg");
            if (energyFrameSprite != null) energyBg.sprite = energyFrameSprite;
            else energyBg.color = new Color(0.2f, 0.2f, 0.2f);
            
            GameObject energyFillObj = CreateUIElement("Fill", energyBarObj.transform, Vector2.zero, Vector2.one, new Vector2(0, 0.5f), Vector2.zero);
            energyFillObj.AddComponent<Image>().color = Color.yellow;
            UI_StatusBar energyStatusBar = energyBarObj.AddComponent<UI_StatusBar>();

            // 2. Top-Center: Timer
            GameObject timerPanel = CreateUIElement("TopCenter_TimerPanel", canvas.transform, new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -20), new Vector2(200, 60));
            TextMeshProUGUI timerText = timerPanel.AddComponent<TextMeshProUGUI>();
            timerText.text = "20:00";
            timerText.fontSize = 48;
            timerText.alignment = TextAlignmentOptions.Center;

            // 3. Top-Right: Pause/Menu
            GameObject menuPanel = CreateUIElement("TopRight_PausePanel", canvas.transform, new Vector2(1, 1), new Vector2(1, 1), new Vector2(1, 1), new Vector2(-20, -20), new Vector2(120, 60));
            Button menuButton = menuPanel.AddComponent<Button>();
            menuPanel.AddComponent<Image>().color = Color.white;
            GameObject menuTextObj = CreateUIElement("Text", menuPanel.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero);
            TextMeshProUGUI menuText = menuTextObj.AddComponent<TextMeshProUGUI>();
            menuText.text = "Menu";
            menuText.color = Color.black;
            menuText.alignment = TextAlignmentOptions.Center;

            // 4. Bottom-Left: Elements (Fuego, Agua, Naturaleza)
            GameObject bottomLeftPanel = CreateUIElement("BottomLeft_ElementsPanel", canvas.transform, new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(20, 20), new Vector2(400, 150));
            HorizontalLayoutGroup elementsLayout = bottomLeftPanel.AddComponent<HorizontalLayoutGroup>();
            elementsLayout.spacing = 15;
            elementsLayout.childAlignment = TextAnchor.LowerLeft;

            UI_ResourceCounter fireC = CreateVerticalCounter(bottomLeftPanel.transform, "Fire", "Assets/Sprites/UI/element_fire.jpg");
            UI_ResourceCounter waterC = CreateVerticalCounter(bottomLeftPanel.transform, "Water", "Assets/Sprites/UI/element_water.jpg");
            UI_ResourceCounter natureC = CreateVerticalCounter(bottomLeftPanel.transform, "Nature", "Assets/Sprites/UI/element_nature.jpg");

            // 5. Bottom-Right: Stats (Ataque, Defensa, Vida, Agilidad, Velocidad, Stamina)
            GameObject bottomRightPanel = CreateUIElement("BottomRight_StatsPanel", canvas.transform, new Vector2(1, 0), new Vector2(1, 0), new Vector2(1, 0), new Vector2(-20, 20), new Vector2(600, 150));
            HorizontalLayoutGroup statsLayout = bottomRightPanel.AddComponent<HorizontalLayoutGroup>();
            statsLayout.spacing = 15;
            statsLayout.childAlignment = TextAnchor.LowerRight;

            UI_ResourceCounter attackC = CreateVerticalCounter(bottomRightPanel.transform, "Attack", "Assets/Sprites/UI/stat_attack.jpg");
            UI_ResourceCounter defenseC = CreateVerticalCounter(bottomRightPanel.transform, "Defense", "Assets/Sprites/UI/stat_defense.jpg");
            UI_ResourceCounter healthStatC = CreateVerticalCounter(bottomRightPanel.transform, "Health", "Assets/Sprites/UI/stat_health.jpg");
            UI_ResourceCounter agilityC = CreateVerticalCounter(bottomRightPanel.transform, "Agility", "Assets/Sprites/UI/stat_agility.jpg");
            UI_ResourceCounter speedC = CreateVerticalCounter(bottomRightPanel.transform, "Speed", "Assets/Sprites/UI/stat_speed.jpg");
            UI_ResourceCounter staminaC = CreateVerticalCounter(bottomRightPanel.transform, "Stamina", "Assets/Sprites/UI/stat_stamina.jpg");

            // Wire up HUDManager
            SerializedObject so = new SerializedObject(hudManager);
            void SetProp(string propName, UnityEngine.Object objRef)
            {
                SerializedProperty prop = so.FindProperty(propName);
                if (prop != null) prop.objectReferenceValue = objRef;
                else Debug.LogWarning($"[HUDCanvasBuilder] Could not find property {propName} on HUDManager.");
            }

            SetProp("healthBar", healthStatusBar);
            SetProp("energyBar", energyStatusBar);
            
            SetProp("fireCounter", fireC);
            SetProp("waterCounter", waterC);
            SetProp("natureCounter", natureC);
            
            SetProp("attackCounter", attackC);
            SetProp("defenseCounter", defenseC);
            SetProp("healthStatCounter", healthStatC);
            SetProp("agilityCounter", agilityC);
            SetProp("speedCounter", speedC);
            SetProp("staminaCounter", staminaC);

            SetProp("timerText", timerText);
            SetProp("pauseButton", menuButton);
            so.ApplyModifiedProperties();

            if (FindAnyObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }

            // Save Prefab
            PrefabUtility.SaveAsPrefabAsset(canvasObj, "Assets/Prefabs/HUDCanvas.prefab");

            Debug.Log("[HUDCanvasBuilder] HUD Layout created and saved as prefab successfully!");
        }

        private static GameObject CreateUIElement(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 anchoredPosition)
        {
            return CreateUIElement(name, parent, anchorMin, anchorMax, pivot, anchoredPosition, Vector2.zero);
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

        private static UI_ResourceCounter CreateVerticalCounter(Transform parent, string resourceName, string spritePath)
        {
            // Vertical container (Icon on top, Text on bottom)
            GameObject container = CreateUIElement(resourceName + "Counter", parent, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(60, 100));
            VerticalLayoutGroup vLayout = container.AddComponent<VerticalLayoutGroup>();
            vLayout.childAlignment = TextAnchor.UpperCenter;
            vLayout.spacing = 5;

            // Icon
            GameObject iconObj = CreateUIElement("Icon", container.transform, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, new Vector2(60, 60));
            Image iconImg = iconObj.AddComponent<Image>();
            
            Sprite loadedSprite = LoadOrFixSprite(spritePath);
            if (loadedSprite != null) iconImg.sprite = loadedSprite;
            else iconImg.color = Color.gray; // fallback

            // Text
            GameObject textObj = CreateUIElement("Text", container.transform, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero, new Vector2(60, 30));
            TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
            text.text = "0/30";
            text.fontSize = 20;
            text.color = Color.white;
            text.alignment = TextAlignmentOptions.Center;

            UI_ResourceCounter counter = container.AddComponent<UI_ResourceCounter>();
            SerializedObject so = new SerializedObject(counter);
            SerializedProperty countProp = so.FindProperty("countText");
            if (countProp != null) countProp.objectReferenceValue = text;
            SerializedProperty iconProp = so.FindProperty("resourceIcon");
            if (iconProp != null) iconProp.objectReferenceValue = iconImg;
            so.ApplyModifiedProperties();

            return counter;
        }

        private static Sprite LoadOrFixSprite(string path)
        {
            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null && importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.SaveAndReimport();
            }
            return AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }

        private static void CleanAllOldHUDObjects()
        {
            string[] namesToDestroy = new string[] {
                "HUD_Canvas",
                "Canvas",
                "HUDManager",
                "HUD_Controller",
                "TopLeft_ProfilePanel",
                "TopCenter_TimerPanel",
                "TopRight_TimerPanel",
                "TopRight_PausePanel",
                "BottomLeft_ElementsPanel",
                "BottomRight_StatsPanel"
            };

            foreach (var name in namesToDestroy)
            {
                GameObject[] foundObjs = System.Array.FindAll(
                    Resources.FindObjectsOfTypeAll<GameObject>(),
                    go => go != null && go.name == name && go.hideFlags == HideFlags.None && !EditorUtility.IsPersistent(go)
                );

                foreach (var obj in foundObjs)
                {
                    Object.DestroyImmediate(obj);
                }
            }
        }
    }
}
