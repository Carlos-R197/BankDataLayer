using System.Data;
using Integracion.Sql;

namespace Integracion.DBClasses
{
    public class Cliente
    {
        public string matricula;
        public string nombres;
        public string apellidos;

        public static void InsertarCliente(string matricula, string nombres, string apellidos)
        {
            var data = new StoredProcedureData()
            {
                nombre = "InsertarCliente",
                nombresParametros = new string[] { "@Cedula", "@Nombres", "@Apellidos" },
                valoresParametros = new object[] { matricula, nombres, apellidos }
            };
            SqlWrapper.EjecutaEscribirStoredProcedure(data);
        }

        public static Cliente ObtenerCliente(string nombre, string apellido)
        {
            var data = new StoredProcedureData()
            {
                nombre = "ObtenerCliente",
                nombresParametros = new string[] { "@Nombres", "@Apellidos" },
                valoresParametros = new object[] { nombre, apellido }
            };
            DataRow row = SqlWrapper.EjecutaLeerUnoStoredProcedure(data);
            if (row != null)
                return ArmarCliente(row);
            else
                return null;
        }

        private static Cliente ArmarCliente(DataRow row)
        {
            var cliente = new Cliente()
            {
                matricula = row[0].ToString(),
                nombres = row[1].ToString(),
                apellidos = row[2].ToString()
            };
            return cliente;
        }
    }
}