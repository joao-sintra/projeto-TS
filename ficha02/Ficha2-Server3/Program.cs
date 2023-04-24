using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Ficha2_Server3
{
    internal class Program
    {
        // Função para decifrar a mensagem com a cifra de césar
        private static string DecifrarComCesar(string messsage, int shift)
        {
            // Converter a mensagem em array
            char[] buffer = messsage.ToCharArray();

            // Percorrer o array
            for (int i = 0; i < buffer.Length; i++)
            {
                // Obter um caracter cifrado
                char letter = buffer[i];

                // Desfazer o desvio no caracter
                letter = (char)(letter - shift);

                // Adiciona o caracter ao buffer decifrado
                buffer[i] = letter;

                
           

            }

            // Devolve a mensagem decifrada
            return new string(buffer);
        }
        static void Main(string[] args)
        {
            // Escrever para a consola
            Console.WriteLine("A iniciar o servidor...");

            // Criar um TCP LISTENER
            TcpListener tcpListener = null;

            // Criar um TCP CLIENT
            TcpClient tcpClient = null;

            // Criar a network stream para poder comunicar com o servidor
            NetworkStream networkStream = null;

            // Ciclo infinito para estar sempre à escuta
            while (true)
            {
                // Try/Catch para fazer o tratamento de erros
                try
                {
                    // Escrever para a consola
                    Console.WriteLine(" A ativar portos de ligação...");

                    // Criar um conjunto IP+PORTO para servidor
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, 50003
                        );

                    // Instanciar o servidor TCP
                    tcpListener = new TcpListener(endPoint);

                    // Escrever para a consola
                    Console.WriteLine(" A Aguardar por novos clientes...");

                    // Ficar à escuta de novos pedidos de ligação
                    tcpListener.Start();

                    // Aguardar por um novo cliente
                    tcpClient = tcpListener.AcceptTcpClient();

                    // Escrever para a consola
                    Console.WriteLine(" Novo cliente detectado...");

                    // Obter ligação do cliente
                    networkStream = tcpClient.GetStream();

                    // Bytes lidos
                    int bytesRead = 0;

                    // Criar um buffer para guardar os dados recebidos
                    byte[] buffer = new byte[tcpClient.ReceiveBufferSize];

                    // Ler dados do cliente
                    bytesRead = networkStream.Read(buffer, 0, buffer.Length);

                    // Escrever para a consola
                    Console.WriteLine("Informação recebida: "
                    + Encoding.UTF8.GetString(buffer, 0, bytesRead));

                    // Extrair a mensagem recebida
                    string result = Encoding.UTF8.GetString(buffer, 0, bytesRead).ToString();

                    // Escrever para a consola
                    Console.WriteLine(" A decifrar a mensagem...");

                    // Decifrar a mensagem
                    string deCipher = DecifrarComCesar(result, 5);

                    // Enviar a mensagem decifrada pra o cliente
                    string response = " A mensagem decifrada é '" + deCipher + "'";


                    // Escrever para a consola
                    Console.WriteLine(" A enviar: A mensagem decifrada é '" + deCipher + "'");

                    // Criar a resposta para o cliente
                    byte[] serverResponse = Encoding.UTF8.GetBytes(response);   

                    // Enviar resposta para o cliente
                    networkStream.Write(serverResponse, 0, response.Length);

                    // Escrever para a consola
                    Console.WriteLine(" A enviar: o Cálculo de (x+2)*3 é " + result);

                    // Criar resposta para o cliente 
                    byte[] Response = Encoding.UTF8.GetBytes
                        (" O cálculo de (x+2)*3 é " + result);

                    // Enviar a resposta para o cliente
                    networkStream.Write(serverResponse, 0, response.Length);


                }
                catch (Exception ex)
                {
                    // Algo de estranho se passou e devolve erro
                    Console.WriteLine("Erro: " + ex.ToString());
                }
                finally
                {
                    // Escrever para a consola 
                    Console.WriteLine(" A desligar as ligações...");

                    // Fecha a ligação se ainda estiver aberta
                    if (networkStream != null)
                    {
                        networkStream.Close();
                    }

                    // Fecha a comunicação se ainda estiver aberta
                    if (tcpClient != null)
                    {
                        tcpClient.Close();
                    }

                    //... e para o servidor
                    if (tcpListener != null)

                    {
                        tcpListener.Stop();
                    }
                }
            }
        }


    }
}
