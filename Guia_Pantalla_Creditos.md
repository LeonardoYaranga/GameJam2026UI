# Guía de Configuración: Pantalla de Créditos (Escena Dedicada `Credits.unity`)

Esta guía explica el funcionamiento de la **escena dedicada de Créditos (`Credits.unity`)** y su integración con `MainMenu.unity`.

---

## 1. Escena y Configuración de Build

- **Ruta de la Escena**: `Assets/Scenes/Credits.unity` (Creada e incluida en las **Build Settings**).
- **Escenas en el Proyecto**:
  1. `Assets/Scenes/MainMenu.unity` (Índice 0)
  2. `Assets/Scenes/Hud.unity` (Índice 1)
  3. `Assets/Scenes/Credits.unity` (Índice 2)

---

## 2. Flujo de Navegación entre Escenas

1. **Desde `MainMenu.unity`**:
   - Al presionar el botón **CREDITS**, el script `MainMenuManager.cs` ejecuta `Credits()`, el cual hace `SceneManager.LoadScene("Credits")`.

2. **Desde `Credits.unity`**:
   - Al presionar el botón **VOLVER / MENÚ PRINCIPAL**, el script `CreditsManager.cs` ejecuta `ReturnToMainMenu()`, cargando de regreso `MainMenu.unity`.

---

## 3. Jerarquía Recomendada para la Escena `Credits.unity`

Al abrir `Assets/Scenes/Credits.unity`, configura la estructura en el Canvas así:

```text
CreditsCanvas (Canvas con Canvas Scaler: Scale With Screen Size 1920x1080)
├── Background (Image - Fondo oscuro / textura)
│
├── HeaderArea (RectTransform Top)
│   ├── MainTitleText (TextMeshProUGUI - "¡GRACIAS POR PROBAR NUESTRO JUEGO!")
│   └── SubtitleText (TextMeshProUGUI - "Game Jam 2026 - Créditos del Equipo")
│
├── ScrollView (ScrollRect con ScrollbarVertical)
│   └── Viewport (Mask / RectMask2D)
│       └── Content (VerticalLayoutGroup + ContentSizeFitter)
│           ├── Section_Programming ("PROGRAMACIÓN" + Integrantes & Redes)
│           ├── Section_Art ("ARTE & ANIMACIÓN" + Integrantes & Redes)
│           ├── Section_Music ("MÚSICA & AUDIO" + Integrantes & Redes)
│           ├── Section_Story ("GUIÓN & NARRATIVA" + Integrantes & Redes)
│           ├── Section_GameDesign ("DISEÑO DE JUEGO" + Integrantes & Redes)
│           ├── Section_AIDisclaimer (Aviso sobre uso de IA en código y assets / Música 100% Humana)
│           └── Section_TeamPhoto (Panel con Imagen / Foto del Equipo)
│
└── FooterArea (RectTransform Bottom)
    └── Btn_Back (Button con TextMeshProUGUI - "VOLVER AL MENÚ")
```

---

## 4. Asignaciones en `CreditsManager` dentro de `Credits.unity`

1. En la escena `Credits.unity`, crea un objeto `CreditsController` (o agrégalo al Canvas).
2. Asigna el script `CreditsManager.cs`.
3. Configura los parámetros:
   - **Scroll Rect**: Asigna el `ScrollRect` de la escena.
   - **Back Button**: Asigna el botón **VOLVER**.
   - **Main Menu Scene Name**: `"MainMenu"`.
   - **Auto Scroll Enabled**: `true`.
   - **Auto Scroll Speed**: `0.05`.
