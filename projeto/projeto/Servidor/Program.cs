using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ProtoIP;
using System.Threading;
using System.Data.Entity;

namespace Servidor {

    class Program : ProtoServer {

        const int PORT = 1234;
        static void Main() {

            //check database exists
            using (var dbContext = new AuthContext()) {
                bool isDatabaseExists = dbContext.Database.Exists();

                if (!isDatabaseExists) {
                    Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AuthContext>());
                    AuthContext db = new AuthContext();
                    db.Database.Initialize(true);
                } 
            }



            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AuthContext>());
            //AuthContext db = new AuthContext();
            //db.Database.Initialize(true);


            Console.WriteLine("Starting server...");
            Server server = new Server();
            Thread serverThread = new Thread(() => server.Start(PORT));
            serverThread.Start();
            Console.WriteLine("Server started on port: " + PORT);
        }
    }
}



