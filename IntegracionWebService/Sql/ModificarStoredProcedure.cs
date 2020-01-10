using System;
using System.Data;
using System.Data.SqlClient;
using Integracion.DBClasses;

namespace Integracion.Sql
{
    public class ModificarStoredProcedure : StoredProcedure
    {
        public void Ejecutar()
        {
            using (var connection = new SqlConnection(Globals.connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (var command = new SqlCommand(nombre, connection, transaction))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        for (int i = 0; i < nombresParametros.Length; i++)
                            command.Parameters.AddWithValue(nombresParametros[i], valoresParametros[i]);

                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Globals.log.Error("Exception was thrown " + e.Message);
                }
            }
        }
    }
}