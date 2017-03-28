using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioClone;
using System.Reflection;
using UnitTest.MockClasses;
using Microsoft.Xna.Framework;

namespace UnitTest
{
    [TestClass]
    public class GoombaTest
    {
        private TestContentManager _contentManager;
        private Level _level;
        private Goomba _goomba;

        [TestInitialize]
        public void Init()
        {
            _contentManager = new TestContentManager();
            _level = new Level();
            _goomba = new Goomba(0, 0, _level, _contentManager);
            _level.ToAddGameObject(_goomba);
            _level.UpdateLevel();
        }

        [TestMethod]
        public void Goomba_UpdateHitbox()
        {            
            Rectangle testRect = new Rectangle((int)_goomba.Position.X, (int)_goomba.Position.Y, _goomba.Hitbox.Width, _goomba.Hitbox.Height);
            testRect.Y += (int)_goomba.Gravity;
            _level.UpdateLevel();
            Assert.AreEqual(testRect, _goomba.Hitbox);
        }

        [TestMethod]
        public void Goomba_AddGravity()
        {
            float test = _goomba.VelocityY + _goomba.Gravity;
            _level.UpdateLevel();
            Assert.AreEqual(test, _goomba.VelocityY);
        }

        [TestMethod]
        public void Goomba_CollisionCheck()
        {
            Floor f = new Floor((int)_goomba.Position.X, (int)_goomba.Position.Y + _goomba.Hitbox.Height, 200, 200, _level, _contentManager);
            Floor f2 = new Floor((int)_goomba.Position.X - 16, (int)_goomba.Position.Y, _goomba.Hitbox.Width, _goomba.Hitbox.Height, _level, _contentManager);
            _level.ToAddGameObject(f);
            _level.ToAddGameObject(f2);
            _level.UpdateLevel();
            Assert.AreEqual(2, _goomba.VelocityX);
        }

        [TestMethod]
        public void Goomba_UpdatePosition()
        {
            Vector2 testPos = new Vector2(-2f, 0.3f);
            _level.UpdateLevel();
            bool isTrue = (testPos.X == _goomba.Position.X && testPos.Y == _goomba.Position.Y);
            Assert.IsTrue(isTrue);
        }
    }
}
