using System.Data.SqlClient;
using System.Data;
using System;
using log4net;

namespace Integracion.Sql
{
    public static class SqlWrapper
    {
        private static readonly string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\GitHub\Integracion\IntegracionWebService\App_Data\DB.mdf;Integrated Security=True";
        private static readonly ILog logger = LogManager.GetLogger(System.Environment.MachineName);

        public static void EjecutaEscribirStoredProcedure(StoredProcedureData data)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (var command = new SqlCommand(data.nombres, connection, transaction))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        for (int i = 0; i < data.nombresParametros.Length; i++)
                            command.Parameters.AddWithValue(data.nombresParametros[i], data.valoresParametros[i]);

                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                }
                catch(Exception e)
                {
                    transaction.Rollback();
                    logger.Error("Exception was thrown " + e.Message);
                }
            }
        }

        public static DataRow EjecutaLeerUnoStoredProcedure(StoredProcedureData data)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(data.nombres, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    for (int i = 0; i < data.nombresParametros.Length; i++)
                        command.Parameters.AddWithValue(data.nombresParametros[i], data.valoresParametros[i]);

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var table = new DataTable();
                        adapter.Fill(table);
                        if (table.Rows.Count > 0)
                            return table.Rows[0];
                        else
                            return null;
                    }
                }
            }
        }

        public static DataTable EjecutaLeerTodoStoredProcedure(StoredProcedureData data)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(data.nombres, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    for (int i = 0; i < data.nombresParametros.Length; i++)
                        command.Parameters.AddWithValue(data.nombresParametros[i], data.valoresParametros[i]);

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
        }
    }
}