using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedDemo.LIB.Services
{
    public class Helper
    {
        public static string HandleQuotes(string waarde)
        {
            return waarde.Replace("'", "''").Trim();
        }
    }
}
