using EasyWMI;
using System;     

namespace EasyWMIDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            WMIProcessor wmi = new EasyWMI.WMIProcessor();
            wmi.Request = WMI_ALIAS.CPU;
            wmi.Filter = "name,threadcount,architecture";
            wmi.RemoteExecute = true;
            wmi.NodeName = "10.1.2.8";
            Console.WriteLine(wmi.ExecuteRequest());

            Console.WriteLine();

            wmi.RemoteExecute = false;
            Console.WriteLine(wmi.ExecuteRequest());

            Console.ReadKey();
        }
    }
}
