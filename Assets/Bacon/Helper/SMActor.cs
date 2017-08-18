using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Maria;

namespace Bacon.Helper {
    public class SMActor : Actor {

        private Queue<string> _queue = new Queue<string>();

        public SMActor(Context ctx, Maria.Service service) : base(ctx, service) {
        }

        public void LoadScene(string name) {
            _queue.Enqueue(name);
            _ctx.EnqueueRenderQueue(RenderOnLoadScene);
        }

        private void RenderOnLoadScene() {
            if (_queue.Count > 0) {
                string name = _queue.Dequeue();
                UnityEngine.Debug.Assert(name.Length > 0);
                SceneManager.LoadScene(name);

                //SceneManager.activeSceneChanged += ActiveSceneChanged;
                //SceneManager.sceneLoaded += SceneLoaded;
            } else {
                UnityEngine.Debug.LogError("no exits");
            }
        }

        private void ActiveSceneChanged(Scene from, Scene to) {
        }

        private void SceneLoaded(Scene scene, LoadSceneMode sm) {
        }
    }
}
