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
    public class CloudBlockTest
    {
        private TestContentManager _contentManager;
        private Level _level;
        private CloudBlock _cloudBlock;

        [TestInitialize]
        public void Init()
        {
            _contentManager = new TestContentManager();
            _level = new Level();
            _cloudBlock = new CloudBlock(1, 1, _level, _contentManager);
        }

        [TestMethod]
        public void CloudBlock_CloudBlockk()
        {
            Assert.AreEqual(_cloudBlock.Position, new Vector2(16, 16));
            Assert.AreEqual(_cloudBlock.Hitbox, new Rectangle(16, 16, 16, 16));
            Assert.AreEqual(_cloudBlock.CurrentLevel, _level);
        }
    }
}