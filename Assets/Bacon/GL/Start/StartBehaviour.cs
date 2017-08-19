﻿using UnityEngine;
using Maria;
using UnityEngine.UI;
using Bacon.Event;
using Maria.Util;
using Maria.Res;

namespace Bacon.GL.Start {
    [RequireComponent(typeof(FindApp))]
    public class StartBehaviour : MonoBehaviour {

        public GameObject _Slider;
        public GameObject _Tips;

        private int _updateres = 0; // 0: 不做任何事，1，与远程服务器对比，2：只做一个展示
        private float _progress = 0.0f;

        // Use this for initialization
        void Start() {
            Maria.Util.App.current.AddObserver(Maria.Util.App.NOTIFICATION_APP_START, (Maria.Util.App.Notification notification) => {
                Maria.Command cmd = new Maria.Command(EventCmd.EVENT_START_SETUP_ROOT, gameObject);
                GetComponent<FindApp>().App.Enqueue(cmd);
            });
            _progress = 0.0f;
        }

        // Update is called once per frame
        void Update() {
            if (_updateres == 2) {
                if (_progress <= 1.0f) {
                    _progress += 0.01f;

                    _Slider.GetComponent<Slider>().value = _progress > 1 ? 1 : _progress;
                    _Tips.transform.Find("Text").GetComponent<Text>().text = string.Format("%{0}", Mathf.FloorToInt((_progress > 1 ? 1 : _progress) * 100));

                    if (_progress > 1.0f) {
                        Command cmd = new Command(MyEventCmd.EVENT_UPdATERES);
                        GetComponent<FindApp>().App.Enqueue(cmd);
                    }
                }
            }
        }


        public void UpdateRes() {
            _updateres = 1;
            Maria.Util.App.current.StartUpdateRes();

            //SayDataSet.Instance.Load();
            //UnityEngine.Debug.Log(SayDataSet.Instance.GetSayItem(1).text);

            //TextAsset data = ABLoader.current.LoadTextAsset("Excels", "data");
            //Configs.dataConfig dataconfig = new Configs.dataConfig();
            //dataconfig.Load(data.text);
            //dataconfig.Items;
        }

        public void TestRes() {
            _updateres = 2;
            // 下面是测试代码
            //ABLoader.current.LoadAssetAsync<AudioClip>("Sound/Man", "peng", (AudioClip clip) => {
            //    UnityEngine.Debug.Log("ok");
            //    //Command cmd = new Command(MyEventCmd.EVENT_UPdATERES);
            //    //_root.App.Enqueue(cmd);
            //});

            //ABLoader.current.LoadAssetAsync<AudioClip>("Sound/Woman", "bam1", (AudioClip clip) => {
            //    UnityEngine.Debug.Log("ok");
            //});

        }
    }
}