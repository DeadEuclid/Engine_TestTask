namespace EngineSimulation
{
    public struct InstantStanding : IInstantStanding
    {
        public readonly double Temerature { get; }
        public readonly double VelosityRotate;
        public readonly double Torque;

        public InstantStanding(double temerature, double velosityRotate, double torque)
        {
            Temerature = temerature;
            VelosityRotate = velosityRotate;
            Torque = torque;
        }
    }

}
