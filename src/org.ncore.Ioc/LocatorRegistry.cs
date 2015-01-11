using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace org.ncore.Ioc
{
    public class LocatorRegistry : ConcurrentDictionary<string, LocatorType>
    {
        public LocatorRegistry() : base()
        {
            _initialize();
        }

        public void Add( LocatorType locatorType )
        {
            if( locatorType.Name.Contains( '*' ) )
            {
                _expandWildcard( locatorType );
            }
            else
            {
                bool success = this.TryAdd( locatorType.Name, locatorType );
                // NOTE: If this fails we *really* want it to pop otherwise our app will likely be totally 
                //  mis-configured.  In fact, we should really probably have a custom exception type for it. -JF
                if( !success )
                {
                    throw new ApplicationException( "Could not add LocatorType '" + locatorType.Name + "' to LocatorRegistry." );
                }
            }
        }

        public void Update( LocatorType original, LocatorType replacement )
        {
            bool success = this.TryUpdate( original.Name, replacement, original );
            // NOTE: If this fails we *really* want it to pop otherwise our app will likely be totally 
            //  mis-configured.  In fact, we should really probably have a custom exception type for it. -JF
            if( !success )
            {
                throw new ApplicationException( "Could not update LocatorType '" + original.Name + "' in LocatorRegistry." );
            }
        }

        private void _initialize()
        {
            lock( this )
            {
                LocatorConfiguration configuration = (LocatorConfiguration)ConfigurationManager.GetSection( "locator" );
                foreach( TypeElement element in configuration.Types )
                {
                    LocatorType entry = new LocatorType()
                    {
                        Name = element.Name,
                        Assembly = element.Assembly,
                        TypeName = element.TypeName,
                        AllowSave = element.AllowSave,
                    };

                    if( entry.Name.Contains( '*' ) )
                    {
                        _expandWildcard( entry );
                    }
                    else
                    {
                        this.Add( entry );
                    }
                }
            }
        }

        private void _expandWildcard( LocatorType wildcard )
        {
            lock( this )
            {
                Assembly assembly = Assembly.Load( wildcard.Assembly );
                foreach( var type in assembly.GetTypes() )
                {
                    if( type.Namespace == wildcard.TypeName )
                    {
                        LocatorType entry = new LocatorType()
                        {
                            Name = wildcard.Name.Replace( "*", type.Name ),
                            Assembly = wildcard.Assembly,
                            TypeName = type.FullName,
                            AllowSave = wildcard.AllowSave
                        };
                        this.Add( entry );
                    }
                }
            }
        }
    }
}
