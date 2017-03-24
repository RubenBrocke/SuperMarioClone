using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.MockClasses
{
    public partial class TestContentManager : ContentManager
    {
        public TestContentManager() : base(new TestServiceProvider())
        {
           
        }

        public override T Load<T>(string assetName)
        {
            if (typeof(Texture2D).IsAssignableFrom(typeof(T)))
            {
                object result = new TestTexture2D(TestItems.gd, 200, 200); //TODO: FIXME, Make it actually return a working mock sprite.
                return (T)result;
            }
            return default(T);
        }
    }
}
