using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Servidor {
    internal class Auth {

        
        public int Id { get; set; }

        [StringLength(30)]
        [Index(IsUnique = true)]
        public string Username { get; set; }
        
        public byte[] Password { get; set; }
       
        public byte[] Salt { get; set; }
      
        
        public DateTime LastAuthentication {get; set; }
        public DateTime AccoutCreation { get; set; }
        
        public Auth() {

        }
    }
}
