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
            using (var testGame = new MainGame())
            {
                testGame.Run();
            }
        }
        [TestMethod]
        public void AddCoinTest()
        {
            Mario mario = new Mario(10, 10, new Level());
            Coin coin = new Coin(10, 20, new Level());
            coin.AddCoin(mario);
            var _coin= typeof(Mario).GetField("_coins", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(mario);
            Assert.AreEqual(1, _coin);
        }
    }
}
