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

        /// <summary>
        /// Constructor for GameObjectFactory, creates a new ArgumentResolver
        /// </summary>
        public GameObjectFactory()
        {
            _argumentResolver = new ArgumentResolver();
        }

        /// <summary>
        /// Creates a GameObject from the given string[]
        /// </summary>
        /// <param name="s">The String array containing the name of the Object and the arguments used to make it</param>
        /// <param name="level">Level the GameObject should be in</param>
        /// <param name="contentManager">ContentManager used to create the GameObject</param>
        /// <returns></returns>
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
