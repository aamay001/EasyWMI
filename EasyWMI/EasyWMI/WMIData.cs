using System;
using System.Collections.Generic;

namespace EasyWMI
{
    class WMIData
    {
        private Dictionary< Alias, Dictionary<String,String> > _properties;
        private String _nodeName;

        public WMIData( String nodeName )
        {
            _nodeName = nodeName;
        }


    }
}
