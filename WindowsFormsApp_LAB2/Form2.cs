using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp_LAB2
{
    public partial class Form2 : Form
    {
        SqlConnection conn;
        SqlDataReader myDataReader;
        public Form2()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Создание открытого подключения
            conn = new SqlConnection();
            conn.ConnectionString =
            @"Data Source=(local)\SQLEXPRESS;Integrated Security=SSPI;" + "Initial Catalog=dbTest";
            conn.Open();
            // Создание объекта команды SQL
            string strSQL = "Select * From dtQuestions";
            SqlCommand myCommand = new SqlCommand(strSQL, conn);
            // Получение объекта чтения данных с помощью ExecuteReader()
            myDataReader = myCommand.ExecuteReader();
            // Просмотр всех результатов
            while (myDataReader.Read())
            {
                listBox1.Items.Add("Код вопроса - " +
                myDataReader["dсQuestID"].ToString().Trim());
                string s = "";
                for (int i = 0; i < myDataReader.FieldCount; i++)
                    s = s + myDataReader.GetValue(i).ToString().Trim() + " \t";
                listBox1.Items.Add(s);
                listBox1.Items.Add("");
            }
            myDataReader.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            Form1 f = new Form1();
            f.ShowDialog();
        }
    }
}
