using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CadastroUsuario
{
    public partial class Form1 : Form
    {

        #region String de conexão com o banco de dados
        private MySqlConnection con;
        private string stringConexao = "datasource=localhost;username=root;password=admin;database=ESTUDO";
        #endregion

        public Form1()
        {
            InitializeComponent();

            #region Caracterização da tabela
            tabUsuarios.View = View.Details;
            tabUsuarios.LabelEdit = true;
            tabUsuarios.AllowColumnReorder = true;
            tabUsuarios.FullRowSelect = true;
            tabUsuarios.GridLines = true;

            tabUsuarios.Columns.Add("ID", 50, HorizontalAlignment.Left);
            tabUsuarios.Columns.Add("NOME", 100, HorizontalAlignment.Left);
            tabUsuarios.Columns.Add("CPF", 100, HorizontalAlignment.Left);
            tabUsuarios.Columns.Add("TELEFONE", 80, HorizontalAlignment.Left);
            tabUsuarios.Columns.Add("EMAIL", 100, HorizontalAlignment.Left);
            tabUsuarios.Columns.Add("SEXO", 50, HorizontalAlignment.Left);
            tabUsuarios.Columns.Add("ENDEREÇO", 100, HorizontalAlignment.Left);
            tabUsuarios.Columns.Add("ANIVERSARIO", 100, HorizontalAlignment.Left);
            #endregion

            CarregaTudo();
        }

        #region Pode ignorar
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {

        }
        #endregion

        private void btnCadastro_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtNome.Text) || String.IsNullOrEmpty(txtCpf.Text)
                  || String.IsNullOrEmpty(txtTelefone.Text) || String.IsNullOrEmpty(txtEmail.Text) ||
                  String.IsNullOrEmpty(txtEndereco.Text) || String.IsNullOrEmpty(dataAniversario.Text))
                {
                    MessageBox.Show("Preencha todos os dados");
                }
                else if (!ValidarCPF(txtCpf.Text))
                {
                    MessageBox.Show("CPF invalido");
                }
                else
                {
                    string sexo = String.Empty;
                    if (radioF.Checked)
                        sexo = "F";
                    else if (radioM.Checked)
                        sexo = "M";
                    else if (radioO.Checked)
                        sexo = "O";

                    if (!String.IsNullOrEmpty(sexo))
                    {
                        #region Comando para executar o comando no banco

                        con = new MySqlConnection(stringConexao);

                        var hoje = dataAniversario.Text;

                        string dataFormatada = FormatarData(hoje);
                        string sql = string.Format(@"INSERT INTO USUARIO (NOME, CPF, TELEFONE, EMAIL, SEXO, ENDERECO, ANIVERSARIO)
                                        VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')",
                                                txtNome.Text, txtCpf.Text, txtTelefone.Text, txtEmail.Text, sexo, txtEndereco.Text, dataFormatada);

                        var comando = new MySqlCommand(sql, con);

                        con.Open();

                        comando.ExecuteReader();

                        MessageBox.Show("Cadastrado");
                        #endregion

                        con.Close();
                        CarregaTudo();
                    }
                }
            }
            catch (Exception)
            {
                con.Close();
            }
        }

        private string FormatarData(string dataString)
        {
            string formatoDataEntrada = "dddd, d 'de' MMMM 'de' yyyy";
            DateTime data = DateTime.ParseExact(dataString, formatoDataEntrada, System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));
            string formatoDataSaida = "yyyy-MM-dd HH:mm:ss";
            string dataFormatada = data.ToString(formatoDataSaida);
            return dataFormatada;
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtNomeProcurado.Text))
                {
                    CarregaTudo();
                }
                else
                {
                    con = new MySqlConnection(stringConexao);

                    string sql = string.Format(@"SELECT * FROM ESTUDO.USUARIO WHERE
                                                 LOWER(NOME) LIKE LOWER('%{0}%')
                                                 OR CPF LIKE '%{0}%'
                                                 OR TELEFONE LIKE '%{0}%'
                                                 OR LOWER(EMAIL) LIKE LOWER('%{0}%')
                                                 OR SEXO = 'OS'
                                                 OR LOWER(ENDERECO) LIKE LOWER('%{0}%')
                                                 OR ANIVERSARIO LIKE '%{0}%'", txtNomeProcurado.Text);

                    var comando = new MySqlCommand(sql, con);

                    con.Open();

                    var leitor = comando.ExecuteReader();

                    #region Atualiza a tabela da tela com os dados encontrados
                    tabUsuarios.Items.Clear();

                    while (leitor.Read())
                    {
                        String[] list = {
                        leitor.GetString(0),
                        leitor.GetString(1),
                        leitor.GetString(2),
                        leitor.GetString(3),
                        leitor.GetString(4),
                        leitor.GetString(5),
                        leitor.GetString(6),
                        leitor.GetString(7)
                        };

                        var linha = new ListViewItem(list);

                        tabUsuarios.Items.Add(linha);
                    }

                    MessageBox.Show("Atualizado");

                    con.Close();
                    #endregion

                }
            }
            catch (Exception)
            {
                con.Close();
            }
        }

        private void CarregaTudo()
        {
            try
            {
                con = new MySqlConnection(stringConexao);

                string sql = string.Format(@"SELECT * FROM ESTUDO.USUARIO");

                var comando = new MySqlCommand(sql, con);

                con.Open();

                var leitor = comando.ExecuteReader();

                tabUsuarios.Items.Clear();

                while (leitor.Read())
                {
                    String[] list = {
                        leitor.GetString(0),
                        leitor.GetString(1),
                        leitor.GetString(2),
                        leitor.GetString(3),
                        leitor.GetString(4),
                        leitor.GetString(5),
                        leitor.GetString(6),
                        leitor.GetString(7)
                        };

                    var linha = new ListViewItem(list);

                    tabUsuarios.Items.Add(linha);
                }

                con.Close();


            }
            catch (Exception)
            {
                con.Close();
            }
        }

        private void btnRemover_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtIdRemovido.Text))
                {
                    MessageBox.Show("Insira 1 ID por favor");
                }
                else
                {

                    con = new MySqlConnection(stringConexao);

                    string sql = string.Format(@"DELETE FROM ESTUDO.USUARIO WHERE ID = {0}", txtIdRemovido.Text);

                    var comando = new MySqlCommand(sql, con);

                    con.Open();

                    comando.ExecuteReader();

                    tabUsuarios.Items.Clear();

                    MessageBox.Show("Removido com sucesso");

                    con.Close();

                    CarregaTudo();

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Aconteceu algum problema");
                con.Close();
            }
        }

        private bool ValidarCPF(string cpf)
        {
            cpf = Regex.Replace(cpf, @"[-.]", "");

            if (cpf.Length != 11)
            {
                return false;
            }

            if (cpf.Distinct().Count() == 1)
            {
                return false;
            }

            int soma = 0;
            for (int i = 0; i < 9; i++)
            {
                soma += (cpf[i] - '0') * (10 - i);
            }
            int resto = soma % 11;
            int digitoVerificador1 = (resto < 2) ? 0 : 11 - resto;

            if (digitoVerificador1 != (cpf[9] - '0'))
            {
                return false;
            }

            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += (cpf[i] - '0') * (11 - i);
            }
            resto = soma % 11;
            int digitoVerificador2 = (resto < 2) ? 0 : 11 - resto;

            if (digitoVerificador2 != (cpf[10] - '0'))
            {
                return false;
            }

            return true;
        }
    }
}
