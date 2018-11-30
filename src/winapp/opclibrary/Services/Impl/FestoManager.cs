using opclibrary.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opclibrary.Services.Impl
{
    public class FestoManager : AbstractOpcManager
    {
        private new static AbstractOpcManager _instance = new FestoManager("CoDeSys.OPC.DA");

        private FestoManager(string serverName) : base(serverName)
        {
            var tags = new List<OpcTag>();
            tags.Add(new OpcTag { Name = "", Handle = 0});
            tags.Add(new OpcTag { Name = "PLC1.Application.PLC_PRG.bHOME_OK", Handle = 1 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL.Record1.lrTarget", Handle = 2 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL.HMI_bShowAuto_1", Handle = 3 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL.HMI_bShowAuto_2", Handle = 4 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL.HMI_bShowAuto_3", Handle = 5 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos1", Handle = 6 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos2", Handle = 7 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos3", Handle = 8 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos4", Handle = 9 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos5", Handle = 10 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos6", Handle = 11 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos7", Handle = 12 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos8", Handle = 13 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos9", Handle = 14 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos10", Handle = 15 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos11", Handle = 16 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos12", Handle = 17 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos13", Handle = 18 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos14", Handle = 19 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos15", Handle = 20 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos16", Handle = 21 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos17", Handle = 22 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos18", Handle = 23 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos19", Handle = 24 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos20", Handle = 25 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos21", Handle = 26 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos22", Handle = 27 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos23", Handle = 28 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos24", Handle = 29 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos25", Handle = 30 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos26", Handle = 31 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos27", Handle = 32 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos28", Handle = 33 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos29", Handle = 34 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos30", Handle = 35 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos31", Handle = 36 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_iPos32", Handle = 37 });
            tags.Add(new OpcTag { Name = "PLC1.Application.GVL_1.HMI_rDelay", Handle = 38 });

            Initialize(new Mappings.OpcConfiguration
            {
                TagCount = 38,
                ClientTags = tags,
                ItemIsArray = new int[39]
            });
        }

        public new static AbstractOpcManager GetInstance()
        {
            return _instance;
        }

    }
}
