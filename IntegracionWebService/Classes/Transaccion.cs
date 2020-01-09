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

        public static void InsertarTransaccion(int numeroCuenta, TipoTransaccion tipoTransaccion, decimal monto)
        {
            string tipoDeTransaccion = string.Empty;
            if (tipoTransaccion == TipoTransaccion.Deposito)
                tipoDeTransaccion = "Deposito";
            else if (tipoTransaccion == TipoTransaccion.Retiro)
                tipoDeTransaccion = "Retiro";
            else if (tipoTransaccion == TipoTransaccion.Transaccion)
                tipoDeTransaccion = "Transaccion";

            var data = new StoredProcedureData()
            {
                nombre = "InsertarTransaccion",
                nombresParametros = new string[] { "@NumeroCuenta", "@TipoTransaccion", "@Monto" },
                valoresParametros = new object[] { numeroCuenta, tipoDeTransaccion, monto }
            };
            SqlWrapper.EjecutaEscribirStoredProcedure(data);
        }

        public static Transaccion[] ObtenerTodasTransacciones(int numeroCuenta)
        {
            var data = new StoredProcedureData()
            {
                nombre = "ObtenerTodasTransacciones",
                nombresParametros = new string[] { "@NumeroCuenta" },
                valoresParametros = new object[] { numeroCuenta }
            };

            return ObtenerTransacciones(data);
        }

        public static Transaccion[] ObtenerTodasTransaccionesDelDia(DateTime fecha)
        {
            var data = new StoredProcedureData()
            {
                nombre = "ObtenerTransaccionesDia",
                nombresParametros = new string[] { "@Date" },
                valoresParametros = new object[] { fecha.Date }
            };
            return ObtenerTransacciones(data);
        }

        public static Transaccion[] ObtenerTodasTransaccionesRango(DateTime fechaComienzo, DateTime fechaFinal)
        {
            var data = new StoredProcedureData()
            {
                nombre = "ObtenerTransaccionesFechaRango",
                nombresParametros = new string[] { "@FechaComienzo", "@FechaFinal" },
                valoresParametros = new object[] { fechaComienzo, fechaFinal }
            };
            return ObtenerTransacciones(data);
        }

        private static Transaccion[] ObtenerTransacciones(StoredProcedureData data)
        {
            DataTable table = SqlWrapper.EjecutaLeerTodoStoredProcedure(data);
            var transacciones = new Transaccion[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
                transacciones[i] = ArmarTransaccion(table.Rows[i]);
            return transacciones;
        }

        private static Transaccion ArmarTransaccion(DataRow row)
        {
            var transaccion = new Transaccion()
            {
                id = int.Parse(row[0].ToString()),
                numeroCuenta = int.Parse(row[1].ToString()),
                tipoTransaccion = row[2].ToString(),
                fecha = DateTime.Parse(row[3].ToString()),
                monto = decimal.Parse(row[4].ToString())
            };
            return transaccion;
        }
    }
}