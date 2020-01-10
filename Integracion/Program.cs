using System;
using Integracion.net.azurewebsites.integracionwebservice201912344;

namespace Integracion
{
    class Program
    {
        static void Main(string[] args)
        {
            var integracion = new WebService();

            var cuenta = integracion.ObtenerCuenta(55555);
            Console.WriteLine("Nombre de cuenta: {0}", cuenta.nombres);

            Console.ReadLine();
        }
    }
}
