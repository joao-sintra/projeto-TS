using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace GereOficina
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var db = new OficinaContext())
            {
                //var cliente = new Cliente { Nome = "Cliente1", Nif = "123456789" };
                //db.Clientes.Add(cliente);
                //var carro = new Carro { Matricula = "AA-01-AA", Cliente = cliente };
                //db.Carro.Add(carro);

                //var servico1 = new Servico { Id = 1, Data = DateTime.Now, Tipo = "Revisão" };

                //var parcela1 = new Parcela { Descriçao = "Oleo", Valor = 100, Servico = servico1 };
                //var parcela2 = new Parcela { Descriçao = "Agua", Valor = 50, Servico = servico1 };
                //db.Parcelas.Add(parcela1);
                //db.Parcelas.Add(parcela2);

                //var carro = db.Carros.SingleOrDefault(c => c.Id == 3);

                //var servico1 = new Servico { Data = DateTime.Now, Tipo = "Lavagem", Carro = carro };
                //db.Servicos.Add(servico1);

                //var parcela1 = new Parcela { Descriçao="Mao de obra", Valor= 70, Servico = servico1 };
                //var parcela2 = new Parcela { Descriçao="Produtos", Valor=15, Servico= servico1 };
                //db.Parcelas.Add(parcela1);
                //db.Parcelas.Add(parcela2);

                var servicos = db.Servicos.Where(s => s.Carro.Id == 3).ToList();

                foreach (var servico in servicos)
                {
                    servico.Data = DateTime.Now;
                }
            
                db.SaveChanges();
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
