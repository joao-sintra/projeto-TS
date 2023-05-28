using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servidor {
    internal class Pacote {
        public const int LOGIN = 101;
        public const int REGISTER = 102;
        public const int MESSAGE = 103;
        public const int PUBLIC_KEY = 104;
        public const int AES_ENCRYPTED_KEY = 105;
    }
}
