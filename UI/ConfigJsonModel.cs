using EngineSimulation;
using System.Collections.Generic;
using System.Linq;

namespace UI
{
    public class ConfigJsonModel
    {
        public List<EngineConfig> Engines { get; set; }
        public EngineConfig Engine => Engines.First();
        public int OverheatWaitingTimeInVirtualHourse { get; set; }
    }


}
