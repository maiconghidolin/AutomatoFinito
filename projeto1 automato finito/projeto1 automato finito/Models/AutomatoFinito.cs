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

        public void ordenaAlfabeto() {
            this.alfabeto.Sort(
              delegate (char x, char y) {
                  return x.CompareTo(y);
              }
            );
        }

        private List<string[]> criaListaSaida() {
            List<string[]> listaSaida = new List<string[]>();
            // Monta o cabeçalho
            this.ordenaAlfabeto();
            listaSaida.Add(Enumerable.Repeat<string>("", this.alfabeto.Count + 1).ToArray());
            listaSaida[0][0] = "δ";
            for (int i = 0; i < this.alfabeto.Count(); i++) {
                listaSaida[0][i + 1] = this.alfabeto[i].ToString();
            }

            // Monta cada linha do autômato 
            foreach (var estado in this.estados) {
                listaSaida.Add(Enumerable.Repeat<string>("-", this.alfabeto.Count + 1).ToArray());
                listaSaida[listaSaida.Count - 1][0] = (estado.label == "0" ? "->" : "") + (estado.final ? "*" : "") + estado.label;
                foreach (var transicao in estado.transicoes) {
                    int indiceLinha = listaSaida.Count() - 1;
                    int indiceColuna = this.alfabeto.IndexOf(transicao.simbolo) + 1;
                    if (listaSaida[indiceLinha][indiceColuna].Equals("-"))
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

        public PdfPTable ToPdf() {
            List<string[]> listaSaida = this.criaListaSaida();

            PdfPTable pdfTable = new PdfPTable(this.alfabeto.Count + 1);
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;

            Font fonteTable = new Font(Font.FontFamily.COURIER, 12);

            // Transforma a lista em tabela 
            foreach (string[] linha in listaSaida) {
                foreach (string item in linha) {
                    PdfPCell cell = new PdfPCell(new Phrase(item, fonteTable));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfTable.AddCell(cell);
                }
            }

            return pdfTable;
        }

    }
}
