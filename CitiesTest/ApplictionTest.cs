using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cities_Latitude_Longitude;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace CitiesTest
{
    [TestClass]
    public class ApplicationTest
    {
        [TestMethod]
        public void TestGetApplicationRootPath()
        {
            string actualPath = Application.GetSolutionRootPath();
            //TODO please change the application root path for the test to work
            string expected = @"C:\Users\indri\source\repos\Cities_Latitude_Longitude\";
            Assert.AreEqual(expected, actualPath);
        }    
    }
   
}
