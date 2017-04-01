using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarioClone
{
    public class GameObjectFactory
    {
        private ArgumentResolver _argumentResolver;

        public GameObjectFactory()
        {
            _argumentResolver = new ArgumentResolver();
        }

        public GameObject Fabricate(string[] s, Level level, ContentManager contentManager)
        {
            Type thingy = Type.GetType(this.GetType().Namespace + "." + s[0]);
            List<object> args = _argumentResolver.Resolve(s[1].Split(','));
            args.Add(level);
            args.Add(contentManager);
            return (GameObject)Activator.CreateInstance(Type.GetType(this.GetType().Namespace + "." + s[0]), args.ToArray());
        }
    }
}
