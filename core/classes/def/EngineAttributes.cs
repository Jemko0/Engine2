using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineAttributes
{
    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = true)]
     public class EConsoleExposed : System.Attribute
     {
        public bool exposed = true;
     }
}
