using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProtoIP;
using ProtoIP.Crypto;
namespace Testes {

    internal class AuthContext : DbContext {
        public DbSet<Auth> Auths { get; set; }
    }
    internal class Auth {


        public int Id { get; set; }

        [StringLength(30)]
        [Index(IsUnique = true)]
        public string Username { get; set; }

        public byte[] Password { get; set; }

        public byte[] Salt { get; set; }
        public bool IsOnline { get; set; }

        public DateTime LastAuthentication { get; set; }
        public DateTime AccoutCreation { get; set; }

        public Auth() {

        }
    }
    class Pacote {

        public const int LOGIN = 101;
        public const int REGISTER = 102;
        public const int MESSAGE = 103;
        public const int PUBLIC_KEY = 104;
        public const int AES_ENCRYPTED_KEY = 105;
        public const int BROADCAST = 106;
        public const int NOTIFICATION = 107;
        public const int INFORM_COMUNICATION = 108;
        public const int COMMUNICATION_AES_ENCRYPTED_KEY = 109;

    }
    public class Client : ProtoClient {
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
            } else if (receivedPacket._GetType() == Pacote.REGISTER) {
                this.registo = receivedPacket.GetDataAs<byte[]>();
            } else if (receivedPacket._GetType() == Pacote.LOGIN) {
                this.login = receivedPacket.GetDataAs<byte[]>();
            } else if (receivedPacket._GetType() == Pacote.INFORM_COMUNICATION) {
                this.otherClientPublicKey = receivedPacket.GetDataAs<byte[]>();
            }

