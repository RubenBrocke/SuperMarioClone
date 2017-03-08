using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioClone;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Mario jeMarioIsEenPlopkoek = new Mario(10, 10, new Level());
        }
    }
}
