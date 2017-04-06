using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarioClone;
using UnitTest.MockClasses;

namespace UnitTest
{
    [TestClass]
    public class MarioTest
    {
        private TestContentManager _contentManager;
        private Level _level;
        private Mario _mario;

        [TestInitialize]
        public void Init()
        {
            _contentManager = new TestContentManager();
            _level = new Level();
            _mario = new Mario(0, 0, _level, _contentManager);
            _level.ToAddGameObject(_mario);
            _level.UpdateLevel();
        }

        [TestMethod]
        public void Mario_UpdateHitbox()
        {
            Rectangle testRect = new Rectangle((int)_mario.Position.X + 1, (int)_mario.Position.Y + 12, _mario.Hitbox.Width, _mario.Hitbox.Height);
            testRect.Y += (int)_mario.Gravity;
            _level.UpdateLevel();
            Assert.AreEqual(testRect, _mario.Hitbox);
        }

        [TestMethod]
        public void Mario_AddGravity()
        {
            float test = _mario.VelocityY + _mario.Gravity;
            _level.UpdateLevel();
            Assert.AreEqual(test, _mario.VelocityY);
        }

        [TestMethod]
        public void Mario_UpdatePosition()
        {
            Vector2 testPos = new Vector2(_mario.Position.X, 0);
            _level.UpdateLevel();
            bool isTrue = (testPos.X == (int)_mario.Position.X && testPos.Y == (int)_mario.Position.Y);
            Assert.IsTrue(isTrue);
        }
    }
}
