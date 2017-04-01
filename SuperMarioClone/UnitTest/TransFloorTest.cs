using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.MockClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework.Graphics;
using SuperMarioClone;

namespace UnitTest
{
    [TestClass]
    public class TransFloorTest
    {
        private TransFloor _floor;
        private TestContentManager _contentManager;
        private Level _level;
        private SpriteBatch _spriteBatch;

        [TestInitialize]
        public void Init()
        {
            _contentManager = new TestContentManager();
            _level = new Level();
            _floor = new TransFloor(3, 3, 3, 3, _level, _contentManager);
            _level.ToAddGameObject(_floor);
            _level.UpdateLevel();
            _spriteBatch = new SpriteBatch(new TestGraphicsDeviceService().GraphicsDevice);
        }

        [TestMethod]
        public void TransFloor_Draw()
        {
            _spriteBatch.Begin();
            _floor.Draw(_spriteBatch);
            _spriteBatch.End();
        }
    }
}