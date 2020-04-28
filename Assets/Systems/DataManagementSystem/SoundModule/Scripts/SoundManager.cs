using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANR {

    public class SoundManager : MonoBehaviour {

        // Singleton - SoundManager should only be available once per scene
        #region Static Interface

        private static SoundManager _instance;

        public static SoundManager Instance {
            get {
                return _instance;
            }
        }

        private SoundManager() {

        }

        #endregion

        #region Properties

        // audio players components
        [SerializeField]
        private AudioSource effectsAudioSource;
        [SerializeField]
        private AudioSource musicAudioSource;

        // predefined audio
        [SerializeField]
        private AudioClip warningAudioClip;
        [SerializeField]
        private AudioClip infoAudioClip;

        #endregion

        private void Awake() {
            // checks if instance already exists
            if (_instance == null) {
                _instance = this;
            } else if (_instance != this) {
                // enforces singleton pattern -> there can only ever be one instance this class
                Destroy(gameObject);
            }

            // sets this to not be destroyed when reloading scene
            //DontDestroyOnLoad(gameObject);
        }

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        // Play a single clip through the sound effects source
        public void Play(AudioClip clip) {
            effectsAudioSource.clip = clip;
            effectsAudioSource.Play();
        }

        // Play a single clip through the music source
        public void PlayMusic(AudioClip clip) {
            musicAudioSource.clip = clip;
            musicAudioSource.Play();
        }

        public void PlayWarning() {
            Play(warningAudioClip);
        }

        public void PlayInfo() {
            Play(infoAudioClip);
        }
    }
}