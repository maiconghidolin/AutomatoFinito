using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Views {
    public partial class ViewAutomatoFinito : Form {

        public string arquivoEntrada, arquivoSaida;
        List<string> listaEntrada = new List<string>();

        public ViewAutomatoFinito() {
            InitializeComponent();
        }

        private void AutomatoFinito_Load(object sender, EventArgs e) {
            this.dataGridViewLogs.Rows.Clear();
            lerArquivo();
            AutomatoFinito automatoFinito = gerarAutomatoFinitoInicial();
            automatoFinito = determinizarAutomato(automatoFinito);
            removerMortos(automatoFinito);
            escreveLog("AFDM concluído...\r\n");
            //escreverArquivo(automatoFinito);
        }

        private void escreveLog(string mensagem) {
            this.dataGridViewLogs.Rows.Add(mensagem);
            this.dataGridViewLogs.CurrentCell = this.dataGridViewLogs.Rows[dataGridViewLogs.RowCount - 1].Cells[0];
        }

        private void lerArquivo() {
            escreveLog("Lendo arquivo da gramática...\r\n");
            System.IO.StreamReader sr = new System.IO.StreamReader(arquivoEntrada);
            while (!sr.EndOfStream) {
                string linha = sr.ReadLine();
                if (!string.IsNullOrEmpty(linha)) {
                    listaEntrada.Add(linha);
                }
            }
            sr.Close();
            sr.Dispose();
        }

        private void escreverArquivo(AutomatoFinito automatoFinito) {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(this.arquivoSaida);
            sw.Write(automatoFinito.ToString(";"));
            sw.Close();
            sw.Dispose();
            if (MessageBox.Show("Autômato gerado... Deseja abrir o arquivo?", "Concluído", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                System.Diagnostics.Process.Start(this.arquivoSaida);
            }
        }

        private AutomatoFinito gerarAutomatoFinitoInicial() {
            List<string> producoes, tokens;
            escreveLog("Iniciando geração do AFND...\r\n");
            escreveLog("Lendo produções...\r\n");
            producoes = (from linha in listaEntrada where linha.Contains(Utilidades.simboloAtribuicao) select linha.Replace(" ", string.Empty)).ToList();
            listaEntrada.RemoveAll(x => producoes.Contains(x.Replace(" ", string.Empty)));
            escreveLog("Lendo tokens...\r\n");
            tokens = (from linha in listaEntrada select linha.Replace(" ", string.Empty)).ToList();

            AutomatoFinito automatoFinitoInicial = new AutomatoFinito();
            analisarProducoes(producoes, automatoFinitoInicial);
            analisarTokens(tokens, automatoFinitoInicial);

            return automatoFinitoInicial;
        }

        private void analisarProducoes(List<string> producoes, AutomatoFinito automatoFinito) {
            int indiceDoSeparador;
            string[] transicoes;
            char simbolo;
            Estado estadoAtual;
            Dictionary<string, string> mapeamentoEstados = new Dictionary<string, string>();

            escreveLog("Analizando produções...\r\n");

            automatoFinito.estados.Add(new Estado((0).ToString(), false)); // primeiro estado: S
            foreach (var producao in producoes) {
                indiceDoSeparador = producao.IndexOf(Utilidades.simboloAtribuicao);
                string labelEstado = producao.Substring(0, indiceDoSeparador).Replace("<", String.Empty).Replace(">", String.Empty);
                if (labelEstado.Equals("S")) { // indica que uma nova gramatica ta comecando
                    mapeamentoEstados.Clear();
                    mapeamentoEstados.Add("S", (0).ToString());
                }
                if (mapeamentoEstados.ContainsKey(labelEstado)) { // se o estado ja ta mapeado busca o estado
                    string valor;
                    mapeamentoEstados.TryGetValue(labelEstado, out valor);
                    estadoAtual = (from obj in automatoFinito.estados where obj.label.Equals(valor) select obj).FirstOrDefault();
                } else { // senao mapeia e cria um estado novo
                    int idEstado = automatoFinito.estados.Count();
                    mapeamentoEstados.Add(labelEstado, idEstado.ToString());
                    estadoAtual = new Estado(idEstado.ToString(), false);
                    automatoFinito.estados.Add(estadoAtual);
                }

                transicoes = producao.Substring(indiceDoSeparador + Utilidades.simboloAtribuicao.Length).Replace("<", String.Empty).Replace(">", String.Empty).Split('|');
                foreach (var transicao in transicoes) {
                    // transicao(0) é simbolo
                    // transicao(1), se tiver, é estado, se não tiver cria um final sem transição
                    // se o estado nao ta no mapeamento, cria um e poe no map
                    simbolo = transicao[0];
                    if (simbolo.Equals(Utilidades.epsolon)) { // se o símbolo é o epsolon, o estado é final
                        estadoAtual.final = true;
                    } else {
                        Estado estadoDestino;
                        if (simbolo.ToString().Equals(transicao)) { // se só tem um símbolo, sem transição, cria um novo estado final
                            int idEstado = automatoFinito.estados.Count();
                            //mapeamentoEstados.Add(labelEstado, idEstado); não mapeia pq não vai ter outra referencia para este estado
                            estadoDestino = new Estado(idEstado.ToString(), true);
                            automatoFinito.estados.Add(estadoDestino);
                        } else {
                            // se tem simbolo e transição para outro estado
                            // verifica se o estado existe, senao cria um e mapeia
                            string lblEstadoTransicao = transicao.Substring(1);
                            if (mapeamentoEstados.ContainsKey(lblEstadoTransicao)) { // se o estado ja ta mapeado busca o estado
                                string valor;
                                mapeamentoEstados.TryGetValue(lblEstadoTransicao, out valor);
                                estadoDestino = (from obj in automatoFinito.estados where obj.label.Equals(valor) select obj).FirstOrDefault();
                            } else { // senao mapeia e cria um estado novo
                                int idEstado = automatoFinito.estados.Count();
                                mapeamentoEstados.Add(lblEstadoTransicao, idEstado.ToString());
                                estadoDestino = new Estado(idEstado.ToString(), false);
                                automatoFinito.estados.Add(estadoDestino);
                            }
                        }
                        estadoAtual.transicoes.Add(new Transicao(simbolo, estadoDestino));
                        escreveLog(String.Format("{0} por {1} vai para {2}...\r\n", estadoAtual.label, simbolo, estadoDestino.label.ToString()));
                    }
                }
            }
            escreveLog("Análise das produções concluída...\r\n");
        }

        private void analisarTokens(List<string> tokens, AutomatoFinito automatoFinito) {
            escreveLog("Analizando tokens...\r\n");
            if (automatoFinito.estados.Count == 0) { // se não tem produções, só tokens
                automatoFinito.estados.Add(new Estado((0).ToString(), false)); // primeiro estado: S
            }
            foreach (var token in tokens) {
                escreveLog("Gerando gramática para o token " + token + "...\r\n");
                Estado estadoAtual = automatoFinito.estados[0]; // estado S
                foreach (var simbolo in token) {
                    // cria um novo estado para transição
                    int idEstado = automatoFinito.estados.Count();
                    Estado estadoNovo = new Estado(idEstado.ToString(), false);
                    automatoFinito.estados.Add(estadoNovo);
                    // cria a transição para o novo estado
                    estadoAtual.transicoes.Add(new Transicao(simbolo, estadoNovo));
                    escreveLog(String.Format("{0} por {1} vai para {2}...\r\n", estadoAtual.label, simbolo, estadoNovo.label));
                    estadoAtual = estadoNovo;
                }
                estadoAtual.final = true;
                escreveLog(estadoAtual.label + " é o estado final que reconhece o token " + token + "...\r\n");
            }
            escreveLog("Análise dos tokens concluída...\r\n");
        }

        private AutomatoFinito determinizarAutomato(AutomatoFinito automatoFinito) {
            AutomatoFinito automatoFinitoDeterminizado = new AutomatoFinito();
            List<Estado> estadosPendentes = new List<Estado>();
            List<Estado> estadosCombinados = new List<Estado>();
            List<Transicao> transicoes = new List<Transicao>();

            escreveLog("Iniciando determinização...\r\n");
            estadosPendentes.Add(automatoFinito.estados[0]);
            while (estadosPendentes.Count > 0) {
                escreveLog("Determinizando transições do estado " + estadosPendentes[0].label + "...\r\n");
                List<string> labelsCombinados = estadosPendentes[0].label.Split(',').ToList();
                estadosCombinados = (from obj in automatoFinito.estados where labelsCombinados.Contains(obj.label) select obj).ToList();

                Estado novoEstado = (from obj in automatoFinitoDeterminizado.estados where obj.label.Equals(estadosPendentes[0].label) select obj).FirstOrDefault();
                if (novoEstado == null) {
                    novoEstado = new Estado(estadosPendentes[0].label, estadosCombinados.Exists(x => x.final));
                    automatoFinitoDeterminizado.estados.Add(novoEstado);
                } else {
                    novoEstado.final = estadosCombinados.Exists(x => x.final);
                }

                foreach (var estado in estadosCombinados) {
                    transicoes.AddRange(estado.transicoes);
                    if (estado.transicoes.Count > 0) {
                        escreveLog(String.Format("Obtendo transições do estado original {0}: {1}...\r\n", estado.label,
                            String.Join(", ", estado.transicoes.Select(x => x.simbolo + "=>" + x.estadoDestino.label))));
                    } else {
                        escreveLog("Obtendo transições do estado original" + estado.label + " (nenhuma transição)...");
                    }
                }

                List<char> simbolosDistintos = transicoes.Select(x => x.simbolo).Distinct().ToList();
                simbolosDistintos.Sort();
                foreach (var simbolo in simbolosDistintos) {
                    List<string> estadosAlcancadosPeloSimbolo = transicoes.Where(x => x.simbolo.Equals(simbolo)).Select(x => x.estadoDestino.label).Distinct().ToList();
                    estadosAlcancadosPeloSimbolo.Sort();
                    string labelEstadoDestino = string.Join(",", estadosAlcancadosPeloSimbolo);
                    Estado estadoDestino = (from obj in automatoFinitoDeterminizado.estados where obj.label.Equals(labelEstadoDestino) select obj).FirstOrDefault();
                    if (estadoDestino == null) {
                        estadoDestino = new Estado(labelEstadoDestino, false);
                        automatoFinitoDeterminizado.estados.Add(estadoDestino);
                        estadosPendentes.Add(estadoDestino);
                    }
                    novoEstado.transicoes.Add(new Transicao(simbolo, estadoDestino));
                    escreveLog(String.Format("{0} por {1} vai para {2}...\r\n", estadosPendentes[0].label, simbolo, labelEstadoDestino));
                }

                transicoes.Clear();
                estadosPendentes.RemoveAt(0);
            }
            escreveLog("Determinização concluída...\r\n");
            return automatoFinitoDeterminizado;
        }

        private void removerMortos(AutomatoFinito automatoFinito) {
            escreveLog("Iniciando remoção de estados mortos...\r\n");
            List<Estado> estadosRemover = new List<Estado>();
            foreach (Estado estado in automatoFinito.estados) {
                if (estado.final) {
                    continue;
                }
                bool vivo = BFS(automatoFinito, estado);
                if (!vivo) {
                    estadosRemover.Add(estado);
                    escreveLog(estado.label + " é um estado morto e será removido...\r\n");
                }
            }
            automatoFinito.estados.RemoveAll(x => estadosRemover.Contains(x));
            foreach (Estado estado in automatoFinito.estados) {
                estado.transicoes = estado.transicoes.Where(x => automatoFinito.estados.Contains(x.estadoDestino)).ToList();
            }
            escreveLog("Remoção de estados mortos concluída...\r\n");
        }

        private bool BFS(AutomatoFinito automatoFinito, Estado estadoInicial) {
            // retorna true se encontra um estado final, senão false
            Queue<Estado> fila = new Queue<Estado>();
            List<Estado> estadosAlcancados = new List<Estado>();
            fila.Enqueue(estadoInicial);
            while (fila.Count > 0) {
                Estado estadoDesempilhado = fila.Dequeue();
                foreach (Transicao transicao in estadoDesempilhado.transicoes) {
                    if(transicao.estadoDestino != null) {
                        if (!estadosAlcancados.Contains(transicao.estadoDestino)) {
                            if (transicao.estadoDestino.final) {
                                return true;
                            }
                            fila.Enqueue(transicao.estadoDestino);
                            estadosAlcancados.Add(transicao.estadoDestino);
                        }
                    }
                }
            }
            return false;
        }
    }
}
