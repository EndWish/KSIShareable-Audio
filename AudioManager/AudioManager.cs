using KSIShareable.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSIShareable.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        [SerializeField] private bool dontDestroyOnLoad = true;

        [ShowScriptableObject, Space(10)]
        [SerializeField] private AudioLibrary audioLibrary;

        [SerializeField, Range(0, 1)] private float bgmVolume = 0.5f;
        public float BgmVolume { 
            get { return bgmVolume; } 
            set { 
                bgmVolume = value; 
                if(bgmSource != null) {
                    bgmSource.volume = bgmVolume;
                }
            }
        }
        AudioSource bgmSource;

        [SerializeField, Range(0, 1)] private float sfxVolume = 0.5f;
        public float SfxVolume {
            get { return sfxVolume; }
            set {
                sfxVolume = value;
                if (sfxSources != null) {
                    for(int i = 0; i < sfxSources.Length; i++) {
                        sfxSources[i].volume = sfxVolume;
                    }
                }
            }
        }

        [SerializeField] private int channelCount = 16;
        public int ChannelCount { get { return channelCount; } }
        protected AudioSource[] sfxSources;
        private int channelIndex;

        protected void Awake() {
            if (Instance == null) {
                Instance = this;
                if (dontDestroyOnLoad) {
                    DontDestroyOnLoad(gameObject);
                }
                audioLibrary.Init();
                Init();
            }
            else {
                Destroy(gameObject);
            }
        }

        private void Init() {
            // 배경음 플레이어 초기화
            GameObject bgmObject = new GameObject("BgmPlayer");
            bgmObject.transform.SetParent(this.transform);
            bgmSource = bgmObject.AddComponent<AudioSource>();
            bgmSource.playOnAwake = false;
            bgmSource.loop = true;
            bgmSource.volume = BgmVolume;

            // 효과음 플레이어 초기화
            GameObject sfxObject = new GameObject("SfxPlayer");
            sfxObject.transform.SetParent(this.transform);
            sfxSources = new AudioSource[channelCount];

            for(int i = 0; i < channelCount; i++) {
                sfxSources[i] = sfxObject.AddComponent<AudioSource>();
                sfxSources[i].playOnAwake = false;
                sfxSources[i].volume = sfxVolume;
            }
        }

        public void PlayBgm(string key) {
            AudioClip clip = audioLibrary.GetClip(key);
            if (clip == null) {
                Debug.LogWarning($"BGM '{key}' not found in AudioLibrary!");
                return;
            }

            bgmSource.clip = clip;
            bgmSource.Play();
        }
        public void StopBgm() {
            bgmSource.Stop();
        }
        public void PauseBgm() {
            bgmSource.Pause();
        }
        public void UnPauseBgm() {
            bgmSource.UnPause();
        }

        public void PlaySfx(string key) {
            AudioClip clip = audioLibrary.GetClip(key);
            if (clip == null) {
                Debug.LogWarning($"SFX '{key}' not found in AudioLibrary!");
                return;
            }

            for(int i = 0; i < channelCount; i++) {
                int loopIndex = (channelIndex + 1 + i) % channelCount;

                if (sfxSources[loopIndex].isPlaying)
                    continue;

                sfxSources[loopIndex].clip = clip;
                sfxSources[loopIndex].Play();
                channelIndex = loopIndex;
                break;
            }
        }

    }
}