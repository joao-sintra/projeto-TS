using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProtoIP;
using System.Threading;
using System.Diagnostics;
using ProtoIP.Crypto;
using System.Xml.Linq;

// Tanto o NotificationHandler como o NotificationPusher foram desenvolvidos com a ajuda do João Matos


namespace Client1 {
    public partial class Cliente1 : Form {

        private const int PORT = 1234;
        private const string IP = "127.0.0.1";
        private int NOTIFICATION_PORT = ProtoIP.Common.Network.GetRandomUnusedPort();
        private string username;
        private int authenticationAttempts = 0;
        private bool clientIsConnected = false;
        private byte[] sharedAESKey;
        private bool conversarCarregado = false;
        private string communicationUsername;
        AES aes;
        private byte[] aesKey;
        Client client = new Client();
        ProtoIP.Crypto.RSA rsa = new ProtoIP.Crypto.RSA();

        public Cliente1() {

            InitializeComponent();

            tabControl2.SelectedTab = paginaLogin;
            timer1.Start();
        }

        //Função para enviar a chave publica e receber a chave AES encriptada
        private void SendPublicKeyAndReciveAESkey() {
            // Generate a new RSA key-pair
            rsa.GenerateKeyPair();
            //Define the packet type
            Packet publicKeyPacket = new Packet(Pacote.PUBLIC_KEY);
            //Set the packet payload
            publicKeyPacket.SetPayload(rsa.ExportPublicKey());
            //Send the packet to Server
            client.Send(Packet.Serialize(publicKeyPacket));
            client.Receive(true);
            this.aesKey = rsa.Decrypt(client.ecryptedAesKey);
        }

        //Função para serializar a mensagem para conseguir mandar a mensagem e a assinatura no mesmo pacote
        //nos primeros 4 bytes vai ter o tamanho da mensagem encriptada, dps dos 4 bytes vai ter a mensagem em si, dps o tamanho da assinatura e dps a assinatura
        public byte[] SerializeMessage(byte[] mensagem, byte[] assinatura) {
            int mensagemLength = mensagem.Length;
            int assinaturaLength = assinatura.Length;
            byte[] serializedMessage = new byte[4 + mensagemLength + 4 + assinaturaLength];

            Array.Copy(BitConverter.GetBytes(mensagemLength), 0, serializedMessage, 0, 4);
            Array.Copy(mensagem, 0, serializedMessage, 4, mensagemLength);
            Array.Copy(BitConverter.GetBytes(assinaturaLength), 0, serializedMessage, 4 + mensagemLength, 4);
            Array.Copy(assinatura, 0, serializedMessage, 8 + mensagemLength, assinaturaLength);

            return serializedMessage;
        }

