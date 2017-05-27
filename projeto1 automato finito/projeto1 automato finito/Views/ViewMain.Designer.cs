namespace Views {
    partial class ViewMain {
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtArquivoEntrada = new System.Windows.Forms.TextBox();
            this.txtArquivoSaida = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGerarAutomato = new System.Windows.Forms.Button();
            this.openFileDialogEntrada = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialogSaida = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Arquivo de Entrada";
            // 
            // txtArquivoEntrada
            // 
            this.txtArquivoEntrada.Location = new System.Drawing.Point(13, 48);
            this.txtArquivoEntrada.Name = "txtArquivoEntrada";
            this.txtArquivoEntrada.Size = new System.Drawing.Size(383, 20);
            this.txtArquivoEntrada.TabIndex = 1;
            this.txtArquivoEntrada.Click += new System.EventHandler(this.txtArquivoEntrada_Click);
            // 
            // txtArquivoSaida
            // 
            this.txtArquivoSaida.Location = new System.Drawing.Point(13, 96);
            this.txtArquivoSaida.Name = "txtArquivoSaida";
            this.txtArquivoSaida.Size = new System.Drawing.Size(383, 20);
            this.txtArquivoSaida.TabIndex = 2;
            this.txtArquivoSaida.Click += new System.EventHandler(this.txtArquivoSaida_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Arquivo de Saída";
            // 
            // btnGerarAutomato
            // 
            this.btnGerarAutomato.Location = new System.Drawing.Point(282, 127);
            this.btnGerarAutomato.Name = "btnGerarAutomato";
            this.btnGerarAutomato.Size = new System.Drawing.Size(112, 23);
            this.btnGerarAutomato.TabIndex = 4;
            this.btnGerarAutomato.Text = "Gerar";
            this.btnGerarAutomato.UseVisualStyleBackColor = true;
            this.btnGerarAutomato.Click += new System.EventHandler(this.btnGerarAutomato_Click);
            // 
            // openFileDialogEntrada
            // 
            this.openFileDialogEntrada.FileName = "openFileDialogEntrada";
            // 
            // ViewMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 172);
            this.Controls.Add(this.btnGerarAutomato);
            this.Controls.Add(this.txtArquivoSaida);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtArquivoEntrada);
            this.Controls.Add(this.label1);
            this.Name = "ViewMain";
            this.Text = "Gerador de Autômatos Finitos";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtArquivoEntrada;
        private System.Windows.Forms.TextBox txtArquivoSaida;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGerarAutomato;
        private System.Windows.Forms.OpenFileDialog openFileDialogEntrada;
        private System.Windows.Forms.SaveFileDialog saveFileDialogSaida;
    }
}

