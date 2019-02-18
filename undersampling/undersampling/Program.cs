using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord;

namespace undersampling
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"D:\Magisterka\zbiory\glass1.dat";

            string [][] data = File.ReadLines(filePath).Where(line => line != "").Select(x => x.Split(';')).ToArray();
            double [][] dataNumber = ConvertArray(data);
            Dictionary<double, int> population = GetPopulationOfClassess(dataNumber);
            Console.ReadKey();
        }
        
        static double [][] ConvertArray(string[][] data)
        {
            double[][] dataNumber = new double[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[j].Length; j++)
                {
                    dataNumber[i] = Array.ConvertAll(data[i], double.Parse);
                }
            }
            return dataNumber;
        }

    static Dictionary<double, int> GetPopulationOfClassess(double[][] dataNumber)
        {
            Dictionary<double, int> population = new Dictionary<double, int>();
            for (int i = 0; i < dataNumber.Length; i++)
            {
                if (population.ContainsKey(dataNumber[i][dataNumber[i].Length - 1]))
                {
                    population[dataNumber[i][dataNumber[i].Length - 1]] += 1;
                }
                else
                {
                    population.Add(dataNumber[i][dataNumber[i].Length - 1], 1);
                }
            }
            return population;
        }
        
        
    }
}
