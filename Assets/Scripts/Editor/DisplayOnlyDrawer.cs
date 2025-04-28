#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DisplayOnlyAttribute))]
public class DisplayOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false; // 禁用GUI
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true; // 恢复GUI状态
    }
}
#endif