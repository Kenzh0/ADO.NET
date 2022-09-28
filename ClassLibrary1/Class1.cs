using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace dtQuestionsDAL
{
    public class NewQuest
    {
        public int dcQuestID { get; set; }
        public string dcQuestion { get; set; }
        public int dcQuestType { get; set; }
        public int dcQuestTime { get; set; }
    }

    public class dbTestConnDAL
    {
        // Объект sqCn будет использоваться всеми методами далее
        public SqlConnection sqlCn = null;

        public void OpenConnection(string connectionString)
        {
            sqlCn = new SqlConnection();
            sqlCn.ConnectionString = connectionString;
            sqlCn.Open();
        }

        public void CloseConnection()
        {
            sqlCn.Close();
        }

        public void InsertQuestion(NewQuest Quest)
        {
            // Формирование и выполнение оператора SQL
            string sql = string.Format("Insert Into dtQuestions" +
            "(dсQuestID, dcQuestion, dcQuestType, dcQuestTime) Values" +
            "('{0}', '{1}', '{2}', '{3}')", Quest.dcQuestID, Quest.dcQuestion, Quest.dcQuestType, Quest.dcQuestTime);
            // Выполнение с помощью нашего подключения
            using (SqlCommand cmd = new SqlCommand(sql, this.sqlCn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteQuestion(int id)
        {
            // Получение номера вопроса перед его удалением
            string sql = string.Format("Delete from dtQuestions where dсQuestID = '{0}'", id);
            using (SqlCommand cmd = new SqlCommand(sql, this.sqlCn))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Вопрос с этим номером отсутствует!", ex);
                    throw error;
                }
            }
        }

        // Логика изменения
        public void UpdateQuestion(int id, string newQuestion)
        {
            // Получение номера вопроса и его формулировки
            string sql =
            string.Format("Update dtQuestions Set dcQuestion = '{O}' Where dсQuestID = '{1}' ", newQuestion, id);
            using (SqlCommand cmd = new SqlCommand(sql, this.sqlCn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        // Логика выборки двумя способами
        public List<NewQuest> GetAlldtQuestionsAsList()
        {
            // Здесь будут находиться записи
            List<NewQuest> qst = new List<NewQuest>();
            // Подготовка объекта команды
            string sql = "Select * From dtQuestions";
            using (SqlCommand cmd = new SqlCommand(sql, this.sqlCn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    qst.Add(new NewQuest
                    {
                        dcQuestID = (int)dr["dcQuestType"],
                        dcQuestion = (string)dr["dcQuestion"],
                        dcQuestType = (int)dr["dcQuestType"],
                        dcQuestTime = (int)dr["dcQuestTime"]
                    });
                }
                dr.Close();
            }
            return qst;
        }

        public DataTable GetAlldtQuestionsAsDataTable()
        {
            // Здесь будут находиться записи
            DataTable qst = new DataTable();
            // Подготовка объекта команды
            string sql = "Select * From dtQuestions";
            using (SqlCommand cmd = new SqlCommand(sql, this.sqlCn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                // Заполнение DataTable данными из объекта чтения
                qst.Load(dr);
                dr.Close();
            }
            return qst;
        }

        public string LookUpQuestion(int id)
        {
            string qst = string.Empty;
            // Задание имени хранимой процедуры
            using (SqlCommand cmd = new SqlCommand("GetQuestion", this.sqlCn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // Входной параметр
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@QuestID";
                param.SqlDbType = SqlDbType.Int;
                param.Value = id;
                param.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(param);
                // Выходной параметр
                param = new SqlParameter();
                param.ParameterName = "@Question";
                param.SqlDbType = SqlDbType.Char;
                param.Size = 100;
                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);
                // Выполнение хранимой процедуры
                cmd.ExecuteNonQuery();
                // Возврат выходного параметра
                qst = ((string)cmd.Parameters["@Question"].Value).Trim();
            }
            return qst;
        }
    }
}
