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
        public static dynamic Instance( string name, dynamic memberRegistry = null, object[] constructorParams = null, bool save = false )
        {
            Injector injector = new Injector()
            {
                ConstructorParams = constructorParams
            };
            if( memberRegistry != null && memberRegistry.GetType() == typeof( InjectorRegistry ) )
            {
                injector.MemberRegistry = memberRegistry;
            }
            else if( memberRegistry != null )
            {
                injector.MemberRegistry = new InjectorRegistry( memberRegistry );
            } 
            return Instance( name, injector, save );
        }

        public static dynamic Instance( string name, Injector injector, bool save = false )
        {
            if( injector == null )
            {
                injector = new Injector();
            }

            dynamic instance;
            if( Locator.Registry.Keys.Contains( name ) )
            {
                // TODO: Highly duplicated code here. Refactor.  -JF
                LocatorType locatorType = Locator.Registry[ name ];
                ObjectHandle handle = Activator.CreateInstance( locatorType.Assembly, locatorType.TypeName,
                                                false, 0, null, injector.ConstructorParams, null, null );
                instance = (dynamic)handle.Unwrap();
                if( save )
                {
                    locatorType.SaveInstance( instance );
                }
            }
            else
            {
                throw new ApplicationException( "The specified name does not refer to a Type object in the Registry." );
            }

            return (dynamic)injector.Inject( instance );
        }

        public static T Instance<T>( dynamic memberRegistry = null, object[] constructorParams = null, bool save = false )
        {
            Injector injector = new Injector()
            {
                ConstructorParams = constructorParams
            };
            if( memberRegistry != null && memberRegistry.GetType() == typeof( InjectorRegistry ) )
            {
                injector.MemberRegistry = memberRegistry;
            }
            else if( memberRegistry != null )
            {
                injector.MemberRegistry = new InjectorRegistry( memberRegistry );
            } 
            return Instance<T>( injector, save );
        }

        public static T Instance<T>( Injector injector, bool save = false )
        {
            if( injector == null )
            {
                injector = new Injector();
            }

            T instance;
            if( Locator.Registry.Keys.Contains( typeof( T ).FullName ) )
            {
                // TODO: Highly duplicated code here. Refactor.  -JF
                LocatorType locatorType = Locator.Registry[ typeof( T ).FullName ];
                ObjectHandle handle = Activator.CreateInstance( locatorType.Assembly, locatorType.TypeName,
                                                false, 0, null, injector.ConstructorParams, null, null );
                instance = (T)handle.Unwrap();
                if( save )
                {
                    locatorType.SaveInstance( instance );
                }
            }
            else
            {
                if(save)
                {
                    throw new ApplicationException( "Cannot save instance because the specified name does not refer to a Type object in the Registry." );
                }
                instance = (dynamic)Activator.CreateInstance( typeof( T ), injector.ConstructorParams );
            }

            return (T)injector.Inject( instance );
        }

        public static T Instance<T>( string name, dynamic memberRegistry = null, object[] constructorParams = null, bool save = false )
        {
            Injector injector = new Injector()
            {
                ConstructorParams = constructorParams
            };
            if( memberRegistry != null && memberRegistry.GetType() == typeof( InjectorRegistry ) )
            {
                injector.MemberRegistry = memberRegistry;
            }
            else if( memberRegistry != null )
            {
                injector.MemberRegistry = new InjectorRegistry( memberRegistry );
            } 
            return Instance<T>( name, injector, save );
        }

        public static T Instance<T>( string name, Injector injector, bool save = false )
        {
            if( injector == null )
            {
                injector = new Injector();
            }

            T instance;
            if( Locator.Registry.Keys.Contains( name ) )
            {
                // TODO: Highly duplicated code here. Refactor.  -JF
                LocatorType locatorType = Locator.Registry[ name ];
                ObjectHandle handle = Activator.CreateInstance( locatorType.Assembly, locatorType.TypeName,
                                                false, 0, null, injector.ConstructorParams, null, null );
                instance = (T)handle.Unwrap();
                if(save)
                {
                    locatorType.SaveInstance( instance );
                }
            }
            else
            {
                throw new ApplicationException( "The specified name does not refer to a Type object in the Registry." );
            }

            return (T)injector.Inject( instance );
        }
    }
}
