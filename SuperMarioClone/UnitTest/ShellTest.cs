using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioClone;
using System.Reflection;
using UnitTest.MockClasses;
using Microsoft.Xna.Framework;

namespace UnitTest
{
    [TestClass]
    public class ShellTest
    {
        private TestContentManager _contentManager;
        private Level _level;
        private Shell _shell;

        [TestInitialize]
        public void Init()
        {
            _contentManager = new TestContentManager();
            _level = new Level();
            _shell = new Shell(0, 0, _level, _contentManager);
            _level.ToAddGameObject(_shell);
            _level.UpdateLevel();
        }

        [TestMethod]
        public void Shell_UpdateHitbox()
        {            
            Rectangle testRect = new Rectangle((int)_shell.Position.X, (int)_shell.Position.Y, _shell.Hitbox.Width, _shell.Hitbox.Height);
            testRect.Y += (int)_shell.Gravity;
            _level.UpdateLevel();
            Assert.AreEqual(testRect, _shell.Hitbox);
        }

        [TestMethod]
        public void Shell_AddGravity()
        {
            float test = _shell.VelocityY + _shell.Gravity;
            _level.UpdateLevel();
            Assert.AreEqual(test, _shell.VelocityY);
        }

        [TestMethod]
        public void Shell_CollisionCheck()
        {
            Floor f = new Floor((int)_shell.Position.X, (int)_shell.Position.Y + _shell.Hitbox.Height, 200, 200, _level, _contentManager);
            Floor f2 = new Floor((int)_shell.Position.X / 16 - 1, (int)_shell.Position.Y / 16, _shell.Hitbox.Width / 16, _shell.Hitbox.Height / 16, _level, _contentManager);
            _level.ToAddGameObject(f);
            _level.ToAddGameObject(f2);
            _level.UpdateLevel();
            _level.UpdateLevel();
            Assert.AreEqual(0, _shell.VelocityX);
        }

        [TestMethod]
        public void Shell_UpdatePosition()
        {
            Vector2 testPos = new Vector2(_shell.Position.X, 0.9f);
            _level.UpdateLevel();
            bool isTrue = ((int)testPos.X == (int)_shell.Position.X && (int)testPos.Y == (int)_shell.Position.Y);
            Assert.IsTrue(isTrue);
        }
    }
}
