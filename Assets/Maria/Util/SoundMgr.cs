using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maria.Util {
    public class SoundMgr : MonoBehaviour {

        public static SoundMgr current = null;

        private float _music = 1.0f;
        private float _sound = 1.0f;
        private AudioClip _musicClip;
        private GameObject _soundGo;
        private AudioClip _soundClip;

        private Dictionary<string, AudioClip> _res = new Dictionary<string, AudioClip>();

        void Awake() {
            if (current == null) {
                current = this;
            }
        }

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void SetMusic(float value) {
            if (_music > 0.0f) {
                if (value == 0.0f) {
                    _music = value;
                    GetComponent<AudioSource>().Stop();
                } else {
                    _music = value;
                    //GetComponent<AudioSource>().
                }
            } else {
                _music = value;
            }
        }

        public void SetSound(float value) {
            if (_sound > 0.0f) {
                if (value == 0.0f) {
                    _sound = value;
                    _soundGo.GetComponent<AudioSource>().Stop();
                }
            } else {
                _sound = value;
            }
        }


        public void PlayMusic(AudioClip clip) {
            _musicClip = clip;
            gameObject.GetComponent<AudioSource>().clip = clip;
            if (_music > 0.0f) {
                gameObject.GetComponent<AudioSource>().Play();
            }
        }

        public void StopMusic() {
            gameObject.GetComponent<AudioSource>().Stop();
        }

        public void PlaySound(GameObject go, AudioClip clip) {
            _soundGo = go;
            _soundClip = clip;
            if (clip != null) {
                go.GetComponent<AudioSource>().clip = clip;
            }
            if (_sound > 0.0f) {
                go.GetComponent<AudioSource>().Play();
            }
        }

        public void PlaySound(GameObject go, string path, string name) {
            string key = path + "/" + name;
            if (_res.ContainsKey(key)) {
                AudioClip clip = _res[path];
                PlaySound(go, clip);
            } else {
                AudioClip clip = ABLoader.current.LoadAsset<AudioClip>(path, name);
                _res[key] = clip;
                PlaySound(go, clip);
            }
        }
    }
}