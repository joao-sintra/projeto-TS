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
using Microsoft.Extensions.Logging;
using ProtoIP;
using ProtoIP.Crypto;

// Tanto o NotificationHandler como o NotificationPusher foram desenvolvidos com a ajuda do João Matos


namespace Servidor {
    class Server : ProtoServer {
        private byte[] publicKey; //Chave publica do ultimo cliente conectado
        public byte[] aesKey; //Chave AES gerada pelo servidor
        private const string _serverIP = "127.0.0.1";

        private List<User> listaUsers = new List<User>(); //Lista de users logados

        public NotificationPusher _notificationPusher = new NotificationPusher();

        //Tipo de pacotes:
        // 1 - PUBLIC_KEY - Função para receber o pacote do cliente e criar uma chave AES ecriptala e enviar para o cliente;
        // 2 - REGISTER - Função para receber o pacote do cliente e registar o utilizador e validar devidamente o mesmo;
        //     2.1 - Recebe os dados do utilizador e desencripta-os com a chave AES criada anteriormente pelo servidor;
        //     2.2 - Com os dados cria um salt com 32 bytes para encriptar a password do utilizador;
        //     2.3 - Valida se o nome do utilizador existe na base de dados;
        //     2.4 - Valida se o email do utilizador existe na base de dados;
        //     2.5 - Insere o utilizador na base de dados e na lista de clietes logados;
        // 3 - LOGIN
        //     3.1 - Recebe os dados do utilizador e desencripta-os com a chave AES criada anteriormente pelo servidor:
        //     3.2 - Valida se o utilizador encontra-se online;
        //     3.3 - Valida se o utilizador existe na base de dados;
        //     3.4 - Valida se a password do utilizador é igual à password encriptada com o salt na base de dados;
        //     3.5 - Adiciona o utilizador à lista de clientes logados;
        // 4 - MESSAGE
        //     4.1 - Recebe os dados do pacote enviado pelo cliente;
        //     4.2 - Vai buscar à lista de utilizadores logados o utilizador que vai receber a mensagem e envia-a por notificação;
        // 5 - NOTIFICATION
        //     5.1 - Recebe os dados do pacote enviado pelo cliente com a porta do mesmo;
        //     5.2 - Define na lista de clientes logados a porta das notificações do cliente que enviou o pacote
        //     5.3 - Vai buscar à lista de clientes todos os nomes dos clientes logados e envia-os por notificação a todos os clientes logados;
        // 6 - INFORM_COMMUNICATION
        //      6.1 - Recebe os dados do pacote enviado pelo cliente com a porta do mesmo;
        //      6.2 - Define na lista de clientes logados o nome do cliente para a comunicação;
        //      6.3 - Vai buscar à lista de clientes logados o nome do cliente que vai receber a mensagem e envia-a por notificação;
     

