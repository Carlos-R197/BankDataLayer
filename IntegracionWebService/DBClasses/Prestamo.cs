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

        public override string ToString()
        {
            return string.Format("Id: {0}, Numero de Cuenta: {1}, Monto: {2}", id, numeroCuenta, montoPendientePorPagar);
        }
        
        public static void InsertarPrestamo(int numeroCuenta, decimal montoPrestamo)
        {
            var storedProcedure = new ModificarStoredProcedure()
            {
                nombre = "InsertarPrestamo",
                nombresParametros = new string[] { "@NumeroCuenta", "@Monto" },
                valoresParametros = new object[] { numeroCuenta, montoPrestamo }
            };
            storedProcedure.Ejecutar();
        }

        public static void ActualizarPrestamo(int idPrestamo, decimal montoAPagar)
        {
            var storedProcedure = new ModificarStoredProcedure()
            {
                nombre = "ActualizarPrestamo",
                nombresParametros = new string[] { "@Id", "@MontoAPagar" },
                valoresParametros = new object[] { idPrestamo, montoAPagar }
            };
            storedProcedure.Ejecutar();
        }

        public static void EliminarPrestamo(int idPrestamo)
        {
            var storedProcedure = new ModificarStoredProcedure()
            {
                nombre = "EliminarPrestamo",
                nombresParametros = new string[] { "@Id" },
                valoresParametros = new object[] { idPrestamo }
            };
            storedProcedure.Ejecutar();
        }

        public static Prestamo[] ObtenerTodosPrestamos(int numeroCuenta)
        {
            var storedProcedure = new LeerTodoStoredProcedure()
            {
                nombre = "ObtenerPrestamos",
                nombresParametros = new string[] { "@NumeroCuenta" },
                valoresParametros = new object[] { numeroCuenta }
            };
            DataTable table = storedProcedure.Ejecutar();
            Prestamo[] prestamos = new Prestamo[table.Rows.Count];
            for (int i = 0; i < prestamos.Length; i++)
            {
                Prestamo prestamo = Prestamo.ArmarPrestamo(table.Rows[i]);
                prestamos[i] = prestamo;
            }

            return prestamos;
        }

        private static Prestamo ArmarPrestamo(DataRow row)
        {
            var prestamo = new Prestamo()
            {
                id = Int32.Parse(row[0].ToString()),
                numeroCuenta = Int32.Parse(row[1].ToString()),
                montoPendientePorPagar = decimal.Parse(row[2].ToString())
            };
            return prestamo;
        }
    }
}