using System;
using System.Web.Services;
using IntegracionWebService.localhost;
using log4net;

namespace IntegracionWebService
{
    public enum TipoTransaccion
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
        private readonly CoreServices core = new CoreServices();
        private readonly ILog logger = LogManager.GetLogger(System.Environment.MachineName);

        /// <summary>
        /// Busca una cuenta existente en la database a partir de su numero. Si no existe retorna null.
        /// </summary>
        [WebMethod]
        public Cuenta ObtenerCuenta(int numeroCuenta)
        {
            return core.ObtenerCuenta(numeroCuenta);
        }

        /// <summary>
        /// Realiza una transaccion interbancaria a partir del numero de la cuenta de la que se sacara el monto,
        /// el numero de cuenta en la cual se depositara y el monto
        /// </summary>
        [WebMethod]
        public void RealizarTransaccion(int numeroCuentaRetiro, int numeroCuentaDeposito, decimal monto, TipoTransaccion tipoTransaccion)
        {
            if (tipoTransaccion == TipoTransaccion.Interbancaria)
            {
                //El monto aqui debe ser negativo ya que estamos retirando del balance de la cuenta
                core.ActualizarCuenta(numeroCuentaRetiro, -monto, localhost.TipoTransaccion.Transaccion);
                
                core.ActualizarCuenta(numeroCuentaDeposito, monto, localhost.TipoTransaccion.Transaccion);
            }
        }

        /// <summary>
        /// Deposita un monto en una cuenta existente y retorna un objeto de tipo cuenta con su balance actualizado.
        /// </summary>
        [WebMethod]
        public Cuenta DepositarMonto(Cuenta cuenta, decimal monto)
        {
            cuenta.balanceDisponible += monto;
            core.ActualizarCuenta(cuenta.numeroCuenta, cuenta.balanceDisponible, localhost.TipoTransaccion.Deposito);
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
                cuenta.balanceDisponible -= monto;
                core.ActualizarCuenta(cuenta.numeroCuenta, cuenta.balanceDisponible, localhost.TipoTransaccion.Retiro);
                return cuenta;
            }
            else
                throw new ArgumentException("La cuenta no tiene balance suficiente disponible para realizar esta transaccion");
        }

        /// <summary>
        /// Busca un cajero existente en la database a partir de su usuario y contraseña. Si no existe retorna null
        /// </summary>
        [WebMethod]
        public Cajero ValidarCajero(string usuario, string contraseña)
        {
            return core.ObtenerCajero(usuario, contraseña);
        }

        /// <summary>
        /// Paga un monto especifico de un prestamo existente
        /// </summary>
        [WebMethod]
        public void PagarPrestamo(Prestamo prestamo, decimal montoAPagar)
        {
            core.ActulizarPrestamo(prestamo.id, prestamo.montoPendientePorPagar, montoAPagar);
        }

    }
}
