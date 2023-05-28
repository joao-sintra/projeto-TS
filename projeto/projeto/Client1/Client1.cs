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

namespace Client1 {
    public partial class Cliente1 : Form {

        private const int PORT = 1234;
        private string username;
       


        private byte[] aesKey;
        Client client = new Client();

        static void ShowBytes(byte[] bytes) {
            foreach (byte b in bytes) {
                Debug.Write(b.ToString("X2") + " ");
            }
            Debug.WriteLine("");
        }

        public Cliente1() {
           
            InitializeComponent();
            tabControl2.SelectedTab = paginaLogin;
            
            

            // Create a new RSA instance


        }

        private void SendPublicKeyAndReciveAESkey() {
            ProtoIP.Crypto.RSA rsa = new ProtoIP.Crypto.RSA();
            // Generate a new RSA key-pair
            rsa.GenerateKeyPair();
            //Define the packet type
            Packet publicKeyPacket = new Packet(Pacote.PUBLIC_KEY);
            //Set the packet payload
            publicKeyPacket.SetPayload(rsa.ExportPublicKey());
            //Send the packet to Server
            client.Send(Packet.Serialize(publicKeyPacket));
            client.Receive();
            this.aesKey = rsa.Decrypt(client.ecryptedAesKey);
        }
        private static string SendAndReciveMessage(Client client, byte[] aesKey, string message) {
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
            client.Receive();

            //Console.WriteLine("Mensagem decriptada: ");
            byte[] msgDecrypt = aes.Decrypt(client.dados);
            //ShowBytes(msgDecrypt);
            string utfString = Encoding.UTF8.GetString(msgDecrypt, 0, msgDecrypt.Length);
            //Console.WriteLine(utfString);
            return utfString;
        }

        // Método para fechar o Client
        private void CloseClient() {
           
        }

        // Método para fechar o formulário
        private void Client_FormClosing(object sender, FormClosingEventArgs e) {
            // Chamar a função para fechar o Client
            CloseClient();
        }

        private void buttonQuit_Click(object sender, EventArgs e) {
            CloseClient();
            this.Close();
        }

        private void btEnviarMensagem_Click(object sender, EventArgs e) {
            //tetste
          
        }

        private void Cliente1_Load(object sender, EventArgs e) {

        }


        private void btEnviarMensagem_Click_1(object sender, EventArgs e) {

           
            string mensagem = txtMensagem.Text.Trim();

            if (!Valida.IsValidString(mensagem)) {
                MessageBox.Show("Mensagem vazia!","Aviso",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            Packet packetMensagem = new Packet(mensagem);
           

           
          
           // client.Send(Packet.Serialize(packet));
            
            //client.Receive();
            txtConsola.AppendText(Environment.NewLine + client.dados);

            //client.Disconnect();

        }

        private void txtMensagem_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void btInicioIni_Click(object sender, EventArgs e) {
            tabControl1.SelectedTab = Inicio;
        }

        private void btIDefinicoesIni_Click_1(object sender, EventArgs e) {
            tabControl1.SelectedTab = Definicoes;
        }

        private void label5_Click(object sender, EventArgs e) {

        }

        private void btRegistar_Click(object sender, EventArgs e) {
            tabControl2.SelectedTab = paginaRegisto;
        }
        private void LoginAndRegister(string nome, string password, string tipoPacote) {
            Cursor.Current = Cursors.WaitCursor;
            client.Connect("127.0.0.1", PORT);
            SendPublicKeyAndReciveAESkey();
            AES aes = new AES(aesKey);

            byte[] mensagemBytes = Encoding.ASCII.GetBytes(nome + ";" + password);
            byte[] mensagemEncriptada = aes.Encrypt(mensagemBytes);
            if(tipoPacote == "LOGIN") {
                Packet messagePacket = new Packet(Pacote.LOGIN);
                messagePacket.SetPayload(mensagemEncriptada);
                client.Send(Packet.Serialize(messagePacket));

                client.Receive();

                byte[] msgDecrypt = aes.Decrypt(client.login);
                string validacao = Encoding.UTF8.GetString(msgDecrypt, 0, msgDecrypt.Length);
                if (validacao == "true") {
                    tabControl2.SelectedTab = Menus;
                    this.username = txtNomeLogin.Text;
                    nomeUser.Text = username;
                } else {
                    MessageBox.Show("Credenciais erradas!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else if(tipoPacote == "REGISTER") {
                Packet messagePacket = new Packet(Pacote.REGISTER);
                messagePacket.SetPayload(mensagemEncriptada);
                client.Send(Packet.Serialize(messagePacket));

                client.Receive();

                //Console.WriteLine("Mensagem decriptada: ");
                byte[] msgDecrypt = aes.Decrypt(client.registo);
                string validacao = Encoding.UTF8.GetString(msgDecrypt, 0, msgDecrypt.Length);
                if (validacao == "true") {
                    tabControl2.SelectedTab = Menus;
                    this.username = txtNomeRegisto.Text;
                    nomeUser.Text = username;
                } else {
                    MessageBox.Show("O username já existe!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            tabControl2.SelectedTab = paginaLogin;
        }

        private void btVoltarLogin_Click(object sender, EventArgs e) {
            tabControl2.SelectedTab = paginaLogin;
        }

        private void btRegistarUtilizador_Click(object sender, EventArgs e) {
            //Fazer o Registo na base de dados e logar o utilizador
            //Falta Ecriptar a password em sha256
            LoginAndRegister(txtNomeLogin.Text, txtPasswordLogin.Text, "REGISTER");
           

        }

        private void btIrFormRegistar_Click(object sender, EventArgs e) {
            tabControl2.SelectedTab = paginaRegisto;
        }

        private void label7_Click(object sender, EventArgs e) {

        }

        private void txtMensagem_KeyDown_1(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                btEnviarMensagem.PerformClick();
                txtMensagem.Text= "";
            }
        }
    }
}

