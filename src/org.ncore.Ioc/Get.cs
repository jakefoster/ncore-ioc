using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace org.ncore.Ioc
{
    public static class Get
    {
        public static dynamic Instance( string name, bool allowCreate = false )
        {
            dynamic instance = _getInstance( name, allowCreate );
            return (dynamic)instance;
        }

        public static T Instance<T>( bool allowCreate = false )
        {
            string name = typeof( T ).ToString();
            return Instance<T>( name, allowCreate );
        }

        public static T Instance<T>( string name, bool allowCreate = false )
        {
            return (T)_getInstance( name, allowCreate );
        }

        private static object _getInstance( string name, bool allowCreate = false )
        {
            object instance = null;
            if( Locator.Registry.Keys.Contains( name ) && Locator.Registry[ name ].Instance != null )
            {
                instance = Locator.Registry[ name ].Instance;
            }
            else if( !Locator.Registry.Keys.Contains( name ) )
            {
                throw new ApplicationException( "The specified entry in the KernalRegistry does not exist." );
            }
            else if( allowCreate )
            {
                instance = New.Instance( name, null, true );
            }
            else
            {
                throw new ApplicationException( "The specified entry in the KernalRegistry does not does not allow a saved instance or does not have one." );
            }
            return instance;
        }
    }
}
