using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp_LAB2
{
    public partial class Form4 : Form
    {
        public string connectionString = @"Data Source=DESKTOP-E5V6EBP\SQLEXPRESS;Initial Catalog=Tours;Integrated Security=True";
        public string commandText;
        public SqlConnection myconnection = null;
        public SqlCommand myCommand;
        public SqlDataAdapter dataAdapter;
        SqlDataReader dataReader;
        public Form4()
        {
            InitializeComponent();
            myconnection = new SqlConnection(connectionString);
            myCommand = new SqlCommand();
            myCommand.Connection = myconnection;
            myCommand.CommandType = CommandType.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            myconnection.Open();
            SqlCommand myCommand = myconnection.CreateCommand();
            myCommand.CommandText =
           @"select * from information_schema.tables Where " + @"Table_type = 'BASE TABLE' AND TABLE_NAME <> 'dtproperties' ";
            dataReader = myCommand.ExecuteReader();
            while (dataReader.Read())
            {
                listBox1.Items.Add(dataReader["TABLE_NAME"]);
            }
            dataReader.Close();
            myconnection.Close();
        }



        private void button2_Click(object sender, EventArgs e)
        {
            myCommand.CommandText = "select * from " + textBox1.Text;
            try
            {
                myconnection.Open();
            }
            catch (Exception openFailedException)
            {
                MessageBox.Show(openFailedException.Message, "Ошибка соединения", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                myconnection.Close();
                return;
            }
            try
            {
                using (SqlDataReader dread = myCommand.ExecuteReader())
                {
                    MessageBox.Show("Таблица с именем " + textBox1.Text + "  существует!");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Создание таблицы " + textBox1.Text);
                myCommand.CommandText = "create table " + textBox1.Text + "(Название varchar(15))";
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myconnection.Close();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            myconnection.Open();
            SqlCommand myCommand = myconnection.CreateCommand();
            myCommand.CommandText = @"select * from [" + listBox1.Text + "] ";
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = myCommand;
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, listBox1.Text);
            myconnection.Close();
            dataGridView1.DataSource = ds.Tables[listBox1.Text].DefaultView;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            myconnection.Open();
            myCommand.CommandText = "drop table " + textBox1.Text;
            myCommand.ExecuteNonQuery();
            myconnection.Close();
            textBox1.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string s;
            int i, n;
            s = "";
            if (richTextBox1.Lines.Length != 0)
            {
                n = richTextBox1.Lines.Length;
                for (i = 0; i < n; i++)
                    s = s + richTextBox1.Lines[i];
                myconnection.Open();
                myCommand.CommandText = s;
                myCommand.ExecuteNonQuery();
                myconnection.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand myCommand = myconnection.CreateCommand();
                myconnection.Open();
                int Code = int.Parse(textBox2.Text);
                string sName = Convert.ToString(textBox3.Text);
                myCommand.CommandText = "INSERT INTO YERA VALUES (@p1, @p2)";
                myCommand.Parameters.Add("@p1", SqlDbType.Int, 4);
                myCommand.Parameters["@p1"].Value = Code;
                myCommand.Parameters.Add("@p2", SqlDbType.NVarChar, 10);
                myCommand.Parameters["@p2"].Value = sName;
                myCommand.ExecuteNonQuery();
                myconnection.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Запись " + textBox3.Text + "  существует!");
            }
            finally
            {
                myconnection.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string s;
            if (textBox4.Text != "")
            {
                myconnection.Open();
                int Code = int.Parse(textBox4.Text);
                SqlCommand myCommand = myconnection.CreateCommand();
                s = @"SELECT [Tours].[Name], [Tours].[Price], [Tours].[Information]," + @"[Seasons].[Start_date], [Seasons].[End_date] FROM [Tours], [Seasons] , [Vouchers] " + @" where [Tours].[Tour_code]= [Seasons].[Tour_code] AND " + @"[Seasons].[Season_code] = [Vouchers].[Season_code]" + @" AND [Vouchers].[Tourist_code]=@p1";
                myCommand.CommandText = s;
                myCommand.Parameters.Add("@p1", SqlDbType.Int, 4);
                myCommand.Parameters["@p1"].Value = Code;

                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = myCommand;
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds, "Выборка");
                myconnection.Close();
                dataGridView1.DataSource = ds.Tables["Выборка"].DefaultView;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            myconnection.Open();
            SqlCommand myCommand = myconnection.CreateCommand();
            myCommand.CommandText = "SELECT COUNT (*) FROM Tours";
            richTextBox1.AppendText("Всего оплат поступило " + Convert.ToString(myCommand.ExecuteScalar()) + "\n");
            myCommand.CommandText = "SELECT SUM (Price) FROM Tours";
            richTextBox1.AppendText("на сумму " + Convert.ToString(myCommand.ExecuteScalar()) + "\n");
            myCommand.CommandText = "SELECT MAX (Price) FROM Tours";
            richTextBox1.AppendText("Максимальная оплата = " + Convert.ToString(myCommand.ExecuteScalar()) + "\n");
            myCommand.CommandText = "SELECT MIN(Price) FROM Tours";
            richTextBox1.AppendText("Минимальная оплата = " + Convert.ToString(myCommand.ExecuteScalar()) + "\n");
            myCommand.CommandText = "SELECT AVG(Price) FROM Tours";
            richTextBox1.AppendText("Средняя величина оплаты = " + Convert.ToString(myCommand.ExecuteScalar()) + "\n");
            myconnection.Close();
        }
    }
}