using System;
using System.Collections.Generic;
using System.Linq;

namespace EngineSimulation
{
    /// <summary>
    /// Абстрактный двигатель, по умолчанию реализован как ДВС
    /// </summary>
    public abstract class Engine : IEngine
    {

        /// <summary>
        /// Момент инерции двигателя
        /// </summary>
        internal readonly double InertionMoment;
        /// <summary>
        ///Коэффициент зависимости скорости нагрева от скорости вращения коленвала
        /// </summary>
        internal readonly double VelosityRotationCoefficient;
        /// <summary>
        /// Коэффициент зависимости скорости нагрева от крутящего момента
        /// </summary>
        internal readonly double TorqueCoefficient;
        /// <summary>
        /// Коэффициент зависимости скорости охлаждения от температуры двигателя и окружающей среды
        /// </summary>
        internal readonly double CollingAmbientCoefficient;
        /// <summary>
        /// Кусочно-линейная зависимость крутящего момента, вырабатываемого двигателем, от
        ///скорости вращения вала
        /// </summary>
        internal readonly IEnumerable<TorquVelosityPoint> TorquVelosityPoints;
        /// <summary>
        /// Температура пререгрева
        /// </summary>
        public double OverheatingTemperature { get; }
        /// <summary>
        /// Текущее состояние двигителя
        /// </summary>
        public IInstantStanding InstantStanding { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collingAmbientCoefficient">Коэффициент зависимости скорости охлаждения от температуры двигателя и окружающей среды</param>
        /// <param name="ambientTemperature">Температура окружающей среды</param>
        /// <param name="overheatingTemperature">Температура прегрева двигателя</param>
        /// <param name="inertionMoment">Момент инерции двигателя</param>
        /// <param name="velosityRotationCoefficient">Коэффициент зависимости скорости нагрева от скорости вращения коленвала</param>
        /// <param name="torqueCoefficient">Коэффициент зависимости скорости нагрева от крутящего момента</param>
        /// <param name="torquVelosityPoints">Кусочно-линейная зависимость крутящего момента, вырабатываемого двигателем, от скорости вращения коленвала</param>
        internal Engine(double inertionMoment, double velosityRotationCoefficient, double torqueCoefficient, double overheatingTemperature, double collingAmbientCoefficient, double ambientTemperature, IEnumerable<TorquVelosityPoint> torquVelosityPoints)
        {
            InertionMoment = inertionMoment;
            VelosityRotationCoefficient = velosityRotationCoefficient;
            TorqueCoefficient = torqueCoefficient;
            TorquVelosityPoints = torquVelosityPoints;
            OverheatingTemperature = overheatingTemperature;
            CollingAmbientCoefficient = collingAmbientCoefficient;
            InstantStanding = new InstantStanding(ambientTemperature, TorquVelosityPoints.First().Velosity, TorquVelosityPoints.First().Torque);
        }
        /// <summary>
        /// Обновляет текущее состояние двигателя при определённой темпрературе окружающей среды на определённое количество секунд вперёд
        /// </summary>
        /// <param name="ambientTemperature">Температура окружающей среды</param>
        /// <param name="timeStep">Количество секунд</param>
        public void NextStanding(double ambientTemperature, double timeStep)
        {
            double acceleration = GetAcceleration();
            var velosityRotate = GetVelosityRotate(acceleration, timeStep);
            var torque = VelosityRotateToTorque(velosityRotate);
            var temperature = GetTemperature(torque, velosityRotate, ambientTemperature);
            InstantStanding = new InstantStanding(temperature, velosityRotate, torque);
        }
        /// <summary>
        /// Возвращает вращающий момент двигателя при данной скорости вращения вала
        /// </summary>
        /// <param name="velosityRotate">Скорость вращения</param>
        /// <returns>Вращающий момент</returns>
        internal double VelosityRotateToTorque(double velosityRotate)
        {
            var startPoint = TorquVelosityPoints.First(point => point.Velosity >= velosityRotate);
            var endPoint = TorquVelosityPoints.First(point => point.Velosity <= velosityRotate);
            var relation = (velosityRotate - startPoint.Velosity) / (endPoint.Velosity - velosityRotate);
            return (startPoint.Torque + relation * endPoint.Torque) / (1 + relation);
        }
        internal double GetAcceleration() => ((InstantStanding)InstantStanding).Torque / InertionMoment;
        internal double GetVelosityRotate(double acceleration, double timeStep) => ((InstantStanding)InstantStanding).VelosityRotate + acceleration * timeStep;
        internal double GetTemperature(double torque, double velosityRotate, double ambientTemperature)
            => InstantStanding.Temerature + GetHeatingRate(torque, velosityRotate) - GetCoolingRate(ambientTemperature);

        internal double GetHeatingRate(double torque, double velosityRotate) =>
            torque * TorqueCoefficient * Math.Pow(velosityRotate, 2) * VelosityRotationCoefficient;
        internal double GetCoolingRate(double ambientTemperature)
            => CollingAmbientCoefficient * (ambientTemperature - InstantStanding.Temerature);

    }

}
