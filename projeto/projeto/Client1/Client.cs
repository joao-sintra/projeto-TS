using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProtoIP;

// Tanto o NotificationHandler como o NotificationPusher foram desenvolvidos com a ajuda do João Matos

namespace Client1 {
    class Client : ProtoClient {
        public byte[] dados;
        public byte[] mensagem;
        public byte[] ecryptedAesKey;
        public byte[] registo;
        public byte[] login;
        public byte[] notification;
        public byte[] informComunication;
        public bool clientConnected = false;
        public byte[] otherClientPublicKey;
        public byte[] encryptedCommunicationAESKey;

        public NotificationHandler _notificationHandler = new NotificationHandler();
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
            }else if (receivedPacket._GetType() == Pacote.INFORM_COMUNICATION) {
                this.otherClientPublicKey = receivedPacket.GetDataAs<byte[]>();
            } /*else if (receivedPacket._GetType() == Pacote.ENCRYPTED_PUBLIC_KEY) {
                //this.encryptedPublicKey = receivedPacket.GetDataAs<byte[]>();
                Debug.WriteLine("CLIENT: Received encrypted public Key!");
            }*/
        }
        // The logic to handle the notifications received from the server
        public void OnNotificationReceive(byte[] data) {
            Packet receivedPacket = Packet.Deserialize(data);
            if (receivedPacket._GetType() == Pacote.NOTIFICATION) {
                this.notification = receivedPacket.GetDataAs<byte[]>();
                Debug.WriteLine("CLIENT: Received notification!");
            } else if (receivedPacket._GetType() == Pacote.INFORM_COMUNICATION) {
                this.informComunication = receivedPacket.GetDataAs<byte[]>();
                Debug.WriteLine("CLIENT: Received Comunication!");
            }else if (receivedPacket._GetType() == Pacote.COMMUNICATION_AES_ENCRYPTED_KEY) {
                this.encryptedCommunicationAESKey = receivedPacket.GetDataAs<byte[]>();
                Debug.WriteLine("CLIENT: Received encrypted AES Key!");
            } else if (receivedPacket._GetType() == Pacote.MESSAGE) {
                this.mensagem = receivedPacket.GetDataAs<byte[]>();
                Debug.WriteLine("CLIENT: Received Mensagem!");
            }
        }
    }

}

