namespace GameOfLife
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numRows = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numCols = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.canvas = new System.Windows.Forms.PictureBox();
            this.flowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.txtDensity = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCellSize = new System.Windows.Forms.TextBox();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnErase = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.labelGenerations = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtAlive = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSurvive = new System.Windows.Forms.TextBox();
            this.labeladf = new System.Windows.Forms.Label();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.btnFillZeros = new System.Windows.Forms.Button();
            this.btnFillOnes = new System.Windows.Forms.Button();
            this.comboPattern = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.labelPatrones = new System.Windows.Forms.Label();
            this.btnCancelarPatrones = new System.Windows.Forms.Button();
            this.checkGraficar = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtCombinaciones = new System.Windows.Forms.TextBox();
            this.btnCombinations = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.flowPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Dimensions:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 100;
            this.label2.Text = "rows:";
            // 
            // numRows
            // 
            this.numRows.Location = new System.Drawing.Point(34, 18);
            this.numRows.Name = "numRows";
            this.numRows.Size = new System.Drawing.Size(33, 20);
            this.numRows.TabIndex = 1;
            this.numRows.Text = "20";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(73, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 1000;
            this.label3.Text = "cols:";
            // 
            // numCols
            // 
            this.numCols.Location = new System.Drawing.Point(108, 18);
            this.numCols.Name = "numCols";
            this.numCols.Size = new System.Drawing.Size(34, 20);
            this.numCols.TabIndex = 2;
            this.numCols.Text = "20";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(6, 429);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(61, 23);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.Button1_Click);
            // 
            // canvas
            // 
            this.canvas.BackColor = System.Drawing.Color.Maroon;
            this.canvas.Location = new System.Drawing.Point(3, 3);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(557, 351);
            this.canvas.TabIndex = 7;
            this.canvas.TabStop = false;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.Canvas_Paint);
            this.canvas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseClick);
            // 
            // flowPanel
            // 
            this.flowPanel.AutoScroll = true;
            this.flowPanel.AutoSize = true;
            this.flowPanel.BackColor = System.Drawing.Color.DimGray;
            this.flowPanel.Controls.Add(this.canvas);
            this.flowPanel.Location = new System.Drawing.Point(166, 2);
            this.flowPanel.Name = "flowPanel";
            this.flowPanel.Size = new System.Drawing.Size(986, 566);
            this.flowPanel.TabIndex = 8;
            // 
            // Timer
            // 
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Density of ones:";
            // 
            // txtDensity
            // 
            this.txtDensity.Location = new System.Drawing.Point(92, 41);
            this.txtDensity.Name = "txtDensity";
            this.txtDensity.Size = new System.Drawing.Size(52, 20);
            this.txtDensity.TabIndex = 3;
            this.txtDensity.Text = "0.75";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "cells size";
            // 
            // txtCellSize
            // 
            this.txtCellSize.Location = new System.Drawing.Point(52, 67);
            this.txtCellSize.Name = "txtCellSize";
            this.txtCellSize.Size = new System.Drawing.Size(34, 20);
            this.txtCellSize.TabIndex = 4;
            this.txtCellSize.Text = "20";
            // 
            // btnPause
            // 
            this.btnPause.Enabled = false;
            this.btnPause.Location = new System.Drawing.Point(6, 458);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(138, 23);
            this.btnPause.TabIndex = 8;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.Button2_Click);
            // 
            // btnErase
            // 
            this.btnErase.Location = new System.Drawing.Point(76, 429);
            this.btnErase.Name = "btnErase";
            this.btnErase.Size = new System.Drawing.Size(68, 23);
            this.btnErase.TabIndex = 9;
            this.btnErase.Text = "Erase";
            this.btnErase.UseVisualStyleBackColor = true;
            this.btnErase.Click += new System.EventHandler(this.Button3_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 501);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Generations:";
            // 
            // labelGenerations
            // 
            this.labelGenerations.AutoSize = true;
            this.labelGenerations.Location = new System.Drawing.Point(73, 501);
            this.labelGenerations.Name = "labelGenerations";
            this.labelGenerations.Size = new System.Drawing.Size(31, 13);
            this.labelGenerations.TabIndex = 14;
            this.labelGenerations.Text = "none";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Born";
            // 
            // txtAlive
            // 
            this.txtAlive.Location = new System.Drawing.Point(9, 104);
            this.txtAlive.Name = "txtAlive";
            this.txtAlive.Size = new System.Drawing.Size(135, 20);
            this.txtAlive.TabIndex = 5;
            this.txtAlive.Text = "3";
            this.txtAlive.TextChanged += new System.EventHandler(this.TxtAlive_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 131);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "Survive";
            // 
            // txtSurvive
            // 
            this.txtSurvive.Location = new System.Drawing.Point(6, 148);
            this.txtSurvive.Name = "txtSurvive";
            this.txtSurvive.Size = new System.Drawing.Size(138, 20);
            this.txtSurvive.TabIndex = 6;
            this.txtSurvive.Text = "23";
            this.txtSurvive.TextChanged += new System.EventHandler(this.TxtSurvive_TextChanged);
            // 
            // labeladf
            // 
            this.labeladf.AutoSize = true;
            this.labeladf.Location = new System.Drawing.Point(9, 176);
            this.labeladf.Name = "labeladf";
            this.labeladf.Size = new System.Drawing.Size(103, 13);
            this.labeladf.TabIndex = 1001;
            this.labeladf.Text = "Interval(Miliseconds)";
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(6, 192);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(138, 20);
            this.txtInterval.TabIndex = 1002;
            this.txtInterval.Text = "100";
            // 
            // btnFillZeros
            // 
            this.btnFillZeros.Location = new System.Drawing.Point(6, 278);
            this.btnFillZeros.Name = "btnFillZeros";
            this.btnFillZeros.Size = new System.Drawing.Size(67, 23);
            this.btnFillZeros.TabIndex = 1003;
            this.btnFillZeros.Text = "Fill Zeros";
            this.btnFillZeros.UseVisualStyleBackColor = true;
            this.btnFillZeros.Click += new System.EventHandler(this.Button4_Click);
            // 
            // btnFillOnes
            // 
            this.btnFillOnes.Location = new System.Drawing.Point(80, 278);
            this.btnFillOnes.Name = "btnFillOnes";
            this.btnFillOnes.Size = new System.Drawing.Size(64, 23);
            this.btnFillOnes.TabIndex = 1004;
            this.btnFillOnes.Text = "Fill Ones";
            this.btnFillOnes.UseVisualStyleBackColor = true;
            this.btnFillOnes.Click += new System.EventHandler(this.Button5_Click);
            // 
            // comboPattern
            // 
            this.comboPattern.FormattingEnabled = true;
            this.comboPattern.Items.AddRange(new object[] {
            "Glider"});
            this.comboPattern.Location = new System.Drawing.Point(6, 235);
            this.comboPattern.Name = "comboPattern";
            this.comboPattern.Size = new System.Drawing.Size(136, 21);
            this.comboPattern.TabIndex = 1005;
            this.comboPattern.SelectedIndexChanged += new System.EventHandler(this.ComboPattern_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 219);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 13);
            this.label9.TabIndex = 1006;
            this.label9.Text = "Insert Pattern";
            // 
            // labelPatrones
            // 
            this.labelPatrones.AutoSize = true;
            this.labelPatrones.Location = new System.Drawing.Point(6, 304);
            this.labelPatrones.Name = "labelPatrones";
            this.labelPatrones.Size = new System.Drawing.Size(101, 13);
            this.labelPatrones.TabIndex = 1007;
            this.labelPatrones.Text = "Insertando patrones";
            this.labelPatrones.Visible = false;
            // 
            // btnCancelarPatrones
            // 
            this.btnCancelarPatrones.Location = new System.Drawing.Point(9, 320);
            this.btnCancelarPatrones.Name = "btnCancelarPatrones";
            this.btnCancelarPatrones.Size = new System.Drawing.Size(135, 23);
            this.btnCancelarPatrones.TabIndex = 1008;
            this.btnCancelarPatrones.Text = "Cancelar";
            this.btnCancelarPatrones.UseVisualStyleBackColor = true;
            this.btnCancelarPatrones.Visible = false;
            this.btnCancelarPatrones.Click += new System.EventHandler(this.BtnCancelarPatrones_Click);
            // 
            // checkGraficar
            // 
            this.checkGraficar.AutoSize = true;
            this.checkGraficar.Location = new System.Drawing.Point(6, 406);
            this.checkGraficar.Name = "checkGraficar";
            this.checkGraficar.Size = new System.Drawing.Size(63, 17);
            this.checkGraficar.TabIndex = 1009;
            this.checkGraficar.Text = "Graficar";
            this.checkGraficar.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 350);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 13);
            this.label10.TabIndex = 1010;
            this.label10.Text = "Tamaño de matriz:";
            // 
            // txtCombinaciones
            // 
            this.txtCombinaciones.Location = new System.Drawing.Point(108, 347);
            this.txtCombinaciones.Name = "txtCombinaciones";
            this.txtCombinaciones.Size = new System.Drawing.Size(34, 20);
            this.txtCombinaciones.TabIndex = 1011;
            // 
            // btnCombinations
            // 
            this.btnCombinations.Location = new System.Drawing.Point(12, 373);
            this.btnCombinations.Name = "btnCombinations";
            this.btnCombinations.Size = new System.Drawing.Size(130, 23);
            this.btnCombinations.TabIndex = 1012;
            this.btnCombinations.Text = "Generate combinations";
            this.btnCombinations.UseVisualStyleBackColor = true;
            this.btnCombinations.Click += new System.EventHandler(this.BtnCombinations_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1152, 627);
            this.Controls.Add(this.btnCombinations);
            this.Controls.Add(this.txtCombinaciones);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.checkGraficar);
            this.Controls.Add(this.btnCancelarPatrones);
            this.Controls.Add(this.labelPatrones);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.comboPattern);
            this.Controls.Add(this.btnFillOnes);
            this.Controls.Add(this.btnFillZeros);
            this.Controls.Add(this.txtInterval);
            this.Controls.Add(this.labeladf);
            this.Controls.Add(this.txtSurvive);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtAlive);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnErase);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.labelGenerations);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtCellSize);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtDensity);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.flowPanel);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.numCols);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numRows);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.flowPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox numRows;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox numCols;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.FlowLayoutPanel flowPanel;
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDensity;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCellSize;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnErase;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelGenerations;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtAlive;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSurvive;
        private System.Windows.Forms.Label labeladf;
        private System.Windows.Forms.TextBox txtInterval;
        private System.Windows.Forms.Button btnFillZeros;
        private System.Windows.Forms.Button btnFillOnes;
        private System.Windows.Forms.ComboBox comboPattern;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelPatrones;
        private System.Windows.Forms.Button btnCancelarPatrones;
        private System.Windows.Forms.CheckBox checkGraficar;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtCombinaciones;
        private System.Windows.Forms.Button btnCombinations;
    }
}

