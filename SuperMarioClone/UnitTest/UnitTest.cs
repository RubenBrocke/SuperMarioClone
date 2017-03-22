using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioClone;
using System.Reflection;

namespace UnitTest
{
    [TestClass]
    public class CoinTest
    {

        [TestInitialize]

        static void Init()
        {
          
        }
        [TestMethod]
        public void AddCoinTest()
        {
           ContentManager c = new ContentManager();
            Mario m = new Mario(0, 0, new Level(), c);
        }
    }
}
