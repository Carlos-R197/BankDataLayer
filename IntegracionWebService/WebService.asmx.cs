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
        /// Inserta un nuevo cliente dentro de la database.
        /// </summary>
        [WebMethod]
        public void InsertarCliente(string matricula, string nombres, string apellidos)
        {
            if (estaCoreAbajo)
            {
                if (matricula.Length <= 12 && nombres.Length <= 50 && apellidos.Length <= 50)
                    Cliente.InsertarCliente(matricula, nombres, apellidos);
                else
                    throw new ArgumentException("Uno de los argumentos es demasiado largo. " +
                        "El largo de la matricula debe ser menor o igual a 12 y los nombres y apellidos menor o igual a 50");
            }
            else
            {
                //TODO.
            }
        }

        /// <summary>
        /// Busca un objeto de tipo cliente existente en la database y lo retorna. Si no existe retorna null.
        /// </summary>
        [WebMethod]
        public Cliente ObtenerCliente(string nombre, string apellido)
        {
            if (estaCoreAbajo)
            {
                if (nombre.Length <= 50 && apellido.Length <= 50)
                    return Cliente.ObtenerCliente(nombre, apellido);
                else
                    throw new ArgumentException("Uno de los argumentos es demasiado largo. " +
                        "El nombre y el apellido no pueden tener un largo de mas de 50");
            }
            else
            {
                //TODO.
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Insertar una nueva cuenta dentro de la database. 
        /// </summary>
        [WebMethod]
        public void InsertarCuenta(int numeroCuenta, string cedula, string nombres, string apellidos)
        {
            if (estaCoreAbajo)
            {
                if (cedula.Length <= 12 && nombres.Length <= 50 && apellidos.Length <= 50)
                    Cuenta.InsertarCuenta(numeroCuenta, cedula, nombres, apellidos);
                else
                    throw new ArgumentException("Uno de los argumentos es demasiado largo. " +
                       "El largo de la matricula debe ser menor o igual a 12 y los nombres y apellidos menor o igual a 50");
            }
            else
            {
                //TODO.
            }
        }

        /// <summary>
        /// Busca todas las cuentas existentes en base a una matricula. Si no existe ninguna cuenta retorna null.
        /// La cedula no puede tener un largo de mas de 12.
        /// </summary>
        [WebMethod]
        public Cuenta[] ObtenerCuentas(string cedula)
        {
            if (cedula.Length <= 12)
                return Cuenta.ObtenerCuentas(cedula);
            else
                throw new ArgumentException("La cedula no puede tener un largo de más de 12");
        }

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
        /// Retorna todas las trancciones ocurridas en una fecha especifica sin tener en cuenta la hora.
        /// Si no existe ninguna retorna un array vacio.
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

        /// <summary>
        /// Retorna todas las transacciones ocurridas entre dos fechas. Si no existe ninguna retorna un array vacio.
        /// </summary>
        [WebMethod]
        public Transaccion[] ObtenerTodasTransaccionesRango(DateTime fechaComienzo, DateTime fechaFinal)
        {
            var transacciones = new Transaccion[0];
            if (estaCoreAbajo)
                transacciones = Transaccion.ObtenerTodasTransaccionesRango(fechaComienzo, fechaFinal);
            else
            {
                //Todo
            }

            return transacciones;
        }

    }
}
