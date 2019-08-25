using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Matze.Algorithms;
using Matze;
using AlogrithmDict = System.Collections.Generic.Dictionary<System.Type, Generate>;
delegate BitGrid Generate(Random rand, int width, int hight);

namespace Matze
{
    public class MazeGenerator
    {

        private int seed;
        private Random random;
        private AlogrithmDict algorithms;

        public MazeGenerator()
        {
            this.seed = GenerateSeed();
            this.random = new Random(this.seed);
            this.algorithms = new AlogrithmDict();
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

        public static int GenerateSeed(){
            var now = DateTime.Now;
            var begin = new DateTime(1970, 1, 1);
            var time = now.Subtract(begin);
            return (int)time.TotalSeconds;
        }
        public bool Remove<T>() where T : Algorithm {
            var type = typeof(T);
            return algorithms.Remove(type);
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
        public bool Add(Type type){
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
            return algorithms[typeof(T)].Invoke(random, width, height);
        }
        public BitGrid Run(Type algorithm,int width = 10,int height = 10){
            return algorithms[algorithm].Invoke(random,width,height);
        }
        public BitGrid RunSafe<T>(int width = 10, int height = 10)
            where T : Algorithm
        {
            if(algorithms.ContainsKey(typeof(T))){
                return algorithms[typeof(T)].Invoke(random, width, height);
            }else{
                throw new System.Exception("The Requested Algorithm was not added to the known Algorithms");
            }
        }

    }
}
