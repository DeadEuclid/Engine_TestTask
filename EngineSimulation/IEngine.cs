namespace EngineSimulation
{
    public interface IEngine
    {
        IInstantStanding InstantStanding { get; }
        double OverheatingTemperature { get; }
        void NextStanding(double ambientTemperature, double timeStep);
    }
}