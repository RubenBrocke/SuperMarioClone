using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.MockClasses
{
    class TestServiceProvider : IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            return this;
        }
    }
}
