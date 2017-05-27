using System;
using System.Windows.Forms;

namespace Views {
    public partial class ViewMain : Form {

        public ViewMain() {
            InitializeComponent();
        }

        private void btnGerarAutomato_Click(object sender, EventArgs e) {
            if (this.txtArquivoEntrada.Text.Trim() == "" || this.txtArquivoSaida.Text.Trim() == "") {
                MessageBox.Show("Selecione o arquivo de entrada e saída.");
                return;
            }
            this.btnGerarAutomato.Enabled = false;
            Views.ViewAutomatoFinito viewAF = new Views.ViewAutomatoFinito();
            viewAF.arquivoEntrada = this.txtArquivoEntrada.Text;
            viewAF.arquivoSaida = this.txtArquivoSaida.Text;
            viewAF.ShowDialog();
            this.btnGerarAutomato.Enabled = true;
        }

        private void txtArquivoEntrada_Click(object sender, EventArgs e) {
            if (this.openFileDialogEntrada.ShowDialog() == DialogResult.OK)
                this.txtArquivoEntrada.Text = this.openFileDialogEntrada.FileName;
        }

        private void txtArquivoSaida_Click(object sender, EventArgs e) {
            saveFileDialogSaida.Filter = "CSV|*.csv";
            if (this.saveFileDialogSaida.ShowDialog() == DialogResult.OK)
                this.txtArquivoSaida.Text = this.saveFileDialogSaida.FileName;
        }
    }
}
