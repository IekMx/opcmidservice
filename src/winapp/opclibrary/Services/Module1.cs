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
        public static AbstractOpcManager GaugeXlManager;
        public static AbstractOpcManager FestoManager;

        static Module1()
        {
            FestoManager = Impl.FestoManager.GetInstance();
            GaugeXlManager = Impl.GaugeXlManager.GetInstance();
        }

    }
}
