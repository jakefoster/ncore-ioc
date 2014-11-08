using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _unittests.org.ncore.Ioc.SampleApp.Interfaces
{
    public interface IWidget
    {
        int Id { get; set; }
        string Name { get; set; }
        string Color { get; set; }
        int Size { get; set; }
    }
}
