using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarioClone
{
    public interface IMovable
    {
        float VelocityX { get; }

        float VelocityY { get; }

        float JumpVelocity { get; }

        float Gravity { get; }
    }
}
