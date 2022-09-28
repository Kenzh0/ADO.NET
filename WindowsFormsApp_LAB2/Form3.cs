using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
// Подключаемые пользовательские пространства имен
using dtQuestionsDAL;
using System.Configuration;

namespace WindowsFormsApp_LAB2
{
    public partial class Form3 : Form
    {
        public dbTestConnDAL qstDAL;
        public Form3()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // Получение строки подключения из App.config
            string cnStr = ConfigurationManager.ConnectionStrings["dbTestSglProvider"].ConnectionString;
            // Создание объекта dtQuestionsDAL
            qstDAL = new dbTestConnDAL();
            qstDAL.OpenConnection(cnStr);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = qstDAL.GetAlldtQuestionsAsDataTable();
            dataGridView1.DataSource = dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int i;
            i = Convert.ToInt32(textBox1.Text);
            textBox2.Clear();
            textBox2.AppendText(qstDAL.LookUpQuestion(i));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int i;
            i = Convert.ToInt32(textBox1.Text);
            try
            {
                qstDAL.DeleteQuestion(i);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Передача информации в библиотеку доступа к данным
            NewQuest qst = new NewQuest
            {
                dcQuestID = Convert.ToInt32(textBox1.Text),
                dcQuestion = textBox2.Lines[0],
                dcQuestType = Convert.ToInt32(textBox3.Text),
                dcQuestTime = Convert.ToInt32(textBox4.Text)
            };
            qstDAL.InsertQuestion(qst);
        }
    }
}
