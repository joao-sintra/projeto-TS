using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using ProtoIP;
using ProtoIP.Crypto;

namespace Servidor {
    class Server : ProtoServer {
        private byte[] publicKey;
        public byte[] aesKey;

        
        public override void OnRequest(int userID) {

            Console.WriteLine(_clients.Count()+" - "+ userID);

            Packet receivedPacket = AssembleReceivedDataIntoPacket(userID);

            if (receivedPacket._GetType() == Pacote.MESSAGE) {
                // Console.WriteLine("mensagem");
                Packet packet2 = new Packet(Pacote.MESSAGE);
                byte[] dados = receivedPacket.GetDataAs<byte[]>();
                packet2.SetPayload(dados);
                Send(Packet.Serialize(packet2), userID);
            } else if (receivedPacket._GetType() == Pacote.PUBLIC_KEY) {
                this.publicKey = receivedPacket.GetDataAs<byte[]>();
                //Console.WriteLine("Public key has been sent");
                AES aes = new AES();
                aes.GenerateKey();
                this.aesKey = aes._key;
                byte[] encrypedKey = RSA.Encrypt(aesKey, publicKey);

                //1000 pacote com a key ecryptada
                Packet ecryptedKeyPacket = new Packet(Pacote.AES_ENCRYPTED_KEY);
                ecryptedKeyPacket.SetPayload(encrypedKey);
                Send(Packet.Serialize(ecryptedKeyPacket), userID);
            }else if(receivedPacket._GetType() == Pacote.REGISTER) {
                //VALIDAR SE O USERNAME JÁ EXISTE NA DB
                
                Packet packet = new Packet(Pacote.REGISTER);
                AES aes = new AES(aesKey);
                byte[] dados = receivedPacket.GetDataAs<byte[]>();
                dados = aes.Decrypt(dados);
                string name = Encoding.UTF8.GetString(dados, 0, dados.Length);
                string[] dadosUser = name.Split(';');
                string userName = dadosUser[0];
                
                string password = dadosUser[1];
               // SHA256 sha = new SHA256(byte[] aaa);
                
                //string passwordIncypted = password;
                
                using (var db = new AuthContext()) {
                   

                    if(db.Auths.Any(u => u.Username == userName)) {
                        packet.SetPayload(aes.Encrypt(Encoding.ASCII.GetBytes("false")));
                    } else {
                        var auth = new Auth { Username = userName, Password = password };
                        db.Auths.Add(auth);
                        db.SaveChanges();
                        packet.SetPayload(aes.Encrypt(Encoding.ASCII.GetBytes("true")));
                    }
                }
                
                Send(Packet.Serialize(packet), userID);

            } else if (receivedPacket._GetType() == Pacote.LOGIN) {
               

                Packet packet = new Packet(Pacote.LOGIN);
                AES aes = new AES(aesKey);
                byte[] dados = receivedPacket.GetDataAs<byte[]>();
                dados = aes.Decrypt(dados);
                string name = Encoding.UTF8.GetString(dados, 0, dados.Length);
                string[] dadosUser = name.Split(';');
                string userName = dadosUser[0];

                string password = dadosUser[1];
                // SHA256 sha = new SHA256(byte[] aaa);

                //string passwordIncypted = password;

                using (var db = new AuthContext()) {

                    
                    if (db.Auths.Any(u => u.Username == userName && u.Password == password)) {
                        packet.SetPayload(aes.Encrypt(Encoding.ASCII.GetBytes("true")));
                        
                        db.SaveChanges();
                    } else {
                        packet.SetPayload(aes.Encrypt(Encoding.ASCII.GetBytes("false")));
                    }
                }

                Send(Packet.Serialize(packet), userID);

            }
        }

       
    }
}

