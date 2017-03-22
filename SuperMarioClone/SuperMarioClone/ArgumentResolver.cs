using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarioClone
{
    class ArgumentResolver
    {
        public ArgumentResolver()
        {

        }

        /// <summary>
        /// Resolves the characters that are being read from the lvl.txt
        /// Converts the characters to their respected function (text to strings, numbers to ints and so forth)
        /// </summary>
        /// <param name="args"></param>
        /// <param name="gridNumberAmmount"></param>
        /// <returns></returns>
        public List<object> Resolve(string[] args, int gridNumberAmmount = 0)
        {
            object[] returnArgs = new object[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                try
                {
                    if (i < gridNumberAmmount)
                    {
                        returnArgs[i] = int.Parse(args[i]) * 16;
                    }
                    else
                    {
                        returnArgs[i] = int.Parse(args[i]);
                    }
                }
                catch
                {
                    try
                    {
                        returnArgs[i] = bool.Parse(args[i]);
                    }
                    catch
                    {
                        try
                        {
                            returnArgs[i] = Type.GetType(this.GetType().Namespace + "." + args[i]);
                        }
                        catch
                        {
                            throw new FormatException(string.Format("Unable to resolve {0}", args[i]));
                        }
                    }
                }
                
            }
            return returnArgs.ToList();
        }
    }
}
