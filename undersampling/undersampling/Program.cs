using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.MachineLearning;

namespace undersampling
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"D:\Repo\Magisterka\zbiory\glass1.dat";

            string [][] data = File.ReadLines(filePath).Where(line => line != "").Select(x => x.Split(';')).ToArray();
            double [][] dataNumber = ConvertArray(data);
            Dictionary<int, int> population = GetPopulationOfClassess(dataNumber);
            int majorityLabel = GetLabelOfMajorityClass(population);
            int minorityLabel = GetLabelOfMinorityClass(population);
            int majorityPopulation = population[majorityLabel];
            int minorityPopulation = population[minorityLabel];
            double[][] majorityClass = GetWholeClass(majorityLabel, dataNumber, majorityPopulation);
            double[][] minorityClass = GetWholeClass(minorityLabel, dataNumber, minorityPopulation);
            KMeans kmeans = new KMeans(k: minorityPopulation);
            var clusters = kmeans.Learn(majorityClass);
            int[] labels = clusters.Decide(majorityClass);
            int[] clustersPopulation = GetClustersPopulation(labels, minorityPopulation);
            double[][] summed = GetSumOfClustersRecords(majorityClass, labels, minorityPopulation);
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

    static Dictionary<int, int> GetPopulationOfClassess(double[][] dataNumber)
        {
            Dictionary<int, int> population = new Dictionary<int, int>();
            for (int i = 0; i < dataNumber.Length; i++)
            {
                if (population.ContainsKey(Convert.ToInt32(dataNumber[i][dataNumber[i].Length - 1])))
                {
                    population[Convert.ToInt32(dataNumber[i][dataNumber[i].Length - 1])] += 1;
                }
                else
                {
                    population.Add(Convert.ToInt32(dataNumber[i][dataNumber[i].Length - 1]), 1);
                }
            }
            return population;
        }

    static int GetLabelOfMajorityClass(Dictionary<int, int> population)
        {
            int majorityPopulation = population.First().Value;
            int majorityLabel = population.First().Key;
            foreach (var record in population)
            {
                if (record.Value > majorityPopulation)
                {
                    majorityLabel = record.Key;
                }
            }
            return majorityLabel;
        }

    static int GetLabelOfMinorityClass(Dictionary<int, int> population)
        {
            int minorityPopulation = population.First().Value;
            int minorityLabel = population.First().Key;
            foreach (var record in population)
            {
                if (record.Value < minorityPopulation)
                {
                    minorityLabel = record.Key;
                }
            }
            return minorityLabel;
        }

    static double[][] GetWholeClass(double label, double[][] dataNumber, int population)
        {
            int counter = 0;
            double[][] wholeClass = new double [population][];
            for (int i = 0; i < dataNumber.Length; i++)
            {
                if (label == Convert.ToInt32(dataNumber[i][dataNumber[i].Length - 1]))
                {
                    wholeClass[counter] = dataNumber[i];
                    counter += 1;
                }
            }
            return wholeClass;
        }

    static int[] GetClustersPopulation(int[] labels, int minorityPopulation)
        {
            int[] clustersPopulation = new int[minorityPopulation];
            Array.Clear(clustersPopulation, 0, clustersPopulation.Length);
            for (int i = 0; i < labels.Length; i++)
            {
                clustersPopulation[labels[i]] += 1;
            }
            return clustersPopulation;
        }

    static double[][] GetSumOfClustersRecords (double[][] majorityClass, int[] labels, int minorityPopulation)
        {
            double [][] summed = new double[minorityPopulation][];
            double[] temp = new double[majorityClass[0].Length]; 
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = 0.0;
            }
            for (int i = 0; i < summed.Length; i++)
            {
                summed[i] = (double[])temp.Clone();
            }
            for (int i = 0; i < labels.Length; i++)
            {
                for (int j = 0; j < majorityClass[i].Length; j++)
                {
                    summed[labels[i]][j] += majorityClass[i][j];
                }
            }
            return summed;
        }
        
    }
}
