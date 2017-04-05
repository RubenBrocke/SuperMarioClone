using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioClone;
using System.Reflection;
using UnitTest.MockClasses;
using Microsoft.Xna.Framework;

namespace UnitTest
{
    [TestClass]
    public class MunsherTest
    {
        private TestContentManager _contentManager;
        private Level _level;
        private Muncher _muncher;

        [TestInitialize]
        public void Init()
        {
            _contentManager = new TestContentManager();
            _level = new Level();
            _muncher = new Muncher(1, 1, _level, _contentManager);
        }

        [TestMethod]
        public void CloudBlock_CloudBlockk()
        {
            Assert.AreEqual(_muncher.Position, new Vector2(16, 16));
            Assert.AreEqual(_muncher.Hitbox, new Rectangle(16, 16, 16, 16));
            Assert.AreEqual(_muncher.CurrentLevel, _level);
        }
    }
}
