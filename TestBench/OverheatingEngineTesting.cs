using EngineSimulation;
using System;

namespace TestBench
{
    public class OverheatingEngineTesting
    {
        private IEngine Engine;
        private double TimeStep;
        private double AmbientTemperature;
        private double TimeToCutoff;
        /// <summary>
        /// Модельное время
        /// </summary>
        public readonly double ModelTime;
        /// <summary>
        /// Класс тестирования двигателей на время до пререгрева
        /// </summary>
        /// <param name="engine">Двигатель для тестирования</param>
        /// <param name="timeStep">Минимальный промежуток времени для расчёта</param>
        /// <param name="ambientTemperature">Темпратура окружающей среды</param>
        /// <param name="timeToCutoff">Время ожидания прегрева в часах</param>
        public OverheatingEngineTesting(IEngine engine, double timeStep, double ambientTemperature, double timeToCutoff)
        {
            Engine = engine;
            TimeStep = timeStep;
            AmbientTemperature = ambientTemperature;
            TimeToCutoff = timeToCutoff;
        }
        public TestResult RunTest()
        {
            do
            {
                if (Engine.InstantStanding.Temerature >= Engine.OverheatingTemperature)
                    return new TestResult(ReasonEnd.Overheating, ModelTime);
                Engine.NextStanding(AmbientTemperature, TimeStep);
            } while (ModelTime < TimeToCutoff);
            return new TestResult(ReasonEnd.OverTime, ModelTime);
        }
        public struct TestResult
        {
            readonly ReasonEnd ReasonEnd;
            readonly double Time;

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
