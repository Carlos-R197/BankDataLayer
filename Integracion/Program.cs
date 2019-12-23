using System;
using Integracion.IntegracionReference;

namespace Integracion
{
    class Program
    {
        static void Main(string[] args)
        {
            WebServiceSoapClient integracion = new WebServiceSoapClient();
            Cuenta cuenta = integracion.ObtenerCuenta(55555);

            Console.WriteLine("Prestamos de esta cuenta: ");
            foreach(Prestamo prestamo in cuenta.prestamos)
            {
                Console.WriteLine("Monto que se debe: {0}", prestamo.montoPendientePorPagar);
            }

            Console.ReadLine();
        }
    }
}
