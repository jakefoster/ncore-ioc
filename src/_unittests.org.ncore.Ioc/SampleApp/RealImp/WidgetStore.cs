using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using org.ncore.Ioc;
using _unittests.org.ncore.Ioc;
using _unittests.org.ncore.Ioc.SampleApp.Interfaces;

namespace _unittests.org.ncore.Ioc.SampleApp.RealImp
{
    public class WidgetStore : Dictionary<int, IWidget>, IWidgetStore
    {
        private static WidgetStore _widgetStore;
        public static WidgetStore Current
        {
            get
            {
                if(_widgetStore == null)
                {
                    _widgetStore = new WidgetStore();
                }
                return _widgetStore;
            }
            set
            {
                // NOTE: We really probably wouldn't want to expose this in a 
                //  production system but then again nothing about this sample
                //  code is meant to be used in production (except the IoC
                //  patterns it's illustrating, that is...)  -JF
                _widgetStore = value;
            }
        }
    }
}
