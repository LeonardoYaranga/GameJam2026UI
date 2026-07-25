using UnityEngine;
using UnityEditor;
using System.IO;

namespace GameJamUI.HUD.Editor
{
    public class UISpriteImporter : MonoBehaviour
    {
        [MenuItem("GameJamUI/Convert UI Textures to Sprites")]
        public static void ConvertUITexturesToSprites()
        {
            string folderPath = "Assets/Sprites/UI";
            if (!Directory.Exists(folderPath))
            {
                Debug.LogWarning($"[UISpriteImporter] Folder {folderPath} does not exist.");
                return;
            }

            string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
            int convertedCount = 0;

            foreach (string filePath in files)
            {
                string ext = Path.GetExtension(filePath).ToLower();
                if (ext == ".png" || ext == ".jpg" || ext == ".jpeg")
                {
                    TextureImporter importer = AssetImporter.GetAtPath(filePath) as TextureImporter;
                    if (importer != null)
                    {
                        bool needsReimport = false;

                        if (importer.textureType != TextureImporterType.Sprite)
                        {
                            importer.textureType = TextureImporterType.Sprite;
                            importer.spriteImportMode = SpriteImportMode.Single;
                            needsReimport = true;
                        }

                        if (needsReimport)
                        {
                            importer.SaveAndReimport();
                            convertedCount++;
                        }
                    }
                }
            }

            AssetDatabase.Refresh();
            Debug.Log($"[UISpriteImporter] Successfully converted {convertedCount} textures to Sprite (2D and UI) in {folderPath}!");
        }
    }
}
