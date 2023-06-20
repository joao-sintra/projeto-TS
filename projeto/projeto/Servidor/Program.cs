using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ProtoIP;
using System.Threading;

namespace Servidor {

    class Program : ProtoServer {

        const int PORT = 1234;
        static void Main() {

            // using (var db = new AuthContext()) {
            //var user = new Auth {Id = 1, Username = "admin", Password = "123" };
            //db.Auths.Add(user);
            //db.SaveChanges();
            //}

            Console.WriteLine("Starting server...");
            Server server = new Server();
            Thread serverThread = new Thread(() => server.Start(PORT));
            serverThread.Start();
            Console.WriteLine("Server started on port: "+PORT);
            // byte[] data = Encoding.UTF8.GetBytes("boas a todos");
            // server.SendBroadcast(data);


            //Broadcast Connections


        }
    }
 }



