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
        private readonly ILog log = LogManager.GetLogger(System.Environment.MachineName);
        private readonly bool estaCoreAbajo = true;

        [WebMethod]
        public void InsertarCliente(Cliente cliente)
        {
            if (cliente.EsValido())
            {
                if (estaCoreAbajo)
                {
                    Cliente.InsertarCliente(cliente);
                    log.Info(string.Format("Se obtuvieron los parametros: {0} y se inserto un nuevo cliente en la database" +
                        "local ya que core no esta funcionando", cliente));
                }
                else
                {
                    //TODO.
                }
            }
            else
            {
                var e = new ArgumentException("Uno de los argumentos es demasiado largo. " +
                    "El largo de la matricula debe ser igual a 11 y los nombres y apellidos menor o igual a 50");
                ExceptionLogger.Log(e, log);
                throw e;
            }
        }

        /// <summary>
        /// Busca un objeto de tipo cliente existente en la database y lo retorna. Si no existe retorna null.
        /// </summary>
        [WebMethod]
        public Cliente ObtenerCliente(string nombre, string apellido)
        {
            if (nombre.Length <= 50 && apellido.Length <= 50)
            {
                if (estaCoreAbajo)
                {
                    Cliente clienteEncontrado = Cliente.ObtenerCliente(nombre, apellido);
                    log.Info(string.Format("Se llamo el web service ObtenerCliente con los parametros: {0} {1} y se " +
                        "retorno un cliente con la informacion: {2}", nombre, apellido, clienteEncontrado));
                    return clienteEncontrado;
                }
                else
                {
                    //TODO.
                    throw new NotImplementedException();
                }
            }
            else
            {
                var e = new ArgumentException("Uno de los argumentos es demasiado largo. " +
                    "El nombre y el apellido no pueden tener un largo de mas de 50");
                ExceptionLogger.Log(e, log);
                throw e;
            }
        }

        [WebMethod]
        public void InsertarCuenta(int numeroCuenta, string cedula, string nombres, string apellidos)
        {
            if (cedula.Length <= 12 && nombres.Length <= 50 && apellidos.Length <= 50)
            {
                if (estaCoreAbajo)
                    Cuenta.InsertarCuenta(numeroCuenta, cedula, nombres, apellidos);
                else
                {
                    //TODO.
                }

                log.Info(string.Format("Se recibio el numero de cuenta: {0}, la cedula {1}, y el nombre {2}. Se inserto nuevo informacion" +
                    "en la tabla de cuentas", numeroCuenta, cedula, nombres + " " + apellidos));
            }
            else
            {
                var e = new ArgumentException("Uno de los argumentos es demasiado largo. " +
                    "El largo de la matricula debe ser menor o igual a 12 y los nombres y apellidos menor o igual a 50");
                ExceptionLogger.Log(e, log);
                throw e;
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
            {
                Cuenta[] cuentas;
                if (estaCoreAbajo)
                    cuentas =  Cuenta.ObtenerCuentas(cedula);
                else
                {
                    throw new NotImplementedException();
                }

                log.Info(string.Format("Se recibio la cedula {0} y se regresaron {1} cuentas", cedula, cuentas.Length));

                return cuentas;
            }
            else
            {
                var e = new ArgumentException("La cedula no puede tener un largo de más de 12");
                ExceptionLogger.Log(e, log);
                throw e;
            }
        }

        /// <summary>
        /// Busca una cuenta existente en la database a partir de su numero. Si no existe retorna null.
        /// </summary>
        [WebMethod]
        public Cuenta ObtenerCuenta(int numeroCuenta)
        {
            Cuenta cuenta;
            if (estaCoreAbajo)
                cuenta = Cuenta.ObtenerCuenta(numeroCuenta);
            else
                throw new NotImplementedException();

            log.Info(string.Format("Se recibio el numero de cuenta {0} y se devolvio un objeto con el contenido: {1}",
                numeroCuenta, cuenta));

            return cuenta;
        }

        /// <summary>
        /// Realiza una transaccion interbancaria a partir del numero de la cuenta de la que se sacara el monto,
        /// el numero de cuenta en la cual se depositara y el monto
        /// </summary>
        [WebMethod]
        public void RealizarTransaccion(Cuenta cuentaRetiro, Cuenta cuentaDeposito, decimal monto)
        {
            if (cuentaRetiro.balanceDisponible >= monto)
            {
                if (estaCoreAbajo)
                {
                    cuentaRetiro.balanceDisponible -= monto;
                    cuentaDeposito.balanceDisponible += monto;
                    Cuenta.ActualizarCuenta(cuentaRetiro.numeroCuenta, cuentaRetiro.balanceDisponible, monto, TipoTransaccion.Transaccion);
                    Cuenta.ActualizarCuenta(cuentaDeposito.numeroCuenta, cuentaDeposito.balanceDisponible, monto, TipoTransaccion.Transaccion);
                }
                else
                {
                    //TODO
                }

                log.Info(string.Format("Se recibieron los numeros de cuentas {0} y {1} para realizar una transaccion interbancaria" +
                    "Se hicieron las actualizaciones adecuadas en la database", cuentaRetiro.numeroCuenta, cuentaDeposito.numeroCuenta));
            }
            else
            {
                var e = new ArgumentException("La cuenta retiro no tiene balance suficiente para realizar esta transaccion");
                ExceptionLogger.Log(e, Globals.log);
                throw e;
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
                Cuenta.ActualizarCuenta(cuenta.numeroCuenta, cuenta.balanceDisponible, monto, TipoTransaccion.Deposito);
                cuenta.transacciones = Transaccion.ObtenerTodasTransacciones(cuenta.numeroCuenta);
            }
            else
            {
                //TODO
            }

            log.Info(string.Format("Se ha recibido la cuenta: {0} y el monto {1}. Se le añadio {1}" +
                " a la cuenta {2} y se regreso un objeto de tipo cuenta actualizado", cuenta, monto, cuenta.numeroCuenta));

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
                    Cuenta.ActualizarCuenta(cuenta.numeroCuenta, cuenta.balanceDisponible, monto, TipoTransaccion.Retiro);
                    cuenta.transacciones = Transaccion.ObtenerTodasTransacciones(cuenta.numeroCuenta);
                }
                else
                {
                    //TODO
                }

                log.Info(string.Format("Se recibio la cuenta: {0} y el monto: {1}. Se modifico el monto actual de la cuenta {2} de manera acorde",
                    cuenta, monto, cuenta.numeroCuenta));
            }
            else
            {
                var e = new ArgumentException("La cuenta no tiene balance suficiente disponible para realizar esta transaccion");
                ExceptionLogger.Log(e, log);
                throw e;
            }

            return cuenta;
        }

        /// <summary>
        /// Inserta un nuevo prestamo dentro de la database y regresa un objeto de tipo cuenta actualizado. 
        /// Deben aseguarse de que ya existe una cuenta dentro de la database con ese numero o lanzara error.
        /// </summary>
        [WebMethod]
        public void InsertarPrestamo(int numeroCuenta, decimal monto)
        {
            Prestamo.InsertarPrestamo(numeroCuenta, monto);
            log.Info(string.Format("Se ha creado un nuevo prestamo con el numero {0} y el monto {1}", numeroCuenta, monto));
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

            log.Info(string.Format("Se recibieron los parametros: {0} {1} {2} y se hicieron las modificaciones acordes en la database",
                cuenta, prestamo, monto));

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

            log.Info(string.Format("Se recibio la fecha: {0}. Se regresaron {1} transacciones existentes en la database",
                fecha, transacciones.Length));

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

            log.Info(string.Format("Se recibieron los parametros: {0} y {1}. Se regresaron {2} transacciones existentes en la database",
                fechaComienzo, fechaFinal, transacciones.Length));

            return transacciones;
        }

    }
}
