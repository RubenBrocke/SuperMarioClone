using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioClone;
using System.Reflection;
using UnitTest.MockClasses;
using Microsoft.Xna.Framework;

namespace UnitTest
{
    [TestClass]
    public class KoopaTest
    {
        private TestContentManager _contentManager;
        private Level _level;
        private Koopa _koopa;

        [TestInitialize]
        public void Init()
        {
            _contentManager = new TestContentManager();
            _level = new Level();
            _koopa = new Koopa(0, 0, _level, _contentManager);
            _level.ToAddGameObject(_koopa);
            _level.UpdateLevel();
        }

        [TestMethod]
        public void Koopa_UpdateHitbox()
        {            
            Rectangle testRect = new Rectangle((int)_koopa.Position.X, (int)_koopa.Position.Y, _koopa.Hitbox.Width, _koopa.Hitbox.Height);
            testRect.Y += (int)_koopa.Gravity;
            _level.UpdateLevel();
            Assert.AreEqual(testRect, _koopa.Hitbox);
        }

        [TestMethod]
        public void Koopa_AddGravity()
        {
            float test = _koopa.VelocityY + _koopa.Gravity;
            _level.UpdateLevel();
            Assert.AreEqual(test, _koopa.VelocityY);
        }

        [TestMethod]
        public void Koopa_CollisionCheck()
        {
            Floor f = new Floor((int)_koopa.Position.X, (int)_koopa.Position.Y + _koopa.Hitbox.Height, 200, 200, _level, _contentManager);
            Floor f2 = new Floor((int)_koopa.Position.X / 16 - 1, (int)_koopa.Position.Y / 16, _koopa.Hitbox.Width / 16, _koopa.Hitbox.Height / 16, _level, _contentManager);
            _level.ToAddGameObject(f);
            _level.ToAddGameObject(f2);
            _level.UpdateLevel();
            _level.UpdateLevel();
            Assert.AreEqual(0.5, _koopa.VelocityX);
        }

        [TestMethod]
        public void Koopa_UpdatePosition()
        {
            Vector2 testPos = new Vector2(_koopa.Position.X + 1f, 0.9f);
            _level.UpdateLevel();
            bool isTrue = ((int)testPos.X == (int)_koopa.Position.X && (int)testPos.Y == (int)_koopa.Position.Y);
            Assert.IsTrue(isTrue);
        }
    }
}
