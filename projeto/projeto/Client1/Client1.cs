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

        private int PORT = 1234;
        private string IP = "127.0.0.1";
        private int NOTIFICATION_PORT = ProtoIP.Common.Network.GetRandomUnusedPort();
        private string username;
        private int authenticationAttempts = 0;
        private bool clientIsConnected = false;
        private byte[] sharedAESKey;
        private bool conversarCarregado = false;
        AES aes;
        private byte[] aesKey;
        Client client = new Client();
        ProtoIP.Crypto.RSA rsa = new ProtoIP.Crypto.RSA();

        public Cliente1() {

            InitializeComponent();
            
            tabControl2.SelectedTab = paginaLogin;
            timer1.Start();
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

        private void btEnviarMensagem_Click_1(object sender, EventArgs e) {
            string mensagem = txtMensagem.Text.Trim();
            if (string.IsNullOrEmpty(mensagem)) {
                MessageBox.Show("Mensagem vazia!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!conversarCarregado) {
                sharedAESKey = rsa.Decrypt(client.encryptedCommunicationAESKey);
            }
            aes = new AES(sharedAESKey);
            Packet pacote = new Packet(Pacote.MESSAGE);
            byte[] mensagemBytes = Encoding.ASCII.GetBytes(mensagem);
            byte[] msgEncript = aes.Encrypt(mensagemBytes);
            pacote.SetPayload(msgEncript);
            client.Send(Packet.Serialize(pacote));
            client.Receive(true);
            txtConsola.AppendText("Eu: " + mensagem + "\r\n");

        }

        private void btInicioIni_Click(object sender, EventArgs e) {
            tabControl1.SelectedTab = Inicio;
        }

        private void AtualizaListaUsers() {
            listaClientesConnectados.Items.Clear();
            string users = Encoding.UTF8.GetString(client.notification);
            string[] dadosUser = users.Split(';');
            foreach (var user in dadosUser)
                listaClientesConnectados.Items.Add(user);
        }

        private void LoginAndRegister(string nome, string password, string tipoPacote) {
            Cursor.Current = Cursors.WaitCursor;

            if (authenticationAttempts == 0) {
                client.Connect(IP, PORT);
                client._notificationHandler.StartListeningForNotifications(NOTIFICATION_PORT, client.OnNotificationReceive);
            }
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
            if (clientIsConnected) {
                client.Disconnect();
                client._notificationHandler.Stop();
            }
        }

        private void btConversar_Click(object sender, EventArgs e) {
            string nomeUser = listaClientesConnectados.GetItemText(listaClientesConnectados.SelectedItem);
            if (nomeUser == username) {
                MessageBox.Show("Não pode conversar consigo mesmo!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (listaClientesConnectados.SelectedIndex == -1) {
                MessageBox.Show("Selecione um utilizador!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] mensagemBytes = Encoding.ASCII.GetBytes(nomeUser);
            Packet packet = new Packet(Pacote.INFORM_COMUNICATION);
            packet.SetPayload(mensagemBytes);
            client.Send(Packet.Serialize(packet));
            client.Receive(true);

            AES aes2 = new AES();
            aes2.GenerateKey();
            byte[] encriptedAESKey = ProtoIP.Crypto.RSA.Encrypt(aes2._key, client.otherClientPublicKey);
            Packet packet2 = new Packet(Pacote.COMMUNICATION_AES_ENCRYPTED_KEY);
            packet2.SetPayload(encriptedAESKey);
            sharedAESKey = aes2.GetKeyBytes();
            client.Send(Packet.Serialize(packet2));
            client.Receive(true);
            conversarCarregado = true;
            // sharedAESKey = client.encryptedCommunicationAESKey;


        }

        public void recieveMessage() {


            if (client.informComunication != null) {
                if (!conversarCarregado) {
                    sharedAESKey = rsa.Decrypt(client.encryptedCommunicationAESKey);
                }

                AES aes = new AES(sharedAESKey);
                string men = "";
                if (client.mensagem != null) {
                    byte[] msg = aes.Decrypt(client.mensagem);
                    men = Encoding.UTF8.GetString(msg, 0, msg.Length);
                    txtConsola.AppendText("Recebida: " + men + "\r\n");
                    client.mensagem = null;
                }



            }



        }


        private void btAtualizar_Click(object sender, EventArgs e) {
            if (clientIsConnected) {
                AtualizaListaUsers();
            }
        }

        private void timer1_Tick(object sender, EventArgs e) {
            if (clientIsConnected) {
                recieveMessage();
            }
        }
    }
}

