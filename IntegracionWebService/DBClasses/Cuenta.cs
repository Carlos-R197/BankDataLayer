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

        public override string ToString()
        {
            if (this == null)
                return "null";

            return string.Format("Numero cuenta: {0}, Cedula: {1}, Balance: {2}, Nombre: {3}, Apellido: {4}",
                numeroCuenta, clienteCedula, balanceDisponible, nombres, apellidos);
        }

        public static void InsertarCuenta(string clienteCedula, string nombres, string apellidos)
        {
            Random random = new Random();
            int numeroCuenta = random.Next(100000, int.MaxValue);
            var storedProcedure = new ModificarStoredProcedure()
            {
                nombre = "InsertarCuenta",
                nombresParametros = new string[] { "@NumeroCuenta", "@ClienteCedula", "@Nombres", "@Apellidos" },
                valoresParametros = new object[] { numeroCuenta, clienteCedula, nombres, apellidos}
            };
            storedProcedure.Ejecutar();
        }

        public static void ActualizarCuenta(int numeroCuenta, decimal nuevoMonto, decimal monto, TipoTransaccion tipoTransaccion)
        {
            var storedProcedure = new ModificarStoredProcedure()
            {
                nombre = "ActualizarCuenta",
                nombresParametros = new string[] { "@NumeroCuenta", "@NuevoMonto" },
                valoresParametros = new object[] { numeroCuenta, nuevoMonto }
            };
            storedProcedure.Ejecutar();
            Transaccion.InsertarTransaccion(numeroCuenta, tipoTransaccion, monto);
        }

        public static Cuenta ObtenerCuenta(int numeroCuenta)
        {
            var storedProcedure = new LeerStoredProcedure()
            {
                nombre = "ObtenerCuenta",
                nombresParametros = new string[] { "@NumeroCuenta" },
                valoresParametros = new object[] { numeroCuenta }
            };
            DataRow row = storedProcedure.Ejecutar();
            if (row != null)
                return Cuenta.ArmarCuenta(row);
            else
                return null;
        }

        public static Cuenta[] ObtenerCuentas(string cedula)
        {
            var storedProcedure = new LeerTodoStoredProcedure()
            {
                nombre = "ObtenerCuentas",
                nombresParametros = new string[] { "@Cedula" },
                valoresParametros = new object[] { cedula }
            };
            DataTable table = storedProcedure.Ejecutar();
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