using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioClone;
using System.Reflection;
using UnitTest.MockClasses;
using Microsoft.Xna.Framework;

namespace UnitTest
{
    [TestClass]
    public class MushroomTest
    {
        private TestContentManager _contentManager;
        private Level _level;
        private Mushroom _Mushroom;

        [TestInitialize]
        public void Init()
        {
            _contentManager = new TestContentManager();
            _level = new Level();
            _Mushroom = new Mushroom(0, 0, _level, _contentManager);
            _level.ToAddGameObject(_Mushroom);
            _level.UpdateLevel();
        }

        [TestMethod]
        public void Mushroom_UpdateHitbox()
        {            
            Rectangle testRect = new Rectangle((int)_Mushroom.Position.X, (int)_Mushroom.Position.Y, _Mushroom.Hitbox.Width, _Mushroom.Hitbox.Height);
            testRect.Y += (int)_Mushroom.Gravity;
            _level.UpdateLevel();
            Assert.AreEqual(testRect, _Mushroom.Hitbox);
        }

        [TestMethod]
        public void Mushroom_AddGravity()
        {
            float test = _Mushroom.VelocityY + _Mushroom.Gravity;
            _level.UpdateLevel();
            Assert.AreEqual(test, _Mushroom.VelocityY);
        }

        [TestMethod]
        public void Mushroom_CollisionCheck()
        {
            Floor f = new Floor((int)_Mushroom.Position.X, (int)_Mushroom.Position.Y + _Mushroom.Hitbox.Height, 200, 200, _level, _contentManager);
            Floor f2 = new Floor((int)_Mushroom.Position.X / 16 - 1, (int)_Mushroom.Position.Y / 16, _Mushroom.Hitbox.Width / 16, _Mushroom.Hitbox.Height / 16, _level, _contentManager);
            _level.ToAddGameObject(f);
            _level.ToAddGameObject(f2);
            _level.UpdateLevel();
            _level.UpdateLevel();
            Assert.AreEqual(-1.5, _Mushroom.VelocityX);
        }

        [TestMethod]
        public void Mushroom_UpdatePosition()
        {
            Vector2 testPos = new Vector2(_Mushroom.Position.X + 1.5f, 2.9f);
            _level.UpdateLevel();
            bool isTrue = ((int)testPos.X == (int)_Mushroom.Position.X && (int)testPos.Y == (int)_Mushroom.Position.Y);
            Assert.IsTrue(isTrue);
        }
    }
}
