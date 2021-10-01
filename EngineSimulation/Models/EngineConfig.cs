using System.Collections.Generic;
using System.Linq;

namespace EngineSimulation
{
    public class EngineConfig
    {
        public int InertionMoment { get; set; }
        public List<int> TorqueArray { get; } = new List<int>();
        public List<int> VelosityRotateArray { get; } = new List<int>();
        /// <summary>
        /// Содержит кусочно заданную функцию зависимости вращающего момента от скорости
        /// </summary>
        public List<TorquVelosityPoint> TorqueVelosityPoints => (List<TorquVelosityPoint>)VelosityRotateArray.OrderBy(velosity => velosity).Zip(TorqueArray, (velosity, torque) => new TorquVelosityPoint(torque, velosity));
        public int OverheatingTemperature { get; set; }
        public double TorqueHeatingCoefficient { get; set; }
        public double VelosityRotateHeatingCoefficient { get; set; }
        public double TemperatureOfEngineAndAmbientHeatingCoefficient { get; set; }
    }
}
