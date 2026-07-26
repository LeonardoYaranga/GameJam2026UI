using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

namespace CardSystem.Editor
{
    public class CardSystemAutoSetup : EditorWindow
    {
        [MenuItem("GameJam2026/Auto Setup Card UI System")]
        public static void SetupCardSystem()
        {
            // 1. Configurar Texturas como Sprites
            ConfigureSpritesImport();

            // 2. Crear carpetas necesarias
            if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
            {
                AssetDatabase.CreateFolder("Assets", "Prefabs");
            }

            // 3. Crear Prefab de StatRow
            GameObject statRowObj = CreateStatRowGameObject();
            string statRowPrefabPath = "Assets/Prefabs/StatRowUI.prefab";
            GameObject statRowPrefab = PrefabUtility.SaveAsPrefabAsset(statRowObj, statRowPrefabPath);
            Object.DestroyImmediate(statRowObj);

            // 4. Crear Prefab de CardUI
            GameObject cardObj = CreateCardGameObject(statRowPrefab.GetComponent<StatRowUI>());
            string cardPrefabPath = "Assets/Prefabs/CardUI.prefab";
            GameObject cardPrefab = PrefabUtility.SaveAsPrefabAsset(cardObj, cardPrefabPath);
            Object.DestroyImmediate(cardObj);

            // 5. Crear la jerarquía en la Escena Actual
            CreateSceneHierarchy(cardPrefab.GetComponent<CardDisplayUI>());

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("<color=#00FF00><b>[CardSystemAutoSetup]</b> ¡Sistema de Tarjetas de UI configurado exitosamente en la escena!</color>");
        }

        private static void ConfigureSpritesImport()
        {
            string folderPath = "Assets/Sprites/UI";
            if (!Directory.Exists(folderPath)) return;

            string[] files = Directory.GetFiles(folderPath, "*.jpg");
            foreach (string file in files)
            {
                string assetPath = file.Replace("\\", "/");
                TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                if (importer != null && importer.textureType != TextureImporterType.Sprite)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    importer.SaveAndReimport();
                }
            }
        }

        private static GameObject CreateStatRowGameObject()
        {
            GameObject row = new GameObject("StatRowUI", typeof(RectTransform), typeof(Image), typeof(HorizontalLayoutGroup), typeof(StatRowUI));
            RectTransform rowRect = row.GetComponent<RectTransform>();
            rowRect.sizeDelta = new Vector2(240, 45);

            Image rowBg = row.GetComponent<Image>();
            rowBg.color = new Color(0.1f, 0.12f, 0.18f, 0.85f);

            HorizontalLayoutGroup hlg = row.GetComponent<HorizontalLayoutGroup>();
            hlg.spacing = 10;
            hlg.padding = new RectOffset(8, 8, 5, 5);
            hlg.childAlignment = TextAnchor.MiddleLeft;
            hlg.childControlWidth = false;
            hlg.childControlHeight = true;

            // Stat Icon
            GameObject iconObj = new GameObject("StatIcon", typeof(RectTransform), typeof(Image));
            iconObj.transform.SetParent(row.transform, false);
            RectTransform iconRect = iconObj.GetComponent<RectTransform>();
            iconRect.sizeDelta = new Vector2(35, 35);
            Image iconImg = iconObj.GetComponent<Image>();

            // Stat Text
            GameObject textObj = new GameObject("StatText", typeof(RectTransform), typeof(TextMeshProUGUI));
            textObj.transform.SetParent(row.transform, false);
            TextMeshProUGUI tmp = textObj.GetComponent<TextMeshProUGUI>();
            tmp.fontSize = 18;
            tmp.fontStyle = FontStyles.Bold;
            tmp.alignment = TextAlignmentOptions.Left;
            tmp.text = "+2 Ataque";
            tmp.color = Color.white;

            StatRowUI script = row.GetComponent<StatRowUI>();
            SerializedObject so = new SerializedObject(script);
            so.FindProperty("statIconImage").objectReferenceValue = iconImg;
            so.FindProperty("statTextLabel").objectReferenceValue = tmp;
            so.ApplyModifiedProperties();

            return row;
        }