            /*else if (receivedPacket._GetType() == Pacote.ENCRYPTED_PUBLIC_KEY) {
                //this.encryptedPublicKey = receivedPacket.GetDataAs<byte[]>();
                Console.WriteLine("CLIENT: Received encrypted public Key!");
            }*/
        }
        // The logic to handle the notifications received from the server
        public void OnNotificationReceive(byte[] data) {
            Packet receivedPacket = Packet.Deserialize(data);
            if (receivedPacket._GetType() == Pacote.NOTIFICATION) {
                this.notification = receivedPacket.GetDataAs<byte[]>();
                Console.WriteLine("CLIENT: Received notification!");
            } else if (receivedPacket._GetType() == Pacote.INFORM_COMUNICATION) {
                this.informComunication = receivedPacket.GetDataAs<byte[]>();
                Console.WriteLine("CLIENT: Received Comunication!");
            } else if (receivedPacket._GetType() == Pacote.COMMUNICATION_AES_ENCRYPTED_KEY) {
                this.encryptedCommunicationAESKey = receivedPacket.GetDataAs<byte[]>();
                Console.WriteLine("CLIENT: Received encrypted AES Key!");
            } else if (receivedPacket._GetType() == Pacote.MESSAGE) {
                this.mensagem = receivedPacket.GetDataAs<byte[]>();
                Console.WriteLine("CLIENT: Received Mensagem!");
            }

        }
    }

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
            //Packet.ByteDump(receivedPacket);


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
                if (ClientIsOnline(listaUsers, userName)) {
                    packet.SetPayload(aes.Encrypt(Encoding.ASCII.GetBytes("userOnline")));
                    Send(Packet.Serialize(packet), userID);
                    return;
                } else {
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
                }


            } else if (receivedPacket._GetType() == Pacote.MESSAGE) {
                byte[] mensagem = receivedPacket.GetDataAs<byte[]>();

                //int portaComunicacao = GetUserNotificationHandlerPort(listaUsers,listaUsers[userID].communicationUsername);
                Packet packet = new Packet(Pacote.MESSAGE);

                packet.SetPayload(mensagem);
                this._notificationPusher.PushNotification("127.0.0.1", GetUserNotificationHandlerPort(listaUsers, listaUsers[userID].communicationUsername), Packet.Serialize(packet));
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
                Packet packet2 = new Packet(10);
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

                packet.SetPayload(GetUserPublicKey(listaUsers, name));

                Console.WriteLine("Message");
                this._notificationPusher.PushNotification("127.0.0.1", porta, Packet.Serialize(packet));
                Send(Packet.Serialize(packet), userID);



                //Send(Packet.Serialize(packet));

            } else if (receivedPacket._GetType() == Pacote.COMMUNICATION_AES_ENCRYPTED_KEY) {
                byte[] encryptedAESKey = receivedPacket.GetDataAs<byte[]>();
                Packet packet = new Packet(Pacote.COMMUNICATION_AES_ENCRYPTED_KEY);
                packet.SetPayload(encryptedAESKey);
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


        private bool ClientIsOnline(List<User> listaUsers, string username) {
            bool existe = false;
            foreach (User user in listaUsers) {
                if (user.username == username) {
                    existe = true;
                }

            }
            return existe;
        }
        public override void OnUserConnect(int usedID) {

        }

        public override void OnUserDisconnect(int usedID) {
            Console.WriteLine("User Disconnected");
        }
    }
    public class User {
        public int userID;
        public string username;
        public byte[] _publicKey;
        public byte[] _aesKey;
        public int portaNotificationHandler;
        public string communicationUsername;
        public byte[] sharedAESKey;

        public User(int u_userId, string u_username, byte[] publicKey, byte[] aesKey) {
            this.userID = u_userId;
            this.username = u_username;
            this._publicKey = publicKey;
            this._aesKey = aesKey;
        }

    }

    internal class Program {

        private const int PORT = 1234;


        [STAThread]
        public static void Main(string[] args) {
            int NOTIFICATION_PORT1 = ProtoIP.Common.Network.GetRandomUnusedPort();
            int NOTIFICATION_PORT2 = ProtoIP.Common.Network.GetRandomUnusedPort();
            string username1;
            string username2;
            int authenticationAttempts = 0;
            bool clientIsConnected = false;
            byte[] sharedAESKey;

            Client client1 = new Client();
            Client client2 = new Client();
            byte[] aesKey1;
            byte[] aesKey2;
            List<string> listaClientesConnectados = new List<string>();



            ProtoIP.Crypto.RSA rsa1 = new ProtoIP.Crypto.RSA();
            ProtoIP.Crypto.RSA rsa2 = new ProtoIP.Crypto.RSA();




            Console.WriteLine("Starting server...");
            Server server = new Server();
            Thread serverThread = new Thread(() => server.Start(PORT));
            serverThread.Start();
            Console.WriteLine("Server started on port: " + PORT);



            // Cliente 1
            Console.WriteLine("Login Cliente 1");
            client1.Connect("127.0.0.1", PORT);
            // Start listening for notifications from the server
            // The NotificationHandler will run on a different thread and port.
            // The second argument is the callback function that will be called when a notification is received.
            client1._notificationHandler.StartListeningForNotifications(NOTIFICATION_PORT1, client1.OnNotificationReceive);

            // Generate a new RSA key-pair
            rsa1.GenerateKeyPair();
            //Define the packet type
            Packet publicKeyPacket = new Packet(Pacote.PUBLIC_KEY);
            //Set the packet payload
            publicKeyPacket.SetPayload(rsa1.ExportPublicKey());
            //Send the packet to Server
            client1.Send(Packet.Serialize(publicKeyPacket));
            client1.Receive(true);
            aesKey1 = rsa1.Decrypt(client1.ecryptedAesKey);

            AES aes = new AES(aesKey1);
            byte[] mensagemBytes = Encoding.ASCII.GetBytes("admin" + ";" + "123");
            byte[] mensagemEncriptada = aes.Encrypt(mensagemBytes);
            string tipoPacote = "LOGIN";
            if (tipoPacote == "LOGIN") {
                Packet messagePacket = new Packet(Pacote.LOGIN);
                messagePacket.SetPayload(mensagemEncriptada);
                client1.Send(Packet.Serialize(messagePacket));
                client1.Receive(true);
                byte[] msgDecrypt = aes.Decrypt(client1.login);
                string validacao = Encoding.UTF8.GetString(msgDecrypt, 0, msgDecrypt.Length);
                if (validacao == "true") {
                    Packet pacoteNotificacao = new Packet(Pacote.NOTIFICATION);
                    pacoteNotificacao.SetPayload(Encoding.ASCII.GetBytes("" + NOTIFICATION_PORT1));
                    client1.Send(Packet.Serialize(pacoteNotificacao));
                    client1.Receive(true);

                    clientIsConnected = true;

                    username1 = "admin";

                    listaClientesConnectados.Add(username1);

                }

                //Cliente 2




                Console.WriteLine("Funções bt consversar");
                //Funções do btConversar
                string nomeUser = "admin";
                Console.WriteLine(nomeUser);

                AES aes1 = new AES(aesKey1);
                byte[] mensagemBytes1 = Encoding.ASCII.GetBytes(nomeUser);

                Packet packet = new Packet(Pacote.INFORM_COMUNICATION);
                packet.SetPayload(mensagemBytes);
                client1.Send(Packet.Serialize(packet));
                client1.Receive(true);
                AES aes3 = new AES();
                aes3.GenerateKey();

                byte[] encriptedAESKey = ProtoIP.Crypto.RSA.Encrypt(aes3._key, client1.otherClientPublicKey);
                Packet packet2 = new Packet(Pacote.COMMUNICATION_AES_ENCRYPTED_KEY);
                packet2.SetPayload(encriptedAESKey);
                sharedAESKey = aes3.GetKeyBytes();
                client1.Send(Packet.Serialize(packet2));


                //-------------------------------------------
                string mensagem = "Mensagem teste";
                //sharedAESKey = rsa.Decrypt(client.encryptedCommunicationAESKey);
                AES aes4 = new AES(sharedAESKey);
                Packet pacote = new Packet(Pacote.MESSAGE);

                byte[] mensagemBytes3 = Encoding.ASCII.GetBytes(mensagem);
                //Console.WriteLine("Mensagem decriptada: ");
                byte[] msgEncript = aes4.Encrypt(mensagemBytes3);
                pacote.SetPayload(msgEncript);
                client1.Send(Packet.Serialize(pacote));


                //-------------------------------------------
                // sharedAESKey = rsa.Decrypt(client.encryptedCommunicationAESKey); neste caso nao necessita pq já temos a chave
                AES aes5 = new AES(sharedAESKey);
                string men = "";
                if (client2.mensagem != null) {
                    byte[] msg = aes5.Decrypt(client2.mensagem);

                    men = Encoding.UTF8.GetString(msg, 0, msg.Length);
                }
                Console.WriteLine("Cliente2: " + men);



            }
        }
    }
}
