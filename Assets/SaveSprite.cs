using UnityEngine;
using UnityEditor;

public class SaveSprite
{
    [MenuItem("Tools/Export Wizard")]
    static void SaveSprites()
    {
        string resourcesPath = "Assets/Resources/";
        foreach (Object obj in Selection.objects)
        {
            string selectionPath = AssetDatabase.GetAssetPath(obj);
            // The top level must be "Assets/Resources/"
            if (selectionPath.StartsWith(resourcesPath))
            {
                string selectionExt = System.IO.Path.GetExtension(selectionPath);
                if (selectionExt.Length == 0)
                {
                    continue;
                }
                // Get the path "UI/testUI" from the path "Assets/Resources/UI/testUI.png"
                string loadPath = selectionPath.Remove(selectionPath.Length - selectionExt.Length);
                loadPath = loadPath.Substring(resourcesPath.Length);
                // Load all resources under this file
                Sprite[] sprites = Resources.LoadAll<Sprite>(loadPath);
                if (sprites.Length > 0)
                {
                    // Create export folder
                    string outPath = Application.dataPath + "/outSprite/" + loadPath;
                    System.IO.Directory.CreateDirectory(outPath);
                    foreach (Sprite sprite in sprites)
                    {
                        // Create a separate texture
                        Texture2D tex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, sprite.texture.format, false);
                        tex.SetPixels(sprite.texture.GetPixels((int)sprite.rect.xMin, (int)sprite.rect.yMin,
                            (int)sprite.rect.width, (int)sprite.rect.height));
                        tex.Apply();
                        // Write to PNG file
                        System.IO.File.WriteAllBytes(outPath + "/" + sprite.name + ".png", tex.EncodeToPNG());
                    }
                    Debug.Log("SaveSprite to " + outPath);
                }
            }
        }
        Debug.Log("SaveSprite Finished");
    }
}