        //
        //Função chamada quando um cliente faz um request (Função do ProtoIP)
        public override void OnRequest(int userID) {


            Packet receivedPacket = AssembleReceivedDataIntoPacket(userID);
          
            if (receivedPacket._GetType() == Pacote.PUBLIC_KEY) {
                Logger.WriteLog(Logger.LogType.INFO, "Chave publica recebida.");
                this.publicKey = receivedPacket.GetDataAs<byte[]>();
                AES aes = new AES();
                aes.GenerateKey();
                this.aesKey = aes._key;
                byte[] encrypedKey = RSA.Encrypt(aesKey, publicKey);
                Packet ecryptedKeyPacket = new Packet(Pacote.AES_ENCRYPTED_KEY);
                Logger.WriteLog(Logger.LogType.INFO, "Chave AES encriptada enviada para o cliente.");
                ecryptedKeyPacket.SetPayload(encrypedKey);
                Send(Packet.Serialize(ecryptedKeyPacket), userID);

            } else if (receivedPacket._GetType() == Pacote.REGISTER) {
                Logger.WriteLog(Logger.LogType.INFO, "Pedido para Registo recebido.");

                Packet packet = new Packet(Pacote.REGISTER);
                AES aes = new AES(aesKey);
                byte[] dados = receivedPacket.GetDataAs<byte[]>();
                dados = aes.Decrypt(dados);
                string name = Encoding.UTF8.GetString(dados, 0, dados.Length);
                string[] dadosUser = name.Split(';');
                string userName = dadosUser[0];
                string password = dadosUser[1];
                byte[] passwordBytes = Encoding.ASCII.GetBytes(password);
                byte[] salt = ProtoIP.Crypto.HASH.GenerateRandomBytes(32);

                passwordBytes = SHA256.Hash(passwordBytes, salt);

                DateTime localDate = DateTime.Now;
                using (var db = new AuthContext()) {

                    //validar se o user ja existe na base de dados 
                    var user = db.Auths.FirstOrDefault(u => u.Username == userName);
                    if (user != null) {
                        packet.SetPayload(aes.Encrypt(Encoding.ASCII.GetBytes("false")));
                        Send(Packet.Serialize(packet), userID); 
                        Logger.WriteLog(Logger.LogType.ERROR, "Registo falhado cliente já existe.");
                        return;
                    }
                    var auth = new Auth { Username = userName, Password = passwordBytes, Salt = salt, AccoutCreation = localDate, LastAuthentication = localDate };
                    User cliente = new User(userID, userName, publicKey, aesKey);
                    listaUsers.Add(cliente);
                    db.Auths.Add(auth);
                    db.SaveChanges();
                 

                    packet.SetPayload(aes.Encrypt(Encoding.ASCII.GetBytes("true")));
                    Logger.WriteLog(Logger.LogType.INFO, "Registo Cliente " + userName + " com sucesso.");

                }


                Send(Packet.Serialize(packet), userID);

            } else if (receivedPacket._GetType() == Pacote.LOGIN) {
                Logger.WriteLog(Logger.LogType.INFO, "Pedido para Login recebido.");

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
                    Logger.WriteLog(Logger.LogType.ERROR, "Login falhado cliente já online.");
                    Send(Packet.Serialize(packet), userID);
                    return;
                } else {
                    using (var db = new AuthContext()) {

                      
                        var user = db.Auths.FirstOrDefault(u => u.Username == userName);
                        if (user == null) {
                            packet.SetPayload(aes.Encrypt(Encoding.ASCII.GetBytes("false")));
                            Logger.WriteLog(Logger.LogType.ERROR, "Login falhado nome não existe.");
                            Send(Packet.Serialize(packet), userID);
                            return;
                        }
                        byte[] passwordBd = user.Password;
                        byte[] salt = user.Salt;

                        byte[] pass = SHA256.Hash(Encoding.ASCII.GetBytes(password), salt);
                        if (ComparaArrayBytes(pass, passwordBd)) {
                            Console.WriteLine("Cliente :"+user.Username+" conectado");
                            User cliente = new User(userID, userName, publicKey, aesKey);
                            listaUsers.Add(cliente);
                            user.LastAuthentication = DateTime.Now;
                            db.SaveChanges();
                            Logger.WriteLog(Logger.LogType.INFO, "Login Cliente " + userName + " com sucesso.");
                            packet.SetPayload(aes.Encrypt(Encoding.ASCII.GetBytes("true")));
                        } else {
                            Logger.WriteLog(Logger.LogType.ERROR, "Login falhado credenciais incorretas.");
                            packet.SetPayload(aes.Encrypt(Encoding.ASCII.GetBytes("false")));
                        }

                    }
                }
                Send(Packet.Serialize(packet), userID);

            } else if (receivedPacket._GetType() == Pacote.MESSAGE) {
                
                byte[] mensagem = receivedPacket.GetDataAs<byte[]>();
                Packet packet = new Packet(Pacote.MESSAGE);
                
                 
                Mensagem msg =Mensagem.DeserializeMessage(mensagem);

                if (msg.VerifySignature(listaUsers[userID]._publicKey, msg.message,msg.signature )) {
                    packet.SetPayload(msg.message);
                    Logger.WriteLog(Logger.LogType.INFO, "Mensagem para um cliente enviada.");

                    this._notificationPusher.PushNotification(_serverIP, GetUserNotificationHandlerPort(listaUsers, listaUsers[userID].communicationUsername), Packet.Serialize(packet));
                } else {
                    Logger.WriteLog(Logger.LogType.ERROR, "Mensagem não enviada para o cliente assinatura inválida.");
                }
                
                Send(Packet.Serialize(packet), userID);

            } else if (receivedPacket._GetType() == Pacote.NOTIFICATION) {
                byte[] portaRecebida = receivedPacket.GetDataAs<byte[]>();
                int portaNotificacao = Int32.Parse(Encoding.UTF8.GetString(portaRecebida, 0, portaRecebida.Length));
                string users = "";
                Packet notificationPacket = new Packet((int)Pacote.NOTIFICATION);
                Packet pacote1 = new Packet(10);
                
                listaUsers[userID].portaNotificationHandler = portaNotificacao;
                foreach (User user in listaUsers) {
                    users += user.username + ";";
                }
                users = users.TrimEnd(';');

                byte[] result = Encoding.UTF8.GetBytes(users);
                notificationPacket.SetPayload(result);

                foreach (User user in listaUsers) {
                    this._notificationPusher.PushNotification(_serverIP, user.portaNotificationHandler, Packet.Serialize(notificationPacket));
                }
                
                Logger.WriteLog(Logger.LogType.INFO, "Notificação para todos os utilizadores enviada.");
                Send(Packet.Serialize(pacote1), userID);



            } else if (receivedPacket._GetType() == Pacote.INFORM_COMUNICATION) {
                byte[] userAESKey = GetUserAESKey(listaUsers, userID);

                Packet packet = new Packet(Pacote.INFORM_COMUNICATION);
                Packet packet2 = new Packet(10);

                byte[] dados = receivedPacket.GetDataAs<byte[]>();
                string name = Encoding.UTF8.GetString(dados, 0, dados.Length);
                listaUsers[userID].communicationUsername = name;
                listaUsers[GetUserIndex(listaUsers, name)].communicationUsername = listaUsers[userID].username;

                int porta = GetUserNotificationHandlerPort(listaUsers, name);

                packet.SetPayload(GetUserPublicKey(listaUsers, name));

                Console.WriteLine("Communication Request");
                this._notificationPusher.PushNotification(_serverIP, porta, Packet.Serialize(packet));
                SendComunicationUsername(listaUsers, name, listaUsers[userID].username);
                Logger.WriteLog(Logger.LogType.INFO, "Pacote para informar uma comunicação entre dois clientes enviada.");
                Send(Packet.Serialize(packet), userID);

            } else if (receivedPacket._GetType() == Pacote.COMMUNICATION_AES_ENCRYPTED_KEY) {
                byte[] encryptedAESKey = receivedPacket.GetDataAs<byte[]>();
                Packet packet = new Packet(Pacote.COMMUNICATION_AES_ENCRYPTED_KEY);
                packet.SetPayload(encryptedAESKey);
                this._notificationPusher.PushNotification(_serverIP, GetUserNotificationHandlerPort(listaUsers, listaUsers[userID].communicationUsername), Packet.Serialize(packet));
                Logger.WriteLog(Logger.LogType.INFO, "Chave AES para a comunicação dos clientes enviada.");
                Send(Packet.Serialize(packet), userID);
            }

        }

