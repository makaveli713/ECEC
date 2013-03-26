using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Art713.ECEC.Auxiliary
{
    static class Math
    {
        public static int Mod(int a, int b)
        {
            if (a < 0)
                return b + (a % b);
            return a % b;
        }
    }
}
