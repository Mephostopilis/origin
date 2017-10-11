using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria.Network;

namespace Bacon.Game {
    partial class GameController {
        public void OpCodeParse(int index, byte[] buffer, int start, int len) {
            int offset = start;
            uint opcode;
            offset = NetUnpack.UnpacklI(buffer, offset, out opcode);
            switch (opcode) {
                case OpCodes.OPCODE_BORN: {

                    }
                    break;
                default:
                    break;
            }
        }
    }
}
