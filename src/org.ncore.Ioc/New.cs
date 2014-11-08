using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace org.ncore.Ioc
{
    public static class New
    {
        public static dynamic Instance( Type type, dynamic memberRegistry = null, object[] constructorParams = null )
        {
            Injector injector = new Injector()
            {
                ConstructorParams = constructorParams
            };
            if( memberRegistry.GetType() == typeof( InjectorRegistry ) )
            {
                injector.MemberRegistry = memberRegistry;
            }
            else
            {
                injector.MemberRegistry = new InjectorRegistry( memberRegistry );
            } 
            return Instance( type, injector );
        }

        public static dynamic Instance( Type type, Injector injector = null)
        {
            if( injector == null )
            {
                injector = new Injector();
            }

            dynamic instance;
            if( Kernel.Registry.Keys.Contains( type.FullName ) )
            {
                instance = Kernel.CreateObject<dynamic>( type.FullName, injector.ConstructorParams );
            }
            else
            {
                instance = (dynamic)Activator.CreateInstance( type, injector.ConstructorParams );
            }

            return (dynamic)injector.Inject( instance );
        }

        public static dynamic Instance( string name, dynamic memberRegistry = null, object[] constructorParams = null )
        {
            Injector injector = new Injector()
            {
                ConstructorParams = constructorParams
            };
            if( memberRegistry.GetType() == typeof( InjectorRegistry ) )
            {
                injector.MemberRegistry = memberRegistry;
            }
            else
            {
                injector.MemberRegistry = new InjectorRegistry( memberRegistry );
            } 
            return Instance( name, injector );
        }

        public static dynamic Instance( string name, Injector injector = null )
        {
            if( injector == null )
            {
                injector = new Injector();
            }

            dynamic instance;
            if( Kernel.Registry.Keys.Contains( name ) )
            {
                instance = Kernel.CreateObject<dynamic>( name, injector.ConstructorParams );
            }
            else
            {
                throw new ApplicationException( "The specified name does not refer to a Type object in the Registry." );
            }

            return (dynamic)injector.Inject( instance );
        }

        public static T Instance<T>( dynamic memberRegistry = null, object[] constructorParams = null )
        {
            Injector injector = new Injector()
            {
                ConstructorParams = constructorParams
            };
            if( memberRegistry.GetType() == typeof( InjectorRegistry ) )
            {
                injector.MemberRegistry = memberRegistry;
            }
            else
            {
                injector.MemberRegistry = new InjectorRegistry( memberRegistry );
            } 
            return Instance<T>( injector );
        }

        public static T Instance<T>( Injector injector = null )
        {
            if( injector == null )
            {
                injector = new Injector();
            }

            T instance;
            if( Kernel.Registry.Keys.Contains( typeof( T ).FullName ) )
            {
                instance = Kernel.CreateObject<T>( injector.ConstructorParams );
            }
            else
            {
                instance = (dynamic)Activator.CreateInstance( typeof( T ), injector.ConstructorParams );
            }

            return (T)injector.Inject( instance );
        }

        public static T Instance<T>( string name, dynamic memberRegistry = null, object[] constructorParams = null )
        {
            Injector injector = new Injector()
            {
                ConstructorParams = constructorParams
            };
            if( memberRegistry.GetType() == typeof(InjectorRegistry) )
            {
                injector.MemberRegistry = memberRegistry;
            }
            else
            {
                injector.MemberRegistry = new InjectorRegistry( memberRegistry );
            }            
            return Instance<T>( name, injector );
        }

        public static T Instance<T>( string name, Injector injector = null )
        {
            if( injector == null )
            {
                injector = new Injector();
            }

            T instance;
            if( Kernel.Registry.Keys.Contains( name ) )
            {
                instance = Kernel.CreateObject<dynamic>( name, injector.ConstructorParams );
            }
            else
            {
                throw new ApplicationException( "The specified name does not refer to a Type object in the Registry." );
            }

            return (T)injector.Inject( instance );
        }

        public static Service Service( Type type )
        {
            return new Service( type );
        }

        public static Service Service( string name )
        {
            return new Service( name );
        }
    }
}
