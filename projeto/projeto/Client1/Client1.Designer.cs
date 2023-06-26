namespace Client1 {
    partial class Cliente1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Cliente1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Inicio = new System.Windows.Forms.TabPage();
            this.btAtualizar = new System.Windows.Forms.Button();
            this.btConversar = new System.Windows.Forms.Button();
            this.listaClientesConnectados = new System.Windows.Forms.ListBox();
            this.txtChat = new System.Windows.Forms.TextBox();
            this.btEnviarMensagem = new System.Windows.Forms.Button();
            this.txtMensagem = new System.Windows.Forms.TextBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.Menus = new System.Windows.Forms.TabPage();
            this.panel5 = new System.Windows.Forms.Panel();
            this.nomeUser = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btTerminarSessao = new System.Windows.Forms.Button();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btInicioIni = new System.Windows.Forms.Button();
            this.paginaLogin = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btIrFormRegistar = new System.Windows.Forms.Button();
            this.btEntrarLogin = new System.Windows.Forms.Button();
            this.txtPasswordLogin = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtNomeLogin = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.paginaRegisto = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btVoltarLogin = new System.Windows.Forms.Button();
            this.btRegistarUtilizador = new System.Windows.Forms.Button();
            this.txtPasswordRegisto = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtNomeRegisto = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.Inicio.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.Menus.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel8.SuspendLayout();
            this.paginaLogin.SuspendLayout();
            this.panel1.SuspendLayout();
            this.paginaRegisto.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Inicio);
            this.tabControl1.Location = new System.Drawing.Point(201, -22);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(969, 430);
            this.tabControl1.TabIndex = 33;
            // 
            // Inicio
            // 
            this.Inicio.Controls.Add(this.btAtualizar);
            this.Inicio.Controls.Add(this.btConversar);
            this.Inicio.Controls.Add(this.listaClientesConnectados);
            this.Inicio.Controls.Add(this.txtChat);
            this.Inicio.Controls.Add(this.btEnviarMensagem);
            this.Inicio.Controls.Add(this.txtMensagem);
            this.Inicio.Location = new System.Drawing.Point(4, 22);
            this.Inicio.Name = "Inicio";
            this.Inicio.Padding = new System.Windows.Forms.Padding(3);
            this.Inicio.Size = new System.Drawing.Size(961, 404);
            this.Inicio.TabIndex = 0;
            this.Inicio.Text = "Inicio";
            this.Inicio.UseVisualStyleBackColor = true;
            // 
            // btAtualizar
            // 
            this.btAtualizar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btAtualizar.Location = new System.Drawing.Point(695, 11);
            this.btAtualizar.Name = "btAtualizar";
            this.btAtualizar.Size = new System.Drawing.Size(232, 34);
            this.btAtualizar.TabIndex = 46;
            this.btAtualizar.Text = "Atualizar";
            this.btAtualizar.UseVisualStyleBackColor = true;
            this.btAtualizar.Click += new System.EventHandler(this.btAtualizar_Click);
            // 
            // btConversar
            // 
            this.btConversar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btConversar.Location = new System.Drawing.Point(766, 320);
            this.btConversar.Name = "btConversar";
            this.btConversar.Size = new System.Drawing.Size(101, 34);
            this.btConversar.TabIndex = 44;
            this.btConversar.Text = "Conversar";
            this.btConversar.UseVisualStyleBackColor = true;
            this.btConversar.Click += new System.EventHandler(this.btConversar_Click);
            // 
            // listaClientesConnectados
            // 
            this.listaClientesConnectados.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listaClientesConnectados.FormattingEnabled = true;
            this.listaClientesConnectados.ItemHeight = 21;
            this.listaClientesConnectados.Location = new System.Drawing.Point(682, 58);
            this.listaClientesConnectados.Name = "listaClientesConnectados";
            this.listaClientesConnectados.Size = new System.Drawing.Size(259, 256);
            this.listaClientesConnectados.TabIndex = 43;
            // 
            // txtChat
            // 
            this.txtChat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtChat.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtChat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.txtChat.Location = new System.Drawing.Point(5, 5);
            this.txtChat.Margin = new System.Windows.Forms.Padding(2);
            this.txtChat.Multiline = true;
            this.txtChat.Name = "txtChat";
            this.txtChat.ReadOnly = true;
            this.txtChat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChat.Size = new System.Drawing.Size(672, 318);
            this.txtChat.TabIndex = 41;
            this.txtChat.WordWrap = false;
            // 
            // btEnviarMensagem
            // 
            this.btEnviarMensagem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btEnviarMensagem.Image = global::Client1.Properties.Resources.icons8_enviado_24;
            this.btEnviarMensagem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btEnviarMensagem.Location = new System.Drawing.Point(584, 349);
            this.btEnviarMensagem.Margin = new System.Windows.Forms.Padding(2);
            this.btEnviarMensagem.Name = "btEnviarMensagem";
            this.btEnviarMensagem.Padding = new System.Windows.Forms.Padding(3);
            this.btEnviarMensagem.Size = new System.Drawing.Size(93, 40);
            this.btEnviarMensagem.TabIndex = 38;
            this.btEnviarMensagem.Text = "      Enviar";
            this.btEnviarMensagem.UseVisualStyleBackColor = true;
            this.btEnviarMensagem.Click += new System.EventHandler(this.btEnviarMensagem_Click_1);
            // 
            // txtMensagem
            // 
            this.txtMensagem.AccessibleDescription = "Escrever aqui!";
            this.txtMensagem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMensagem.Location = new System.Drawing.Point(5, 352);
            this.txtMensagem.Margin = new System.Windows.Forms.Padding(2);
            this.txtMensagem.Multiline = true;
            this.txtMensagem.Name = "txtMensagem";
            this.txtMensagem.Size = new System.Drawing.Size(575, 37);
            this.txtMensagem.TabIndex = 34;
            this.txtMensagem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMensagem_KeyDown);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.Menus);
            this.tabControl2.Controls.Add(this.paginaLogin);
            this.tabControl2.Controls.Add(this.paginaRegisto);
            this.tabControl2.Location = new System.Drawing.Point(2, -21);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1174, 461);
            this.tabControl2.TabIndex = 34;
            // 
            // Menus
            // 
            this.Menus.Controls.Add(this.panel5);
            this.Menus.Controls.Add(this.tabControl1);
            this.Menus.Location = new System.Drawing.Point(4, 22);
            this.Menus.Name = "Menus";
            this.Menus.Padding = new System.Windows.Forms.Padding(3);
            this.Menus.Size = new System.Drawing.Size(1166, 435);
            this.Menus.TabIndex = 0;
            this.Menus.Text = "Menu";
            this.Menus.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(45)))));
            this.panel5.Controls.Add(this.nomeUser);
            this.panel5.Controls.Add(this.label11);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Controls.Add(this.panel8);
            this.panel5.Location = new System.Drawing.Point(-4, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(209, 426);
            this.panel5.TabIndex = 34;
            // 
            // nomeUser
            // 
            this.nomeUser.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nomeUser.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.nomeUser.Image = global::Client1.Properties.Resources.icons8_usuário_homem_com_círculo_24__1_;
            this.nomeUser.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nomeUser.Location = new System.Drawing.Point(10, 394);
            this.nomeUser.Name = "nomeUser";
            this.nomeUser.Size = new System.Drawing.Size(189, 21);
            this.nomeUser.TabIndex = 43;
            this.nomeUser.Text = "Nome";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(17, 13);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(161, 45);
            this.label11.TabIndex = 42;
            this.label11.Text = "Safe Chat";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(45)))));
            this.panel6.Controls.Add(this.btTerminarSessao);
            this.panel6.Location = new System.Drawing.Point(7, 270);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(189, 52);
            this.panel6.TabIndex = 2;
            // 
            // btTerminarSessao
            // 
            this.btTerminarSessao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(45)))));
            this.btTerminarSessao.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(45)))));
            this.btTerminarSessao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btTerminarSessao.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btTerminarSessao.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btTerminarSessao.Image = global::Client1.Properties.Resources.icons8_exportar_24__1_;
            this.btTerminarSessao.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btTerminarSessao.Location = new System.Drawing.Point(-3, 3);
            this.btTerminarSessao.Margin = new System.Windows.Forms.Padding(0);
            this.btTerminarSessao.Name = "btTerminarSessao";
            this.btTerminarSessao.Size = new System.Drawing.Size(192, 47);
            this.btTerminarSessao.TabIndex = 0;
            this.btTerminarSessao.Text = "Sair";
            this.btTerminarSessao.UseVisualStyleBackColor = false;
            this.btTerminarSessao.Click += new System.EventHandler(this.btTerminarSessao_Click);
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(45)))));
            this.panel8.Controls.Add(this.btInicioIni);
            this.panel8.Location = new System.Drawing.Point(7, 70);
            this.panel8.Margin = new System.Windows.Forms.Padding(0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(189, 53);
            this.panel8.TabIndex = 0;
            // 
            // btInicioIni
            // 
            this.btInicioIni.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(45)))));
            this.btInicioIni.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(45)))));
            this.btInicioIni.FlatAppearance.BorderSize = 0;
            this.btInicioIni.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btInicioIni.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btInicioIni.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btInicioIni.Image = global::Client1.Properties.Resources.icons8_página_inicial_24__1_;
            this.btInicioIni.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btInicioIni.Location = new System.Drawing.Point(-3, 3);
            this.btInicioIni.Margin = new System.Windows.Forms.Padding(0);
            this.btInicioIni.Name = "btInicioIni";
            this.btInicioIni.Size = new System.Drawing.Size(192, 47);
            this.btInicioIni.TabIndex = 0;
            this.btInicioIni.Text = "Início";
            this.btInicioIni.UseVisualStyleBackColor = false;
            this.btInicioIni.Click += new System.EventHandler(this.btInicioIni_Click);
            // 
            // paginaLogin
            // 
            this.paginaLogin.BackColor = System.Drawing.Color.Gainsboro;
            this.paginaLogin.Controls.Add(this.label7);
            this.paginaLogin.Controls.Add(this.panel1);
            this.paginaLogin.Location = new System.Drawing.Point(4, 22);
            this.paginaLogin.Name = "paginaLogin";
            this.paginaLogin.Padding = new System.Windows.Forms.Padding(3);
            this.paginaLogin.Size = new System.Drawing.Size(1166, 435);
            this.paginaLogin.TabIndex = 1;
            this.paginaLogin.Text = "Login";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(547, 75);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 32);
            this.label7.TabIndex = 5;
            this.label7.Text = "Login";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btIrFormRegistar);
            this.panel1.Controls.Add(this.btEntrarLogin);
            this.panel1.Controls.Add(this.txtPasswordLogin);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtNomeLogin);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Location = new System.Drawing.Point(447, 125);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(279, 169);
            this.panel1.TabIndex = 4;
            // 
            // btIrFormRegistar
            // 
            this.btIrFormRegistar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btIrFormRegistar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btIrFormRegistar.ForeColor = System.Drawing.Color.White;
            this.btIrFormRegistar.Location = new System.Drawing.Point(190, 131);
            this.btIrFormRegistar.Name = "btIrFormRegistar";
            this.btIrFormRegistar.Size = new System.Drawing.Size(84, 33);
            this.btIrFormRegistar.TabIndex = 5;
            this.btIrFormRegistar.Text = "Registar";
            this.btIrFormRegistar.UseVisualStyleBackColor = false;
            this.btIrFormRegistar.Click += new System.EventHandler(this.btIrFormRegistar_Click);
            // 
            // btEntrarLogin
            // 
            this.btEntrarLogin.BackColor = System.Drawing.Color.Green;
            this.btEntrarLogin.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btEntrarLogin.ForeColor = System.Drawing.Color.White;
            this.btEntrarLogin.Location = new System.Drawing.Point(15, 131);
            this.btEntrarLogin.Name = "btEntrarLogin";
            this.btEntrarLogin.Size = new System.Drawing.Size(84, 33);
            this.btEntrarLogin.TabIndex = 4;
            this.btEntrarLogin.Text = "Entrar";
            this.btEntrarLogin.UseVisualStyleBackColor = false;
            this.btEntrarLogin.Click += new System.EventHandler(this.btEntrarLogin_Click);
            // 
            // txtPasswordLogin
            // 
            this.txtPasswordLogin.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtPasswordLogin.Location = new System.Drawing.Point(108, 89);
            this.txtPasswordLogin.Name = "txtPasswordLogin";
            this.txtPasswordLogin.Size = new System.Drawing.Size(129, 25);
            this.txtPasswordLogin.TabIndex = 1;
            this.txtPasswordLogin.UseSystemPasswordChar = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label5.Location = new System.Drawing.Point(52, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 17);
            this.label5.TabIndex = 2;
            this.label5.Text = "Nome:";
            // 
            // txtNomeLogin
            // 
            this.txtNomeLogin.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtNomeLogin.Location = new System.Drawing.Point(108, 31);
            this.txtNomeLogin.Name = "txtNomeLogin";
            this.txtNomeLogin.Size = new System.Drawing.Size(129, 25);
            this.txtNomeLogin.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(34, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 17);
            this.label6.TabIndex = 3;
            this.label6.Text = "Password:";
            // 
            // paginaRegisto
            // 
            this.paginaRegisto.BackColor = System.Drawing.Color.Gainsboro;
            this.paginaRegisto.Controls.Add(this.label8);
            this.paginaRegisto.Controls.Add(this.panel2);
            this.paginaRegisto.Location = new System.Drawing.Point(4, 22);
            this.paginaRegisto.Name = "paginaRegisto";
            this.paginaRegisto.Size = new System.Drawing.Size(1166, 435);
            this.paginaRegisto.TabIndex = 2;
            this.paginaRegisto.Text = "Registo";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(547, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(99, 32);
            this.label8.TabIndex = 7;
            this.label8.Text = "Registo";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btVoltarLogin);
            this.panel2.Controls.Add(this.btRegistarUtilizador);
            this.panel2.Controls.Add(this.txtPasswordRegisto);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.txtNomeRegisto);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Location = new System.Drawing.Point(447, 125);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(279, 169);
            this.panel2.TabIndex = 6;
            // 
            // btVoltarLogin
            // 
            this.btVoltarLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btVoltarLogin.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btVoltarLogin.ForeColor = System.Drawing.Color.White;
            this.btVoltarLogin.Location = new System.Drawing.Point(190, 131);
            this.btVoltarLogin.Name = "btVoltarLogin";
            this.btVoltarLogin.Size = new System.Drawing.Size(84, 33);
            this.btVoltarLogin.TabIndex = 5;
            this.btVoltarLogin.Text = "Voltar";
            this.btVoltarLogin.UseVisualStyleBackColor = false;
            this.btVoltarLogin.Click += new System.EventHandler(this.btVoltarLogin_Click);
            // 
            // btRegistarUtilizador
            // 
            this.btRegistarUtilizador.BackColor = System.Drawing.Color.Green;
            this.btRegistarUtilizador.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRegistarUtilizador.ForeColor = System.Drawing.Color.White;
            this.btRegistarUtilizador.Location = new System.Drawing.Point(15, 131);
            this.btRegistarUtilizador.Name = "btRegistarUtilizador";
            this.btRegistarUtilizador.Size = new System.Drawing.Size(84, 33);
            this.btRegistarUtilizador.TabIndex = 4;
            this.btRegistarUtilizador.Text = "Registar";
            this.btRegistarUtilizador.UseVisualStyleBackColor = false;
            this.btRegistarUtilizador.Click += new System.EventHandler(this.btRegistarUtilizador_Click);
            // 
            // txtPasswordRegisto
            // 
            this.txtPasswordRegisto.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtPasswordRegisto.Location = new System.Drawing.Point(108, 89);
            this.txtPasswordRegisto.Name = "txtPasswordRegisto";
            this.txtPasswordRegisto.Size = new System.Drawing.Size(129, 25);
            this.txtPasswordRegisto.TabIndex = 1;
            this.txtPasswordRegisto.UseSystemPasswordChar = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label9.Location = new System.Drawing.Point(52, 34);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 17);
            this.label9.TabIndex = 2;
            this.label9.Text = "Nome:";
            // 
            // txtNomeRegisto
            // 
            this.txtNomeRegisto.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtNomeRegisto.Location = new System.Drawing.Point(108, 31);
            this.txtNomeRegisto.Name = "txtNomeRegisto";
            this.txtNomeRegisto.Size = new System.Drawing.Size(129, 25);
            this.txtNomeRegisto.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(34, 92);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 17);
            this.label10.TabIndex = 3;
            this.label10.Text = "Password:";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Cliente1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1176, 428);
            this.Controls.Add(this.tabControl2);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximumSize = new System.Drawing.Size(1366, 768);
            this.Name = "Cliente1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cliente 1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Cliente1_FormClosed);
            this.tabControl1.ResumeLayout(false);
            this.Inicio.ResumeLayout(false);
            this.Inicio.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.Menus.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.paginaLogin.ResumeLayout(false);
            this.paginaLogin.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.paginaRegisto.ResumeLayout(false);
            this.paginaRegisto.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Inicio;
        private System.Windows.Forms.TextBox txtChat;
        private System.Windows.Forms.Button btEnviarMensagem;
        private System.Windows.Forms.TextBox txtMensagem;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage Menus;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btTerminarSessao;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button btInicioIni;
        private System.Windows.Forms.TabPage paginaLogin;
        private System.Windows.Forms.TabPage paginaRegisto;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPasswordLogin;
        private System.Windows.Forms.TextBox txtNomeLogin;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btEntrarLogin;
        private System.Windows.Forms.Button btIrFormRegistar;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btVoltarLogin;
        private System.Windows.Forms.Button btRegistarUtilizador;
        private System.Windows.Forms.TextBox txtPasswordRegisto;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtNomeRegisto;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label nomeUser;
        private System.Windows.Forms.ListBox listaClientesConnectados;
        private System.Windows.Forms.Button btConversar;
        private System.Windows.Forms.Button btAtualizar;
        private System.Windows.Forms.Timer timer1;
    }
}

