using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ficha02
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Criar Métodos
        private string EnviarDados (String message, string ipAdress, string ipPorto)
        {
            //Criar um Cliente TCP
            TcpClient tcpClient = null;

            //Criar a network stream para poder comunicar com o servidor
            NetworkStream networkStream = null;

            //try/catch para fazer o tratamento de erros
            try
            {
                //Criar um conjunto ip+Porto do servidor remoto
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipAdress),
                    int.Parse(ipPorto));

                //Instaciar o cliente TCP
                tcpClient = new TcpClient();

                //Efetuar a ligação ao Servidor
                tcpClient.Connect(endPoint);

                //Obter a ligação do Servidor
                networkStream = tcpClient.GetStream();

                //BYTES LIDOS
                int bytesRead = 0;

                //Preparar a mensagem a enviar
                byte[] msgBytes = Encoding.UTF8.GetBytes(message);

                //Enviar a mensagem pela ligação
                networkStream.Write(msgBytes, 0, msgBytes.Length);

                //Receber a confirmação do servidor
                byte[] ack = new byte[tcpClient.ReceiveBufferSize];

                // Receber a resposta do servidor
                bytesRead = networkStream.Read(ack, 0, ack.Length);

                // Extrair a mensagem da resposta
                string response = Encoding.UTF8.GetString(ack, 0, bytesRead);

                // Caso a resposta esteja vazia
                if (response == "")
                {
                    return "Erro";

                }
                // Devolve a resposta recebida
                return response;
            }
            catch (Exception ex)

            {   // Algo de estranho se passou e devolve erro
                return "erro";
            }
            finally
            {
                // 
                if (networkStream != null) { 
                networkStream.Close();
            }
                if (tcpClient != null)
                {

                    tcpClient.Close();
                }
            } 
        }

        private void buttonServer01_Click(object sender, EventArgs e)
        {
            string response = EnviarDados(textBoxServer01.Text, textboxIPServer01.Text, textBoxPortoServer01.Text).ToString();
            textBoxConsola.AppendText("Resposta do server 01 : " + response + Environment.NewLine);
        }

        private void buttonServer02_Click(object sender, EventArgs e)
        {
            string response = EnviarDados(textBoxServer02.Text, textboxIPServer02.Text, textBoxPortoServer02.Text).ToString();
            textBoxConsola.AppendText("Resposta do server 01 : " + response + Environment.NewLine);
        }

        private void buttonServer03_Click(object sender, EventArgs e)
        {
            string response = EnviarDados(textBoxServer03.Text, textboxIPServer03.Text, textBoxPortoServer03.Text).ToString();
            textBoxConsola.AppendText("Resposta do server 01 : " + response + Environment.NewLine);
        }
    }
}
