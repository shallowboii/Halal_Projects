using System;
using System.Collections.Generic;

namespace PSO_Warehouse
    {
        internal class WareHousePlaceFinder
        {
            const int Population_Size = 5;
            const int Number_Of_Buildings = 10;
            static List<double[]> Stores = new List<double[]>();
            static List<double[]> Residental = new List<double[]>();
            

            static void Main()
            {
                /*
                 A probléma: Le kell rakni egy térképen egy raktárat, úgy
                 hogy a optimalizáljuk a távolságát a raktár és a lakossági épületek között oly módon,
                 hogy a lehető legnagyobb távolság legyen köztük.
                 Továbbá optimalizáljuk a raktár és a kereskedelmi épületek közötti távolságot, hogy a 
                 lehető legközelebb legyenek egymáshoz.
                 */
              

                
                


            }

            private static List<double[]> SetCoordinates()
            {
                List<double[]> doubles = [];
                for (int i = 0; i < Number_Of_Buildings; i++)
                {
                    doubles.Add([Random.Shared.NextDouble(), Random.Shared.NextDouble()]);
                }
                return doubles;

            }

            static double f(double[] particle)
            {
                double ClosestResidental = 1; 
                double FurthestStore = 0; 
                foreach (var store in Stores)
                {
                    double distance = Math.Sqrt(Math.Pow((store[0] - particle[0]), 2) + Math.Pow((store[1] - particle[1]), 2));
                    if (distance > FurthestStore)
                    {
                        FurthestStore = distance;
                    }
                }
                foreach (var res in Residental)
                {
                    double distance = Math.Sqrt(Math.Pow((res[0] - particle[0]), 2) + Math.Pow((res[1] - particle[1]), 2));
                    if (distance < ClosestResidental)
                    {
                        ClosestResidental = distance;
                    }
                }

                return ClosestResidental - FurthestStore; 


        }

        static double[] ParticeSwarmOptimization()
        {
            double[] Global_Opt = [0.5, 0.5];
            Particle[] Population = InitPopulation();


            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true); //just an exit statement

                    if (key.Key == ConsoleKey.Escape)
                    {
                        Console.WriteLine("ESC pressed. Exiting loop...");
                        break;
                    }
                }
                DrawMap(Population);
                CalculateVelocity(Population, Global_Opt);
                MovePopulation(Population);
                Evaluation(Population, ref Global_Opt);
            }
            return Global_Opt;
        }

            private static Particle[] InitPopulation()
            {

            }

            private static void DrawMap(Particle[] P)
            {

            }

            private static void MovePopulation(Particle[] P)
            {

            }

            static void Evaluation(Particle[] Population, ref double[] globalOpt)
            {

            }

            private static void CalculateVelocity(Particle[] P, double[] globalOpt)
            {
                
            }
        }

        internal class Particle
        {
            //
            public double[] Optimum {  get; set; }
            public double[] Velocity { get; set; }
            public double[] Position { get; set; }

            public Particle()
            {
                Optimum = [Random.Shared.NextDouble(), Random.Shared.NextDouble()];
                Position = [Random.Shared.NextDouble(), Random.Shared.NextDouble()];
                Velocity = [Random.Shared.NextDouble() / 10, Random.Shared.NextDouble() / 10];
            }

        }
}