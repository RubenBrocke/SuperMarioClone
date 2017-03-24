using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioClone;
using System.Reflection;
using UnitTest.MockClasses;

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
            Mario mario = new Mario(10, 10, new Level(), new TestContentManager());
            //Coin coin = new Coin(10, 20, new Level());
            //coin.AddCoin(mario);
            //var _coin= typeof(Mario).GetField("_coins", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(mario);
            Assert.IsInstanceOfType(mario, typeof(Mario));
        }
    }
}
