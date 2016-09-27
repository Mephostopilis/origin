using System.Collections.Generic;
using System.Threading;
using System.Net.Sockets;

namespace Maria.Network
{
    public class Gate
    {
        public enum SocketType
        {
            ST_DATA,
            ST_CLOSE,
            ST_OPEN,
            ST_ACCEPT,
            ST_ERROR,
            ST_EXIT,
            ST_UDP,
        }

        public class SocketMsg
        {
            public SocketType Type { get; set; }
            public IPackageSocket From { get; set; }
            public IProxySocket To { get; set; }
            public byte[] Buffer { get; set; }
        }

        public enum CMD
        {
            S, // Start Socket
            B, // Bind Socket
            L, // Listen Socket
            K, // Close Socket
            O, // Connect to (Open)
        }

        public class Req
        {
            public CMD CMD { get; set; }
        }

        public class ROpen : Req
        {
            public object opaque { get; set; }
            public string addr { get; set; }
            public int port { get; set; }
        }

        public class RStart : Req
        {
            public object opaque { get; set; }
            public int id { get; set; }
        }


        private Context _ctx = null;
        private Queue<SocketMsg> _smq = new Queue<SocketMsg>();
        private Queue<Req> _rq = new Queue<Req>();

        private Dictionary<IPackageSocket, IProxySocket> _slot = new Dictionary<IPackageSocket, IProxySocket>();
        private Dictionary<IProxySocket, IPackageSocket> _proxy = new Dictionary<IProxySocket, IPackageSocket>();
        
        public Gate(Context ctx)
        {
            _ctx = ctx;
        }

        // 主线程调用
        void Update()
        {
            if (_smq.Count > 0)
            {
                SocketMsg sm = _smq.Dequeue();
                IProxySocket ps = sm.To;
                switch (sm.Type)
                {
                    case SocketType.ST_DATA:
                        break;
                    case SocketType.ST_CLOSE:
                        break;
                    case SocketType.ST_OPEN:
                        break;
                    case SocketType.ST_ACCEPT:
                        break;
                    case SocketType.ST_ERROR:
                        break;
                    case SocketType.ST_EXIT:
                        break;
                    case SocketType.ST_UDP:
                        break;
                    default:
                        break;
                }
            }
        }

        public int Command(Req r)
        {
            return 0;
        }

        public void Run()
        {
            if (_rq.Count > 0)
            {
                Req r = _rq.Dequeue();
                CMD cmd = r.CMD;
                switch (cmd)
                {
                    case CMD.S:
                        break;
                    case CMD.B:
                        break;
                    case CMD.L:
                        break;
                    case CMD.K:
                        break;
                    case CMD.O:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}