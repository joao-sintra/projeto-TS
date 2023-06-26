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

        public byte[] mensagem;
        public byte[] ecryptedAesKey;
        public byte[] registo;
        public byte[] login;
        public byte[] notification;
        public byte[] informComunication;
        public bool clientConnected = false;
        public byte[] otherClientPublicKey;
        public byte[] encryptedCommunicationAESKey;
        public byte[] communicationUsername;
        public byte[] random;



        public NotificationHandler _notificationHandler = new NotificationHandler();

        //Função chamada quando recebe um pacote do servidor (Função do ProtoIP)
        public override void OnReceive() {

            Packet receivedPacket = AssembleReceivedDataIntoPacket();

            switch (receivedPacket._GetType()) {
                case Pacote.MESSAGE:
                    this.random = receivedPacket.GetDataAs<byte[]>();
                    break;
                case Pacote.AES_ENCRYPTED_KEY:
                    this.ecryptedAesKey = receivedPacket.GetDataAs<byte[]>();
                    break;
                case Pacote.REGISTER:
                    this.registo = receivedPacket.GetDataAs<byte[]>();
                    break;
                case Pacote.LOGIN:
                    this.login = receivedPacket.GetDataAs<byte[]>();
                    break;
                case Pacote.INFORM_COMUNICATION:
                    this.otherClientPublicKey = receivedPacket.GetDataAs<byte[]>();
                    this.informComunication = receivedPacket.GetDataAs<byte[]>();
                    break;

                default:
                    break;
            }

        }
        // A lógica para lidar com as notificações recebidas do servidor
        public void OnNotificationReceive(byte[] data) {
            Packet receivedPacket = Packet.Deserialize(data);

            switch (receivedPacket._GetType()) {
                case Pacote.NOTIFICATION:
                    this.notification = receivedPacket.GetDataAs<byte[]>();
                    Debug.WriteLine("CLIENT: Received notification!");
                    break;
                case Pacote.INFORM_COMUNICATION:
                    this.informComunication = receivedPacket.GetDataAs<byte[]>();
                    Debug.WriteLine("CLIENT: Received Comunication!");
                    break;
                case Pacote.COMMUNICATION_AES_ENCRYPTED_KEY:
                    this.encryptedCommunicationAESKey = receivedPacket.GetDataAs<byte[]>();
                    Debug.WriteLine("CLIENT: Received encrypted AES Key!");
                    break;
                case Pacote.MESSAGE:
                    this.mensagem = receivedPacket.GetDataAs<byte[]>();
                    Debug.WriteLine("CLIENT: Received Mensagem!");
                    break;
                case Pacote.INFORM_COMUNICATION_USERNAME:
                    this.communicationUsername = receivedPacket.GetDataAs<byte[]>();
                    Debug.WriteLine("CLIENT: Received Comunication Username!");
                    break;
                default:
                    break;

            }


           

        }




    }

}

