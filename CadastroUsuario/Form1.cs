using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CadastroUsuario
{
    public partial class Form1 : Form
    {
        MySqlConnection con;
        public Form1()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCadastro_Click(object sender, EventArgs e)
        {
            try
            {
                string stringConexao = "datasource=localhost;username=root;password=admin;database=ESTUDO";

                con = new MySqlConnection(stringConexao);

                var hoje = DateTime.Now;

                string dataFormatada = hoje.ToString("yyyy-MM-dd HH:mm:ss"); // Formate a data corretamente
                string sql = string.Format(@"INSERT INTO USUARIO (NOME, CPF, TELEFONE, EMAIL, SEXO, ENDERECO, ANIVERSARIO)
                                        VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')",
                                        "NOME", "CPF", "TEL", "EMAIL", "F", "ENDEREÇO", dataFormatada);

                var comando = new MySqlCommand(sql, con);

                con.Open();

                comando.ExecuteReader();

                MessageBox.Show("Aqui");

                con.Close();
            }
            catch (Exception)
            {
                con.Close();
                throw;
            }
        }
    }
}
