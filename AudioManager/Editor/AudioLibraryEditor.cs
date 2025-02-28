using UnityEditor;
using UnityEngine;

namespace KSIShareable.Audio
{
    [CustomEditor(typeof(AudioLibrary))]
    public class AudioLibraryEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            AudioLibrary library = (AudioLibrary)target;

            if (GUILayout.Button("Sort by Key")) {
                library.SortByKey();
            }
        }
    }
}