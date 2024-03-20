using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FOOD_REVIEWS_INTERFACE
{

    class OracleDB
    {
        static OracleConnection connection;

        static public void connectToDataBase()
        {
            string connectionString = "User Id=bd143; Password=bd143; Data Source=bd-dc.cs.tuiasi.ro:1539/orcl; Connection Timeout=60;";
            connection = new OracleConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        static public bool runInsertCommand(string tableName, List<string> columns, List<string> values)
        {
            string columnsAsString = "";
            for(int i = 0; i < columns.Count; ++i)
            {
                columnsAsString += columns[i] + ", ";
            }
            columnsAsString = columnsAsString.Substring(0, columnsAsString.Length - 2);

            string valuesAsString = "";
            for (int i = 0; i < values.Count; ++i)
            {
                valuesAsString += values[i] + ", ";
            }
            valuesAsString = valuesAsString.Substring(0, valuesAsString.Length - 2);

            bool ok = true;
            string sqlQuery = $"INSERT INTO {tableName} ({columnsAsString}) VALUES ({valuesAsString})";
            try
            {
                using (OracleCommand command = new OracleCommand(sqlQuery, connection))
                {
                    command.CommandTimeout = 5;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ok = false;
                MessageBox.Show($"Error: {ex.Message}");
            }
            return ok;
        }

        static public bool runTransactionCommand(string tableName, List<string> columns, List<string> values)
        {
            string columnsAsString = "";
            for (int i = 0; i < columns.Count; ++i)
            {
                columnsAsString += columns[i] + ", ";
            }
            columnsAsString = columnsAsString.Substring(0, columnsAsString.Length - 2);

            string valuesAsString = "";
            for (int i = 0; i < values.Count; ++i)
            {
                valuesAsString += values[i] + ", ";
            }
            valuesAsString = valuesAsString.Substring(0, valuesAsString.Length - 2);

            bool ok = true;

            using (OracleTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    string sqlQuery = $"INSERT INTO {tableName} ({columnsAsString}) VALUES ({valuesAsString})";

                    using (OracleCommand command = new OracleCommand(sqlQuery, connection))
                    {
                        command.CommandTimeout = 5;
                        command.ExecuteNonQuery();
                    }

                    sqlQuery = $"UPDATE USERS U SET POINTS = POINTS + ((SELECT PRICE FROM RESTAURANT_DISHES WHERE ID_RESTAURANT_DISHES = {values[2]}) / 10) WHERE U.NAME = (SELECT NAME FROM USERS WHERE ID_USER = {values[1]})";
                    using (OracleCommand command = new OracleCommand(sqlQuery, connection))
                    {
                        command.CommandTimeout = 5;
                        command.ExecuteNonQuery();
                    }

                    // Commit the transaction if everything is successful
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // An error occurred, rollback the transaction
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);
                }
            }
            /*try
            {
                string sqlQuery = "BEGIN TRANSACTION";

                using (OracleCommand command = new OracleCommand(sqlQuery, connection))
                {
                    command.CommandTimeout = 5;
                    command.ExecuteNonQuery();
                }

                sqlQuery = $"INSERT INTO {tableName} ({columnsAsString}) VALUES ({valuesAsString})";

                using (OracleCommand command = new OracleCommand(sqlQuery, connection))
                {
                    command.CommandTimeout = 5;
                    command.ExecuteNonQuery();
                }

                sqlQuery = $"UPDATE USERS U SET POINTS = POINTS + (SELECT PRICE FROM RESTAURANT_DISHES WHERE ID_RESTAURANT_DISHES = {values[1]} / 10)";
                using (OracleCommand command = new OracleCommand(sqlQuery, connection))
                {
                    command.CommandTimeout = 5;
                    command.ExecuteNonQuery();
                }

                sqlQuery = "COMMIT";
                using (OracleCommand command = new OracleCommand(sqlQuery, connection))
                {
                    command.CommandTimeout = 5;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                string sqlQuery = "ROLLBACK";
                using (OracleCommand command = new OracleCommand(sqlQuery, connection))
                {
                    command.CommandTimeout = 5;
                    command.ExecuteNonQuery();
                }
                ok = false;
                MessageBox.Show($"Error: {ex.Message}");
            }*/
            return ok;
        }

        static public string[][] runSelectCommand(List<SelectClass> selectClasses, Control control)
        {
            return null;
            for(int i = 0; i < selectClasses.Count; ++i)
            {

            }
        }

        static public void getChoices(ComboBox comboBox, string tableName, string columnName)
        {
            string sqlQuery = $"SELECT {columnName} FROM {tableName}";
            try
            {
                using (OracleCommand command = new OracleCommand(sqlQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox.Items.Add(reader[0]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        static public void runDeleteCommand(string tableName, string column, string value)
        {
            if (column != null && column != "")
            {
                string sqlQuery = $"DELETE FROM {tableName} WHERE {column} = '{value}'";
                try
                {
                    using (OracleCommand command = new OracleCommand(sqlQuery, connection))
                    {
                        command.CommandTimeout = 5;
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MessageBox.Show(reader[0].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        static public void runEditCommand(string tableName, string whereColumn, string whereValue, List<string> changedColumns, List<string> changedValues)
        {
            if (changedColumns.Count != 0)
            {
                string sqlQuery = $"UPDATE {tableName} SET ";
                for (int i = 0; i < changedColumns.Count; ++i)
                {
                    sqlQuery += $"{changedColumns[i]} = '{changedValues[i]}', ";
                }
                sqlQuery = sqlQuery.Substring(0, sqlQuery.Length - 2);
                sqlQuery += $" WHERE {whereColumn} = '{whereValue}'";

                try
                {
                    using ( OracleCommand command = new OracleCommand(sqlQuery, connection))
                    {
                        command.CommandTimeout = 5;
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MessageBox.Show(reader[0].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        static public List<List<string>> runSelectAllCommand(string tableName)
        {
            List<List<string>> allData = new List<List<string>>();

            try
            {
                string sqlQuery = $"SELECT column_name FROM all_tab_columns WHERE table_name = '{tableName}'";
                using (OracleCommand command = new OracleCommand(sqlQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        List<string> columns = new List<string>();
                        while (reader.Read())
                        {
                            columns.Add(reader.GetString(0));
                        }
                        allData.Add(columns);
                    }
                }

                sqlQuery = $"SELECT * FROM {tableName}";
                using (OracleCommand command = new OracleCommand(sqlQuery, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            List<string> columns = new List<string>();
                            for (int i = 0; i < reader.FieldCount; ++i)
                            {
                                columns.Add(reader[i].ToString());
                            }
                            allData.Add(columns);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            return allData;
        }

        public static List<List<string>> runBigSelectCommand(DataGridView dg, bool []wheres, Control []controls)
        {
            List<List<string>> allData = new List<List<string>>();

            List<string> col = new List<string> { "Dish Name", "Food Category Name", "Restaurant Name", "Price", "Weight", "Score", "Average Score"};
            allData.Add(col);

            string sqlQuery = "SELECT D.NAME AS \"Dish Name\", FC.NAME AS \"Food Category\", R.NAME AS \"Restaurant\", RD.PRICE, RD.WEIGHT, RE.SCORE AS \"Score\", (SELECT AVG(RE2.SCORE) FROM REVIEWS RE2 JOIN RESTAURANT_DISHES RD2 ON re2.id_restaurant_dishes = rd2.id_restaurant_dishes WHERE RD.ID_RESTAURANT_DISHES = RD2.ID_RESTAURANT_DISHES ) AS \"Average Score\" FROM DISHES D JOIN FOOD_CATEGORIES FC ON d.id_food_category = fc.id_food_category JOIN RESTAURANT_DISHES RD ON d.id_dish = rd.id_dish JOIN RESTAURANTS R ON rd.id_restaurant = r.id_restaurant JOIN REVIEWS RE ON re.id_restaurant_dishes = rd.id_restaurant_dishes";

            bool ok = false;
            if (wheres[0])
            {
                if (!ok)
                    sqlQuery += " WHERE";
                ComboBox comboBox = (ComboBox)controls[0];
                sqlQuery += $" (FC.NAME = \'{comboBox.Text}\') AND";
                ok = true;
            }
            if (wheres[1])
            {
                if (!ok)
                    sqlQuery += " WHERE";
                ComboBox comboBox = (ComboBox)controls[1];
                sqlQuery += $" (D.NAME = \'{comboBox.Text}\') AND";
                ok = true;
            }
            if (wheres[2])
            {
                if (!ok)
                    sqlQuery += " WHERE";
                NumericUpDown numericUpDown = (NumericUpDown)controls[2];
                sqlQuery += $" (RD.PRICE <= {numericUpDown.Value}) AND";
                ok = true;
            }
            if (wheres[3])
            {
                if (!ok)
                    sqlQuery += " WHERE";
                NumericUpDown numericUpDown = (NumericUpDown)controls[3];
                ok = true;
                sqlQuery += $" (RD.WEIGHT <= {numericUpDown.Value}) AND";
            }
            if (wheres[4])
            {
                if (!ok)
                    sqlQuery += " WHERE";
                NumericUpDown numericUpDown = (NumericUpDown)controls[4];
                sqlQuery += $" ((SELECT AVG(RE2.SCORE) FROM REVIEWS RE2 JOIN RESTAURANT_DISHES RD2 ON re2.id_restaurant_dishes = rd2.id_restaurant_dishes WHERE RD.ID_RESTAURANT_DISHES = RD2.ID_RESTAURANT_DISHES) >= {numericUpDown.Value}) AND";
                ok = true;
            }

            if (ok)
                sqlQuery = sqlQuery.Substring(0, sqlQuery.Length - 3);

            using (OracleCommand command = new OracleCommand(sqlQuery, connection))
            {
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        List<string> columns = new List<string>();
                        for (int i = 0; i < reader.FieldCount; ++i)
                        {
                            columns.Add(reader[i].ToString());
                        }
                        allData.Add(columns);
                    }
                }
            }
            return allData;
        }
    }
}
