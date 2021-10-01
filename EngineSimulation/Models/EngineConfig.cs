using System.Collections.Generic;
using System.Linq;

namespace EngineSimulation
{
    public class EngineConfig
    {
        public int InertionMoment { get; set; }
        public List<int> TorqueArray { get; set; }
        public List<int> VelosityRotateArray { get; set; }
        /// <summary>
        /// Содержит кусочно заданную функцию зависимости вращающего момента от скорости
        /// </summary>
        public List<TorquVelosityPoint> TorqueVelosityPoints =>
          VelosityRotateArray.OrderBy(velosity => velosity).Zip(TorqueArray, (velosity, torque) => new TorquVelosityPoint(torque, velosity)).ToList();
        public int OverheatingTemperature { get; set; }
        public double TorqueHeatingCoefficient { get; set; }
        public double VelosityRotateHeatingCoefficient { get; set; }
        public double TemperatureOfEngineAndAmbientHeatingCoefficient { get; set; }
    }
}
