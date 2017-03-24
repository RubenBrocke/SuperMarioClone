using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnitTest.MockClasses
{
    [TestClass]
    public static class TestItems
    {
        public static TestGraphicsDeviceService tgds;
        public static GraphicsDevice gd;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext tc)
        {

            tgds = new TestGraphicsDeviceService();
            gd = tgds.GraphicsDevice;

        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            tgds.Release();
        }
    }
}
