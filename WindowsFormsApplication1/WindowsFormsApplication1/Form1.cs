using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        OpenFileDialog file = null;
        long tamanho = 0;
        byte[] imagem;
        string str;
        MySqlCommand sqlcmd;
        MySqlConnection conn;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                conn = conectar("banco001");
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected MySqlConnection conectar(string onde)
        {
            MySqlConnection conn;
            string Configuracao = "server=localhost; userid=root; database="+onde;
            conn = new MySqlConnection(Configuracao);
            try
            {
                conn.Open();
                //MessageBox.Show("Conectado");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro" + ex.Message);

            }
            return conn;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Carregar();
        }

        protected void Carregar()
        {
            try
            {
                openFileDialog1.ShowDialog(this);
                str = this.openFileDialog1.FileName;
                //MessageBox.Show("." + str);

                if (string.IsNullOrEmpty(str))
                {
                    return;
                }

                this.pictureBox1.Image = Image.FromFile(str);
                FileInfo arq = new FileInfo(str);
                tamanho = arq.Length;
                FileStream fs = new FileStream(str, FileMode.Open, FileAccess.Read, FileShare.Read);
                imagem = new byte[Convert.ToInt32(tamanho + 1)];
                int byteRead = fs.Read(imagem, 0, Convert.ToInt32(this.tamanho));
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("erro na carregar" + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                conn = conectar("banco001");
                string comando;

                comando = "insert into funcionario(nome,figura)" +
                    " values (@nome,@figura)";
                sqlcmd = new MySqlCommand(comando, conn);


                sqlcmd.Parameters.AddWithValue("@nome", textBox2.Text);
                sqlcmd.Parameters.AddWithValue("@figura", imagem);

                MySqlDataReader insert = sqlcmd.ExecuteReader();

                if (!insert.Read())
                {
                    MessageBox.Show("Paciente cadastrado com sucesso!");
                }

                else
                {
                    MessageBox.Show("Cadastro inválido");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(sqlcmd.ToString() + " " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                conn = conectar("banco001");
                string comando;
                int i = 0;

                comando = "select * from funcionario";
                sqlcmd = new MySqlCommand(comando, conn);


                MySqlDataReader consulta = sqlcmd.ExecuteReader();

                while (consulta.Read())
                {
                    dg1.Rows.Add();

                    dg1.Rows[i].Cells["id"].Value = Convert.ToString(consulta["id"]);

                    dg1.Rows[i].Cells["nome"].Value = Convert.ToString(consulta["nome"]);


                    i++;
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(sqlcmd.ToString() + " " + ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            conn = conectar("banco001");


            string sql = "Select id,nome from funcionario;";

            MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);

            DataSet dataset_eaquiquevai = new DataSet();
            da.Fill(dataset_eaquiquevai, "funcionario");

            dg1.DataSource = dataset_eaquiquevai;
            dg1.DataMember = "funcionario";
        }

        private void dg1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dg1.Rows[e.RowIndex];
            string id = row.Cells[0].Value.ToString();

            conn = conectar("banco001");
            string comando;
            int i = 0;

            comando = "select * from funcionario WHERE id="+id;
            sqlcmd = new MySqlCommand(comando, conn);


            MySqlDataReader consulta = sqlcmd.ExecuteReader();

            while (consulta.Read())
            {
                Console.WriteLine(Convert.ToString(consulta["id"]));
                textBox1.Text = Convert.ToString(consulta["id"]);
                textBox2.Text = Convert.ToString(consulta["nome"]);
            }

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            conn = conectar("banco001");


            string sql = "Select id,nome from funcionario WHERE nome LIKE '%" + txtConsulta.Text + "%';";

            MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);

            DataSet dataset_eaquiquevai = new DataSet();
            da.Fill(dataset_eaquiquevai, "funcionario");

            dg1.DataSource = dataset_eaquiquevai;
            dg1.DataMember = "funcionario";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                
                string comando;

                for (int i = 0; i < 100; i++)
                {
                    conn = conectar("banco001");
                    comando = "insert into funcionario(nome,figura)" +
                    " values (@nome,@figura)";
                    sqlcmd = new MySqlCommand(comando, conn);
                    sqlcmd.Parameters.AddWithValue("@nome", textBox2.Text+" "+i);
                    sqlcmd.Parameters.AddWithValue("@figura", imagem);
                    MySqlDataReader insert = sqlcmd.ExecuteReader();
                    conn.Close();
                }

               
            }
            catch (Exception ex)
            {
                MessageBox.Show(sqlcmd.ToString() + " " + ex.Message);
            }
        }

        private void txtConsulta_KeyUp(object sender, KeyEventArgs e)
        {
            conn = conectar("banco001");


            string sql = "Select id,nome from funcionario WHERE nome LIKE '%" + txtConsulta.Text + "%';";

            MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);

            DataSet dataset_eaquiquevai = new DataSet();
            da.Fill(dataset_eaquiquevai, "funcionario");

            dg1.DataSource = dataset_eaquiquevai;
            dg1.DataMember = "funcionario";
            conn.Close();
        }

        private void button4_Click_2(object sender, EventArgs e)
        {
            conn.Close();
            
        }

        private void txtConsulta_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click_3(object sender, EventArgs e)
        {
            try
            {
                conn = conectar("banco001");
                string comando;

                comando = "UPDATE funcionario SET nome=@nome,figura=@figura WHERE id="+textBox1.Text;
                sqlcmd = new MySqlCommand(comando, conn);


                sqlcmd.Parameters.AddWithValue("@nome", textBox2.Text);
                sqlcmd.Parameters.AddWithValue("@figura", imagem);

                MySqlDataReader insert = sqlcmd.ExecuteReader();

                if (!insert.Read())
                {
                    MessageBox.Show("Paciente alterado com sucesso!");
                }

                else
                {
                    MessageBox.Show("Cadastro inválido");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(sqlcmd.ToString() + " " + ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                conn = conectar("mysql");
                string comando;
                comando = "CREATE DATABASE banco001";
                sqlcmd = new MySqlCommand(comando, conn);
                sqlcmd.ExecuteNonQuery();
                MessageBox.Show("Banco criado" );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro " + ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                conn = conectar("banco001");
                string comando;
                comando = "CREATE TABLE IF NOT EXISTS `funcionario` ("
                          +"`id` int(11) NOT NULL AUTO_INCREMENT,"
                          +"`nome` varchar(50) NOT NULL,"
                          +"`figura` mediumblob NOT NULL,"
                          +"`dataGravacao` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,"
                          +"PRIMARY KEY (`id`)"
                          +") ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;";
                sqlcmd = new MySqlCommand(comando, conn);
                sqlcmd.ExecuteNonQuery();
                MessageBox.Show("Tabela criada");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro " + ex.Message);
            }
        }
    }
}
