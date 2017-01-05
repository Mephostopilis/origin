using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Maria;

namespace Bacon {
    public class SMActor : Actor {

        private Queue<string> _queue = new Queue<string>();

        public SMActor(Context ctx, Service service) : base(ctx, service) {
        }

        public void LoadScene(string name) {
            _queue.Enqueue(name);
            _ctx.EnqueueRenderQueue(RenderOnLoadScene);
        }

        private void RenderOnLoadScene() {
            if (_queue.Count > 0) {
                string name = _queue.Dequeue();
                Debug.Assert(name.Length > 0);
                SceneManager.LoadSceneAsync(name);

                //SceneManager.activeSceneChanged += ActiveSceneChanged;
                //SceneManager.sceneLoaded += SceneLoaded;
            } else {
                Debug.LogError("no exits");
            }
        }

        private void ActiveSceneChanged(Scene from, Scene to) {
        }

        private void SceneLoaded(Scene scene, LoadSceneMode sm) {
        }
    }
}
