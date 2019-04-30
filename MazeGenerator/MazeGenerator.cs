using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Matze.Algorithms;
using Matze.Utils;
using AlogrithmDict = System.Collections.Generic.Dictionary<System.Type, Generate>;

delegate BitGrid Generate(int seed, int width, int hight);


namespace Matze
{
    class MazeGenerator
    {

        private int seed;
        private AlogrithmDict algorithms;

        public MazeGenerator()
        {
            seed = new System.DateTime().Millisecond;
            algorithms = new AlogrithmDict();
        }

        public MazeGenerator(int seed)
        {
            this.seed = seed;
            algorithms = new AlogrithmDict();
        }

        public int Seed
        {
            set => this.seed = value;
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
                algorithms.Add(type, del
                   );
                return true;
            }
            else
            {
                return false;
            }
        }

        public R Run<T,R>(int width = 10, int height = 10) 
            where T : Algorithm
            where R : IGrid
        {
            return (R) Convert.ChangeType(algorithms[typeof(T)].Invoke(seed,width,width),typeof(R));
        }
    }
}
