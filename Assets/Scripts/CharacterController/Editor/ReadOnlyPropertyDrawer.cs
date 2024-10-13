using UnityEngine;
using UnityEditor;

namespace KinematicCharacterController
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            UnityEngine.GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            UnityEngine.GUI.enabled = true;
        }
    }
}