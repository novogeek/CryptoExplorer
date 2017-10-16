using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoExplorer.ImageCryptoLib
{
    public enum Operation {
        NONE = 0,
        AND = 1,
        OR = 2,
        XOR = 3
    }
    public static class BitwiseOperation
    {
        //Performs byte-wise XOR between elements of @a and @b
        public static byte[] Compute(byte[] a, byte[] b, Operation op)
        {
            if (a == null || b == null)
                return null;

            int len = Math.Max(a.Length, b.Length);

            byte[] res = new byte[len];

            if (a.Length > b.Length)
            {
                a.CopyTo(res, 0);
                for (int i = 0; i < b.Length; i++)
                {
                    switch (op) {
                        case Operation.AND:
                            res[i] &= b[i];
                            break;
                        case Operation.OR:
                            res[i] |= b[i];
                            break;
                        case Operation.XOR:
                            res[i] ^= b[i];
                            break;
                    }
                }
            }
            else
            {
                b.CopyTo(res, 0);
                for (int i = 0; i < a.Length; i++)
                {
                    switch (op) {
                        case Operation.AND:
                            res[i] &= a[i];
                            break;
                        case Operation.OR:
                            res[i] |= a[i];
                            break;
                        case Operation.XOR:
                            res[i] ^= a[i];
                            break;
                    }
                }
            }

            return res;
        }
    }
}
