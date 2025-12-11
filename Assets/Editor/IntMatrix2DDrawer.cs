using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(IntMatrix2D))]
public class IntMatrix2DDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty rowsProp = property.FindPropertyRelative("rows");
        SerializedProperty colsProp = property.FindPropertyRelative("columns");
        SerializedProperty dataProp = property.FindPropertyRelative("data");

        int rows = rowsProp.intValue;
        int cols = colsProp.intValue;

        EditorGUI.LabelField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), label);

        float cellSize = 30f;
        float spacing = 4f;

        float startY = position.y + EditorGUIUtility.singleLineHeight + spacing;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int index = r * cols + c;

                if (index >= dataProp.arraySize) continue;

                SerializedProperty element = dataProp.GetArrayElementAtIndex(index);
                float x = position.x + c * (cellSize + spacing);
                float y = startY + r * (cellSize + spacing);

                Rect fieldRect = new Rect(x, y, cellSize, cellSize);
                element.intValue = EditorGUI.IntField(fieldRect, element.intValue);
            }
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int rows = property.FindPropertyRelative("rows").intValue;
        float cellSize = 30f;
        float spacing = 4f;

        return EditorGUIUtility.singleLineHeight + spacing + rows * (cellSize + spacing);
    }
}
