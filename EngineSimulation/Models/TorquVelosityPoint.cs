namespace EngineSimulation
{
    public struct TorquVelosityPoint
    {

        public readonly double Torque;
        public readonly double Velosity;

        public TorquVelosityPoint(double torque, double velosity)
        {
            Torque = torque;
            Velosity = velosity;
        }
    }

}
