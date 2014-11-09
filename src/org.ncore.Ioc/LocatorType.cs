using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.ncore.Ioc
{
    public class LocatorType : IEqualityComparer<LocatorType>
    {
        public string Name { get; set; }
        public string Assembly { get; set; }
        public string TypeName { get; set; }
        public bool AllowSave { get; set; }
        public object Instance { get; set; }

        public LocatorType(){}

        public LocatorType( string name, string assembly, string typeName )
        {
            this.Name = name;
            this.Assembly = assembly;
            this.TypeName = typeName;
            this.AllowSave = false;
        }

        public LocatorType( string name, Type type )
        {
            this.Name = name;
            this.Assembly = type.Assembly.FullName;
            this.TypeName = type.FullName;
            this.AllowSave = false;
        }

        public LocatorType( Type name, Type type )
        {
            this.Name = name.FullName;
            this.Assembly = type.Assembly.FullName;
            this.TypeName = type.FullName;
            this.AllowSave = false;
        }

        public LocatorType( Type name, string assembly, string typeName )
        {
            this.Name = name.FullName;
            this.Assembly = assembly;
            this.TypeName = typeName;
            this.AllowSave = false;
        }

        public LocatorType( object instance )
        {
            _validateInstance( instance );

            Type type = instance.GetType();

            this.Name = type.FullName;
            this.Assembly = type.Assembly.FullName;
            this.TypeName = type.FullName;
            this.AllowSave = true;
            this.Instance = instance;
        }

        public LocatorType(string name, object instance)
        {
            _validateInstance( instance );

            Type type = instance.GetType();

            this.Name = name;
            this.Assembly = type.Assembly.FullName;
            this.TypeName = type.FullName;
            this.AllowSave = true;
            this.Instance = instance;
        }

        public LocatorType( Type type, object instance )
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

        #region IEqualityComparer<LocatorType> Members

        public bool Equals( LocatorType x, LocatorType y )
        {
            return x.Name == y.Name;
        }

        public int GetHashCode( LocatorType obj )
        {
            LocatorType locatorType = (LocatorType)obj;
            return locatorType.Name.GetHashCode();
        }

        #endregion
    }
}
