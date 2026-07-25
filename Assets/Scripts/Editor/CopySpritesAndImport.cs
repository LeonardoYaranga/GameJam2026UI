using System.IO;
using UnityEngine;
using UnityEditor;

namespace CardSystem.Editor
{
    public class CopySpritesAndImport
    {
        [MenuItem("GameJam2026/Import Generated Sprites")]
        public static void ImportSprites()
        {
            string sourceFolder = "/home/leo/.gemini/antigravity/brain/ca097368-91a6-40fa-aee7-5bac9a920d28";
            string targetFolder = Path.Combine(Application.dataPath, "Sprites/UI");

            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            string[,] fileMap = new string[,]
            {
                { "element_fire_icon_1785001336739.jpg", "element_fire.jpg" },
                { "element_water_icon_1785001347982.jpg", "element_water.jpg" },
                { "element_nature_icon_1785001360701.jpg", "element_nature.jpg" },
                { "stat_sword_icon_1785001373822.jpg", "stat_attack.jpg" },
                { "stat_shield_icon_1785001390144.jpg", "stat_defense.jpg" },
                { "stat_heart_icon_1785001403622.jpg", "stat_health.jpg" }
            };

            for (int i = 0; i < fileMap.GetLength(0); i++)
            {
                string srcName = fileMap[i, 0];
                string destName = fileMap[i, 1];

                string srcPath = Path.Combine(sourceFolder, srcName);
                string destPath = Path.Combine(targetFolder, destName);

                if (File.Exists(srcPath))
                {
                    File.Copy(srcPath, destPath, true);
                    Debug.Log($"[CopySprites] Copiado {srcName} -> {destName}");
                }
                else
                {
                    Debug.LogWarning($"[CopySprites] Archivo no encontrado: {srcPath}");
                }
            }

            AssetDatabase.Refresh();

            // Configurar cada textura como Sprite 2D / UI
            for (int i = 0; i < fileMap.GetLength(0); i++)
            {
                string relativePath = "Assets/Sprites/UI/" + fileMap[i, 1];
                TextureImporter importer = AssetImporter.GetAtPath(relativePath) as TextureImporter;
                if (importer != null)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    importer.SaveAndReimport();
                    Debug.Log($"[CopySprites] Configurado como Sprite: {relativePath}");
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
