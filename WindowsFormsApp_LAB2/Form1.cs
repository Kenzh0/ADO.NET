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
    public partial class Form1 : Form
    {
        SqlConnection conn;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Создание строк подключения с помощью объекта построителя
            SqlConnectionStringBuilder cnStrBuilder = new SqlConnectionStringBuilder();
 
            // Для второй строки подключения вместо свойства AttachDBFilename
            // указать свойство InitialCatalog и изменить свойство DataSource
            cnStrBuilder.InitialCatalog = "dbTest";
            cnStrBuilder.DataSource = @"DESKTOP-E5V6EBP\SQLEXPRESS";
            cnStrBuilder.ConnectTimeout = 30;
            cnStrBuilder.IntegratedSecurity = true;
            // Для второй строки подключения убрать свойство UserInstance
            conn = new SqlConnection();
            conn.ConnectionString = cnStrBuilder.ConnectionString;
            conn.Open();
            // Вывод различных сведений о текущем объекте подключения
            richTextBox1.AppendText("Информация о подключении" + "\n");
            richTextBox1.AppendText("Местоположение базы данных - " + conn.DataSource + "\n");
            richTextBox1.AppendText("Имя базы данных - " + conn.Database + "\n");
            richTextBox1.AppendText("Тайм-аут -" + conn.ConnectionTimeout + "\n");
            richTextBox1.AppendText("Состояние подключения - " + conn.State.ToString() + "\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            Form2 f = new Form2();
            f.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            Form3 f = new Form3();
            f.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Hide();
            Form4 f1 = new Form4();
            f1.ShowDialog();
        }
    }
}
