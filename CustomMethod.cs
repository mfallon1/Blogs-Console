using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogsConsole
{
    class CustomMethod  // error handler
    {

        public static bool IsBlank(string s)
        {
            if (s == " " || s == "" || s == null)
            {
                return true;
            }
            return false;
        }

    }
}
