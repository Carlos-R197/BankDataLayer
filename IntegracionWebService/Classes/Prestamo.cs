using System;
using System.Data;
using Integracion.Sql;

namespace Integracion.DBClasses
{
    public class Prestamo
    {
        public int id;
        public int numeroCuenta;
        public decimal montoPendientePorPagar;

        public static Prestamo ArmarPrestamo(DataRow row)
        {
            var prestamo = new Prestamo()
            {
                id = Int32.Parse(row[0].ToString()),
                numeroCuenta = Int32.Parse(row[1].ToString()),
                montoPendientePorPagar = decimal.Parse(row[2].ToString())
            };
            return prestamo;
        }
        
        public static void InsertarPrestamo(int numeroCuenta, decimal montoPrestamo)
        {
            var data = new StoredProcedureData()
            {
                nombres = "InsertarPrestamo",
                nombresParametros = new string[] { "@NumeroCuenta", "@Monto" },
                valoresParametros = new object[] { numeroCuenta, montoPrestamo }
            };
            SqlWrapper.EjecutaEscribirStoredProcedure(data);
        }

        public static void ActualizarPrestamo(int idPrestamo, decimal montoAPagar)
        {
            var data = new StoredProcedureData()
            {
                nombres = "ActualizarPrestamo",
                nombresParametros = new string[] { "@Id", "@MontoAPagar" },
                valoresParametros = new object[] { idPrestamo, montoAPagar }
            };
            SqlWrapper.EjecutaEscribirStoredProcedure(data);
        }

        public static void EliminarPrestamo(int idPrestamo)
        {
            var data = new StoredProcedureData()
            {
                nombres = "EliminarPrestamo",
                nombresParametros = new string[] { "@Id" },
                valoresParametros = new object[] { idPrestamo }
            };
            SqlWrapper.EjecutaEscribirStoredProcedure(data);
        }

        public static Prestamo[] ObtenerTodosPrestamos(int numeroCuenta)
        {
            var data = new StoredProcedureData()
            {
                nombres = "ObtenerPrestamos",
                nombresParametros = new string[] { "@NumeroCuenta" },
                valoresParametros = new object[] { numeroCuenta }
            };
            DataTable table = SqlWrapper.EjecutaLeerTodoStoredProcedure(data);
            Prestamo[] prestamos = new Prestamo[table.Rows.Count];
            for (int i = 0; i < prestamos.Length; i++)
            {
                Prestamo prestamo = Prestamo.ArmarPrestamo(table.Rows[i]);
                prestamos[i] = prestamo;
            }

            return prestamos;
        }
    }
}