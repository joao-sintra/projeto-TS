using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servidor {
    public class User {
        public int userID;
        public string username;
        public byte[] _publicKey;
        public byte[] _aesKey;
        public int portaNotificationHandler;
        public string communicationUsername;
        public byte[] sharedAESKey;

        public User(int u_userId, string u_username, byte[] publicKey, byte[] aesKey) {
            this.userID = u_userId;
            this.username = u_username; 
            this._publicKey = publicKey;
            this._aesKey = aesKey;
        }

    }
}
