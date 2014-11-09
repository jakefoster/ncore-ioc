using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using org.ncore.Extensions;

namespace org.ncore.Ioc
{
    public class Injector
    {
        public bool SearchKernel { get; set; }
        public InjectorRegistry MemberRegistry { get; set; }        
        public object[] ConstructorParams{get;set;}

        public Injector()
        {
            MemberRegistry = new InjectorRegistry();
            ConstructorParams = null;
            SearchKernel = true;
        }

        public Injector( bool searchKernel, InjectorRegistry memberRegistry = null, object[] constructorParams = null )
        {
            MemberRegistry = new InjectorRegistry();
            SearchKernel = searchKernel;
        }

        public Injector( InjectorRegistry registry )
        {
            MemberRegistry = registry;
            SearchKernel = true;
        }

        public Injector( InjectorRegistry registry, bool searchKernel )
        {
            MemberRegistry = registry;
            SearchKernel = searchKernel;
        }
                
        public object Inject( object instance )
        {
            if( MemberRegistry == null || MemberRegistry.Count == 0 )
            {
                return instance;
            }

            PropertyInfo[] properties = instance.GetType().GetProperties( BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance );

            foreach( PropertyInfo property in properties )
            {
                Debug.WriteLine( "Property: " + property.Name );
                object injectable = null;
                if( MemberRegistry.Keys.Contains( property.Name ) )
                {
                    injectable = _getOrCreate( property.Name );
                    Debug.WriteLine( "---> Retrieved instance from InjectorMemberRegistry" );
                }
                else if( MemberRegistry.Keys.Contains( property.PropertyType.FullName ) )
                {
                    injectable = _getOrCreate( property.PropertyType.FullName );
                    Debug.WriteLine( "---> Retrieved instance from InjectorRegistry" );
                }
                else if( SearchKernel && Locator.Registry.Keys.Contains( property.Name ) )
                {
                    injectable = New.Instance<object>( property.Name, null );
                    Debug.WriteLine( "---> Retrieved instance from KernelRegistry" );
                }

                if( injectable != null )
                {
                    if( property.GetSetMethod( true ) != null )
                    {
                        property.SetValue( instance, injectable );
                        Debug.WriteLine( "---> Set value!" );
                    }
                    else
                    {
                        PropertyInfo baseProperty = _getPropertyFromBase( instance.GetType().BaseType, property.Name );
                        baseProperty.SetValue( instance, injectable );
                        Debug.WriteLine( "---> Set value!" );
                    }
                }

            }

            FieldInfo[] fields = _getFields( instance.GetType() );
            
            foreach( FieldInfo field in fields )
            {
                object injectable = null;
                if( MemberRegistry.Keys.Contains( field.Name ) )
                {
                    injectable = _getOrCreate( field.Name ); 
                    Debug.WriteLine( "---> Retrieved instance from InjectorRegistry" );
                }
                else if( MemberRegistry.Keys.Contains( field.FieldType.FullName ) )
                {
                    injectable = _getOrCreate( field.FieldType.FullName );
                    Debug.WriteLine( "---> Retrieved instance from InjectorRegistry" );
                }
                else if( SearchKernel && Locator.Registry.Keys.Contains( field.Name ) )
                {
                    injectable = New.Instance<object>( field.Name, null );
                    Debug.WriteLine( "---> Retrieved instance from KernelRegistry" );
                }

                if( injectable != null )
                {
                    field.SetValue( instance, injectable );
                    Debug.WriteLine( "---> Set value!" );
                }
            }

            return instance;
        }

        private object _getOrCreate( string key )
        {
            object injectable = MemberRegistry[ key ];

            if( injectable is Type )
            {
                return Activator.CreateInstance( (Type)injectable );
            }
            else if( injectable is InjectorLiteral )
            {
                return ( (InjectorLiteral)injectable ).Value;
            }
            else if( injectable is InjectorType )
            {
                return _createInstance( (InjectorType)injectable );
            }
            else if( _resemblesInjectorType( injectable ) )
            {
                InjectorType proxy = new InjectorType();
                proxy.Assembly = ( (dynamic)injectable ).Assembly;
                proxy.TypeName = ( (dynamic)injectable ).TypeName;
                return _createInstance( proxy );
            }
            else if( injectable == InjectorType.Null )
            {
                return null;
            }
            else
            {
                // NOTE: Finally, they appear to have just placed an instance in the registry 
                //  that they want handed in to the new instance so just return that.  -JF
                return injectable;
            }
        }

        private static bool _resemblesInjectorType( object target )
        {
            return _isAnonymousType( target.GetType() )
                && _hasProperty( target, "Assembly" )
                && _hasProperty( target, "TypeName" );
        }

        // HACK: Imperfect test but the best we can do as far as I can tell.  -JF
        private static bool _isAnonymousType( Type type )
        {
            bool hasCompilerGeneratedAttribute = type.GetCustomAttributes( typeof( CompilerGeneratedAttribute ), false ).Count() > 0;
            bool nameContainsAnonymousType = type.FullName.Contains( "AnonymousType" );
            bool isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;

            return isAnonymousType;
        }

        private static bool _hasProperty( dynamic target, string name )
        {
            return target.GetType().GetProperty( name ) != null;
        }

        private static object _createInstance( InjectorType injectorType )
        {
            ObjectHandle handle = Activator.CreateInstance( injectorType.Assembly, injectorType.TypeName );
            Object target = handle.Unwrap();
            return target;
        }

        private static FieldInfo[] _getFields( Type type )
        {
            FieldInfo[] fields = type.GetFields( BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance );
            if( type.BaseType != typeof( System.Object ) )
            {
                FieldInfo[] baseFields = _getFields( type.BaseType );
                if( baseFields != null )
                {
                    fields = fields.Concat( baseFields ).ToArray();
                }
            }
            return fields;
        }

        private static PropertyInfo _getPropertyFromBase( Type baseType, string propertyName )
        {
            PropertyInfo property = baseType.GetProperty( propertyName );
            if( property == null )
            {
                if( baseType == typeof( System.Object ) )
                {
                    return null;
                }
                else
                {
                    property = _getPropertyFromBase( baseType.BaseType, propertyName );
                }
            }
            return property;
        }
    }
}
