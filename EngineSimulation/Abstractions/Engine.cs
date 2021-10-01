using System;
using System.Collections.Generic;
using System.Linq;

namespace EngineSimulation
{
    /// <summary>
    /// Абстрактный двигатель, по умолчанию реализован как ДВС
    /// </summary>
    public abstract class Engine : IOverheatableMachine
    {

        /// <summary>
        /// Момент инерции двигателя
        /// </summary>
        internal double InertionMoment { get; }
        /// <summary>
        ///Коэффициент зависимости скорости нагрева от скорости вращения коленвала
        /// </summary>
        internal double VelosityRotationCoefficient { get; }
        /// <summary>
        /// Коэффициент зависимости скорости нагрева от крутящего момента
        /// </summary>
        internal double TorqueCoefficient { get; }
        /// <summary>
        /// Коэффициент зависимости скорости охлаждения от температуры двигателя и окружающей среды
        /// </summary>
        internal double CollingAmbientCoefficient { get; }
        /// <summary>
        /// Кусочно-линейная зависимость крутящего момента, вырабатываемого двигателем, от
        ///скорости вращения вала
        /// </summary>
        internal IEnumerable<TorquVelosityPoint> TorquVelosityPoints { get; }

        public double OverheatingTemperature { get; }
        public double CurrentTemperature { get; private set; }
        public double CurrentVelosityRotate { get; private set; }
        public double CurrentTorque { get; private set; }

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
            CurrentTemperature = ambientTemperature;
            CurrentTorque = TorquVelosityPoints.First().Torque;
            CurrentVelosityRotate = 0;
        }
        internal Engine(EngineConfig config, double ambientTemperature) : this(config.InertionMoment, config.VelosityRotateHeatingCoefficient, config.TorqueHeatingCoefficient, config.OverheatingTemperature, config.TemperatureOfEngineAndAmbientHeatingCoefficient, ambientTemperature, config.TorqueVelosityPoints)
        {
        
        }
        /// <summary>
        /// Обновляет текущее состояние двигателя при определённой темпрературе окружающей среды на определённое количество секунд вперёд
        /// </summary>
        /// <param name="ambientTemperature">Температура окружающей среды</param>
        /// <param name="timeStep">Количество секунд</param>
        public virtual void NextStanding(double ambientTemperature, double timeStep)
        {
            double acceleration = GetAcceleration();
            CurrentVelosityRotate = GetVelosityRotate(acceleration, timeStep);
            CurrentTorque = VelosityRotateToTorque(CurrentVelosityRotate);
            CurrentTemperature = GetTemperature(CurrentTorque, CurrentVelosityRotate, ambientTemperature);

        }
        /// <summary>
        /// Возвращает вращающий момент двигателя при данной скорости вращения вала
        /// </summary>
        /// <param name="velosityRotate">Скорость вращения</param>
        /// <returns>Вращающий момент</returns>
        internal virtual double VelosityRotateToTorque(double velosityRotate)///ToDo
        {
            TorquVelosityPoint startPoint;
            TorquVelosityPoint endPoint;
            if (TorquVelosityPoints.Any(point => velosityRotate<=point.Velosity  ))
            {
                endPoint = TorquVelosityPoints.First(point => velosityRotate <= point.Velosity);
            }
            else
            {
                startPoint = TorquVelosityPoints.Last();
                endPoint = TorquVelosityPoints.Last();
            }
            if (TorquVelosityPoints.Any(point => velosityRotate >= point.Velosity))
            {
                startPoint = TorquVelosityPoints.First(point => velosityRotate >= point.Velosity);
            }
            else
            {
                endPoint= TorquVelosityPoints.First();
                startPoint= TorquVelosityPoints.First();
            }

            var relation = (velosityRotate - startPoint.Velosity) / (endPoint.Velosity - velosityRotate);
            return (startPoint.Torque + relation * endPoint.Torque) / (1 + relation);
        }
        internal virtual double GetAcceleration() => CurrentTorque/ InertionMoment;
        internal virtual double GetVelosityRotate(double acceleration, double timeStep) => CurrentVelosityRotate + acceleration * timeStep;
        internal virtual double GetTemperature(double torque, double velosityRotate, double ambientTemperature)
            => CurrentTemperature + GetHeatingRate(torque, velosityRotate) - GetCoolingRate(ambientTemperature);

        internal virtual double GetHeatingRate(double torque, double velosityRotate) =>
            torque * TorqueCoefficient * Math.Pow(velosityRotate, 2) * VelosityRotationCoefficient;
        internal virtual double GetCoolingRate(double ambientTemperature)
            => CollingAmbientCoefficient * (ambientTemperature - CurrentTemperature);

    }

}
