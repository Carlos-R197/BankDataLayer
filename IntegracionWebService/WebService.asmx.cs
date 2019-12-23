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

        [WebMethod]
        public Cuenta ObtenerCuenta(int numeroCuenta)
        {
            return core.ObtenerCuenta(numeroCuenta);
        }

        [WebMethod]
        public void RealizarTransaccion(int numeroCuentaRetiro, int numeroCuentaDeposito, decimal monto, TipoTransaccion tipoTransaccion)
        {
            if (tipoTransaccion == TipoTransaccion.Interbancaria)
            {
                //El monto aqui debe ser negatio ya que estamos retirando del balance de la cuenta
                core.ActualizarCuenta(numeroCuentaRetiro, -monto, localhost.TipoTransaccion.Transaccion);
                
                core.ActualizarCuenta(numeroCuentaDeposito, monto, localhost.TipoTransaccion.Transaccion);
            }
        }

        [WebMethod]
        public Cuenta DepositarMonto(Cuenta cuenta, decimal monto)
        {
            cuenta.balanceDisponible += monto;
            core.ActualizarCuenta(cuenta.numeroCuenta, cuenta.balanceDisponible, localhost.TipoTransaccion.Deposito);
            return cuenta;
        }

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

        [WebMethod]
        public Cajero ValidarCajero(string usuario, string contraseña)
        {
            return core.ObtenerCajero(usuario, contraseña);
        }

    }
}
