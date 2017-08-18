using System;
using System.Collections.Generic;
using System.Xml;

namespace Configs {
    class CSLoader {

        public class Value {
            public bool n; // nil
            public bool b;
            public long l;
            public string s;
        }

        protected void ParseDict(XmlNode key, Dictionary<string, Value> dict) {
            while (key != null) {
                XmlNode value = key.NextSibling;
                if (value.Name == "nil") {
                    Value v = new Value();
                    v.n = true;
                    dict.Add(key.InnerText, v);
                } else if (value.Name == "boolean") {
                    Value v = new Value();
                    v.n = false;
                    v.b = true;
                    dict.Add(key.InnerText, v);
                } else if (value.Name == "integer") {
                    Value v = new Value();
                    v.n = false;
                    v.l = long.Parse(value.InnerText);
                    dict.Add(key.InnerText, v);
                } else if (value.Name == "string") {
                    Value v = new Value();
                    v.n = false;
                    v.s = value.InnerText;
                    dict.Add(key.InnerText, v);
                }
                key = value.NextSibling;
            }
        }

    }
}
