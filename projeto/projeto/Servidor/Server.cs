using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading;
using ProtoIP;
using ProtoIP.Crypto;

// Tanto o NotificationHandler como o NotificationPusher foram desenvolvidos com a ajuda do João Matos


namespace Servidor {
    class Server : ProtoServer {
        private byte[] publicKey;
        public byte[] aesKey;

        public List<User> listaUsers = new List<User>();

        // Create a new NotificationPusher object
        // The server will push notifications to the client
        public NotificationPusher _notificationPusher = new NotificationPusher();
        public override void OnRequest(int userID) {

            // _clients.ForEach(Console.WriteLine);
            // Console.WriteLine(_clients.Count()+" - "+ userID);

            Packet receivedPacket = AssembleReceivedDataIntoPacket(userID);
            Console.WriteLine(receivedPacket._GetType());
            Packet.ByteDump(receivedPacket);


            if (receivedPacket._GetType() == Pacote.PUBLIC_KEY) {
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

            } else if (receivedPacket._GetType() == Pacote.REGISTER) {

                //VALIDAR SE O USERNAME JÁ EXISTE NA DB

                Packet packet = new Packet(Pacote.REGISTER);
                AES aes = new AES(aesKey);
                byte[] dados = receivedPacket.GetDataAs<byte[]>();
                dados = aes.Decrypt(dados);
                string name = Encoding.UTF8.GetString(dados, 0, dados.Length);
                string[] dadosUser = name.Split(';');
                string userName = dadosUser[0];

                string password = dadosUser[1];
                Console.WriteLine("nome registo: (" + userName + ")");
                Console.WriteLine("password registo: (" + password + ")");
                byte[] passwordBytes = Encoding.ASCII.GetBytes(password);
                byte[] salt = ProtoIP.Crypto.HASH.GenerateRandomBytes(32);

                passwordBytes = SHA256.Hash(passwordBytes, salt);
                //string sPassword = HASH.GetDigestString(passwordBytes);
                //string sSalt = HASH.GetDigestString(salt);

                //string passwordIncypted = password;
                DateTime localDate = DateTime.Now;
                using (var db = new AuthContext()) {

                    //falta validações

                    var auth = new Auth { Username = userName, Password = passwordBytes, Salt = salt, AccoutCreation = localDate, IsOnline = true, LastAuthentication = localDate };
                    db.Auths.Add(auth);
                    db.SaveChanges();
                    Console.WriteLine(userName);

                    packet.SetPayload(aes.Encrypt(Encoding.ASCII.GetBytes("true")));

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
                // Console.WriteLine("Passowd campo: (" + password+")");


                using (var db = new AuthContext()) {

                    //falta validar se o username existe
                    var user = db.Auths.FirstOrDefault(u => u.Username == userName);
                    byte[] passwordBd = user.Password;
                    byte[] salt = user.Salt;

                    byte[] pass = SHA256.Hash(Encoding.ASCII.GetBytes(password), salt);
                    // Console.WriteLine("Salt db: " + salt);
                    // Console.WriteLine("Passowd db: " + passwordBd);
                    //Console.WriteLine("Pass: " + pass);
                    if (ComparaArrayBytes(pass, passwordBd)) {
                        Console.WriteLine(user.Username);
                        User cliente = new User(userID, userName, publicKey, aesKey);
                        listaUsers.Add(cliente);
                        packet.SetPayload(aes.Encrypt(Encoding.ASCII.GetBytes("true")));

                    } else {
                        // Console.WriteLine("FALSE LOGIN");
                        packet.SetPayload(aes.Encrypt(Encoding.ASCII.GetBytes("false")));
                    }

                }

                Send(Packet.Serialize(packet), userID);
            } else if (receivedPacket._GetType() == Pacote.NOTIFICATION) {
                // listaClientes.;
                // Send a notification to the client.
                // The client NotificationHandler will receive the notification and act accordingly.
                byte[] portaRecebida = receivedPacket.GetDataAs<byte[]>();

                int portaNotificacao = Int32.Parse(Encoding.UTF8.GetString(portaRecebida, 0, portaRecebida.Length));

                Packet notificationPacket = new Packet((int)Pacote.NOTIFICATION);
                string users = "";
                listaUsers[userID].portaNotificationHandler = portaNotificacao;
                foreach (User user in listaUsers) {
                    users += user.username + ";";
                }
                users = users.TrimEnd(';');

                byte[] result = Encoding.UTF8.GetBytes(users);
                notificationPacket.SetPayload(result);

                foreach (User user in listaUsers) {
                    this._notificationPusher.PushNotification("127.0.0.1", user.portaNotificationHandler, Packet.Serialize(notificationPacket));
                }
                Packet pacote1 = new Packet(10);
                Send(Packet.Serialize(pacote1), userID);



            } else if (receivedPacket._GetType() == Pacote.INFORM_COMUNICATION) {

                byte[] userAESKey = GetUserAESKey(listaUsers, userID);

                //Retirei a encriptação temporariamente

                // Console.WriteLine("mensagem");
                Packet packet = new Packet(Pacote.INFORM_COMUNICATION);
                Packet packet2 = new Packet(Pacote.INFORM_COMUNICATION);
                //AES aes = new AES(userAESKey);
                byte[] dados = receivedPacket.GetDataAs<byte[]>();
                //dados = aes.Decrypt(dados);
                string name = Encoding.UTF8.GetString(dados, 0, dados.Length);
                listaUsers[userID].communicationUsername = name;
                //string[] dadosUser = name.Split(';');
                // string userName = dadosUser[0];

                // string mensagem = dadosUser[1];

                int porta = GetUserNotificationHandlerPort(listaUsers, name);


                // byte[] msg = Encoding.ASCII.GetBytes(mensagem);

                packet2.SetPayload(GetUserPublicKey(listaUsers, name));

                Console.WriteLine("Message");
                this._notificationPusher.PushNotification("127.0.0.1", porta, Packet.Serialize(packet));
                Send(Packet.Serialize(packet2), userID);



                //Send(Packet.Serialize(packet));

            } else if (receivedPacket._GetType() == Pacote.COMMUNICATION_AES_ENCRYPTED_KEY) {
                byte[] encryptedAESKey = receivedPacket.GetDataAs<byte[]>();
                Packet packet = new Packet(Pacote.COMMUNICATION_AES_ENCRYPTED_KEY);
                packet.SetPayload(encryptedAESKey);
                this._notificationPusher.PushNotification("127.0.0.1", GetUserNotificationHandlerPort(listaUsers, listaUsers[userID].communicationUsername), Packet.Serialize(packet));
                Send(Packet.Serialize(packet), userID);
            } else if (receivedPacket._GetType() == Pacote.MESSAGE) {
                byte[] mensagem = receivedPacket.GetDataAs<byte[]>();

                //int portaComunicacao = GetUserNotificationHandlerPort(listaUsers,listaUsers[userID].communicationUsername);
                Packet packet = new Packet(Pacote.MESSAGE);

                packet.SetPayload(mensagem);
                this._notificationPusher.PushNotification("127.0.0.1", GetUserNotificationHandlerPort(listaUsers, listaUsers[userID].communicationUsername), Packet.Serialize(packet));
                Send(Packet.Serialize(packet), userID);

            }
        }


        private static bool ComparaArrayBytes(byte[] a, byte[] b) {
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; ++i) {

                if (a[i] != b[i]) return false;
            }
            return true;
        }

        private int GetUserNotificationHandlerPort(List<User> listaUsers, string username) {
            int porta = 0;
            foreach (User user in listaUsers) {
                if (user.username == username) {
                    porta = user.portaNotificationHandler;
                }

            }
            return porta;
        }

        private byte[] GetUserPublicKey(List<User> listaUsers, string username) {
            byte[] publicKey = new byte[0];
            foreach (User user in listaUsers) {
                if (user.username == username) {
                    publicKey = user._publicKey;
                }

            }
            return publicKey;
        }

        private byte[] GetUserAESKey(List<User> listaUsers, int userID) {
            byte[] userAesKey = new byte[0];
            foreach (User user in listaUsers) {
                if (user.userID == userID) {
                    userAesKey = user._aesKey;
                }

            }
            return userAesKey;
        }
        public override void OnUserConnect(int usedID) {

        }

        public override void OnUserDisconnect(int usedID) {
            Console.WriteLine("User Disconnected");
        }

    }
}

