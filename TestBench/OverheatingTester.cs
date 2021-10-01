using EngineSimulation;
using System;

namespace TestBench
{
    public class OverheatingTester
    {
        private IOverheatableMachine Engine;
        private double TimeStep;
        private double AmbientTemperature;
        private double OverheatWaiting;
        /// <summary>
        /// Модельное время в секундах
        /// </summary>
        private double ModelTime;
        /// <summary>
        /// Класс тестирования устройств на время до пререгрева
        /// </summary>
        /// <param name="machine">Устройство для тестирования</param>
        /// <param name="timeStep">Минимальный промежуток времени для расчёта</param>
        /// <param name="ambientTemperature">Темпратура окружающей среды</param>
        /// <param name="overheatWaiting">Время ожидания прегрева в часах</param>
        public OverheatingTester(IOverheatableMachine machine, double timeStep, double ambientTemperature, double overheatWaiting)
        {
            Engine = machine;
            TimeStep = timeStep;
            AmbientTemperature = ambientTemperature;
            OverheatWaiting = overheatWaiting;
        }
        public TestResult RunTest()
        {
            do
            {
                if (Engine.CurrentTemperature >= Engine.OverheatingTemperature)

                    return new TestResult(ReasonEnd.Overheating, ModelTime);

                Engine.NextStanding(AmbientTemperature, TimeStep);

                ModelTime += TimeStep;

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
