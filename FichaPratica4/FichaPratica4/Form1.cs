using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FichaPratica4
{
    public partial class Form1 : Form
    {
        private byte[] key;
        private byte[] iv;

        AesCryptoServiceProvider aes;
        public Form1()
        {
            InitializeComponent();
        }
        //GERAR UMA CHAVE SIMÉTRICA A PARTIR DE UMA STRING
        private string GerarChavePrivada(string pass)
        {
            byte[] salt = new byte[] { 1, 9, 3, 4, 1, 0, 5, 8};

            Rfc2898DeriveBytes pwdGen = new Rfc2898DeriveBytes(pass, salt, 1000);

            // GENERATE KEY
            byte[] key = pwdGen.GetBytes(16);

            //CONVERTER A PASS EM BASE64
            string passB64 = Convert.ToBase64String(key);

            // DEVOLVER A PASS EM BYTES
            return passB64;

        }
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void bt_Cifrar_Click(object sender, EventArgs e)
        {

        }

        private void bt_decifrar_Click(object sender, EventArgs e)
        {

        }
    }
}
