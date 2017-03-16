using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarioClone
{
    class GameObjectFactory
    {
        private Dictionary<Type, int> _objectDict;
        private ArgumentResolver _argumentResolver;

        public GameObjectFactory()
        {
            _objectDict = new Dictionary<Type, int>();
            _objectDict.Add(typeof(Coin), 2);
            _objectDict.Add(typeof(Floor), 4);
            _objectDict.Add(typeof(TransFloor), 4);
            _objectDict.Add(typeof(Goomba), 2);
            _objectDict.Add(typeof(CoinBlock), 2);
            _objectDict.Add(typeof(MysteryBlock), 2);
            _argumentResolver = new ArgumentResolver();
        }

        public GameObject Fabricate(string[] s, Level level, ContentManager contentManager)
        {
            Type thingy = Type.GetType(this.GetType().Namespace + "." + s[0]);
            List<object> args = _argumentResolver.Resolve(s[1].Split(','), _objectDict[thingy]);
            args.Add(level);
            args.Add(contentManager);
            return (GameObject)Activator.CreateInstance(Type.GetType(this.GetType().Namespace + "." + s[0]), args.ToArray());
        }
    }
}
