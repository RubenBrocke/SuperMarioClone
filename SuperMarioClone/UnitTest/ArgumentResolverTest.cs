using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioClone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.MockClasses;

namespace UnitTest
{
    [TestClass]
    public class ArgumentResolverTest
    {
        private ArgumentResolver _argumentResolver;

        [TestInitialize]
        public void Init()
        {
            _argumentResolver = new ArgumentResolver();
        }

        [TestMethod]
        public void ArgumentResolver_Resolve()
        {
            List<object> testList = new List<object>() { 1, 5243, true, false, typeof(Coin), null };
            string[] testArray = new string[6] { "1", "5243", "true", "false", "Coin", "three" };
            List<object> expected = _argumentResolver.Resolve(testArray);
            CollectionAssert.AreEqual(expected, testList);
        }
    }
}
