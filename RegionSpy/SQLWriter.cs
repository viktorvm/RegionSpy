using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace RegionSpy
{
    public class NothingWrittenException : Exception
    {
        public override string Message
        {
            get
            {
                return "Был вызван метод записи в базу SQL, но не внесено ни одного изменения. Проверьте передаваемые аргументы.";
            }
        }

        //Стандартные конструкторы
        public NothingWrittenException() : base() { }
        public NothingWrittenException(string message) : base(message) { }
        public NothingWrittenException(string message, Exception innerException) : base(message, innerException) { }
    }

    class SQLWriter
    {
        private string _servAddress;
        private string _sqlName;
        private string _dbName;
        private string _conString
        {
            get
            {
                return String.Format("Data Source={0}\\{1};Integrated Security=SSPI", _servAddress, _sqlName);
            }
        }

        public SQLWriter(string servAddress, string sqlName, string dbName)
        {
            _servAddress = servAddress;
            _sqlName = sqlName;
            _dbName = dbName;
        }

        public string ServAddress
        {
            get { return _servAddress; }
        }
        public string SqlName
        {
            get { return _sqlName; }
        }
        public string DBName
        {
            get { return _dbName; }
        }

        /// <summary>
        /// Прописывает все теги в базу UDM
        /// </summary>
        public void UpdateUDMItems(PumpSU SU)
        {
            using (SqlConnection con = new SqlConnection(_conString + ";Initial Catalog=" + _dbName))
            {
                int rowsInserted = 0;
                try
                {
                    con.Open();
                }
                catch (Exception ex)
                {
                    throw new Exception("Ошибка подключения к серверу БД: " + ex.Message);
                }
                foreach (string TKey in SU.PValues.Keys)
                {
                    rowsInserted += WriteToUDM(con, SU.Kust, SU.Well, TKey, SU.PValues[TKey].ToString());
                }
                con.Close();

                if (rowsInserted == 0)
                    throw new NothingWrittenException();
            }
        }

        /// <summary>
        /// Прописывает тег в базу UDM
        /// </summary>
        /// <param name="con">активное подключение SQLConnection</param>
        /// <param name="kNum">номер куста</param>
        /// <param name="wNum">номер скважины</param>
        /// <param name="param">имя параметра</param>
        /// <param name="value">значение параметра</param>
        private int WriteToUDM(SqlConnection con, int kNum, int wNum, string param, string value)
        {
            try
            {
                const string comText =
                    "UPDATE [dbo].[DMG_ExpressionItems] SET [ReadExpression] = @Value WHERE [ID] = @ID";
                SqlCommand command = new SqlCommand(comText, con);
                command.Parameters.Add(new SqlParameter("Value", SqlDbType.VarChar));
                command.Parameters.Add(new SqlParameter("ID", SqlDbType.Int));

                string id =
                    GetUDMItemId(con,
                        GetUDMParId(con,
                            GetUDMParId(con,
                                GetUDMParId(con, null, "k_" + kNum, 1),
                            "well_" + wNum, 1),
                        param, 1),
                    "Val", 1);

                if (id == null)
                    return 0;
                command.Parameters["Value"].Value = value.Replace(",", ".");
                command.Parameters["ID"].Value = id;

                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось обновить базу! \n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        /// <summary>
        /// Получает значение ID заданной папки
        /// </summary>
        /// <param name="con">активное подключение SQLConnection</param>
        /// <param name="recParId">значение столбца RecursiveParentID</param>
        /// <param name="name">значение столбца Name</param>
        /// <param name="table">0-DMG_RegisterFolders; 1-DMG_ExpressionFolders</param>
        /// <returns>ID в формате string, null если параметр не найден или в случае ошибки выполнения</returns>
        private string GetUDMParId(SqlConnection con, string recParId, string name, int table)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand selectCommand =
                    table == 0
                        ? new SqlCommand("SELECT [ID],[RecursiveParentID],[Name] FROM [dbo].[DMG_RegisterFolders]", con)
                        : table == 1
                            ? new SqlCommand("SELECT [ID],[RecursiveParentID],[Name] FROM [dbo].[DMG_ExpressionFolders]",
                                con)
                            : null;
                SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                adapter.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Name"].ToString() != name) continue;
                    if (dt.Rows[i]["RecursiveParentID"].ToString() ==
                        (string.IsNullOrEmpty(recParId) ? string.Empty : recParId))
                        return dt.Rows[i]["ID"].ToString();
                }
                return null;
            }
            catch (Exception)
            {
                //MessageBox.Show(string.Format(
                //    "Метод: GetParId(SqlConnection con, string recParId, string name, int table) \n\nТекст ошибки: {0} \n\nИмя параметра: {1}",
                //    ex.Message, name), "Не удалось создать SqlCommand");
                return null;
            }
        }

        /// <summary>
        /// Получает значение ID указанного параметра (из базы UDM)
        /// </summary>
        /// <param name="con">активное подключение SQLConnection</param>
        /// <param name="parId">значение столбца ParentID</param>
        /// <param name="name">значение столбца Name</param>
        /// <param name="table">0-DMG_RegisterItems; 1-DMG_ExpressionItems</param>
        /// <returns>ID в формате string, null если параметр не найден и в случае ошибки выполнения</returns>
        private string GetUDMItemId(SqlConnection con, string parId, string name, int table)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand selectCommand =
                    table == 0
                        ? new SqlCommand("SELECT [ID],[ParentID],[Name] FROM [dbo].[DMG_RegisterItems]", con)
                        : table == 1
                            ? new SqlCommand("SELECT [ID],[ParentID],[Name] FROM [dbo].[DMG_ExpressionItems]",
                                con)
                            : null;
                SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                adapter.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Name"].ToString() != name) continue;
                    if (dt.Rows[i]["ParentID"].ToString() ==
                        (string.IsNullOrEmpty(parId) ? string.Empty : parId))
                        return dt.Rows[i]["ID"].ToString();
                }
                return null;
            }
            catch (Exception)
            {
                //MessageBox.Show(string.Format(
                //    "Метод: GetUDMItemId(SqlConnection con, string parId, string name, int table) \n\nТекст ошибки: {0} \n\nИмя параметра: {1}",
                //    ex.Message, name), "Не удалось получить ID из таблицы dbo.DMG_RegisterItems\\dbo.DMG_ExpressionItems");
                return null;
            }
        }
    }
}

