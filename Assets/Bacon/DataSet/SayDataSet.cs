using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Configs;
using Maria.Util;

namespace Bacon.DataSet {
    public class SayDataSet : Maria.Singleton<SayDataSet> {
        public class SayItem {
            public long id;
            public long code;
            public string text;
            public string sound;
        }

        public class DataItem {
            public long key;
            public string value;
        }

        public class HuTypeItem {
            public long id;
            public string ch;
        }

        private Dictionary<long, SayItem> _says = new Dictionary<long, SayItem>();
        private Dictionary<long, DataItem> _datas = new Dictionary<long, DataItem>();
        private Dictionary<long, HuTypeItem> _hutypes = new Dictionary<long, HuTypeItem>();

        public void Load() {
            LoadSay();
            LoadData();
            LoadHuType();
        }

        public Dictionary<long, SayItem> GetSays() {
            return _says;
        }

        public SayItem GetSayItem(long id) {
            if (_says.ContainsKey(id)) {
                return _says[id];
            }
            return null;
        }

        public Dictionary<long, DataItem> GetDatas() {
            return _datas;
        }

        public DataItem GetDataItem(long id) {
            if (_datas.ContainsKey(id)) {
                return _datas[id];
            }
            return null;
        }

        public Dictionary<long, HuTypeItem> GetHuTypes() {
            return _hutypes;
        }

        public HuTypeItem GetHuTypeItem(long id) {
            if (_hutypes.ContainsKey(id)) {
                return _hutypes[id];
            }
            return null;
        }

        private void LoadSay() {
            UnityEngine.TextAsset ts = ABLoader.current.LoadTextAsset("Excels", "say");
            sayConfig config = new sayConfig();
            config.Load(ts.text);
            foreach (var item in config.Items) {
                SayItem say = new SayItem();
                say.id = item.Value["id"].l;
                say.code = item.Value["code"].l;
                say.text = item.Value["text"].s;
                say.sound = item.Value["sound"].s;
                _says.Add(say.id, say);
            }
        }

        private void LoadData() {
            UnityEngine.TextAsset ts = ABLoader.current.LoadTextAsset("Excels", "data");
            dataConfig config = new dataConfig();
            config.Load(ts.text);
            foreach (var item in config.Items) {
                DataItem say = new DataItem();
                say.key = item.Value["key"].l;
                say.value = item.Value["value"].s;
                _datas.Add(say.key, say);
            }
        }

        private void LoadHuType() {
            UnityEngine.TextAsset ts = ABLoader.current.LoadTextAsset("Excels", "hutype");
            hutypeConfig config = new hutypeConfig();
            config.Load(ts.text);
            foreach (var item in config.Items) {
                HuTypeItem say = new HuTypeItem();
                say.id = item.Value["id"].l;
                say.ch = item.Value["ch"].s;
                _hutypes.Add(say.id, say);
            }
        }
    }
}
