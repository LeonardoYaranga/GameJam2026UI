# Plan y Configuración: Menú de Pausa

Este documento detalla el plan implementado para el Menú de Pausa basándonos en la imagen de referencia proporcionada.

## 1. Estructura de UI Propuesta (Jerarquía en Unity)

El menú debe estructurarse utilizando el sistema de Canvas de Unity:

*   **PauseMenuCanvas** (Canvas principal, orden de clasificación alto para superponerse al juego)
    *   **BackgroundPanel**: Un panel oscuro semitransparente con una imagen o textura que simule la salpicadura de sangre en el lado izquierdo.
    *   **LeftMenuContainer** (Vertical Layout Group para alinear los botones):
        *   `TitleText`: "PAUSE" (Texto grande, rojo).
        *   `Btn_Continue`: Botón "CONTINUE".
        *   `Btn_SaveGame`: Botón "SAVE GAME".
        *   `Btn_Settings`: Botón "SETTINGS".
        *   `Btn_Quit`: Botón "QUIT".
    *   **RightInfoContainer**:
        *   **MinimapPanel** (Arriba derecha): Imagen circular con máscara para simular el mapa.
        *   **MissionsPanel** (Centro derecha): Texto de título "Missions" y un contenedor para la lista de misiones activas.
        *   **ControlsPanel** (Abajo derecha): Lista de textos con los atajos de teclado (Q - Interact, Space - Jump, etc.).

## 2. Archivos Creados
Se ha creado el siguiente script en la ruta `Assets/Scripts/UI/PauseMenuManager.cs`:

*   `PauseMenuManager.cs`: Controlador principal del menú de pausa. Se encarga de detectar el input del jugador para abrir/cerrar el menú, pausar el juego (`Time.timeScale = 0f`), reanudarlo (`Time.timeScale = 1f`), bloquear/desbloquear el cursor y proporciona las funciones para los botones (`ResumeGame()`, `SaveGame()`, `OpenSettings()`, `QuitGame()`).

## 3. Instrucciones de Configuración en el Editor (Walkthrough)

Sigue estos pasos en el Editor de Unity para armar la interfaz:

1. **Crear la Base de la UI (Canvas)**
   - Haz clic derecho en la ventana **Hierarchy** -> **UI** -> **Canvas**. Nómbralo `PauseMenuCanvas`.
   - Selecciona el Canvas y en el componente **Canvas Scaler**, cambia "UI Scale Mode" a `Scale With Screen Size` (Resolución de referencia recomendada: 1920x1080).
   - Crea un Panel dentro (Clic derecho -> UI -> Panel). Nómbralo `PauseMenuPanel` y cámbiale el color a negro con algo de transparencia (Alpha).

2. **Crear el Menú Izquierdo (Botones)**
   - Dentro de `PauseMenuPanel`, crea un objeto vacío (UI -> Empty) y nómbralo `LeftMenuContainer`. Ánclalo a la izquierda.
   - Añade a `LeftMenuContainer` el componente **Vertical Layout Group**.
   - Dentro, añade un Texto (TextMeshPro) para el título "PAUSE" y 4 botones (TextMeshPro) (`Btn_Continue`, `Btn_SaveGame`, `Btn_Settings`, `Btn_Quit`). Configúralos con color rojo y sus respectivos textos.

3. **Crear el Menú Derecho (Información)**
   - Dentro de `PauseMenuPanel`, crea otro objeto vacío, nómbralo `RightInfoContainer` y ánclalo a la derecha.
   - Añade la imagen del minimapa arriba, el panel de Misiones en el centro y los textos de Controles en la parte inferior.

4. **Conectar el Script**
   - Selecciona `PauseMenuCanvas` y añádele el script `PauseMenuManager`.
   - Arrastra el `PauseMenuPanel` a la variable del script en el Inspector.
   - Selecciona cada botón, en la sección `On Click ()`, añade un evento, arrastra el `PauseMenuCanvas` y selecciona la función correspondiente en `PauseMenuManager` (ej. para continuar, elige `ResumeGame`).
