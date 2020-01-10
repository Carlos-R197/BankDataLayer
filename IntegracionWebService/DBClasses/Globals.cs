using log4net;
using System.Configuration;

namespace Integracion.DBClasses
{
    public static class Globals
    {
        public static readonly string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public static readonly ILog log = LogManager.GetLogger(System.Environment.MachineName);
    }
}