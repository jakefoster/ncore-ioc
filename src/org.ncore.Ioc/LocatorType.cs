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
        public object Instance { get; private set; }

        public LocatorType(){}

        public LocatorType( string name, string assembly, string typeName, bool allowSave = false )
        {
            this.Name = name;
            this.Assembly = assembly;
            this.TypeName = typeName;
            this.AllowSave = allowSave;
        }

        public LocatorType( string name, Type type, bool allowSave = false )
        {
            this.Name = name;
            this.Assembly = type.Assembly.FullName;
            this.TypeName = type.FullName;
            this.AllowSave = allowSave;
        }

        public LocatorType( Type name, Type type, bool allowSave = false )
        {
            this.Name = name.FullName;
            this.Assembly = type.Assembly.FullName;
            this.TypeName = type.FullName;
            this.AllowSave = allowSave;
        }

        public LocatorType( Type name, string assembly, string typeName, bool allowSave = false )
        {
            this.Name = name.FullName;
            this.Assembly = assembly;
            this.TypeName = typeName;
            this.AllowSave = allowSave;
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

        public void SaveInstance( object instance )
        {
            if( this.Instance != null )
            {
                throw new ApplicationException( "This locator type already has a saved instance." );
            }
            if( this.AllowSave == false )
            {
                throw new ApplicationException( "This locator type does not allow a saved instance." );
            }
            _validateInstance( instance );
            // TODO: I just have to point out that it's a little bit weird to allow this since it could 
            //  result in an instance of any arbitrary type (not matching the specified locator type)
            //  to be saved.  Is this a bug or a feature?!  -JF
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
