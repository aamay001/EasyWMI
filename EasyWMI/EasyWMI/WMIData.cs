/*************************************************************************************************************************
** Object: WMIData Class
** Name: EasyWMI.WMIData.cs
** Date: 5/18/2017
** Author: Andy Amaya

License: EasyWMI

Copyright © 2017 Andy S. Amaya

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions 
of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
DEALINGS IN THE SOFTWARE.				 

** Edit Date		Edited By		Changes
** 5/18/2017		Andy Amaya		Inital Version
*************************************************************************************************************************/
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System;

namespace EasyWMI
{
    /// <summary>
    /// WMIData class. Wrapper for WMIProcess to manage data requests.
    /// </summary>
    public class WMIData
    {
        const int PROPERTY = 0;
        const int VALUE = 1;

        private Dictionary<Alias, Dictionary<String,String>> _properties;
        private String _nodeName;
        private bool _defaultPopulated;
        private bool _isRemote;
        
        /// <summary>
        /// Properties dictionary contains all data that is requested.
        /// </summary>
        public Dictionary<Alias, Dictionary<String, String>> Properties
        {
            get
            {
                return _properties;
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="getDefault">Optional parameter. If true will get default wmi data.</param>
        public WMIData( bool getDefault = false )
        {
            _nodeName = Environment.MachineName;
            _isRemote = false;
            InitializeProperties();
            if ( getDefault )
                GetDefault();
        }

        /// <summary>
        /// Constructor with node name (or IP Address) and option to make node remote.
        /// </summary>
        /// <param name="nodeName">Name (or IP Address) of remote node to get data for.</param>
        /// <param name="getDefault">Optional parameter. If true will get default wmi data.</param>
        /// <param name="remote">Optional parameter. If true, data will be requested over network.</param>
        public WMIData( String nodeName , bool getDefault = false, bool remote = false )
        {
            _nodeName = nodeName;
            _isRemote = remote;
            InitializeProperties();

            if ( getDefault )
                GetDefault();            
        }

        /// <summary>
        /// Executes requests for default wmi data. Default data is: 
        /// 1.) CSPRODUCT name and vendor
        /// 2.) OPERATING_SYSTEM caption and osarchitecture
        /// </summary>
        public void GetDefault()
        {
            if (!_defaultPopulated)
            {
                _defaultPopulated = true;
                KeyValuePair<Alias, String> [] defaultRequests =
                {
                    new KeyValuePair<Alias, String>( WMI_ALIAS.CSPRODUCT, "name,vendor" ) ,
                    new KeyValuePair<Alias, String>( WMI_ALIAS.OPERATING_SYSTEM, "caption,osarchitecture" )
                };

                Parallel.ForEach(defaultRequests, request => 
                {
                    WMIProcessor wmi = new WMIProcessor(request.Key, request.Value, _isRemote);
                    _properties.Add(wmi.Request, ParseWMIOutput(wmi.ExecuteRequest()));
                                                                               
                });
            }
        }

        /// <summary>
        /// Request additional wmi data using WMI_ALIAS and optional filter.
        /// </summary>
        /// <param name="request">WMI alias for type of data to request.</param>
        /// <param name="filter">Optional parameter. Filter results with a comma separated list of properties. Empty filter returns all data for alias.</param>
        public void GetData( Alias request, String filter = "" )
        {
            if (request == null)
                return;

            if (!_defaultPopulated)
            {
                GetDefault();
            }

            filter = DeduplicateFilter(request, filter);
            
            WMIProcessor wmi = new WMIProcessor(request, filter, _isRemote);
            if (_properties.ContainsKey(request))
            {
                Parallel.ForEach(ParseWMIOutput(wmi.ExecuteRequest()), currentData =>
                {
                    // If property for alias already exists, update it.
                    if (_properties[request].ContainsKey(currentData.Key))
                        _properties[request][currentData.Key] = currentData.Value;

                    else
                        _properties[request].Add(currentData.Key, currentData.Value);
                });                                                                              
            }

            else
            {
                _properties.Add(request, ParseWMIOutput(wmi.ExecuteRequest()));
            }          
        }

        #region Utility Methods

        /// <summary>
        /// Initializes the properties dictionay and sets the defulatPopulated flag to false 
        /// to indicate that the default values have not been requested.
        /// </summary>
        private void InitializeProperties()
        {
            _properties = new Dictionary<Alias, Dictionary<String, String>>();
            _defaultPopulated = false;
        }

        /// <summary>
        /// Parses the request data into a dictionary.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Dictionary<String,String> ParseWMIOutput( String data )
        {
            Dictionary<String, String> _data = new Dictionary<String, String>(StringComparer.OrdinalIgnoreCase);

            using ( StringReader sr = new StringReader(data) )
            {
                while (true)
                {
                    String line = sr.ReadLine();

                    if (String.IsNullOrEmpty(line))
                    {
                        if (line == null)
                            break;

                        continue;
                    }

                    String[] propVal = line.Split('=');

                    if ( !String.IsNullOrEmpty(propVal[PROPERTY]) &&
                         !String.IsNullOrEmpty(propVal[VALUE]))
                    {
                        if ( _data.ContainsKey( propVal[PROPERTY]) )
                        {
                            propVal[PROPERTY] = String.Concat( propVal[PROPERTY] , '-' , _data.Keys.Where(x => x.StartsWith(propVal[PROPERTY]) ).Count() );
                        }

                        _data.Add(propVal[PROPERTY], propVal[VALUE]);
                    }
                }
            }

            return _data;
        }
        
        /// <summary>
        /// Parses and trims the filter keys in the filter string.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        private String [] ParseFilter( String filter )
        {
            if (String.IsNullOrEmpty(filter))
                return new String[] { };

            else if ( !filter.Contains(',') )
                return new String [] { filter };

            String [] filterKeys = filter.Split(',');
            Parallel.ForEach(filterKeys, (currentKey) => { currentKey = currentKey.Trim(); });
            return filterKeys;
        }

        /// <summary>
        /// Removed duplicate filter keys and keys that have already been requested.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private String DeduplicateFilter( Alias request, String filter )
        {
            List<String> uniqueFilters = new List<String>();

            if (_properties.ContainsKey(request))
            {
                Parallel.ForEach(ParseFilter(filter), currentFilter =>
               {
                   if ( !_properties[request].ContainsKey(currentFilter) && 
                        !uniqueFilters.Contains(currentFilter) )
                       uniqueFilters.Add(currentFilter);
               });
            }
            else
            {
                Parallel.ForEach(ParseFilter(filter), currentFilter =>
                {
                    uniqueFilters.Add(currentFilter);  
                });
            }

            String newFilter = String.Empty;
            
            for ( int i = 0; i < uniqueFilters.Count; i++ )
            {
                newFilter = newFilter + ( i > 0 ? "," : "" ) + uniqueFilters[i];
            }

            return newFilter;           
        }

        #endregion

#if DEBUG
        public Dictionary<String,String> AssertParseWMIOutput( String data )
        {
            return ParseWMIOutput(data);
        }

        public String AssertDeduplicateFilter( Alias request, String filter )
        {
            return DeduplicateFilter(request, filter);
        }
#endif

    }
}
