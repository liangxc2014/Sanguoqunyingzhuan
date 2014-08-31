using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class CreateDecorationPrefab 
{

    public static string artDirectory = "Assets/Art/Decorations";
    public static string prefabDirectory = "Assets/Resources/Prefabs/Map/Decorations";

    [MenuItem("Tools/BatchPrefab All Decoration")]
    public static void BatchPrefab()
    {
        string[] files = Directory.GetFiles(artDirectory);

        Object prefab;
        for (int i = 0; i < files.Length; i++)
        {
            string filePath = files[i];

            if (Path.GetExtension(filePath) != ".png")
                continue;

            string fileName = Path.GetFileNameWithoutExtension(filePath);
            prefab = PrefabUtility.CreateEmptyPrefab(prefabDirectory + "/" + fileName + ".prefab");

            GameObject go = new GameObject();
            SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = AssetDatabase.LoadAssetAtPath(filePath, typeof(Sprite)) as Sprite;

            prefab = PrefabUtility.ReplacePrefab(go, prefab);

            GameObject.DestroyImmediate(go);
            //return;
        }

    }
}
