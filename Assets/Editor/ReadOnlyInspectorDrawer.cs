using UnityEditor;
using UnityEngine;
using Assets.Scripts.Utills;

[CustomPropertyDrawer(typeof(ReadOnlyInspectorAttribute))]
public class ReadOnlyInspectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label);
        GUI.enabled = true;
    }
}
