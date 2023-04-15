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
            // Definição das variáveis na função principal
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, PORT);
            TcpListener listener = new TcpListener(endPoint);

            // Iniciar o listener; apresentação da primeira mensagem na linha de comandos e inicialização do contador.
            listener.Start();
            Console.WriteLine("SERVIDOR PRONTO");
            int clientCounter = 0;

            //Criação do ciclo infinito de forma a que este esteja sempre em execução até ordem em contrário
            while (true) {
                // Definição da variável client do tipo TcpClient
                TcpClient client = listener.AcceptTcpClient();

                // Incrementação do contador, de forma a que vá sempre somando 1 (+1)
                clientCounter++;

                //Apresentação da mensagem indicativa do nº do client na linha de comandos
                Console.WriteLine("Client {0} connected", clientCounter);

                //Definição da variável clientHandler do tipo ClientHandler
                ClientHandler clientHandler
                    = new ClientHandler(client, clientCounter);
                clientHandler.Handle();
            }
        }
    }

    class ClientHandler {

    }
}

