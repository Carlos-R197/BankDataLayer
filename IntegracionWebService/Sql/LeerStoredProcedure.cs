using System.Data;
using System.Data.SqlClient;
using Integracion.DBClasses;

namespace Integracion.Sql
{
    public class LeerStoredProcedure : StoredProcedure
    {
        public DataRow Ejecutar()
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
                        if (table.Rows.Count > 0)
                            return table.Rows[0];
                        else
                            return null;
                    }
                }
            }
        }
    }
}