using System;
using TestBench;

namespace UI
{
    class Program
    {
        static void Main(string[] args)
        {
            App.Run();
        }

    }
    public class App
    {
        public static void Run()
        {
            while (true)
            {
                Console.WriteLine("Введите температуру окружающей среды");
                if (double.TryParse(Console.ReadLine(), out double ambientTemperature))
                {
                    OverheatingTester tester = GetTester(ambientTemperature);

                    var testResult = tester.RunTest();
                    if (testResult.ReasonEnd == OverheatingTester.ReasonEnd.Overheating)
                    {
                        Console.WriteLine($"Прегрев наступил через {testResult.Time} секунд");
                    }
                    else
                    {
                        Console.WriteLine("Превышенно время ожидания перегрева, возможно он не достижим при данной температуре окружающей среды, поднимите её или повысте время ожидания в файле конфигурации ");
                    }
                }
                else
                {
                    Console.WriteLine("Температура введена не корректно");
                }
            }
        }
        private static OverheatingTester GetTester(double ambientTemperature)
        {
            try
            {
                return new Configurator().GetInternalСombustionEngineOverheatingTester(ambientTemperature);

            }
            catch (Exception e)
            {
                
                Console.WriteLine(@"Файл конфигурации несуществует или написан некорректно, проверьте корректность конфигурационного файла и повторите попытку
 Нажмите любую клавишу для закрытия консоли");

                Console.ReadKey();
                Environment.Exit(0);
                return null;
            }

        }
    }
}

