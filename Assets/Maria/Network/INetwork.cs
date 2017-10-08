using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria.Network {
    public interface INetwork {
        void OnLoginAuthed(int code, byte[] secret, string dummy);
        void OnLoginConnected(bool connected);
        void OnLoginDisconnected();

        void OnGateAuthed(int code);
        void OnGateConnected(bool connected);
        void OnGateDisconnected();

        void OnUdpRecv(byte[] data, int start, int len);

    }
}
