using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.ncore.Ioc
{
    [AttributeUsage( AttributeTargets.Property | AttributeTargets.Field )]
    public class InjectAttribute : Attribute
    {
        public string Name { get; set; }
        public InjectAttribute()
        {

        }

        public InjectAttribute( string name )
        {
            Name = name;
        }

        public InjectAttribute( Type type )
        {
            Name = type.FullName;
        }
    }
}
