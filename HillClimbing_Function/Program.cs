namespace HillClimbing_Function
{
    internal class Program
    {
        public static Random r = new Random();
        static void Main(string[] args)
        {
            //CTRL X FUNNY MONKEY
            //We need a function to found it's global optimum which in this case is the minimum of the function
            //testing some values
            using (var sw = new StreamWriter(path:"function_values.txt")) {
                for (int i = 0; i < 10; i++)
                {
                    sw.WriteLine(Function(i));
                }
            }

            //Hill climbing implementation
            double best = 10;
            for (int i = 0; i<500; i++)
            {
                double curr = HillClimbingSteepest();
                if (Function(curr) < Function(best))
                {
                    best = curr;
                }
            }
            Console.WriteLine("DA BEST: " + best + " and its fitness: " + Function(best));

        }

        static double Function (double x)
        {
            
            return Math.Sin(x) + 0.5 * Math.Sin(2 * x) + 0.25 * Math.Sin(3 * x) + 0.5 * Math.Cos(Math.PI * x);
        }

        static double HillClimbingSteepest(double eps=0.005)
        {
            double x = r.NextDouble()*10;
            bool stuck = false;
            while (!stuck)
            {
                double next = MinDecider(x, eps);
                if (Function(x)>Function(next))
                {
                    x = next;
                }
                else
                {
                   stuck = true;
                }
            }

            return x;

        }

        static double MinDecider(double x,double eps)
        {
            
            if (Function(x + eps) < Function(x - eps))
            {
                return Math.Min(x + eps, 10);
            }
            else
            {
                return Math.Max(x + eps, 0);
            }
                   
        }
    }
}