        private static GameObject CreateCardGameObject(StatRowUI statRowPrefab)
        {
            GameObject card = new GameObject("CardUI", typeof(RectTransform), typeof(Image), typeof(Button), typeof(VerticalLayoutGroup), typeof(CardDisplayUI));
            RectTransform cardRect = card.GetComponent<RectTransform>();
            cardRect.sizeDelta = new Vector2(280, 420);

            Image cardBg = card.GetComponent<Image>();
            cardBg.color = new Color(0.08f, 0.09f, 0.14f, 0.95f);

            VerticalLayoutGroup vlg = card.GetComponent<VerticalLayoutGroup>();
            vlg.spacing = 12;
            vlg.padding = new RectOffset(15, 15, 20, 20);
            vlg.childAlignment = TextAnchor.UpperCenter;
            vlg.childControlWidth = true;
            vlg.childControlHeight = false;

            // Element Header Icon
            GameObject headerObj = new GameObject("ElementHeader", typeof(RectTransform), typeof(Image));
            headerObj.transform.SetParent(card.transform, false);
            RectTransform headerRect = headerObj.GetComponent<RectTransform>();
            headerRect.sizeDelta = new Vector2(80, 80);
            Image headerImg = headerObj.GetComponent<Image>();

            // Element Label
            GameObject elemLabelObj = new GameObject("ElementLabel", typeof(RectTransform), typeof(TextMeshProUGUI));
            elemLabelObj.transform.SetParent(card.transform, false);
            TextMeshProUGUI elemTmp = elemLabelObj.GetComponent<TextMeshProUGUI>();
            elemTmp.fontSize = 14;
            elemTmp.fontStyle = FontStyles.Bold;
            elemTmp.alignment = TextAlignmentOptions.Center;
            elemTmp.text = "FUEGO";

            // Title Text
            GameObject titleObj = new GameObject("CardTitle", typeof(RectTransform), typeof(TextMeshProUGUI));
            titleObj.transform.SetParent(card.transform, false);
            TextMeshProUGUI titleTmp = titleObj.GetComponent<TextMeshProUGUI>();
            titleTmp.fontSize = 22;
            titleTmp.fontStyle = FontStyles.Bold;
            titleTmp.alignment = TextAlignmentOptions.Center;
            titleTmp.text = "Nombre de Tarjeta";
            titleTmp.color = Color.gold;

            // Stats Container
            GameObject statsContainer = new GameObject("StatsContainer", typeof(RectTransform), typeof(VerticalLayoutGroup));
            statsContainer.transform.SetParent(card.transform, false);
            VerticalLayoutGroup statsVlg = statsContainer.GetComponent<VerticalLayoutGroup>();
            statsVlg.spacing = 8;

            CardDisplayUI script = card.GetComponent<CardDisplayUI>();
            SerializedObject so = new SerializedObject(script);
            so.FindProperty("elementHeaderIcon").objectReferenceValue = headerImg;
            so.FindProperty("cardGlowFrame").objectReferenceValue = cardBg;
            so.FindProperty("titleText").objectReferenceValue = titleTmp;
            so.FindProperty("elementLabelText").objectReferenceValue = elemTmp;
            so.FindProperty("statsContainer").objectReferenceValue = statsContainer.transform;
            so.FindProperty("cardButton").objectReferenceValue = card.GetComponent<Button>();
            so.FindProperty("statRowPrefab").objectReferenceValue = statRowPrefab;
            so.ApplyModifiedProperties();

            return card;
        }

