using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC
{
    public static class Config
    {
        public static string ConnectionString { get; set; }
        public static void GetConnectionString(string _connectionstring)
        {
            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;
            if (settings != null)
            {
                foreach (ConnectionStringSettings cs in settings)
                {
                    if (cs.Name == _connectionstring)
                        ConnectionString = cs.ConnectionString;
                }
            }
        }
    }
}
