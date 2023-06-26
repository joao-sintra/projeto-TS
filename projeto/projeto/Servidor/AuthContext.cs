using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Servidor {
    internal class AuthContext : DbContext {
        public DbSet<Auth> Auths { get; set; }

        
    }
}
