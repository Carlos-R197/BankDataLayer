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
                nombre = "InsertarCuenta",
                nombresParametros = new string[] { "@NumeroCuenta", "@ClienteCedula", "@Nombres", "@Apellidos" },
                valoresParametros = new object[] { numeroCuenta, clienteCedula, nombres, apellidos}
            };
            SqlWrapper.EjecutaEscribirStoredProcedure(data);
        }

        public static void ActualizarCuenta(int numeroCuenta, decimal montoPorAñadir, TipoTransaccion tipoTransaccion)
        {
            var data = new StoredProcedureData()
            {
                nombre = "ActualizarCuenta",
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
                nombre = "ObtenerCuenta",
                nombresParametros = new string[] { "@NumeroCuenta" },
                valoresParametros = new object[] { numeroCuenta }
            };
            DataRow row = SqlWrapper.EjecutaLeerUnoStoredProcedure(data);
            if (row != null)
                return Cuenta.ArmarCuenta(row);
            else
                return null;
        }

        public static Cuenta[] ObtenerCuentas(string cedula)
        {
            var data = new StoredProcedureData()
            {
                nombre = "ObtenerCuentas",
                nombresParametros = new string[] { "@Cedula" },
                valoresParametros = new object[] { cedula }
            };
            DataTable table = SqlWrapper.EjecutaLeerTodoStoredProcedure(data);
            var cuentas = new Cuenta[table.Rows.Count];
            for (int i = 0; i < cuentas.Length; i++)
                cuentas[i] = ArmarCuenta(table.Rows[i]);

            return cuentas;
        }

        private static Cuenta ArmarCuenta(DataRow row)
        {
            var cuenta = new Cuenta()
            {
                numeroCuenta = Int32.Parse(row[0].ToString()),
                clienteCedula = row[1].ToString(),
                balanceDisponible = Int32.Parse(row[2].ToString()),
                nombres = row[3].ToString(),
                apellidos = row[4].ToString()
            };
            cuenta.prestamos = Prestamo.ObtenerTodosPrestamos(cuenta.numeroCuenta);
            cuenta.transacciones = Transaccion.ObtenerTodasTransacciones(cuenta.numeroCuenta);

            return cuenta;
        }
    }
}