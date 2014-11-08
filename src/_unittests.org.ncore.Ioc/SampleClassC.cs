using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _unittests.org.ncore.Ioc
{
    public class SampleClassC : ISampleInterfaceA
    {
        public static string Greet( string greeting )
        {
            SampleClassC instance = new SampleClassC();
            return greeting + ", " + instance.WhoAmI();
        }

        public string WhoAmI()
        {
            return "I am a SampleClassC";
        }
    }
}
