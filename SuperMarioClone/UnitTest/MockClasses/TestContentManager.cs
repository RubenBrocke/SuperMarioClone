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
            object result = new Texture2D(new GraphicsDevice(new GraphicsAdapter(), new GraphicsProfile(), new PresentationParameters()), 200, 200); //TODO: FIXME, Make it actually return a working mock sprite.
            if (result is T)
            {
                return (T)result;
            }
            return default(T);
        }
    }
}
