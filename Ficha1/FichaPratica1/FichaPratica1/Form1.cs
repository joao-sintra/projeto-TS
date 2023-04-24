using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FichaPratica1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Definir Variáveis
            int bufferSize = 20480;
            byte[] buffer = new byte[bufferSize]; //Byte array servem para armazenar dados binários


            int bytesRead = 0;

            String originalFilePath = "security.jpg";
            String copyFilePath = "copy_security.jpg";


            //----- Implementar o copiar ficheiro ----
            // Verificar sse o ficheiro já existe ou não
            if (File.Exists(copyFilePath))
                File.Delete(copyFilePath);
            // Criar controladores de ficheiros
            FileStream originalFileStream = new FileStream(originalFilePath, FileMode.Open);
            FileStream copyFileStream = new FileStream(copyFilePath, FileMode.Create);

            // ---- Copiar Conteúdo ---
            // Lê um bloco de bytes de um stream e escrever os dados num buffer:
            // https://docs.microsoft.com/en-us/dotnet/api/system.io.filestream.read?view=net-6.0


            while ((bytesRead = originalFileStream.Read(buffer, 0, bufferSize)) > 0) {
                copyFileStream.Write(buffer, 0, bytesRead);

            }
            // Mostrar uma mensagem informativa
            String msg = "File Copied[" + originalFileStream.Length + "bytes]\r\n";
            labelCopyMessage.Text = msg;
            labelCopyMessage.Visible = true;

            // Libertar Recursos
            originalFileStream.Close();
            originalFileStream.Close();


        }
    }
}

