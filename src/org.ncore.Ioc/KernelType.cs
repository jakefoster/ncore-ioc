using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.ncore.Ioc
{
    public class KernelType
    {
        public string Name { get; set; }
        public string Assembly { get; set; }
        public string TypeName { get; set; }
        public bool AllowSave { get; set; }
        public object Instance { get; set; }

        public KernelType(){}

        public KernelType( string name, string assembly, string typeName )
        {
            this.Name = name;
            this.Assembly = assembly;
            this.TypeName = typeName;
            this.AllowSave = false;
        }

        public KernelType( string name, Type type )
        {
            this.Name = name;
            this.Assembly = type.Assembly.FullName;
            this.TypeName = type.FullName;
            this.AllowSave = false;
        }

        public KernelType( Type name, Type type )
        {
            this.Name = name.FullName;
            this.Assembly = type.Assembly.FullName;
            this.TypeName = type.FullName;
            this.AllowSave = false;
        }

        public KernelType( Type name, string assembly, string typeName )
        {
            this.Name = name.FullName;
            this.Assembly = assembly;
            this.TypeName = typeName;
            this.AllowSave = false;
        }

        public KernelType( object instance )
        {
            _validateInstance( instance );

            Type type = instance.GetType();

            this.Name = type.FullName;
            this.Assembly = type.Assembly.FullName;
            this.TypeName = type.FullName;
            this.AllowSave = true;
            this.Instance = instance;
        }

        public KernelType(string name, object instance)
        {
            _validateInstance( instance );

            Type type = instance.GetType();

            this.Name = name;
            this.Assembly = type.Assembly.FullName;
            this.TypeName = type.FullName;
            this.AllowSave = true;
            this.Instance = instance;
        }

        public KernelType( Type type, object instance )
        {
            _validateInstance( instance );

            this.Name = type.FullName;
            this.Assembly = type.Assembly.FullName;
            this.TypeName = type.FullName;
            this.AllowSave = true;
            this.Instance = instance;
        }

        private static void _validateInstance( object instance )
        {
            if( instance == null )
            {
                throw new ArgumentException( "The 'instance' parameter cannot be null.", "instance" );
            }
        }
    }
}
