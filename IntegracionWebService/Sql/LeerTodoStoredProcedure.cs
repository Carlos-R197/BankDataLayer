using System.Data;
using System.Data.SqlClient;
using Integracion.DBClasses;

namespace Integracion.Sql
{
    public class LeerTodoStoredProcedure : StoredProcedure
    {
        public DataTable Ejecutar()
        {
            using (var connection = new SqlConnection(Globals.connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(nombre, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    for (int i = 0; i < nombresParametros.Length; i++)
                        command.Parameters.AddWithValue(nombresParametros[i], valoresParametros[i]);

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