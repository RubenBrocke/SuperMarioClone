using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioClone;
using System.Reflection;
using UnitTest.MockClasses;

namespace UnitTest
{
    [TestClass]
    public class MysteryBlockTest
    {
        private TestContentManager _contentManager;
        private Level _level;
        private MysteryBlock _mysteryBlock;

        [TestInitialize]
        public void Init()
        {
            _contentManager = new TestContentManager();
            _level = new Level();
            _mysteryBlock = new MysteryBlock(0, 0, typeof(Coin), _level, _contentManager);
            _level.ToAddGameObject(_mysteryBlock);
            _level.UpdateLevel();
        }

        [TestMethod]
        public void MysteryBlock_EjectCoin()
        {
            _mysteryBlock = new MysteryBlock(0, 0, typeof(Coin), _level, _contentManager);
            Mario m = new Mario(0, 1, _level, _contentManager);
            m.Jump();
            _mysteryBlock.Eject(m);
            _level.UpdateLevel();
            _level.UpdateLevel();
            bool isTrue = false;
            foreach (GameObject g in _level.GameObjects)
            {
                if (g.GetType() == typeof(Coin))
                {
                    isTrue = true;
                }
            }
            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void MysteryBlock_EjectMushroom()
        {
            _mysteryBlock = new MysteryBlock(0, 0, typeof(Mushroom), _level, _contentManager);
            Mario m = new Mario(0, 1, _level, _contentManager);
            m.Jump();
            _mysteryBlock.Eject(m);
            _level.UpdateLevel();
            _level.UpdateLevel();
            bool isTrue = false;
            foreach (GameObject g in _level.GameObjects)
            {
                if (g.GetType() == typeof(Mushroom))
                {
                    isTrue = true;
                }
            }
            Assert.IsTrue(isTrue);
        }
    }
}
