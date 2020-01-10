using System;
using Integracion.net.azurewebsites.integracionwebservice201912344;

namespace Integracion
{
    class Program
    {
        static void Main(string[] args)
        {
            var integracion = new WebService();
            //var cuenta = integracion.ObtenerCuenta(55555);
            //cuenta = integracion.DepositarMonto(cuenta, 20000);

            var date = new DateTime(2020, 1, 1);
            var endDate = new DateTime(2020, 1, 11);

            Transaccion[] transacciones = integracion.ObtenerTodasTransaccionesRango(date, endDate);
            foreach (var transaccion in transacciones)
                Console.WriteLine("{0}", transaccion);
            Console.WriteLine(transacciones.Length);

            Console.ReadLine();
        }
    }
}
