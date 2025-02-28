using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace KSIShareable.Audio
{
    [CreateAssetMenu(fileName = "AudioLibrary", menuName = "Audio/Audio Library")]
    public class AudioLibrary : ScriptableObject
    {
        [System.Serializable]
        public class AudioData
        {
            public string key;
            public AudioClip clip;
        }

        [ContextMenuItem("SortByKey", "SortByKey")]
        public List<AudioData> AudioList = new List<AudioData>();

        private bool isInitialized = false;
        private Dictionary<string, AudioClip> audioDict;

        private void OnDisable() {
            isInitialized = false;
            audioDict = null;
        }

        public void Init() {
            if (isInitialized)
                return;

            isInitialized = true;
            audioDict = new Dictionary<string, AudioClip>();

            foreach (var data in AudioList) {

                if (data.clip == null) {
                    Debug.LogWarning($"[AudioLibrary] Key '{data.key}': AudioClip is null. \n(This message only appears in the editor)");
                    continue;
                }

                if (audioDict.ContainsKey(data.key)) {
                    Debug.LogWarning($"[AudioLibrary] Duplicate key detected: '{data.key}'. \n(This message only appears in the editor)");
                    continue;
                }

                audioDict[data.key] = data.clip;
            }
        }

        public AudioClip GetClip(string key) {
            if (!isInitialized) {
                Debug.LogWarning($"[AudioLibrary] `{name}` is not initialized. Initializing now.");
                Init();  // 사용될 때 자동 초기화
            }
            return audioDict.TryGetValue(key, out var clip) ? clip : null;
        }
        public List<string> GetAllKeys() {
            if (!isInitialized) {
                Debug.LogWarning($"[AudioLibrary] `{name}` is not initialized. Initializing now.");
                Init();  // 사용될 때 자동 초기화
            }
            return audioDict.Keys.ToList();
        }


#if UNITY_EDITOR
        public void SortByKey() {
            AudioList = AudioList.OrderBy(data => data.key).ToList();
            EditorUtility.SetDirty(this); // 변경 사항을 저장
            Debug.Log($"[AudioLibrary] {name} has been sorted by key.");
        }
#endif

    }
}