        private static void CreateSceneHierarchy(CardDisplayUI cardPrefab)
        {
            // 1. Limpieza de duplicados previos (incluyendo objetos desactivados)
            CleanOldCardSystemObjects();

            // 2. EventSystem (InputSystem Package)
            UnityEngine.EventSystems.EventSystem eventSystem = Object.FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>();
            if (eventSystem == null)
            {
                GameObject esObj = new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem));
#if ENABLE_INPUT_SYSTEM
                esObj.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
#else
                esObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
#endif
            }
            else
            {
                var standalone = eventSystem.GetComponent<UnityEngine.EventSystems.StandaloneInputModule>();
                if (standalone != null)
                {
                    Object.DestroyImmediate(standalone);
#if ENABLE_INPUT_SYSTEM
                    if (eventSystem.GetComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>() == null)
                    {
                        eventSystem.gameObject.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
                    }
#endif
                }
            }

            // 3. PlayerStats Manager
            PlayerStats ps = Object.FindFirstObjectByType<PlayerStats>();
            if (ps == null)
            {
                GameObject psObj = new GameObject("PlayerStatsSystem", typeof(PlayerStats));
                ps = psObj.GetComponent<PlayerStats>();
            }

            // 4. Canvas exclusivo para el Sistema de Tarjetas (Separado del HUD)
            GameObject cardCanvasObj = new GameObject("CardSelection_Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas cardCanvas = cardCanvasObj.GetComponent<Canvas>();
            cardCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            cardCanvas.sortingOrder = 10; // Prioridad alta por encima del HUD

            CanvasScaler scaler = cardCanvasObj.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            // 5. Controller Principal
            CardSelectionOverlayManager manager = cardCanvasObj.AddComponent<CardSelectionOverlayManager>();

            // 6. Test Button ("Generar Tarjetas") dentro del CardSelection_Canvas
            GameObject btnObj = new GameObject("Btn_GenerarTarjetas", typeof(RectTransform), typeof(Image), typeof(Button));
            btnObj.transform.SetParent(cardCanvasObj.transform, false);
            RectTransform btnRect = btnObj.GetComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(0.5f, 0.08f);
            btnRect.anchorMax = new Vector2(0.5f, 0.08f);
            btnRect.anchoredPosition = Vector2.zero;
            btnRect.sizeDelta = new Vector2(280, 65);

            Image btnImg = btnObj.GetComponent<Image>();
            btnImg.color = new Color(0.15f, 0.55f, 0.95f, 1f);

            GameObject btnTextObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
            btnTextObj.transform.SetParent(btnObj.transform, false);
            TextMeshProUGUI btnTmp = btnTextObj.GetComponent<TextMeshProUGUI>();
            btnTmp.fontSize = 20;
            btnTmp.fontStyle = FontStyles.Bold;
            btnTmp.alignment = TextAlignmentOptions.Center;
            btnTmp.text = "Generar Tarjetas [G]";
            btnTmp.color = Color.white;

            // 7. Overlay Panel (Fondo oscuro)
            GameObject overlayPanelObj = new GameObject("CardSelectionOverlayPanel", typeof(RectTransform), typeof(Image));
            overlayPanelObj.transform.SetParent(cardCanvasObj.transform, false);
            RectTransform overlayRect = overlayPanelObj.GetComponent<RectTransform>();
            overlayRect.anchorMin = Vector2.zero;
            overlayRect.anchorMax = Vector2.one;
            overlayRect.sizeDelta = Vector2.zero;

            Image overlayImg = overlayPanelObj.GetComponent<Image>();
            overlayImg.color = new Color(0f, 0f, 0f, 0.88f);

            // 8. Container para las 3 tarjetas
            GameObject containerObj = new GameObject("CardsContainer", typeof(RectTransform), typeof(HorizontalLayoutGroup));
            containerObj.transform.SetParent(overlayPanelObj.transform, false);
            RectTransform containerRect = containerObj.GetComponent<RectTransform>();
            containerRect.anchorMin = new Vector2(0.5f, 0.5f);
            containerRect.anchorMax = new Vector2(0.5f, 0.5f);
            containerRect.sizeDelta = new Vector2(950, 450);
            containerRect.anchoredPosition = Vector2.zero;

            HorizontalLayoutGroup hlg = containerObj.GetComponent<HorizontalLayoutGroup>();
            hlg.spacing = 30;
            hlg.childAlignment = TextAnchor.MiddleCenter;
            hlg.childControlWidth = false;
            hlg.childControlHeight = false;

            // 9. Asignar referencias al manager
            SerializedObject so = new SerializedObject(manager);
            so.FindProperty("overlayPanel").objectReferenceValue = overlayPanelObj;
            so.FindProperty("cardsContainer").objectReferenceValue = containerObj.transform;
            so.FindProperty("generateCardsButton").objectReferenceValue = btnObj.GetComponent<Button>();
            so.FindProperty("cardPrefab").objectReferenceValue = cardPrefab;
            so.FindProperty("playerStats").objectReferenceValue = ps;

            // Cargar Sprites de Elementos
            so.FindProperty("fireSprite").objectReferenceValue = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/element_fire.jpg");
            so.FindProperty("waterSprite").objectReferenceValue = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/element_water.jpg");
            so.FindProperty("natureSprite").objectReferenceValue = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/element_nature.jpg");

            // Cargar Sprites de Fondos de Tarjetas
            so.FindProperty("fireCardBgSprite").objectReferenceValue = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/card_bg_fire.jpg");
            so.FindProperty("waterCardBgSprite").objectReferenceValue = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/card_bg_water.jpg");
            so.FindProperty("natureCardBgSprite").objectReferenceValue = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/card_bg_nature.jpg");

            // Cargar Sprites de Stats
            so.FindProperty("attackSprite").objectReferenceValue = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/stat_attack.jpg");
            so.FindProperty("defenseSprite").objectReferenceValue = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/stat_defense.jpg");
            so.FindProperty("healthSprite").objectReferenceValue = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/stat_health.jpg");
            so.FindProperty("speedSprite").objectReferenceValue = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/stat_speed.jpg");
            so.FindProperty("agilitySprite").objectReferenceValue = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/stat_agility.jpg");
            so.FindProperty("staminaSprite").objectReferenceValue = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/UI/stat_stamina.jpg");
            so.ApplyModifiedProperties();

            // Conectar Listener al Botón
            Button btn = btnObj.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            UnityEditor.Events.UnityEventTools.AddPersistentListener(btn.onClick, manager.OnGenerateCardsButtonPressed);

            // 10. Guardar como Prefab modular en Assets/Prefabs/CardSelectionCanvas.prefab
            string cardCanvasPrefabPath = "Assets/Prefabs/CardSelectionCanvas.prefab";
            PrefabUtility.SaveAsPrefabAsset(cardCanvasObj, cardCanvasPrefabPath);

            overlayPanelObj.SetActive(false);
        }

        private static void CleanOldCardSystemObjects()
        {
            string[] namesToDestroy = new string[] {
                "CardSelection_Canvas",
                "CardSelectionOverlayPanel",
                "CardSelectionOverlay",
                "CardSystemController",
                "Btn_GenerarTarjetas"
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
