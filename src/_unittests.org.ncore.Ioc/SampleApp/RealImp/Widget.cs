using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using org.ncore.Ioc;
using _unittests.org.ncore.Ioc;
using _unittests.org.ncore.Ioc.SampleApp.Interfaces;

namespace _unittests.org.ncore.Ioc.SampleApp.RealImp
{
    public class Widget : IWidget
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int Size { get; set; }
        public Widget()
        {
            Debug.WriteLine( "In RealImp.Widget:ctor()" );
        }
    }
}
