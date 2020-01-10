using System.Data;
using Integracion.Sql;

namespace Integracion.DBClasses
{
    public class Cliente
    {
        public string cedula;
        public string nombres;
        public string apellidos;

        public override string ToString()
        {
            return string.Format("Matricula: {0} Nombres: {1}, Apellidos: {2}", cedula, nombres, apellidos);
        }

        public bool EsValido()
        {
            return cedula.Length == 11 && nombres.Length <= 50 && apellidos.Length <= 50;
        }

        public static void InsertarCliente(Cliente cliente)
        {
            var storedProcedure = new ModificarStoredProcedure()
            {
                nombre = "InsertarCliente",
                nombresParametros = new string[] { "@Cedula", "@Nombres", "@Apellidos" },
                valoresParametros = new object[] { cliente.cedula, cliente.nombres, cliente.apellidos }
            };
            storedProcedure.Ejecutar();
        }

        public static Cliente ObtenerCliente(string nombre, string apellido)
        {
            var storedProcedure = new LeerStoredProcedure()
            {
                nombre = "ObtenerCliente",
                nombresParametros = new string[] { "@Nombres", "@Apellidos" },
                valoresParametros = new object[] { nombre, apellido }
            };
            DataRow row = storedProcedure.Ejecutar();
            if (row != null)
                return ArmarCliente(row);
            else
                return null;
        }

        private static Cliente ArmarCliente(DataRow row)
        {
            var cliente = new Cliente()
            {
                cedula = row[0].ToString(),
                nombres = row[1].ToString(),
                apellidos = row[2].ToString()
            };
            return cliente;
        }
    }
}