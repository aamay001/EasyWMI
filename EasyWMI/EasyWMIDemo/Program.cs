using EasyWMI;
using System;
using System.Collections.Generic;

namespace EasyWMIDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Request WMI data from a remote machine.
            WMIProcessor wmi = new WMIProcessor();
            wmi.Request = WMI_ALIAS.CPU;
            wmi.Filter = "name,threadcount,architecture";
            wmi.RemoteExecute = true;
            wmi.NodeName = "10.1.2.8";
            Console.WriteLine(wmi.ExecuteRequest());

            Console.WriteLine();
            
            // Change RemoteExecute to false. Executes query on local machine.
            wmi.RemoteExecute = false;
            Console.WriteLine(wmi.ExecuteRequest());

            Console.WriteLine();
            
            // WMIData object with default request + Extra Request.
            WMIData wmiData = new WMIData(true);
            wmiData.GetData(WMI_ALIAS.NETWORK_INTERFACE_CARD_CONFIG, "ipaddress");
            
            foreach ( var currentAlias in wmiData.Properties )
            {
                Console.WriteLine(currentAlias.Key.Value);
                foreach( var currentProperty in wmiData.Properties[currentAlias.Key] )
                {
                    Console.WriteLine("{0} : {1}", currentProperty.Key, currentProperty.Value);
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }
}
