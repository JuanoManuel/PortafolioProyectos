namespace Langton_Ant
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.canvas = new System.Windows.Forms.PictureBox();
            this.btnCrearHormiga = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelNumHormigas = new System.Windows.Forms.Label();
            this.flowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.comboOrientation = new System.Windows.Forms.ComboBox();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.txtPopulation = new System.Windows.Forms.TextBox();
            this.btnGeneratePopulation = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPadding = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelGenerations = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.labelReinas = new System.Windows.Forms.Label();
            this.labelSoldado = new System.Windows.Forms.Label();
            this.labelObreras = new System.Windows.Forms.Label();
            this.checkGraficar = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.flowPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // canvas
            // 
            this.canvas.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.canvas.Location = new System.Drawing.Point(3, 3);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(602, 472);
            this.canvas.TabIndex = 0;
            this.canvas.TabStop = false;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.Canvas_Paint);
            this.canvas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseClick);
            // 
            // btnCrearHormiga
            // 
            this.btnCrearHormiga.Location = new System.Drawing.Point(12, 563);
            this.btnCrearHormiga.Name = "btnCrearHormiga";
            this.btnCrearHormiga.Size = new System.Drawing.Size(161, 28);
            this.btnCrearHormiga.TabIndex = 1;
            this.btnCrearHormiga.Text = "Crear hormiga";
            this.btnCrearHormiga.UseVisualStyleBackColor = true;
            this.btnCrearHormiga.Click += new System.EventHandler(this.BtnCrearHormiga_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Hormigas:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelNumHormigas
            // 
            this.labelNumHormigas.AutoSize = true;
            this.labelNumHormigas.Location = new System.Drawing.Point(78, 105);
            this.labelNumHormigas.Name = "labelNumHormigas";
            this.labelNumHormigas.Size = new System.Drawing.Size(13, 13);
            this.labelNumHormigas.TabIndex = 3;
            this.labelNumHormigas.Text = "0";
            // 
            // flowPanel
            // 
            this.flowPanel.AutoSize = true;
            this.flowPanel.Controls.Add(this.canvas);
            this.flowPanel.Location = new System.Drawing.Point(179, 12);
            this.flowPanel.Name = "flowPanel";
            this.flowPanel.Size = new System.Drawing.Size(777, 579);
            this.flowPanel.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 535);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Orientación:";
            // 
            // comboOrientation
            // 
            this.comboOrientation.FormattingEnabled = true;
            this.comboOrientation.Items.AddRange(new object[] {
            "Norte",
            "Sur",
            "Este",
            "Oeste"});
            this.comboOrientation.Location = new System.Drawing.Point(80, 532);
            this.comboOrientation.Name = "comboOrientation";
            this.comboOrientation.Size = new System.Drawing.Size(94, 21);
            this.comboOrientation.TabIndex = 6;
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(12, 241);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(161, 27);
            this.btnPlay.TabIndex = 7;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.BtnPlay_Click);
            // 
            // btnPause
            // 
            this.btnPause.Enabled = false;
            this.btnPause.Location = new System.Drawing.Point(12, 274);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(161, 30);
            this.btnPause.TabIndex = 8;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.BtnPause_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 199);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Interval(Miliseconds):";
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(12, 215);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(161, 20);
            this.txtInterval.TabIndex = 10;
            this.txtInterval.Text = "1";
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Population:";
            // 
            // txtPopulation
            // 
            this.txtPopulation.Location = new System.Drawing.Point(74, 15);
            this.txtPopulation.Name = "txtPopulation";
            this.txtPopulation.Size = new System.Drawing.Size(100, 20);
            this.txtPopulation.TabIndex = 12;
            this.txtPopulation.Text = "5";
            // 
            // btnGeneratePopulation
            // 
            this.btnGeneratePopulation.Location = new System.Drawing.Point(15, 66);
            this.btnGeneratePopulation.Name = "btnGeneratePopulation";
            this.btnGeneratePopulation.Size = new System.Drawing.Size(157, 23);
            this.btnGeneratePopulation.TabIndex = 13;
            this.btnGeneratePopulation.Text = "Generate population";
            this.btnGeneratePopulation.UseVisualStyleBackColor = true;
            this.btnGeneratePopulation.Click += new System.EventHandler(this.BtnGeneratePopulation_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Padding:";
            // 
            // txtPadding
            // 
            this.txtPadding.Location = new System.Drawing.Point(74, 42);
            this.txtPadding.Name = "txtPadding";
            this.txtPadding.Size = new System.Drawing.Size(79, 20);
            this.txtPadding.TabIndex = 15;
            this.txtPadding.Text = "40";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(157, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "%";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 92);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Generations:";
            // 
            // labelGenerations
            // 
            this.labelGenerations.AutoSize = true;
            this.labelGenerations.Location = new System.Drawing.Point(78, 92);
            this.labelGenerations.Name = "labelGenerations";
            this.labelGenerations.Size = new System.Drawing.Size(13, 13);
            this.labelGenerations.TabIndex = 18;
            this.labelGenerations.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 118);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "Reinas:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 131);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Soldado:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 144);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 13);
            this.label10.TabIndex = 21;
            this.label10.Text = "Obreras:";
            // 
            // labelReinas
            // 
            this.labelReinas.AutoSize = true;
            this.labelReinas.Location = new System.Drawing.Point(78, 118);
            this.labelReinas.Name = "labelReinas";
            this.labelReinas.Size = new System.Drawing.Size(13, 13);
            this.labelReinas.TabIndex = 22;
            this.labelReinas.Text = "0";
            // 
            // labelSoldado
            // 
            this.labelSoldado.AutoSize = true;
            this.labelSoldado.Location = new System.Drawing.Point(78, 131);
            this.labelSoldado.Name = "labelSoldado";
            this.labelSoldado.Size = new System.Drawing.Size(13, 13);
            this.labelSoldado.TabIndex = 23;
            this.labelSoldado.Text = "0";
            // 
            // labelObreras
            // 
            this.labelObreras.AutoSize = true;
            this.labelObreras.Location = new System.Drawing.Point(78, 144);
            this.labelObreras.Name = "labelObreras";
            this.labelObreras.Size = new System.Drawing.Size(13, 13);
            this.labelObreras.TabIndex = 24;
            this.labelObreras.Text = "0";
            // 
            // checkGraficar
            // 
            this.checkGraficar.AutoSize = true;
            this.checkGraficar.Checked = true;
            this.checkGraficar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkGraficar.Location = new System.Drawing.Point(15, 179);
            this.checkGraficar.Name = "checkGraficar";
            this.checkGraficar.Size = new System.Drawing.Size(63, 17);
            this.checkGraficar.TabIndex = 25;
            this.checkGraficar.Text = "Graficar";
            this.checkGraficar.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(965, 603);
            this.Controls.Add(this.checkGraficar);
            this.Controls.Add(this.labelObreras);
            this.Controls.Add(this.labelSoldado);
            this.Controls.Add(this.labelReinas);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.labelGenerations);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtPadding);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnGeneratePopulation);
            this.Controls.Add(this.txtPopulation);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtInterval);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.comboOrientation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.flowPanel);
            this.Controls.Add(this.labelNumHormigas);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCrearHormiga);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.flowPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.Button btnCrearHormiga;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelNumHormigas;
        private System.Windows.Forms.FlowLayoutPanel flowPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboOrientation;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtInterval;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPopulation;
        private System.Windows.Forms.Button btnGeneratePopulation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPadding;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelGenerations;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label labelReinas;
        private System.Windows.Forms.Label labelSoldado;
        private System.Windows.Forms.Label labelObreras;
        private System.Windows.Forms.CheckBox checkGraficar;
    }
}

