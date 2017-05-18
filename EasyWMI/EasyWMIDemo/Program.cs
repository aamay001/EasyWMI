using EasyWMI;
using System;     

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

            Console.ReadKey();
        }
    }
}
