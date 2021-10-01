namespace EngineSimulation
{
    public interface IOverheatableMachine
    {
       double CurrentTemperature  { get; }
         double OverheatingTemperature { get; }

        void NextStanding(double ambientTemperature, double timeStep);
    }
}