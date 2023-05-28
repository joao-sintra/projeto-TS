using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProtoIP;

namespace Client1 {
    class Client : ProtoClient {
        public byte[] dados;
        public byte[] ecryptedAesKey;
        public byte[] registo;
        public byte[] login;

        public override void OnReceive() {

            Packet receivedPacket = AssembleReceivedDataIntoPacket();

            
            if (receivedPacket._GetType() == Pacote.AES_ENCRYPTED_KEY) {
                this.ecryptedAesKey = receivedPacket.GetDataAs<byte[]>();
            } else if (receivedPacket._GetType() == Pacote.MESSAGE) {
                this.dados = receivedPacket.GetDataAs<byte[]>();
            }else if(receivedPacket._GetType() == Pacote.REGISTER) {
                this.registo = receivedPacket.GetDataAs<byte[]>();
            } else if (receivedPacket._GetType() == Pacote.LOGIN) {
                this.login = receivedPacket.GetDataAs<byte[]>();
            }
        }
    }

}

