using EI.SI;
using System;
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

namespace Client1 {
    public partial class Cliente1 : Form {

        private const int PORT = 10000;
        NetworkStream networkStream;
        ProtocolSI protocolSI;
        TcpClient client;


        public Cliente1() {
            
            InitializeComponent();
            tabPrincipal.SelectedTab = paginaLogin;

        }
        private string EnviarDados(string message, string ipAddress, string ipPorto) {
            // CRIAR UM CLIENTE TCP
            TcpClient tcpClient = null;
            // CRIAR A NETWORK STREAM PARA PODER COMUNICAR COM O SERVIDOR
            NetworkStream networkStream = null;

            // TRY/CATCH PARA FAZER O TRATAMENTO DE ERROS 
            try {
                // CRIAR UM CONJUNTO IP+PORTO DO SERVIDOR REMOTO
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipAddress),
                    int.Parse(ipPorto));

                // INSTANCIAR O CLIENTE TCP
                tcpClient = new TcpClient();

                // EFETUAR A LIGAÇÃO AO SERVIDOR
                tcpClient.Connect(endPoint);

                // OBTER A LIGAÇÃO DO SERVIDOR
                networkStream = tcpClient.GetStream();

                // BYTES LIDOS
                int bytesRead = 0;

                // PREPARAR A MENSAGEM A ENVIAR
                byte[] msgBytes = Encoding.UTF8.GetBytes(message);

                // ENVIAR A MENSAGEM PELA LIGAÇÃO
                networkStream.Write(msgBytes, 0, msgBytes.Length);

                // RECEBER A CONFIRMAÇÃO DO SERVIDOR
                byte[] ack = new byte[tcpClient.ReceiveBufferSize];

                // RECEBER A RESPOSTA DO SERVIDOR
                bytesRead = networkStream.Read(ack, 0, ack.Length);

                // EXTRAIR A MENSAGEM DE RESPOSTA
                string response = Encoding.UTF8.GetString(ack, 0, bytesRead);

                // CASO A RESPOSTA ESTEJA VAZIA
                if (response == "") {
                    // SE ALGO DE ESTRANHO SE PASSAR DEVOLVE O ERRO
                    return "Erro";
                }

                // DEVOLVE A RESPOSTA RECEBIDA
                return response;

            } catch (Exception ex) {
                // ALGO DE ESTRANHO SE PASSOU E DEVOLVE ERRO
                return "erro"+ex.Message;
            } finally {
                // FECHA A LIGAÇÃO SE AINDA ESTIVER ABERTA
                if (networkStream != null) {
                    networkStream.Close();
                }

                // FECHA A COMUNICAÇÃO SE AINDA ESTIVER ABERTA
                if (tcpClient != null) {
                    tcpClient.Close();
                }
            }
        }
        
        

        // Método para fechar o Client
        private void CloseClient() {
            // Definição da variável eot (End of Transmission) do tipo array de byte.
            // Utilização do método Make. ProtocolSICmdType serve para enviar dados
            byte[] eot = protocolSI.Make(ProtocolSICmdType.EOT);

            // A classe NetworkStream disponibiliza métodos para enviar/receber dados através de socket Stream
            // O Socket de rede é um endpoint interno para envio e recepção de dados com um nó/computador presente na rede.
            networkStream.Write(eot, 0, eot.Length);
            networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
            networkStream.Close();
            client.Close();
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
            string response = EnviarDados(txtMensagem.Text, txtIP.Text, txtPorto.Text).ToString();
            txtConsola.AppendText("Resposta do server 01: " + response +
                Environment.NewLine);
        }

        private void Cliente1_Load(object sender, EventArgs e) {

        }

       

       

        private void btEnviarMensagem_Click_1(object sender, EventArgs e) {
            string response = EnviarDados(txtMensagem.Text, txtIP.Text, txtPorto.Text).ToString();
            txtConsola.AppendText("Resposta do server 01: " + response +
                Environment.NewLine);
        }

        private void btInicioIni_Click(object sender, EventArgs e) {
            tabSecundaria.SelectedTab = Inicio;
        }

        private void btIDefinicoesIni_Click_1(object sender, EventArgs e) {
            tabSecundaria.SelectedTab = Definicoes;
        }

        private void label5_Click(object sender, EventArgs e) {

        }

        private void btRegistar_Click(object sender, EventArgs e) {
            tabPrincipal.SelectedTab = paginaRegisto;
        }

        private void btEntrarLogin_Click(object sender, EventArgs e) {
            tabPrincipal.SelectedTab = Menus;
        }

        private void btTerminarSessao_Click(object sender, EventArgs e) {
            //Colocar a validação para terminar a sessao colocar false
            tabPrincipal.SelectedTab = paginaLogin;
        }

        private void btVoltarLogin_Click(object sender, EventArgs e) {
            tabPrincipal.SelectedTab = paginaLogin;
        }

        private void btRegistarUtilizador_Click(object sender, EventArgs e) {
            //Fazer o Registo na base de dados e logar o utilizador
            tabPrincipal.SelectedTab = Menus;
        }

        private void btIrFormRegistar_Click(object sender, EventArgs e) {
            tabPrincipal.SelectedTab = paginaRegisto;
        }
    }
}

