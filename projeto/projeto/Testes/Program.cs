using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProtoIP;
using ProtoIP.Crypto;

namespace Testes {
    class Client : ProtoClient {
        public byte[] dados;
        public byte[] ecryptedAesKey;

        public override void OnReceive() {

            Packet receivedPacket = AssembleReceivedDataIntoPacket();

            //1000 pacote com a key ecryptada
            if (receivedPacket._GetType() == 1000) {
                this.ecryptedAesKey = receivedPacket.GetDataAs<byte[]>();
            } else if (receivedPacket._GetType() == 1002) {
                //Console.WriteLine("recebi");
                this.dados = receivedPacket.GetDataAs<byte[]>();
            }
        }
    }

    class Server : ProtoServer {
        private byte[] publicKey;
        public byte[] aesKey;

        public override void OnRequest(int userID) {

            Packet receivedPacket = AssembleReceivedDataIntoPacket(userID);

            if (receivedPacket._GetType() == 1001) {
               // Console.WriteLine("mensagem");
                Packet packet2 = new Packet(1002);

                byte[] dados = receivedPacket.GetDataAs<byte[]>();

                packet2.SetPayload(dados);

                Send(Packet.Serialize(packet2), userID);
            } else if (receivedPacket._GetType() == (int)Packet.Type.PUBLIC_KEY) {
                this.publicKey = receivedPacket.GetDataAs<byte[]>();
                //Console.WriteLine("Public key has been sent");
                AES aes = new AES();
                aes.GenerateKey();
                this.aesKey = aes._key;
                byte[] encrypedKey = RSA.Encrypt(aesKey, publicKey);

                //1000 pacote com a key ecryptada
                Packet ecryptedKeyPacket = new Packet(1000);
                ecryptedKeyPacket.SetPayload(encrypedKey);
                Send(Packet.Serialize(ecryptedKeyPacket), userID);
            }
        }
    }

    internal class Program {
        private const int PORT = 1234;

        static void ShowBytes(byte[] bytes) {
            foreach (byte b in bytes) {
                Console.Write(b.ToString("X2") + " ");
            }
            Console.WriteLine("");
        }

        static void Main(string[] args) {
            //Inicialização do server
            Server server = new Server();
            Thread serverThread = new Thread(() => server.Start(PORT));
            serverThread.Start();

            //Inicialização do Cliente
            Client client = new Client();
            client.Connect("127.0.0.1", PORT);

            // Create a new RSA instance
            ProtoIP.Crypto.RSA rsa = new ProtoIP.Crypto.RSA();

            // Generate a new RSA key-pair
            rsa.GenerateKeyPair();

            //Define the packet type
            Packet publicKeyPacket = new Packet(Packet.Type.PUBLIC_KEY);

            //Set the packet payload
            publicKeyPacket.SetPayload(rsa.ExportPublicKey());

            //Send the packet to Server
            client.Send(Packet.Serialize(publicKeyPacket));


            client.Receive();

            //rsa.Decrypt(client.ecryptedAesKey);
            Console.WriteLine("Server AES key:");
            ShowBytes(server.aesKey);
            Console.WriteLine("\nChave incrip cliente: ");
            ShowBytes(client.ecryptedAesKey);
            Console.WriteLine("\nChave decrincrip cliente: ");
            ShowBytes(rsa.Decrypt(client.ecryptedAesKey));

            //-----------------------------------------------------------//
            //Envio de uma mensagem
            string mensagem = "Mensagem teste";
            string mensagem2 = "Mensagem teste 123654636!JSDHSAFVDGHSAV ASHDGTGSAFDGAVS GHJASDTGYACSDTGCV GHAVDTCVASDV HAVSDGVGBHJVDGYVDGHVA";
            byte[] aesKey = rsa.Decrypt(client.ecryptedAesKey);

            Console.WriteLine(sendAndReciveMessage(client, aesKey, mensagem));

            Console.WriteLine(sendAndReciveMessage(client, aesKey, mensagem2));
            // client.Disconnect();
            //server.Stop();

        }
        // No caso da aplicação real vai ter um metodo chamado sendMesage e outro reciveMessage
        public static string sendAndReciveMessage(Client client, byte[] aesKey, string message) {
            AES aes = new AES(aesKey);

            byte[] mensagemBytes = Encoding.ASCII.GetBytes(message);
            // ShowBytes(mensagemBytes);
            byte[] mensagemEncriptada = aes.Encrypt(mensagemBytes);
            // ShowBytes(mensagemEncriptada);
            //Console.WriteLine("Mensagem em bytes:");
            // ShowBytes(mensagemBytes);

            Packet messagePacket = new Packet(1001);
            messagePacket.SetPayload(mensagemEncriptada);
            client.Send(Packet.Serialize(messagePacket));
            client.Receive();

            //Console.WriteLine("Mensagem decriptada: ");
            byte[] msgDecrypt = aes.Decrypt(client.dados);
            //ShowBytes(msgDecrypt);
            string utfString = Encoding.UTF8.GetString(msgDecrypt, 0, msgDecrypt.Length);
            //Console.WriteLine(utfString);
            return utfString;
        }
    }
}