        private void btEnviarMensagem_Click_1(object sender, EventArgs e) {
            string mensagem = txtMensagem.Text.Trim();
            if (string.IsNullOrEmpty(mensagem)) {
                MessageBox.Show("Mensagem vazia!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //validacao para o cimprimento da mensagem 255 caracteres
            if (mensagem.Length > 255) {
                MessageBox.Show("Mensagem demasiado longa MAX 225!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (client.informComunication == null) {
                MessageBox.Show("Não está a comunicar com ninguém!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!conversarCarregado) {
                sharedAESKey = rsa.Decrypt(client.encryptedCommunicationAESKey);
               
            }

            //Encriptar a mensagem com a chave partilhada
            aes = new AES(sharedAESKey);
            Packet pacote = new Packet(Pacote.MESSAGE);
            byte[] mensagemBytes = Encoding.ASCII.GetBytes(mensagem);
            byte[] msgEncript = aes.Encrypt(mensagemBytes);

            //hash da mensagem encriptada para a assinatura
            byte[] messageHash = new ProtoIP.Crypto.SHA256(msgEncript)._digest;
            //Assinar a mensagem
            byte[] assinatura = rsa.Sign(messageHash);
            //Serializar a mensagem para mandar a mensagem e a assinatura no mesmo pacote
            byte[] serializedMessage = SerializeMessage(msgEncript, assinatura);
            //Envia o pacote com a mensagem e a assinatura
            pacote.SetPayload(serializedMessage);
            client.Send(Packet.Serialize(pacote));
            client.Receive(true);
            txtChat.AppendText("Eu: " + mensagem + "\r\n");

        }

        private void btInicioIni_Click(object sender, EventArgs e) {
            tabControl1.SelectedTab = Inicio;
        }
        //Atualiza a lista de utilizadores
        private void AtualizaListaUsers() {
            listaClientesConnectados.Items.Clear();
            string users = Encoding.UTF8.GetString(client.notification);
            string[] dadosUser = users.Split(';');
            foreach (var user in dadosUser)
                listaClientesConnectados.Items.Add(user);
        }
        //Função para autenticar o cliente com login ou registo
        private void AuthenticateClient(string nome, string password, string tipoPacote) {
            Cursor.Current = Cursors.WaitCursor;

            if (authenticationAttempts == 0) {
                client.Connect(IP, PORT);
                client._notificationHandler.StartListeningForNotifications(NOTIFICATION_PORT, client.OnNotificationReceive);
            }
            SendPublicKeyAndReciveAESkey();

            AES aes = new AES(aesKey);
            byte[] mensagemBytes = Encoding.ASCII.GetBytes(nome + ";" + password);
            byte[] mensagemEncriptada = aes.Encrypt(mensagemBytes);

            //Se a autenticação for para fazer login irá enviar um pacote do tipo LOGIN encripta os dados do cliente e envia para o servidor
            if (tipoPacote == "LOGIN") {
                Packet messagePacket = new Packet(Pacote.LOGIN);
                messagePacket.SetPayload(mensagemEncriptada);
                client.Send(Packet.Serialize(messagePacket));
                client.Receive(true);
                byte[] msgDecrypt = aes.Decrypt(client.login);
                string validacao = Encoding.UTF8.GetString(msgDecrypt, 0, msgDecrypt.Length);
                //Se a validação for true o cliente é autenticado e é enviado um pacote do tipo NOTIFICATION para o servidor com a porta para receber as notificações
                if (validacao == "true") {
                    Packet pacoteNotificacao = new Packet(Pacote.NOTIFICATION);
                    pacoteNotificacao.SetPayload(Encoding.ASCII.GetBytes("" + NOTIFICATION_PORT));
                    client.Send(Packet.Serialize(pacoteNotificacao));
                    client.Receive(true);

                    clientIsConnected = true;
                    tabControl2.SelectedTab = Menus;
                    this.username = txtNomeLogin.Text;
                    nomeUser.Text = username;

                    AtualizaListaUsers();
                    //client.msgRecivedEvent += recieveMessage;

                } else if (validacao == "false") {
                    MessageBox.Show("Credenciais erradas!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    authenticationAttempts++;
                    return;
                } else if (validacao == "userOnline") {
                    MessageBox.Show("Utilizador já online!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    authenticationAttempts++;
                }
                //Se a autenticação for para fazer login irá enviar um pacote do tipo REGISTER encripta os dados do cliente e envia para o servidor
            } else if (tipoPacote == "REGISTER") {
                Packet messagePacket = new Packet(Pacote.REGISTER);

                messagePacket.SetPayload(mensagemEncriptada);
                client.Send(Packet.Serialize(messagePacket));
                client.Receive(true);
                byte[] msgDecrypt = aes.Decrypt(client.registo);
                string validacao = Encoding.UTF8.GetString(msgDecrypt, 0, msgDecrypt.Length);
                //Se a validação for true o cliente é autenticado e é enviado um pacote do tipo NOTIFICATION para o servidor com a porta para receber as notificações

                if (validacao == "true") {
                    Packet pacoteNotificacao = new Packet(Pacote.NOTIFICATION);
                    pacoteNotificacao.SetPayload(Encoding.ASCII.GetBytes("" + NOTIFICATION_PORT));
                    client.Send(Packet.Serialize(pacoteNotificacao));
                    client.Receive(true);
                    tabControl2.SelectedTab = Menus;
                    this.username = txtNomeRegisto.Text;
                    nomeUser.Text = username;
                    clientIsConnected = true;

                    AtualizaListaUsers();
                } else {
                    MessageBox.Show("O username já existe!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    authenticationAttempts++;
                    return;
                }
            }
            txtNomeLogin.Clear();
            txtNomeRegisto.Clear();
            txtPasswordLogin.Clear();
            txtPasswordRegisto.Clear();
            Cursor.Current = Cursors.Default;
        }
        //Funcão para autenticar o cliente com o servidor
        private void btEntrarLogin_Click(object sender, EventArgs e) {
            AuthenticateClient(txtNomeLogin.Text, txtPasswordLogin.Text, "LOGIN");
        }

        private void btTerminarSessao_Click(object sender, EventArgs e) {
            //Colocar a validação para terminar a sessao colocar false
            client.Disconnect();
            client._notificationHandler.Stop();
            tabControl2.SelectedTab = paginaLogin;
        }

        private void btVoltarLogin_Click(object sender, EventArgs e) {
            tabControl2.SelectedTab = paginaLogin;
        }
        //Funcão para autenticar o cliente com o servidor

        private void btRegistarUtilizador_Click(object sender, EventArgs e) {
            AuthenticateClient(txtNomeRegisto.Text, txtPasswordRegisto.Text, "REGISTER");
        }

        private void btIrFormRegistar_Click(object sender, EventArgs e) {
            tabControl2.SelectedTab = paginaRegisto;
        }


        private void Cliente1_FormClosed(object sender, FormClosedEventArgs e) {
            if (clientIsConnected) {
                client.Disconnect();
                client._notificationHandler.Stop();
            }
        }
        //Função para conversar com outro cliente
        private void btConversar_Click(object sender, EventArgs e) {
            //Seleciona o cliente da lista e envia o nome para o servidor
            string nomeUser = listaClientesConnectados.GetItemText(listaClientesConnectados.SelectedItem);
            communicationUsername = nomeUser;
            if (nomeUser == username) {
                MessageBox.Show("Não pode conversar consigo mesmo!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (listaClientesConnectados.SelectedIndex == -1) {
                MessageBox.Show("Selecione um utilizador!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //Informa o servidor de uma comunicação com o cliente selecionado
            byte[] mensagemBytes = Encoding.ASCII.GetBytes(nomeUser);
            Packet packet = new Packet(Pacote.INFORM_COMUNICATION);
            packet.SetPayload(mensagemBytes);
            client.Send(Packet.Serialize(packet));
            client.Receive(true);
            //Gera uma chave AES para conversar com o cliente selecionado e manda-a para o servidor
            AES aes2 = new AES();
            aes2.GenerateKey();
            byte[] encriptedAESKey = ProtoIP.Crypto.RSA.Encrypt(aes2._key, client.otherClientPublicKey);
            Packet packet2 = new Packet(Pacote.COMMUNICATION_AES_ENCRYPTED_KEY);
            packet2.SetPayload(encriptedAESKey);
            sharedAESKey = aes2.GetKeyBytes();
            client.Send(Packet.Serialize(packet2));
            client.Receive(true);
            conversarCarregado = true;

        }
        //Função para receber a mensagem do servidor
        public void RecieveMessage() {
            string men;
            //Primeiro verifica se o cliente foi contactado
            if (client.informComunication != null) {
                //Segundo verifica se o botão de conversar foi carregado se não vai buscar a chaveAES partilhada e o nome do utilizador com quem se está a comunicar
                if (!conversarCarregado) {
                    //Depois vai buscar o nome do utilizador que enviou a mensagem e desencripta-o com a chave partilhada para aprentar no chat
                    sharedAESKey = rsa.Decrypt(client.encryptedCommunicationAESKey);
                    if (String.IsNullOrEmpty(communicationUsername) && client.informComunication != null) {
                        AES aes2 = new AES(aesKey);
                        byte[] comUsername = aes2.Decrypt(client.communicationUsername);
                        communicationUsername = Encoding.UTF8.GetString(comUsername, 0, comUsername.Length);
                    }
                }
                AES aes = new AES(sharedAESKey);
                //Depois desencripta a mensagem com a chave partilhada e mostra na consola
                if (client.mensagem != null) {
                    byte[] msg = aes.Decrypt(client.mensagem);
                    men = Encoding.UTF8.GetString(msg, 0, msg.Length);
                    txtChat.AppendText(communicationUsername + ": " + men + "\r\n");
                    client.mensagem = null;
                }
            }
        }

        //Função para atualizar a lista de utilizadores conectados
        private void btAtualizar_Click(object sender, EventArgs e) {
            if (clientIsConnected) {
                AtualizaListaUsers();
            }
        }
        //Função para receber as mensagem, técnica chamada de polling, de 1 em 1 segundo recebe as mensagens 
        private void timer1_Tick(object sender, EventArgs e) {

            timer1.Interval = 1000;

            if (clientIsConnected) {
                RecieveMessage();
            }
        }
    }
}