        //Função para enviar o nome do cliente comunicador para o cliente comunicado para mostrar no ecrã o nome do cliente que está a comunicar com ele
        private void SendComunicationUsername(List<User> listaUsers, string username, string comunicationUsername) {
            Packet packet = new Packet(Pacote.INFORM_COMUNICATION_USERNAME);
            int userIndex = GetUserIndex(listaUsers, username);
            byte[] userAESKey = GetUserAESKey(listaUsers, userIndex);
            AES aes = new AES(userAESKey);
            byte[] nomeEnc = aes.Encrypt(Encoding.ASCII.GetBytes(comunicationUsername));
            Logger.WriteLog(Logger.LogType.INFO, "Nome do cliente comunicador enviado para o cliente comunicado.");
            packet.SetPayload(nomeEnc);
            this._notificationPusher.PushNotification(_serverIP, GetUserNotificationHandlerPort(listaUsers, username), Packet.Serialize(packet));
            // Send(Packet.Serialize(packet), userID);

        }

        

        //Função para comparar dois arrays de bytes por exemplo para comparar passwords
        private static bool ComparaArrayBytes(byte[] a, byte[] b) {
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; ++i) {

                if (a[i] != b[i]) return false;
            }
            return true;
        }
        //Função para obter a porta da NotificationHandler de um cliente pelo nome para conseguir enviar notificações
        private int GetUserNotificationHandlerPort(List<User> listaUsers, string username) {
            int porta = 0;
            foreach (User user in listaUsers) {
                if (user.username == username) {
                    porta = user.portaNotificationHandler;
                }

            }
            return porta;
        }
        //Função para obter a publicKey de um cliente pelo nome
        private byte[] GetUserPublicKey(List<User> listaUsers, string username) {
            byte[] publicKey = new byte[0];
            foreach (User user in listaUsers) {
                if (user.username == username) {
                    publicKey = user._publicKey;
                }

            }
            return publicKey;
        }
        //Função para obter a AES key de um cliente pelo userID
        private byte[] GetUserAESKey(List<User> listaUsers, int userID) {
            byte[] userAesKey = new byte[0];
            foreach (User user in listaUsers) {
                if (user.userID == userID) {
                    userAesKey = user._aesKey;
                }

            }
            return userAesKey;
        }

        //função para ver index do user na lista com um nome
        private int GetUserIndex(List<User> listaUsers, string username) {
            int index = 0;
            foreach (User user in listaUsers) {
                if (user.username == username) {
                    index = listaUsers.IndexOf(user);
                }

            }
            return index;
        }
        //Função para ver se um cliente está online pelo nome
        private bool ClientIsOnline(List<User> listaUsers, string username) {
            bool existe = false;
            foreach (User user in listaUsers) {
                if (user.username == username) {
                    existe = true;
                }

            }
            return existe;
        }
        //Função chamada quando um cliente conecta-se ao servidor (Função do ProtoIP)
        public override void OnUserConnect(int usedID) {
            Logger.WriteLog(Logger.LogType.INFO, "Cliente Conectado.");
        }
        //Função chamada quando um cliente desconecta-se-se ao servidor (Função do ProtoIP)
        public override void OnUserDisconnect(int usedID) {
            Logger.WriteLog(Logger.LogType.INFO, "Cliente" + listaUsers[usedID].username + " Desconectado.");

            Console.WriteLine("User Disconnected");
            listaUsers.RemoveAt(usedID);
        }

    }
}

