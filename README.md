# EasyWMI

EasyWMI is a C# Class Library that interfaces with Windows Management Instrumentation (WMI). The intended purpose for this class library is to provide an easy method to query WMI via C#.  

Supported Alias(es) can be found here:
- [Useful WMIC Queries](https://blogs.technet.microsoft.com/askperf/2012/02/17/useful-wmic-queries/)

## Easy Syntax

### WMIData.cs
The [WMIData](https://github.com/aamay001/EasyWMI/blob/master/EasyWMI/EasyWMI/WMIData.cs) class is a wrapper for the [WMIProcessor](https://github.com/aamay001/EasyWMI/blob/master/EasyWMI/EasyWMI/WMIProcessor.cs) class; it gives you a managed use of the WMIProcessor object.
```c#

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

```

### WMIProcessor.cs
The [WMIProcessor](https://github.com/aamay001/EasyWMI/blob/master/EasyWMI/EasyWMI/WMIProcessor.cs) class uses a [Process](https://msdn.microsoft.com/en-us/library/system.diagnostics.process(v=vs.110).aspx) object to execute the request.
```c#

// Request WMI data from a remote machine.
WMIProcessor wmi = new WMIProcessor();
wmi.Request = WMI_ALIAS.CPU;
wmi.Filter = "name,threadcount,architecture";
wmi.RemoteExecute = true;
wmi.NodeName = "10.1.2.8";
String result = wmi.ExecuteRequest();

// Change RemoteExecute to false. Executes query on local machine.
wmi.RemoteExecute = false;
String result = wmi.ExecuteRequest();

// Shorthand Remote 
WMIProcessor wmi = new WMIProcessor("10.1.2.8", WMI_ALIAS.CPU, "name,threadcount,architecture", true);
String result = wmi.ExecuteRequest();

// Shorthand Local
WMIProcessor wmi = new WMIProcessor(WMI_ALIAS.CPU, "name,threadcount,architecture");
String result = wmi.ExecuteRequest();

```
<span stye="color:red; font-weight:bold;">Filter is optional for all cases.</span>
