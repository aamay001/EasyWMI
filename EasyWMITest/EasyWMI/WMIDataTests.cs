using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyWMI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWMI.Tests
{
    [TestClass()]
    public class WMIDataTests
    {
        [TestMethod]
        public void PropertyValueParse()
        {
            WMIData wmiData = new WMIData();
            WMIProcessor wmi = new WMIProcessor(WMI_ALIAS.CPU, "name");
            Dictionary<String, String> result = wmiData.AssertParseWMIOutput(wmi.ExecuteRequest());
            Assert.AreEqual("Intel(R) Core(TM) i7-4790 CPU @ 3.60GHz", result["name"]);
        }

        [TestMethod()]
        public void GetDefaultTest()
        {
            WMIData wmiData = new WMIData(true);
            Assert.AreEqual("Hewlett-Packard", wmiData.Properties[WMI_ALIAS.CSPRODUCT]["vendor"]);
        }

        [TestMethod()]
        public void GetDefaultTest2()
        {
            WMIData wmiData = new WMIData(true);
            Assert.AreEqual("Name", wmiData.Properties[WMI_ALIAS.CSPRODUCT].Keys.ElementAt(0));
        }

        [TestMethod()]
        public void DeduplicateFilterTest()
        {
            WMIData wmiData = new WMIData(true);
            Assert.AreEqual("description", wmiData.AssertDeduplicateFilter(WMI_ALIAS.CSPRODUCT, "name,vendor,description"));
        }

        [TestMethod()]
        public void AssertDeduplicateFilterTest2()
        {
            WMIData wmiData = new WMIData(true);
            wmiData.GetData(WMI_ALIAS.CSPRODUCT, "description");
            Assert.AreEqual("", wmiData.AssertDeduplicateFilter(WMI_ALIAS.CSPRODUCT, "name,vendor,description"));
        }

        [TestMethod()]
        public void GetDataTest()
        {
            WMIData wmiData = new WMIData(true);
            wmiData.GetData(WMI_ALIAS.CSPRODUCT, "description");
            Assert.AreEqual("Computer System Product", wmiData.Properties[WMI_ALIAS.CSPRODUCT]["description"]);
        }
    }
}