using System;
using System.Collections.Generic;
using System.Text;

namespace CsMigemo
{
    class BitVector
    {
        internal readonly ulong[] Words;
        internal readonly uint[] Lb;
        internal readonly ushort[] Sb;
        internal readonly int SizeInBits;

        public BitVector(ulong[] words, int sizeInBits)
        {
            if ((sizeInBits + 63) / 64 != words.Length)
            {
                throw new ArgumentException();
            }
            Words = words;
            SizeInBits = sizeInBits;
            Lb = new uint[(sizeInBits + 511) / 512];
            Sb = new ushort[Lb.Length * 8];
            uint sum = 0;
            ushort sumInLb = 0;
            for (int i = 0; i < Sb.Length; i++)
            {
                ushort bitCount = i < Words.Length ? (ushort)BitCount(Words[i]) : (ushort)0;
                Sb[i] = sumInLb;
                sumInLb += bitCount;
                if ((i & 7) == 7)
                {
                    Lb[i >> 3] = sum;
                    sum += sumInLb;
                    sumInLb = 0;
                }
            }
        }

        public int Rank(int pos, bool b)
        {
            int count1 = Sb[pos / 64] + (int)Lb[pos / 512];
            ulong word = Words[pos / 64];
            var shiftSize = 64 - (pos & 63);
            var mask = shiftSize == 64 ? 0 : 0xFFFFFFFFFFFFFFFFLu >> shiftSize;
            count1 += BitCount(word & mask);
            return b ? count1 : (pos - count1);
        }

        public int Select(int count, bool b)
        {
            var lb_index = LowerBoundBinarySearchLB(count, b) - 1;
            int count_in_lb = b ?
                count - (int)Lb[lb_index] :
                count - (int)(512 * lb_index - Lb[lb_index]);
            var sb_index = LowerBoundBinarySearchSB(count_in_lb, lb_index * 8, lb_index * 8 + 8, b) - 1;
            var count_in_sb = b ?
                count_in_lb - Sb[sb_index] :
                count_in_lb - (64 * (sb_index % 8) - Sb[sb_index]);
            var word = Words[sb_index];
            if (!b)
            {
                word = ~word;
            }
            return sb_index * 64 + SelectInWord(word, count_in_sb) - 1;
        }

        int LowerBoundBinarySearchLB(int key, bool b)
        {
            var high = Lb.Length;
            var low = -1;
            while (high - low > 1)
            {
                var mid = (high + low) >> 1;
                if ((b ? Lb[mid] : 512 * mid - Lb[mid]) < key)
                {
                    low = mid;
                }
                else
                {
                    high = mid;
                }
            }
            return high;
        }

        int LowerBoundBinarySearchSB(int key, int from_index, int to_index, bool b)
        {
            var high = to_index;
            var low = from_index - 1;
            while (high - low > 1)
            {
                var mid = (high + low) >> 1;
                if ((b ? Sb[mid] : 64 * (mid % 8) - Sb[mid]) < key)
                {
                    low = mid;
                }
                else
                {
                    high = mid;
                }
            }
            return high;
        }

        public int NextClearBit(int from_index)
        {
            var u = from_index >> 6;
            if (u >= Words.Length)
            {
                return from_index;
            }
            var word = ~Words[u] & (0xFFFFFFFFFFFFFFFFLu << (from_index & 63));
            while (true)
            {
                if (word != 0)
                {
                    return (u * 64) + NumberOfTrailingZeros(word);
                }
                u += 1;
                if (u == Words.Length)
                {
                    return 64 * Words.Length;
                }
                word = ~Words[u];
            }
        }

        public int Size()
        {
            return SizeInBits;
        }

        public bool Get(int pos)
        {
            if (SizeInBits < pos)
            {
                throw new IndexOutOfRangeException();
            }
            return ((Words[(pos >> 6)] >> (pos & 63)) & 1) == 1;
        }

        static int SelectInWord(ulong word, int count)
        {
            int i = 0;
            while (count != 0)
            {
                count -= (int)(word & 1);
                word >>= 1;
                i++;
            }
            return i;
        }

        static int BitCount(ulong v)
        {
            ulong count = (v & 0x5555555555555555LU) + ((v >> 1) & 0x5555555555555555LU);
            count = (count & 0x3333333333333333LU) + ((count >> 2) & 0x3333333333333333LU);
            count = (count & 0x0f0f0f0f0f0f0f0fLU) + ((count >> 4) & 0x0f0f0f0f0f0f0f0fLU);
            count = (count & 0x00ff00ff00ff00ffLU) + ((count >> 8) & 0x00ff00ff00ff00ffLU);
            count = (count & 0x0000ffff0000ffffLU) + ((count >> 16) & 0x0000ffff0000ffffLU);
            return (int)((count & 0x00000000ffffffffLU) + ((count >> 32) & 0x00000000ffffffffLU));
        }

        static int NumberOfTrailingZeros(ulong i)
        {
            if (i == 0)
            {
                return 64;
            }
            var pos = 0;
            while ((i & 1) == 0)
            {
                pos++;
                i >>= 1;
            }
            return pos;
            /*
            ulong x, y;
            if (i == 0) return 64;
            ulong n = 63;
            y = i; if (y != 0) { n = n - 32; x = y; } else x = (i >> 32);
            y = x << 16; if (y != 0) { n = n - 16; x = y; }
            y = x << 8; if (y != 0) { n = n - 8; x = y; }
            y = x << 4; if (y != 0) { n = n - 4; x = y; }
            y = x << 2; if (y != 0) { n = n - 2; x = y; }
            return (int)(n - ((x << 1) >> 31));
            */
        }
    }
}
