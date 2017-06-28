using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.MockClasses;
using SuperMarioClone;
using Microsoft.Xna.Framework;

namespace UnitTest
{
    [TestClass]
    public class FallingSpikeTest
    {
        private TestContentManager _contentManager;
        private Level _level;
        private FallingSpike _fallingSpike;

        [TestInitialize]
        public void Init()
        {
            _contentManager = new TestContentManager();
            _level = new Level();
            _fallingSpike = new FallingSpike(0, 0, _level, _contentManager);
            _level.ToAddGameObject(_fallingSpike);
            _level.UpdateLevel();
        }

        [TestMethod]
        public void FallingSpike_UpdateHitbox()
        {
            Rectangle testRect = new Rectangle((int)_fallingSpike.Position.X, (int)_fallingSpike.Position.Y, _fallingSpike.Hitbox.Width, _fallingSpike.Hitbox.Height);
            testRect.Y += (int)_fallingSpike.Gravity;
            _level.UpdateLevel();
            Assert.AreEqual(testRect, _fallingSpike.Hitbox);
        }

        [TestMethod]
        public void FallingSpike_AddGravity()
        {
            float test = _fallingSpike.VelocityY + _fallingSpike.Gravity;
            _level.UpdateLevel();
            Assert.AreEqual(test, _fallingSpike.VelocityY);
        }

        [TestMethod]
        public void FallingSpike_CheckFall()
        {
            bool test = true;
            _level.UpdateLevel();
            Assert.AreEqual(test, _fallingSpike._goFall);
        }

        [TestMethod]
        public void FallingSpike_UpdatePosition()
        {
            Vector2 testPos = new Vector2(_fallingSpike.Position.X, 0.9f);
            _level.UpdateLevel();
            bool isTrue = ((int)testPos.X == (int)_fallingSpike.Position.X && (int)testPos.Y == (int)_fallingSpike.Position.Y);
            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void FallingSpike_ResetSpikeBoolean()
        {
            bool test = false;
            _level.UpdateLevel();
            Assert.AreEqual(test, _fallingSpike._goFall);
        }

        [TestMethod]
        public void FallingSpike_ResetSpikeVelocityY()
        {
            float test = 0;
            _level.UpdateLevel();
            Assert.AreEqual(test, _fallingSpike.VelocityY);
        }

        [TestMethod]
        public void FallingSpike_ResetSpikePosition()
        {
            _level.UpdateLevel();
            Vector2 testPos = new Vector2(_fallingSpike.Position.X, _fallingSpike.Position.Y);
            _level.UpdateLevel();
            Assert.AreEqual(testPos, _fallingSpike.StartPosition);
        }
    }
}
