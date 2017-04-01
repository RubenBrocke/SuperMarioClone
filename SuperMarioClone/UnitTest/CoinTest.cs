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
        private TestContentManager _contentManager;
        private Level _level;
        private Coin _coin;
        private Mario _mario;

        [TestInitialize]
        public void Init()
        {
            _contentManager = new TestContentManager();
            _level = new Level();
            _mario = new Mario(1, 1, _level, _contentManager);
            _coin = new Coin(1, 1, _level, _contentManager);
            _level.ToAddGameObject(_mario);
            _level.ToAddGameObject(_coin);
            _level.UpdateLevel();
        }

        [TestMethod]
        public void Coin_AddCoin()
        {
            _coin.AddCoin(_mario);
            Assert.AreEqual(_mario.Coins, 1);
        }

        [TestMethod]
        public void Coin_AddMysteryCoin()
        {
            _coin.IsMysteryCoin = true;
            _coin.AddCoin(_mario);
            Assert.AreEqual(_mario.Coins, 1);
        }

        [TestMethod]
        public void Coin_DeleteCoin()
        {
            _coin.DeleteCoin(null);
            _level.UpdateLevel();
            Assert.IsFalse(_level.GameObjects.Contains(_coin));
        }
    }
}
