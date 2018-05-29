using opclibrary.Mappings;
using OPCAutomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opclibrary.Services
{
    public static class Module1
    {
        public static AbstractOpcManager GaugeXlManager = Impl.GaugeXlManager.GetInstance();
        public static AbstractOpcManager FestoManager = Impl.FestoManager.GetInstance();

        static Module1()
        {
            
        }

    }
}
