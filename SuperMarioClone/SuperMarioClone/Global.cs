using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarioClone
{
    class Global
    {
        public int GridSize { get; private set; }
        public MainGame MainGame { get; set; }

        private static Global _instance = new Global();

        private Global()
        {
            GridSize = 16;
        }

        public static Global Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
