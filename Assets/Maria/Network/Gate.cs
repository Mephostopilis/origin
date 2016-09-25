using System.Collections.Generic;
using System.Threading;
using System.Net.Sockets;

namespace Maria.Network
{


    public interface IProxy
    {
        void OnConnect(int id, bool connected);
        void OnRecvive(int id, byte[] buffer);
        void OnDisconnect(int id, SocketError socketError, PackageSocketError packageSocketError);
    }

    // singlen
    public class Gate
    {
        enum SOCKET_TYPE
        {
            SOCKET_DATA,
            SOCKET_CLOSE,
            SOCKET_OPEN,
            SOCKET_ACCEPT,
            SOCKET_ERROR,
            SOCKET_EXIT,
            SOCKET_UDP,
        }

        class SOCKET_MESSAGE
        {
            public SOCKET_TYPE Type { get; set; }
            public int Id { get; set; }
            public IProxy Opaque { get; set; }
            public byte[] Buffer { get; set; }
        }

        private enum CMD
        {
            S, // Start socket
            B, // Bind socket
            L, // Listen socket
            K, // Close socket
            O, // Connect to (Open)
        }

        class Req
        {
            public CMD cmd { get; set; }
        }

        class ROpen : Req
        {
            public object opaque { get; set; }
            public string addr { get; set; }
            public int port { get; set; }
        }

        class RStart : Req
        {
            public object opaque { get; set; }
            public int id { get; set; }
        }

        private Queue<SOCKET_MESSAGE> smq = new Queue<SOCKET_MESSAGE>();
        private Queue<Req> rq = new Queue<Req>();
        private int index = 0;
        private Dictionary<int, IProxy> slot = new Dictionary<int, IProxy>();
        private Dictionary<IProxy, int> proxy = new Dictionary<IProxy, int>();
        private List<ProxySocket> socks = new List<ProxySocket>();

        public Gate()
        {
            var t = new Thread(new ThreadStart(run));
            t.IsBackground = true;
            t.Start();
        }

        // 主线程调用
        void Update()
        {
            if (smq.Count > 0)
            {
                SOCKET_MESSAGE sm = smq.Dequeue();
                switch (sm.Type)
                {
                    case SOCKET_TYPE.SOCKET_DATA:
                        sm.Opaque.OnRecvive(sm.Id, sm.Buffer);
                        break;
                    case SOCKET_TYPE.SOCKET_CLOSE:
                        sm.Opaque.OnDisconnect(sm.Id, SocketError.AccessDenied, PackageSocketError.None);
                        break;
                    case SOCKET_TYPE.SOCKET_OPEN:
                        sm.Opaque.OnConnect(sm.Id, true);
                        break;
                    case SOCKET_TYPE.SOCKET_ACCEPT:
                        break;
                    case SOCKET_TYPE.SOCKET_ERROR:
                        sm.Opaque.OnDisconnect(sm.Id, SocketError.AccessDenied, PackageSocketError.None);
                        break;
                    case SOCKET_TYPE.SOCKET_EXIT:
                        break;
                    case SOCKET_TYPE.SOCKET_UDP:
                        break;
                    default:
                        break;
                }
            }
        }

        public void Start(object opaque, int id)
        {
            var R = new RStart();
            R.cmd = CMD.S;
            R.opaque = opaque;
            R.id = id;
            rq.Enqueue(R);
        }

        public int Connect(object opaque, string addr, int port)
        {
            var R = new ROpen();
            R.cmd = CMD.O;
            R.opaque = opaque;
            R.addr = addr;
            R.port = port;
            rq.Enqueue(R);
            return 0;
        }

        private int Command(CMD cmd, Req r)
        {
            switch (cmd)
            {
                case CMD.S:
                    return StartSocket();
                case CMD.B:
                    break;
                case CMD.L:
                    break;
                case CMD.K:
                    break;
                case CMD.O:
                    return OpenSocket();
                default:
                    break;
            }
            return 0;
        }

        private int StartSocket()
        {
            return 0;
        }

        private int OpenSocket()
        {
            index++;
            PackageSocket sock = new PackageSocket();
            //slot[index] = new Socket() { ID = index, P = sock };

            return 0;
        }

        private void run()
        {
            while (true)
            {
                if (rq.Count > 0)
                {
                    Req r = rq.Dequeue();
                    Command(r.cmd, r);
                }
                foreach (var item in socks)
                {
                    //item.Update();
                }
            }
        }
    }
}