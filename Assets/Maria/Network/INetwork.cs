using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria.Network {
    public interface INetwork {
        void OnGateAuthed(int code);
        void OnGateDisconnected();

        void OnUdpSync();
        void OnUdpRecv(PackageSocketUdp.R r);

    }
}
