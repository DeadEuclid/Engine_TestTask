using EngineSimulation;
using System;

namespace TestBench
{
    public class OverheatingTester
    {
        private IOverheatableDevice Device;
        private double AmbientTemperature;
        private double OverheatWaiting;
        /// <summary>
        /// Модельное время в секундах
        /// </summary>
        private double ModelTime;
        /// <summary>
        /// Класс тестирования устройств на время до пререгрева
        /// </summary>
        /// <param name="device">Устройство для тестирования</param>
        /// <param name="ambientTemperature">Темпратура окружающей среды</param>
        /// <param name="overheatWaiting">Время ожидания прегрева в часах</param>
        public OverheatingTester(IOverheatableDevice device, double ambientTemperature, double overheatWaiting)
        {
            Device = device;
            AmbientTemperature = ambientTemperature;
            OverheatWaiting = overheatWaiting;
        }
        public TestResult RunTest()
        {
            do
            {
                if (Device.CurrentTemperature >= Device.OverheatingTemperature)

                    return new TestResult(ReasonEnd.Overheating, ModelTime);

                Device.NextStanding(AmbientTemperature);

                ModelTime++;

            } while (ModelTime < OverheatWaiting * 360);
            return new TestResult(ReasonEnd.OverTime, ModelTime);
        }
        public struct TestResult
        {
            public readonly ReasonEnd ReasonEnd;
            public readonly double Time;

            public TestResult(ReasonEnd reasonEnd, double time)
            {
                ReasonEnd = reasonEnd;
                Time = time;
            }
        }
        public enum ReasonEnd
        {
            OverTime,
            Overheating
        }
    }



}
