using Microsoft.Xna.Framework.Content;
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
            T result = default(T);
            return result;
        }
    }
}
