using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GereOficina
{
    internal class Parcela
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public string Descriçao { get; set; }
        public Servico Servico { get; internal set; }
    }
}
