using EngineSimulation;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using TestBench;

namespace UI
{
    class Configurator
    {
        public OverheatingTester GetInternalСombustionEngineOverheatingTester(double ambientTemperature)
        {
            
                var config = JsonSerializer.Deserialize<ConfigJsonModel>(File.ReadAllText("configuration.json"));
                var engine = new InternalСombustionEngine(config.Engine, ambientTemperature); 
                return new OverheatingTester(engine, config.TimeStep, ambientTemperature, config.OverheatWaitingTimeInHourse);
        }
    }


}
