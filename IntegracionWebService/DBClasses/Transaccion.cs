using System;
using System.Data;
using Integracion.Sql;

namespace Integracion.DBClasses
{
    public enum TipoTransaccion
    {
        Deposito,
        Retiro,
        Transaccion
    }

    public class Transaccion
    {
        public int id;
        public int numeroCuenta;
        public string tipoTransaccion;
        public decimal monto;
        public DateTime fecha;

        public override string ToString()
        {
            return string.Format("Id: {0}, Numero de Cuenta: {1}, Tipo de transaccion: {2}, Monto: {3}, Fecha: {4}",
                id, numeroCuenta, tipoTransaccion, monto, fecha);
        }

        public static void InsertarTransaccion(int numeroCuenta, TipoTransaccion tipoTransaccion, decimal monto)
        {
            string tipoDeTransaccion = string.Empty;
            if (tipoTransaccion == TipoTransaccion.Deposito)
                tipoDeTransaccion = "Deposito";
            else if (tipoTransaccion == TipoTransaccion.Retiro)
                tipoDeTransaccion = "Retiro";
            else if (tipoTransaccion == TipoTransaccion.Transaccion)
                tipoDeTransaccion = "Transaccion";

            var storedProcedure = new ModificarStoredProcedure()
            {
                nombre = "InsertarTransaccion",
                nombresParametros = new string[] { "@NumeroCuenta", "@TipoTransaccion", "@Monto", "@Fecha" },
                valoresParametros = new object[] { numeroCuenta, tipoDeTransaccion, monto, DateTime.Now }
            };
            storedProcedure.Ejecutar();
        }

        public static Transaccion[] ObtenerTodasTransacciones(int numeroCuenta)
        {
            var storedProcedure = new LeerTodoStoredProcedure()
            {
                nombre = "ObtenerTodasTransacciones",
                nombresParametros = new string[] { "@NumeroCuenta" },
                valoresParametros = new object[] { numeroCuenta }
            };

            return ObtenerTransacciones(storedProcedure);
        }

        public static Transaccion[] ObtenerTodasTransaccionesDelDia(DateTime fecha)
        {
            var storedProcedure = new LeerTodoStoredProcedure()
            {
                nombre = "ObtenerTransaccionesDia",
                nombresParametros = new string[] { "@Date" },
                valoresParametros = new object[] { fecha.Date }
            };
            return ObtenerTransacciones(storedProcedure);
        }

        public static Transaccion[] ObtenerTodasTransaccionesRango(DateTime fechaComienzo, DateTime fechaFinal)
        {
            var storedProcedure = new LeerTodoStoredProcedure()
            {
                nombre = "ObtenerTransaccionesFechaRango",
                nombresParametros = new string[] { "@FechaComienzo", "@FechaFinal" },
                valoresParametros = new object[] { fechaComienzo, fechaFinal }
            };
            return ObtenerTransacciones(storedProcedure);
        }

        private static Transaccion[] ObtenerTransacciones(LeerTodoStoredProcedure storedProcedure)
        {
            DataTable table = storedProcedure.Ejecutar();
            var transacciones = new Transaccion[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
                transacciones[i] = ArmarTransaccion(table.Rows[i]);
            return transacciones;
        }

        private static Transaccion ArmarTransaccion(DataRow row)
        {
            var transaccion = new Transaccion();
            transaccion.id = int.Parse(row[0].ToString());
            transaccion.numeroCuenta = int.Parse(row[1].ToString());
            transaccion.tipoTransaccion = row[2].ToString();
            transaccion.fecha = DateTime.Parse(row[3].ToString());
            transaccion.monto = decimal.Parse(row[4].ToString());
            return transaccion;
        }
    }
}