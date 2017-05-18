using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyWMI;

namespace EasyWMITest
{
    [TestClass]
    public class WMIProcessorTests
    {
        [TestMethod]
        public void ShellPath()
        {
            String expected = "C:\\WINDOWS\\system32";                 
            WMIProcessor wmip = new WMIProcessor();
            Assert.AreEqual(expected, wmip.AssertShellPath());                    
        }

        [TestMethod]
        public void Arguments()
        {
            String expected = "/C wmic os get filter /format:list";
            WMIProcessor wmi = new WMIProcessor(WMI_ALIAS.OPERATING_SYSTEM, "filter", false);
            Assert.AreEqual(expected, wmi.AssertArguments());
        }

        [TestMethod]
        public void ArgumentsRemote()
        {
            String expected = "/C wmic /node:10.1.2.8 os get filter /format:list";
            WMIProcessor wmi = new WMIProcessor( "10.1.2.8", WMI_ALIAS.OPERATING_SYSTEM, "filter", false);
            wmi.RemoteExecute = true;
            Assert.AreEqual(expected, wmi.AssertArguments());
        }

        [TestMethod]
        public void ExecuteTask()
        {
            WMIProcessor wmip = new WMIProcessor(WMI_ALIAS.OPERATING_SYSTEM);    
            Assert.IsNotNull(wmip.ExecuteRequest());
        }

        [TestMethod]
        public void GetOSCaption()
        {
            WMIProcessor wmi = new WMIProcessor(WMI_ALIAS.OPERATING_SYSTEM, "caption", false);
            Assert.AreEqual("Caption=Microsoft Windows 10 Enterprise", wmi.ExecuteRequest());
        }
    }
}
