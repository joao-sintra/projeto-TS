using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Servidor {
    internal class Auth {

        
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
      //  public bool isOnline { get; set; }
        public Auth() {

        }
    }
}
