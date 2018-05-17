using System;

namespace opclibrary.Mappings
{
    public class OpcConfiguration
    {
        public int TagCount { get; set; }
        public Array ServerErrors { get; set; }
        public Array ServerHandles { get; set; }
        public int[] ItemIsArray { get; set; }
        public System.Collections.Generic.List<OpcTag> ClientTags { get; set; }

        public OpcConfiguration()
        {
        }
    }
}
