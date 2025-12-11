using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(GameObject))]
public class SlotSymbolDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.ObjectField(position, property, label);

        if (property.objectReferenceValue is GameObject go)
        {
            GameObject prefabContents = PrefabUtility.LoadPrefabContents(AssetDatabase.GetAssetPath(go));
            if (prefabContents != null)
            {
                SpriteRenderer sr = prefabContents.GetComponent<SpriteRenderer>();
                if (sr != null && sr.sprite != null)
                {
                    float previewSize = 50f;
                    Rect previewRect = new Rect(position.x + position.width - previewSize, position.y, previewSize, previewSize);

                    EditorGUI.DrawPreviewTexture(previewRect, sr.sprite.texture);
                }
                PrefabUtility.UnloadPrefabContents(prefabContents);
            }
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 54f; 
    }
}
