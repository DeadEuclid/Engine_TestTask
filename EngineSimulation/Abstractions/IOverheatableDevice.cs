namespace EngineSimulation
{
    public interface IOverheatableDevice
    {
       double CurrentTemperature  { get; }
         double OverheatingTemperature { get; }

        void NextStanding(double ambientTemperature);
    }
}