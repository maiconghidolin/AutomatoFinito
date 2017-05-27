using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Models {
    class AutomatoFinito {

        public List<Estado> estados;
        public List<char> alfabeto;

        public AutomatoFinito() {
            this.estados = new List<Estado>();
            this.alfabeto = new List<char>();
        }

        private List<string[]> criaListaSaida() {
            List<string[]> listaSaida = new List<string[]>();
            // Monta o cabeçalho
            listaSaida.Add(Enumerable.Repeat<string>("", this.alfabeto.Count + 1).ToArray());
            listaSaida[0][0] = "δ";
            for (int i = 0; i < this.alfabeto.Count(); i++)
            {
                listaSaida[0][i + 1] = this.alfabeto[i].ToString();
            }

            // Monta cada linha do autômato 
            foreach (var estado in this.estados) {
                listaSaida.Add(Enumerable.Repeat<string>("X", this.alfabeto.Count + 1).ToArray());
                listaSaida[listaSaida.Count - 1][0] = (estado.final ? "*" : "") + estado.label;
                foreach (var transicao in estado.transicoes)
                {
                    int indiceLinha = listaSaida.Count() - 1;
                    int indiceColuna = this.alfabeto.IndexOf(transicao.simbolo) + 1;
                    if (listaSaida[indiceLinha][indiceColuna].Equals("X"))
                        listaSaida[indiceLinha][indiceColuna] = transicao.estadoDestino.label;
                    else
                        listaSaida[indiceLinha][indiceColuna] += "," + transicao.estadoDestino.label;
                }
            }
            return listaSaida;
        }

        public string ToCsv(string separator) {
            StringBuilder saida = new StringBuilder();
            List<string[]> listaSaida = this.criaListaSaida();

            // Transforma a lista em csv
            foreach (string[] linha in listaSaida) {
                saida.AppendLine(String.Join(separator, linha));
            }
            return saida.ToString();
        }

        public string ToPdf()
        {
            StringBuilder saida = new StringBuilder();
            List<string[]> listaSaida = this.criaListaSaida();

            MemoryStream stream = new MemoryStream();
            Document doc = new Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, stream);
            doc.Open();
            GerarPDF(doc, writer);
            doc.Close();
            writer.Close();
            // stream.GetBuffer
            stream.Close();

            // Transforma a lista em csv
            foreach (string[] linha in listaSaida)
            {
                
            }
            return saida.ToString();
        }

        private void GerarPDF(Document doc, PdfWriter writer) {
            //Dim table As PdfPTable = New PdfPTable(5)
            //Dim largura As Single = ((qntCaracteres / 10) / 2.54) * 72
            //Dim NumeroLinhas As Integer = tag.NumeroLinhas

            //Dim fonteTable = New Font(Font.FontFamily.COURIER, tamLetra)

            //table.TotalWidth = largura
            //table.LockedWidth = True
            //Dim widths = New Single() { largura * 0.14999999999999999, largura * 0.34999999999999998, largura * 0.25, largura * 0.14999999999999999, largura * 0.10000000000000001}
            //'tamanho das colunas
            //table.SetWidths(widths)

            //table.HorizontalAlignment = 0

            //table.AddCell(GeraCelula("Código", fonteTable, 1))
            //table.AddCell(GeraCelula("Descrição", fonteTable, 0))
            //table.AddCell(GeraCelula("Quantd.", fonteTable, 2))
            //table.AddCell(GeraCelula("Ordem", fonteTable, 1))
            //table.AddCell(GeraCelula("Item", fonteTable, 1))

            // table.WriteSelectedRows(0, -1, x + tag.X, y - tag.Y, writer.DirectContent)
        }

        private PdfPCell GeraCelula(string texto, iTextSharp.text.Font fonte, int align) {
            PdfPCell cell = new PdfPCell(new Phrase(texto, fonte));
            cell.HorizontalAlignment = align;
            return cell;
        }

    }
}
