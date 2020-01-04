using System;
using System.Data;
using Integracion.Sql;

namespace Integracion.DBClasses
{
        public class Cajero
        {
            public int id;
            public string usuario;
            public string contraseña;
            public string nombres;

            public static Cajero ArmarCajero(DataRow row)
            {
                var cajero = new Cajero()
                {
                    id = Int32.Parse(row[0].ToString()),
                    usuario = row[1].ToString(),
                    contraseña = row[2].ToString(),
                    nombres = row[3].ToString()
                };
                return cajero;
            }

            public static Cajero ObtenerCajero(string usuario, string contraseña)
            {
                var data = new StoredProcedureData()
                {
                    nombres = "ConsultarCajero",
                    nombresParametros = new string[] { "@Usuario", "@Contraseña" },
                    valoresParametros = new object[] { usuario, contraseña }
                };
                DataRow cajeroTablaRow = SqlWrapper.EjecutaLeerUnoStoredProcedure(data);
                if (cajeroTablaRow != null)
                    return Cajero.ArmarCajero(cajeroTablaRow);
                else
                    return null;
            }
        }
}