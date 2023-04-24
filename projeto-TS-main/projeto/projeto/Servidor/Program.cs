using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Servidor {
    class Program {
        //Criar novamente uma constante, tal como feito do lado do cliente.
        private const int PORT = 10000;

        static void Main(string[] args) {
            // ESCREVER PARA A CONSOLA
            Console.WriteLine("A iniciar o servidor...");

            // CRIAR UM TCP LISTENER
            TcpListener tcpListener = null;

            // CRIAR UM TCP CLIENT
            TcpClient tcpClient = null;

            // CRIAR A NETWORK STREAM PARA PODER COMUNICAR COM O SERVIDOR
            NetworkStream networkStream = null;

            // CICLO INFINITO PARA ESTAR SEMPRE À ESCUTA
            while (true) {
                // TRY/CATCH PARA FAZER O TRATAMENTO DE ERROS
                try {
                    // ESCREVER PARA A CONSOLA
                    Console.WriteLine("A activar portos de ligação...");

                    // CRIAR UM CONJUNTO IP+PORTO PARA SERVIDOR
                    IPEndPoint endPoint = new IPEndPoint(
                        IPAddress.Loopback, PORT);

                    // INSTANCIAR O SERVIDOR TCP
                    tcpListener = new TcpListener(endPoint);

                    // ESCREVER PARA A CONSOLA
                    Console.WriteLine("A aguardar por novos clientes...");

                    // FICAR À ESCUTA DE NOVOS PEDIDOS DE LIGAÇÃO
                    tcpListener.Start();

                    // AGUARDAR POR UM NOVO CLIENTE
                    tcpClient = tcpListener.AcceptTcpClient();

                    // ESCREVER PARA A CONSOLA
                    Console.WriteLine("Novo cliente detectado...");

                    // OBTER A LIGAÇÃO DO CLIENTE
                    networkStream = tcpClient.GetStream();

                    // BYTES LIDOS
                    int bytesRead = 0;

                    // CRIAR UM BUFFER PARA GUARDAR OS DADOS RECEBIDOS
                    byte[] buffer = new byte[tcpClient.ReceiveBufferSize];

                    // LER DADOS DO CLIENTE
                    bytesRead = networkStream.Read(buffer, 0, buffer.Length);

                    // ESCREVER PARA A CONSOLA
                    Console.WriteLine("Informação recebida: "
                        + Encoding.UTF8.GetString(buffer, 0, bytesRead));

                    // EFECTUAR O CÁLCULO MATEMÁTICO
                    string result =(Encoding.UTF8.GetString(buffer, 0, bytesRead));

                    // ESCREVER PARA A CONSOLA
                    Console.WriteLine("Mensagem: " + result);

                    // CRIAR RESPOSTA PARA O CLIENTE
                    byte[] response = Encoding.UTF8.GetBytes(result);

                    // ENVIAR RESPOSTA PARA O CLIENTE
                    networkStream.Write(response, 0, response.Length);
                } catch (Exception ex) {
                    // ALGO DE ESTRANHO SE PASSOU E DEVOLVE ERRO
                    Console.WriteLine("Erro: " + ex.ToString());
                } finally {
                    // ESCREVER PARA A CONSOLA
                    Console.WriteLine("A desligar as ligações...");

                    // FECHA A LIGAÇÃO SE AINDA ESTIVER ABERTA
                    if (networkStream != null) {
                        networkStream.Close();
                    }

                    // FECHA A COMUNICAÇÃO SE AINDA ESTIVER ABERTA
                    if (tcpClient != null) {
                        tcpClient.Close();
                    }

                    // ... E PARA O SERVIDOR
                    if (tcpListener != null) {
                        tcpListener.Stop();
                    }
                }
            }
        }
    }
}


