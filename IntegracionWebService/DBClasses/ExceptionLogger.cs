using log4net;
using System;

namespace Integracion.DBClasses
{
    public static class ExceptionLogger
    {
        public static void Log(Exception e, ILog log)
        {
            log.Error("Ha ocurrido una excepcion: " + e.Message);
        }
    }
}