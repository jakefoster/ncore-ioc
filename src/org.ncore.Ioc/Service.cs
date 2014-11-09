using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace org.ncore.Ioc
{
    public class Service : DynamicObject
    {
        private Type _type;

        public Service( Type type )
        {
            // NOTE: Non-obvious behavior here, but basically we're harmonizing
            //  support for both direct type use and mapping from the Locator registry.
            //  The way this works is simple: if you pass in a type we first try to
            //  look it up in the registry.  If we find it we use the type mapping 
            //  from the registry.  If not, we just use the type you passed in.  -JF
            if( Locator.Registry.Keys.Contains( type.FullName ) )
            {
                LocatorType entry = Locator.Registry[ type.FullName ];
                _type = Type.GetType( entry.TypeName + ", " + entry.Assembly );
            }
            else
            {
                _type = type;
            }
        }

        public Service( string name )
        {
            if( Locator.Registry.Keys.Contains( name ) )
            {
                LocatorType entry = Locator.Registry[ name ];
                _type = Type.GetType( entry.TypeName + ", " + entry.Assembly );
            }
            else
            {
                throw new ApplicationException( "The specified name does not refer to a Type object in the Registry." );
            }
        }

        // NOTE: For static properties.
        // TODO: What about static fields?!
        public override bool TryGetMember( GetMemberBinder binder, out object result )
        {
            PropertyInfo prop = _type.GetProperty( binder.Name, BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public );
            if( prop == null )
            {
                result = null;
                return false;
            }

            result = prop.GetValue( null, null );
            return true;
        }

        // NOTE: For static methods.
        public override bool TryInvokeMember( InvokeMemberBinder binder, object[] args, out object result )
        {
            MethodInfo method = _type.GetMethod( binder.Name, BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public );
            if( method == null )
            {
                result = null;
                return false;
            }

            result = method.Invoke( null, args );
            return true;
        }
    }

}
