using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;

namespace org.ncore.Ioc
{
    // TODO: Should this be changed to a ConcurrentDictionary to ensure thread safety?  -JF
    public class InjectorRegistry : IDictionary<string, object>
    {
        private Dictionary<string, object> _dictionary;

        // TODO: Not sure I like this approach at all.  -JF
        public void Add( InjectorType value )
        {
            _dictionary.Add( value.TypeName, value );
        }

        public void Add( string key, object value )
        {
            _dictionary.Add( key, value );
        }

        public void Add( object key, object value )
        {
            string derivedKey = string.Empty;
            if( key is Type )
            {
                derivedKey = ( (Type)key ).FullName;
            }
            else
            {
                derivedKey = key.ToString();
            }
            _dictionary.Add( derivedKey, value );
        }

        public InjectorRegistry()
            : base()
        {
            this._initialize();
        }

        // NOTE: Allows the caller to pass in an anonymous type where
        //  the properties are the name of the InjectoryRegistry key
        //  and the values are the InjectoryRegistry value. It's just
        //  a nice shorthand for the InjectorRegistry/Syntax.  -JF
        //    new
        //    {
        //        Name = "Nike FuelBand",
        //        Color = "teal",
        //        Size = 3
        //    }
        public InjectorRegistry( dynamic items )
            : base()
        {
            this._initialize();

            PropertyInfo[] properties = items.GetType().GetProperties( BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance );

            foreach( PropertyInfo property in properties )
            {
                this._dictionary.Add( property.Name, property.GetValue( items ) );
            }
        }

        // NOTE: Honestly, this should really only be used for unit testing.  -JF
        public void Reset()
        {
            this._initialize();
        }

        private void _initialize()
        {
            lock( this )
            {
                _dictionary = new Dictionary<string, object>();
            }
        }

        #region IDictionary<string,object> Members


        public bool ContainsKey( string key )
        {
            return _dictionary.ContainsKey( key );
        }

        public ICollection<string> Keys
        {
            get { return _dictionary.Keys; }
        }

        public bool Remove( string key )
        {
            return _dictionary.Remove( key );
        }

        public bool TryGetValue( string key, out object value )
        {
            return _dictionary.TryGetValue( key, out value );
        }

        public ICollection<object> Values
        {
            get { return _dictionary.Values; }
        }

        public object this[ string key ]
        {
            get
            {
                return _dictionary[ key ];
            }
            set
            {
                _dictionary[ key ] = value;
            }
        }

        #endregion

        #region ICollection<KeyValuePair<string, object>> Members

        public void Add( KeyValuePair<string, object> item )
        {
            ( (ICollection<KeyValuePair<string, object>>)_dictionary ).Add( item );
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains( KeyValuePair<string, object> item )
        {
            return _dictionary.Contains( item );
        }

        public void CopyTo( KeyValuePair<string, object>[] array, int arrayIndex )
        {
            ( (ICollection<KeyValuePair<string, object>>)_dictionary ).CopyTo( array, arrayIndex );
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return ( (ICollection<KeyValuePair<string, object>>)_dictionary ).IsReadOnly; }
        }

        public bool Remove( KeyValuePair<string, object> item )
        {
            return ( (ICollection<KeyValuePair<string, object>>)_dictionary ).Remove( item );
        }

        #endregion

        #region IEnumerable<KeyValuePair<string, object>> Members

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        #endregion
    }
}

