using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.MockClasses
{   
    [TestClass]
    class TestItems
    {
        public static TestGraphicsDeviceService tgds;
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext tc)
        {
            tgds = new TestGraphicsDeviceService();

        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            tgds.Release();
        }
    }
}
