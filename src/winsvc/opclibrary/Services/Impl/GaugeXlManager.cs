using opclibrary.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opclibrary.Services.Impl
{
    public class GaugeXlManager : AbstractOpcManager
    {
        private new static AbstractOpcManager _instance = new GaugeXlManager("GaugeToolsXL OPC Server");

        public GaugeXlManager(string serverName) : base(serverName)
        {
            var tags = new List<OpcTag>();
            tags.Add(new OpcTag { Name = "", Handle = 0 });
            tags.Add(new OpcTag { Name = "Output", Handle = 1 });
            Initialize(new Mappings.OpcConfiguration
            {
                TagCount = 1,
                ClientTags = tags,
                ItemIsArray = new int[2]
            });
        }

        public new static AbstractOpcManager GetInstance()
        {
            return _instance;
        }
    }
}
