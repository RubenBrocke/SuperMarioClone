using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using SuperMarioClone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.MockClasses;

namespace UnitTest
{
    [TestClass]
    public class StoneBlockTest
    {
        private TestContentManager _contentManager;
        private Level _level;
        private StoneBlock _stoneBlock;

        [TestInitialize]
        public void Init()
        {
            _contentManager = new TestContentManager();
            _level = new Level();
            _stoneBlock = new StoneBlock(1, 1, _level, _contentManager);
        }

        [TestMethod]
        public void StoneBlock_StoneBlock()
        {
            Assert.AreEqual(_stoneBlock.Position, new Vector2(16, 16));
        }
    }
}