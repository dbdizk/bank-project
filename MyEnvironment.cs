using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace bank_project
{
    internal static class MyEnvironment
    {
        internal static string GetBaseUrl()
        {
            return "https://localhost:7080/api";
        }
    }
}