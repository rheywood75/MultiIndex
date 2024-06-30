using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MultiIndex
{
    /// <summary>
    /// This struct is used to "concatenate" two ints (key0 and key1) into a long (fullKey).  
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct TwoKeyUnion
    {
        [FieldOffset(4)]
        public int key0;

        [FieldOffset(0)]
        public int key1;

        [FieldOffset(0)]
        public long fullKey;
    }
}
