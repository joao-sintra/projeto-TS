namespace FichaPratica4
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.tb_SegredoPartilhado = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_chave = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_vi = new System.Windows.Forms.TextBox();
            this.tb_TextoACifrar = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.bt_Cifrar = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.bt_TextoCifrado = new System.Windows.Forms.TextBox();
            this.bt_decifrar = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_TextoDecifrado = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(500, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Segredo partilhado (a KEY e o IV são gerado a partir deste segredo): ";
            // 
            // tb_SegredoPartilhado
            // 
            this.tb_SegredoPartilhado.Location = new System.Drawing.Point(26, 67);
            this.tb_SegredoPartilhado.Name = "tb_SegredoPartilhado";
            this.tb_SegredoPartilhado.Size = new System.Drawing.Size(675, 26);
            this.tb_SegredoPartilhado.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Chave Simétrica:";
            // 
            // tb_chave
            // 
            this.tb_chave.Location = new System.Drawing.Point(26, 171);
            this.tb_chave.Name = "tb_chave";
            this.tb_chave.Size = new System.Drawing.Size(675, 26);
            this.tb_chave.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 226);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(162, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Vetor de inicialização:";
            // 
            // tb_vi
            // 
            this.tb_vi.Location = new System.Drawing.Point(26, 249);
            this.tb_vi.Name = "tb_vi";
            this.tb_vi.Size = new System.Drawing.Size(675, 26);
            this.tb_vi.TabIndex = 5;
            // 
            // tb_TextoACifrar
            // 
            this.tb_TextoACifrar.Location = new System.Drawing.Point(26, 323);
            this.tb_TextoACifrar.Name = "tb_TextoACifrar";
            this.tb_TextoACifrar.Size = new System.Drawing.Size(675, 26);
            this.tb_TextoACifrar.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 300);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Texto a cifrar:";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.button1.Location = new System.Drawing.Point(26, 99);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(675, 32);
            this.button1.TabIndex = 8;
            this.button1.Text = "GERAR CHAVE SIMÉTRICA";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // bt_Cifrar
            // 
            this.bt_Cifrar.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.bt_Cifrar.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.bt_Cifrar.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.bt_Cifrar.Location = new System.Drawing.Point(26, 355);
            this.bt_Cifrar.Name = "bt_Cifrar";
            this.bt_Cifrar.Size = new System.Drawing.Size(675, 32);
            this.bt_Cifrar.TabIndex = 9;
            this.bt_Cifrar.Text = "CIFRAR";
            this.bt_Cifrar.UseVisualStyleBackColor = false;
            this.bt_Cifrar.Click += new System.EventHandler(this.bt_Cifrar_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 411);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 20);
            this.label5.TabIndex = 10;
            this.label5.Text = "Texto cifrado:";
            // 
            // bt_TextoCifrado
            // 
            this.bt_TextoCifrado.Location = new System.Drawing.Point(26, 444);
            this.bt_TextoCifrado.Multiline = true;
            this.bt_TextoCifrado.Name = "bt_TextoCifrado";
            this.bt_TextoCifrado.Size = new System.Drawing.Size(675, 95);
            this.bt_TextoCifrado.TabIndex = 11;
            // 
            // bt_decifrar
            // 
            this.bt_decifrar.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.bt_decifrar.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.bt_decifrar.Location = new System.Drawing.Point(26, 554);
            this.bt_decifrar.Name = "bt_decifrar";
            this.bt_decifrar.Size = new System.Drawing.Size(675, 32);
            this.bt_decifrar.TabIndex = 12;
            this.bt_decifrar.Text = "DECIFRAR";
            this.bt_decifrar.UseVisualStyleBackColor = false;
            this.bt_decifrar.Click += new System.EventHandler(this.bt_decifrar_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 603);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 20);
            this.label6.TabIndex = 13;
            this.label6.Text = "Texto decifrado:";
            // 
            // tb_TextoDecifrado
            // 
            this.tb_TextoDecifrado.BackColor = System.Drawing.SystemColors.Window;
            this.tb_TextoDecifrado.Location = new System.Drawing.Point(26, 626);
            this.tb_TextoDecifrado.Name = "tb_TextoDecifrado";
            this.tb_TextoDecifrado.ReadOnly = true;
            this.tb_TextoDecifrado.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tb_TextoDecifrado.Size = new System.Drawing.Size(675, 26);
            this.tb_TextoDecifrado.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 694);
            this.Controls.Add(this.tb_TextoDecifrado);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.bt_decifrar);
            this.Controls.Add(this.bt_TextoCifrado);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bt_Cifrar);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tb_TextoACifrar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_vi);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_chave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_SegredoPartilhado);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_SegredoPartilhado;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_chave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_vi;
        private System.Windows.Forms.TextBox tb_TextoACifrar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button bt_Cifrar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox bt_TextoCifrado;
        private System.Windows.Forms.Button bt_decifrar;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_TextoDecifrado;
    }
}

