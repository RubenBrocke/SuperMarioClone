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
