using UnityEngine;

namespace Maria.App
{
    public class Assets
    {
        public Assets()
        {
        }

        public UnityEngine.GameObject GetCard(string name)
        {
            string path = "Prefabs/App/";
            string pathname = path + name;

            Object o = Resources.Load(pathname, typeof(GameObject));
            GameObject go = GameObject.Instantiate(o) as GameObject;
            return go;
        }
    }
}
