using System;
using System.Collections.Generic;

namespace PSO_Warehouse
{
    internal class WareHousePlaceFinder
    {
        private const int PopulationSize = 5;
        private const double InertiaWeight = 0.7;
        private const double PersonalBestWeight = 0.7;
        private const double GlobalBestWeight = 0.5;

        private static List<double[]> stores = [];
        private static List<double[]> residentials = [];

        static void Main()
        {
            int numOfBuildings = 10;
            stores = GenerateCoordinates(numOfBuildings);
            residentials = GenerateCoordinates(numOfBuildings);

            ParticleSwarmOptimization(() => false);
        }

        private static List<double[]> GenerateCoordinates(int count)
        {
            var coordinates = new List<double[]>();
            for (int i = 0; i < count; i++)
            {
                coordinates.Add([Random.Shared.NextDouble(), Random.Shared.NextDouble()]);
            }
            return coordinates;
        }

        private static double EvaluateFitness(double[] particle)
        {
            double closestResidential = 1.0;
            double furthestStore = 0.0;

            foreach (var store in stores)
            {
                double distance = CalculateDistance(store, particle);
                furthestStore = Math.Max(furthestStore, distance);
            }

            foreach (var residential in residentials)
            {
                double distance = CalculateDistance(residential, particle);
                closestResidential = Math.Min(closestResidential, distance);
            }

            return closestResidential - furthestStore;
        }

        private static double CalculateDistance(double[] a, double[] b)
        {
            double dx = a[0] - b[0];
            double dy = a[1] - b[1];
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private static double[] ParticleSwarmOptimization(Func<bool> stopCondition)
        {
            double[] globalBest = [0.5, 0.5];
            var population = InitializePopulation();

            DrawMap(population);
            EvaluatePopulation(population, ref globalBest);

            while (!stopCondition())
            {
                DrawMap(population);
                UpdateVelocities(population, globalBest);
                MoveParticles(population);
                EvaluatePopulation(population, ref globalBest);
                Thread.Sleep(100);
            }

            return globalBest;
        }

        private static Entity[] InitializePopulation()
        {
            var population = new Entity[PopulationSize];
            for (int i = 0; i < population.Length; i++)
            {
                population[i] = new Entity();
            }
            return population;
        }

        private static void DrawMap(Entity[] population)
        {
            Console.Clear();
            const int scaleX = 50;
            const int scaleY = 25;

            DrawPoints(stores, scaleX, scaleY, ConsoleColor.Green, 'S');
            DrawPoints(residentials, scaleX, scaleY, ConsoleColor.Red, 'R');
            DrawBorders(scaleX, scaleY);

            for (int i = 0; i < population.Length; i++)
            {
                Console.SetCursorPosition((int)(population[i].Position[0] * scaleX), (int)(population[i].Position[1] * scaleY));
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(i);
            }
        }

        private static void DrawPoints(List<double[]> points, int scaleX, int scaleY, ConsoleColor color, char symbol)
        {
            Console.ForegroundColor = color;
            foreach (var point in points)
            {
                Console.SetCursorPosition((int)(point[0] * scaleX), (int)(point[1] * scaleY));
                Console.Write(symbol);
            }
        }

        private static void DrawBorders(int scaleX, int scaleY)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            for (int y = 0; y < scaleY; y++)
            {
                Console.SetCursorPosition(scaleX, y);
                Console.Write('|');
            }
            for (int x = 0; x < scaleX; x++)
            {
                Console.SetCursorPosition(x, scaleY);
                Console.Write('-');
            }
        }

        private static void MoveParticles(Entity[] population)
        {
            foreach (var particle in population)
            {
                for (int i = 0; i < particle.Position.Length; i++)
                {
                    particle.Position[i] += particle.Velocity[i];
                    particle.Position[i] = Math.Clamp(particle.Position[i], 0.0, 1.0);
                }
            }
        }

        private static void EvaluatePopulation(Entity[] population, ref double[] globalBest)
        {
            foreach (var particle in population)
            {
                if (EvaluateFitness(particle.Position) >= EvaluateFitness(particle.BestPosition))
                {
                    Array.Copy(particle.Position, particle.BestPosition, particle.Position.Length);

                    if (EvaluateFitness(particle.BestPosition) >= EvaluateFitness(globalBest))
                    {
                        globalBest = (double[])particle.BestPosition.Clone();
                    }
                }
            }
        }

        private static void UpdateVelocities(Entity[] population, double[] globalBest)
        {
            foreach (var particle in population)
            {
                double rPersonal = Random.Shared.NextDouble();
                double rGlobal = Random.Shared.NextDouble();

                for (int i = 0; i < particle.Velocity.Length; i++)
                {
                    particle.Velocity[i] =
                          InertiaWeight * particle.Velocity[i]
                        + PersonalBestWeight * rPersonal * (particle.BestPosition[i] - particle.Position[i])
                        + GlobalBestWeight * rGlobal * (globalBest[i] - particle.Position[i]);
                }
            }
        }
    }

    internal class Entity
    {
        public double[] BestPosition { get; set; }
        public double[] Position { get; set; }
        public double[] Velocity { get; set; }

        public Entity()
        {
            BestPosition = [Random.Shared.NextDouble(), Random.Shared.NextDouble()];
            Position = [Random.Shared.NextDouble(), Random.Shared.NextDouble()];
            Velocity = [Random.Shared.NextDouble() / 10, Random.Shared.NextDouble() / 10];
        }
    }
}
