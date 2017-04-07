using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarioClone
{
    public class Global
    {
        //Properties
        public int GridSize { get; private set; }
        public bool WeirdSounds { get; set; }
        public MainGame MainGame { get; set; }

        //Private fields
        private static Global _instance = new Global();

        /// <summary>
        /// Constructor of Global, sets the Properties
        /// </summary>
        private Global()
        {
            GridSize = 16;
            WeirdSounds = false;
        }

        /// <summary>
        /// Returns the instance of Global
        /// </summary>
        public static Global Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
