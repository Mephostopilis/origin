using UnityEngine;
using System.Collections;

namespace Maria
{
    public class Controller
    {
        protected Context _ctx = null;

        public Controller(Context ctx)
        {
            Debug.Assert(ctx != null);
            _ctx = ctx;
        }

        // Use this for initialization
        void start()
        {

        }

        // Update is called once per frame
        void update()
        {

        }
    }
}
