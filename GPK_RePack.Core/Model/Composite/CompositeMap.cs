using System;
using System.Collections.Generic;

namespace GPK_RePack.Core.Model.Composite
{
    [Serializable]
    public class CompositeMap
    {
        public Dictionary<String, List<CompositeMapEntry>> Map;


        public CompositeMap()
        {
            Map = new Dictionary<string, List<CompositeMapEntry>>();
        }
    }
}
