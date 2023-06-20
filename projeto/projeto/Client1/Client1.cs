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
        private int NOTIFICATION_PORT = ProtoIP.Common.Network.GetRandomUnusedPort();
        private string username;
        private int authenticationAttempts = 0;
        private bool clientIsConnected = false;
        private byte[] sharedAESKey;

        private byte[] aesKey;
        Client client = new Client();
        ProtoIP.Crypto.RSA rsa = new ProtoIP.Crypto.RSA();

        static void ShowBytes(byte[] bytes) {
            foreach (byte b in bytes) {
                Debug.Write(b.ToString("X2") + " ");
            }
            Debug.WriteLine("");
        }

        public Cliente1() {

            InitializeComponent();
            tabControl2.SelectedTab = paginaLogin;

        }




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
        private static void SendMessage(Client client, byte[] aesKey, string message) {
            AES aes = new AES(aesKey);
            byte[] mensagemBytes = Encoding.ASCII.GetBytes(message);
            // ShowBytes(mensagemBytes);

            byte[] mensagemEncriptada = aes.Encrypt(mensagemBytes);
            // ShowBytes(mensagemEncriptada);
            //Console.WriteLine("Mensagem em bytes:");
            // ShowBytes(mensagemBytes);

            Packet messagePacket = new Packet(Pacote.MESSAGE);
            messagePacket.SetPayload(mensagemEncriptada);
            client.Send(Packet.Serialize(messagePacket));
        }



        private void btEnviarMensagem_Click_1(object sender, EventArgs e) {
            string mensagem = txtMensagem.Text.Trim();
            AES aes = new AES(sharedAESKey);
            Packet pacote = new Packet(Pacote.MESSAGE);
            if (!Valida.IsValidString(mensagem)) {
                MessageBox.Show("Mensagem vazia!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            byte[] mensagemBytes = Encoding.ASCII.GetBytes(mensagem);
            //Console.WriteLine("Mensagem decriptada: ");
            byte[] msgEncript = aes.Encrypt(mensagemBytes);
            pacote.SetPayload(msgEncript);
            client.Send(Packet.Serialize(pacote));
         
           
            //ReciveMessage(client, aesKey);
           // txtConsola.AppendText(utfString);

        }

        private void btInicioIni_Click(object sender, EventArgs e) {
            tabControl1.SelectedTab = Inicio;
        }

        private void btIDefinicoesIni_Click_1(object sender, EventArgs e) {
            tabControl1.SelectedTab = Definicoes;
        }

        private void AdicionaUserLista() {
            listaClientesConnectados.Items.Clear();
            string users = Encoding.UTF8.GetString(client.notification);
            string[] dadosUser = users.Split(';');
            foreach (var user in dadosUser)
                listaClientesConnectados.Items.Add(user);
        }

        private void LoginAndRegister(string nome, string password, string tipoPacote) {
            Cursor.Current = Cursors.WaitCursor;
            client.Connect("127.0.0.1", PORT);
            // Start listening for notifications from the server
            // The NotificationHandler will run on a different thread and port.
            // The second argument is the callback function that will be called when a notification is received.
            client._notificationHandler.StartListeningForNotifications(NOTIFICATION_PORT, client.OnNotificationReceive);


            // this.clientIsConnected = true;
            SendPublicKeyAndReciveAESkey();
            AES aes = new AES(aesKey);

            byte[] mensagemBytes = Encoding.ASCII.GetBytes(nome + ";" + password);
            byte[] mensagemEncriptada = aes.Encrypt(mensagemBytes);

            if (tipoPacote == "LOGIN") {
                Packet messagePacket = new Packet(Pacote.LOGIN);
                messagePacket.SetPayload(mensagemEncriptada);
                client.Send(Packet.Serialize(messagePacket));
                client.Receive(true);
                byte[] msgDecrypt = aes.Decrypt(client.login);
                string validacao = Encoding.UTF8.GetString(msgDecrypt, 0, msgDecrypt.Length);
                if (validacao == "true") {
                    Packet pacoteNotificacao = new Packet(Pacote.NOTIFICATION);
                    pacoteNotificacao.SetPayload(Encoding.ASCII.GetBytes("" + NOTIFICATION_PORT));
                    client.Send(Packet.Serialize(pacoteNotificacao));
                    client.Receive(true);

                    clientIsConnected = true;
                    tabControl2.SelectedTab = Menus;
                    this.username = txtNomeLogin.Text;
                    nomeUser.Text = username;
                    AdicionaUserLista();

                } else {
                    MessageBox.Show("Credenciais erradas!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    authenticationAttempts++;
                    return;
                }
            } else if (tipoPacote == "REGISTER") {
                Packet messagePacket = new Packet(Pacote.REGISTER);

                messagePacket.SetPayload(mensagemEncriptada);
                client.Send(Packet.Serialize(messagePacket));
                client.Receive(true);

                //Console.WriteLine("Mensagem decriptada: ");
                byte[] msgDecrypt = aes.Decrypt(client.registo);
                string validacao = Encoding.UTF8.GetString(msgDecrypt, 0, msgDecrypt.Length);
                if (validacao == "true") {
                    Packet pacoteNotificacao = new Packet(Pacote.NOTIFICATION);
                    pacoteNotificacao.SetPayload(Encoding.ASCII.GetBytes("" + NOTIFICATION_PORT));
                    client.Send(Packet.Serialize(pacoteNotificacao));
                    client.Receive(true);
                    tabControl2.SelectedTab = Menus;
                    this.username = txtNomeRegisto.Text;
                    nomeUser.Text = username;
                    clientIsConnected = true;

                    AdicionaUserLista();
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
            // ClientRecieve();
            // Thread listenerThread = new Thread(ListenForChange);
            // listenerThread.Start();




        }

        private void btEntrarLogin_Click(object sender, EventArgs e) {
            LoginAndRegister(txtNomeLogin.Text, txtPasswordLogin.Text, "LOGIN");


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

        private void btRegistarUtilizador_Click(object sender, EventArgs e) {
            LoginAndRegister(txtNomeRegisto.Text, txtPasswordRegisto.Text, "REGISTER");

        }

        private void btIrFormRegistar_Click(object sender, EventArgs e) {
            tabControl2.SelectedTab = paginaRegisto;
        }


        private void Cliente1_FormClosed(object sender, FormClosedEventArgs e) {
            client.Disconnect();
            client._notificationHandler.Stop();
        }

        private void btConversar_Click(object sender, EventArgs e) {
            string nomeUser = listaClientesConnectados.GetItemText(listaClientesConnectados.SelectedItem);
            Debug.WriteLine(nomeUser);

            AES aes = new AES(aesKey);
            byte[] mensagemBytes = Encoding.ASCII.GetBytes(nomeUser);
            // ShowBytes(mensagemBytes);

          //  byte[] mensagemEncriptada = aes.Encrypt(mensagemBytes);
            // ShowBytes(mensagemEncriptada);
            //Console.WriteLine("Mensagem em bytes:");
            // ShowBytes(mensagemBytes);

            Packet packet = new Packet(Pacote.INFORM_COMUNICATION);
            packet.SetPayload(mensagemBytes);
            client.Send(Packet.Serialize(packet));
            client.Receive(true);
            AES aes2 = new AES();
            aes2.GenerateKey();

            byte[] encriptedAESKey = ProtoIP.Crypto.RSA.Encrypt(aes2._key, client.otherClientPublicKey);
            Packet packet2 = new Packet(Pacote.COMMUNICATION_AES_ENCRYPTED_KEY);
            packet2.SetPayload(encriptedAESKey);
            client.Send(Packet.Serialize(packet2));
            

        }

   





        private void Cliente1_MouseMove(object sender, MouseEventArgs e) {
            if (clientIsConnected) {
                AdicionaUserLista();
                
            }

        }

        private void txtConsola_MouseClick(object sender, MouseEventArgs e) {
            //AES aes = new AES(aesKey);
           // byte[] msgDecrypt = aes.Decrypt(client.mensagem);
           //TEMPORARIMENTE O ENVENTO DE RECEVER A CHAVE AES ENCRIPTADA 
            sharedAESKey = rsa.Decrypt(client.encryptedCommunicationAESKey);
            //FALTA O PASSO 11
            //string txt = Encoding.UTF8.GetString(client.mensagem, 0, client.mensagem.Length);
            //generate a string from the byte array above



          //  txtConsola.AppendText("MSG RECebida: \n" + txt);

        }

        private void button1_Click(object sender, EventArgs e) {
            //botao temporario para reveber as mensagens 
            AES aes = new AES(sharedAESKey);    
            byte[] msg=  aes.Decrypt(client.mensagem);
            string men = Encoding.UTF8.GetString(msg, 0, msg.Length);
            txtConsola.AppendText(men);
        }
    }
}

