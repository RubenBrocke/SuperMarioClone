using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioClone;
using System.Reflection;
using UnitTest.MockClasses;
using Microsoft.Xna.Framework;

namespace SuperMarioClone
{
    [TestClass]
    public class GameObjectFactoryTest
    {
        private TestContentManager _contentManager;
        private GameObjectFactory _gameObjectFactory;
        private Level _level;


        [TestInitialize]
        public void Init()
        {
            _gameObjectFactory = new GameObjectFactory();
            _contentManager = new TestContentManager();
            _level = new Level();
        }

        [TestMethod]
        public void GameObjectFactory_Fabricate()
        {
            string[] args = new string[] { "MysteryBlock", "0,0,Coin" };
            _gameObjectFactory.Fabricate(args, _level, _contentManager);
            bool isTrue = _level.GameObjects.Contains(new MysteryBlock(0, 0, typeof(Coin), _level, _contentManager));
        }
    }
}
