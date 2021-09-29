using System.Collections.Generic;

namespace EngineSimulation
{
    public class InternalСombustionEngine : Engine
    {
        public InternalСombustionEngine(double inertionMoment, double velosityRotationCoefficient, double torqueCoefficient, double overheatingTemperature, double collingAmbientCoefficient, double ambientTemperature, IEnumerable<TorquVelosityPoint> torquVelosityPoints)
            : base(inertionMoment, velosityRotationCoefficient, torqueCoefficient, overheatingTemperature, collingAmbientCoefficient, ambientTemperature, torquVelosityPoints)
        {
        }
    }

}
