using System;
using System.Web.Services;
using Integracion.DBClasses;
using log4net;
using System.Linq;

namespace IntegracionWebService
{
    public enum TipoTransaccionesBancarias
    {
        Interbancaria,
    }

    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://intec.edu.do")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {
        private readonly ILog logger = LogManager.GetLogger(System.Environment.MachineName);
        private readonly bool estaCoreAbajo = true;

        /// <summary>
        /// Busca una cuenta existente en la database a partir de su numero. Si no existe retorna null.
        /// </summary>
        [WebMethod]
        public Cuenta ObtenerCuenta(int numeroCuenta)
        {
            if (estaCoreAbajo)
                return Cuenta.ObtenerCuenta(numeroCuenta);
            else
                return null;//TODO
        }

        /// <summary>
        /// Realiza una transaccion interbancaria a partir del numero de la cuenta de la que se sacara el monto,
        /// el numero de cuenta en la cual se depositara y el monto
        /// </summary>
        [WebMethod]
        public void RealizarTransaccion(int numeroCuentaRetiro, int numeroCuentaDeposito, decimal monto, TipoTransaccionesBancarias tipoTransaccion)
        {
            if (tipoTransaccion == TipoTransaccionesBancarias.Interbancaria)
            {
                if (estaCoreAbajo)
                {
                    Cuenta.ActualizarCuenta(numeroCuentaRetiro, -monto, TipoTransaccion.Transaccion);
                    Cuenta.ActualizarCuenta(numeroCuentaDeposito, monto, TipoTransaccion.Transaccion);
                }
                else
                {
                    //TODO
                }
            }
        }

        /// <summary>
        /// Deposita un monto en una cuenta existente y retorna un objeto de tipo cuenta 
        /// con su balance actualizado y transacciones actualizadas
        /// </summary>
        [WebMethod]
        public Cuenta DepositarMonto(Cuenta cuenta, decimal monto)
        {
            if (estaCoreAbajo)
            {
                cuenta.balanceDisponible += monto;
                Cuenta.ActualizarCuenta(cuenta.numeroCuenta, cuenta.balanceDisponible, TipoTransaccion.Deposito);
                cuenta.transacciones = Transaccion.ObtenerTodasTransacciones(cuenta.numeroCuenta);
            }
            else
            {
                //TODO
            }

            return cuenta;
        }

        /// <summary>
        /// Retirar un monto de una cuenta existente y retorna un objeto de tipo cuenta con su balance actualizado. Si
        /// la cuenta no tiene suficiente balance disponible para retirar el monto tira una newArgumentException.
        /// </summary>
        [WebMethod]
        public Cuenta RetirarMonto(Cuenta cuenta, decimal monto)
        {
            if (cuenta.balanceDisponible >= monto)
            {
                if (estaCoreAbajo)
                {
                    cuenta.balanceDisponible -= monto;
                    Cuenta.ActualizarCuenta(cuenta.numeroCuenta, cuenta.balanceDisponible, TipoTransaccion.Retiro);
                    cuenta.transacciones = Transaccion.ObtenerTodasTransacciones(cuenta.numeroCuenta);
                }
                else
                {
                    //TODO
                }
            }
            else
                throw new ArgumentException("La cuenta no tiene balance suficiente disponible para realizar esta transaccion");

            return cuenta;
        }

        /// <summary>
        /// Busca un cajero existente en la database a partir de su usuario y contraseña. Si no existe retorna null
        /// </summary>
        [WebMethod]
        public Cajero ValidarCajero(string usuario, string contraseña)
        {
            return Cajero.ObtenerCajero(usuario, contraseña);
        }

        /// <summary>
        /// Paga un monto especifico de un prestamo existente
        /// </summary>
        [WebMethod]
        public Cuenta PagarPrestamo(Cuenta cuenta, Prestamo prestamo, decimal monto)
        {
            if (estaCoreAbajo)
            {
                if (monto >= prestamo.montoPendientePorPagar)
                {
                    Prestamo.EliminarPrestamo(prestamo.id);
                    cuenta.prestamos = cuenta.prestamos.Where(t => t.id != prestamo.id).ToArray();
                }
                else
                {
                    Prestamo.ActualizarPrestamo(prestamo.id, monto);
                    Prestamo prestamoActual = cuenta.prestamos.First(t => t.id == prestamo.id);
                    prestamoActual.montoPendientePorPagar -= monto;
                }
            }
            else
            {
                //TODO
            }

            return cuenta;
        }

        /// <summary>
        /// Retorna todas las trancciones ocurridas en una fecha especifica sin tener en cuenta la hora
        /// </summary>
        [WebMethod]
        public Transaccion[] ObtenerTodasTransaccionesDelDia(DateTime fecha)
        {
            Transaccion[] transacciones = new Transaccion[0];
            if (estaCoreAbajo)
                transacciones = Transaccion.ObtenerTodasTransaccionesDelDia(fecha);
            else
            {
                //TODO
            }

            return transacciones;
        }

    }
}
