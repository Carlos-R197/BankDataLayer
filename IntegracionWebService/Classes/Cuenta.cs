using System.Data;
using System;
using Integracion.Sql;

namespace Integracion.DBClasses
{
    public class Cuenta
    {
        public int numeroCuenta;
        public string clienteCedula;
        public decimal balanceDisponible;
        public string nombres;
        public string apellidos;
        public Prestamo[] prestamos;
        public Transaccion[] transacciones;

        public static void InsertarCuenta(int numeroCuenta, string clienteCedula, string nombres, string apellidos)
        {
            var data = new StoredProcedureData()
            {
                nombres = "InsertarCuenta",
                nombresParametros = new string[] { "@NumeroCuenta", "@ClienteCedula", "@Nombres", "@Apellidos" },
                valoresParametros = new object[] { numeroCuenta, clienteCedula, nombres, apellidos}
            };
            SqlWrapper.EjecutaEscribirStoredProcedure(data);
        }

        public static void ActualizarCuenta(int numeroCuenta, decimal montoPorAñadir, TipoTransaccion tipoTransaccion)
        {
            var data = new StoredProcedureData()
            {
                nombres = "ActualizarCuenta",
                nombresParametros = new string[] { "@NumeroCuenta", "@MontoPorAñadir" },
                valoresParametros = new object[] { numeroCuenta, montoPorAñadir }
            };
            SqlWrapper.EjecutaEscribirStoredProcedure(data);
            Transaccion.InsertarTransaccion(numeroCuenta, tipoTransaccion, montoPorAñadir);
        }

        public static Cuenta ObtenerCuenta(int numeroCuenta)
        {
            var data = new StoredProcedureData()
            {
                nombres = "ObtenerCuenta",
                nombresParametros = new string[] { "@NumeroCuenta" },
                valoresParametros = new object[] { numeroCuenta }
            };
            DataRow cuentaTableRow = SqlWrapper.EjecutaLeerUnoStoredProcedure(data);
            if (cuentaTableRow != null)
                return Cuenta.ArmarCuenta(cuentaTableRow, numeroCuenta);
            else
                return null;
        }

        private static Cuenta ArmarCuenta(DataRow row, int numeroCuenta)
        {
            var cuenta = new Cuenta()
            {
                numeroCuenta = Int32.Parse(row[0].ToString()),
                clienteCedula = row[1].ToString(),
                balanceDisponible = Int32.Parse(row[2].ToString()),
                nombres = row[3].ToString(),
                apellidos = row[4].ToString(),
                prestamos = Prestamo.ObtenerTodosPrestamos(numeroCuenta),
                transacciones = Transaccion.ObtenerTodasTransacciones(numeroCuenta)
            };

            return cuenta;
        }
    }
}