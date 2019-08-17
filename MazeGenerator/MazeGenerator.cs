using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Matze.Algorithms;
using Matze.Utils;
using AlogrithmDict = System.Collections.Generic.Dictionary<System.Type, Generate>;

delegate BitGrid Generate(Random rand, int width, int hight);


namespace Matze
{
    class MazeGenerator
    {

        private int seed;
        private Random random;
        private AlogrithmDict algorithms;

        public MazeGenerator()
        {
            var now = DateTime.Now;
            var begin = new DateTime(1970, 1, 1);
            var time = now.Subtract(begin);
            seed = (int)time.TotalSeconds;
            random = new Random(seed);
            algorithms = new AlogrithmDict();
        }

        public MazeGenerator(int seed)
        {
            this.seed = seed;
            this.random = new Random(seed);
            this.algorithms = new AlogrithmDict();
        }

        public int Seed
        {
            set
            {
                this.seed = value;
                this.random = new Random(value);
            }
            get => seed;
        }

        public bool Add<T>() where T : Algorithm
        {
            var type = typeof(T);
            MethodInfo method = type.GetMethod("Generate",
                System.Reflection.BindingFlags.Static |
                BindingFlags.Public
            );
            // see http://blogs.microsoft.co.il/bursteg/2006/11/15/invoke-a-static-generic-method-using-reflection/
            if (method != null)
            {
                var del = (Generate)Delegate.CreateDelegate(typeof(Generate), method);
                algorithms.Add(type, del);
                return true;
            }
            else
            {
                return false;
            }
        }
        /**
         * @brief The function which shall be executed from the outside to invoke the actual algorithm
         * @return This function returns a Grid based in IGrid.
         */
        public BitGrid Run<T>(int width = 10, int height = 10)
            where T : Algorithm
        {
            return algorithms[typeof(T)].Invoke(random, width, width);
        }
        public BitGrid RunSafe<T>(int width = 10, int height = 10)
            where T : Algorithm
        {
            if(algorithms.ContainsKey(typeof(T))){
                return algorithms[typeof(T)].Invoke(random, width, width);
            }else{
                throw new System.Exception("The Requested Algorithm was not added to the known Algorithms");
            }
        }

    }
}
