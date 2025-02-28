using UnityEngine;
using UnityEditor;

namespace KSIShareable.Audio
{
    [CustomPropertyDrawer(typeof(AudioLibrary.AudioData))]
    public class AudioDataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            float halfWidth = position.width / 2;

            // Key 필드
            SerializedProperty keyProperty = property.FindPropertyRelative("key");
            Rect keyRect = new Rect(position.x, position.y, halfWidth - 5, position.height);
            keyProperty.stringValue = EditorGUI.TextField(keyRect, keyProperty.stringValue);

            // Clip 필드
            SerializedProperty clipProperty = property.FindPropertyRelative("clip");
            Rect clipRect = new Rect(position.x + halfWidth + 5, position.y, halfWidth - 5, position.height);
            clipProperty.objectReferenceValue = EditorGUI.ObjectField(clipRect, clipProperty.objectReferenceValue, typeof(AudioClip), false);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight; // 한 줄 높이로 설정
        }
    }
}
