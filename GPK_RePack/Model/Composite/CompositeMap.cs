using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPK_RePack.Model.Composite
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
