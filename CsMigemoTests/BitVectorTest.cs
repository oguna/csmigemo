using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsMigemo.Tests
{
    [TestClass]
    public class BitVectorTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            const int SIZE = 1000;
            var rand = new Random();
            var bits = new bool[SIZE];
            for (int i = 0; i < bits.Length; i++)
            {
                bits[i] = rand.Next(2) == 0;
            }
            var words = BitsToWords(bits);
            var bitvector = new BitVector(words, SIZE);
            for (int i = 0; i < SIZE; i++)
            {
                var expected = NaiveNextClearBit(bits, i);
                Assert.AreEqual(expected, bitvector.NextClearBit(i));
            }
        }

        [TestMethod]
        public void TestRank()
        {
            const int SIZE = 1000;
            var rand = new Random();
            var bits = new bool[SIZE];
            for (int i = 0; i < bits.Length; i++)
            {
                bits[i] = rand.Next(2) == 0;
            }
            var words = BitsToWords(bits);
            var bitvector = new BitVector(words, SIZE);
            for (int i = 0; i < SIZE; i++)
            {
                var expected = NaiveRank(bits, i, true);
                Assert.AreEqual(expected, bitvector.Rank(i, true));
            }
            for (int i = 0; i < SIZE; i++)
            {
                var expected = NaiveRank(bits, i, false);
                Assert.AreEqual(expected, bitvector.Rank(i, false));
            }
        }

        [TestMethod]
        public void TestSelect()
        {
            const int SIZE = 1000;
            var rand = new Random();
            var bits = new bool[SIZE];
            for (int i = 0; i < bits.Length; i++)
            {
                bits[i] = rand.Next(2) == 0;
            }
            var words = BitsToWords(bits);
            var count1 = 0;
            foreach (var b in bits)
            {
                if (b)
                {
                    count1++;
                }
            }
            var count0 = bits.Length - count1;
            var bitvector = new BitVector(words, SIZE);
            for (int i = 1; i < count1; i++)
            {
                var expected = NaiveSelect(bits, i, true);
                Assert.AreEqual(expected, bitvector.Select(i, true));
            }
            for (int i = 1; i < count0; i++)
            {
                var expected = NaiveSelect(bits, i, false);
                Assert.AreEqual(expected, bitvector.Select(i, false));
            }
        }

        private static int NaiveRank(bool[] bits, int pos, bool b)
        {
            var count = 0;
            for(int i = 0; i < pos; i++)
            {
                if (bits[i] == b)
                {
                    count++;
                }
            }
            return count;
        }
        private static int NaiveSelect(bool[] bits, int count, bool b)
        {
            for(int i = 0; i < bits.Length; i++)
            {
                if (bits[i] == b)
                {
                    count--;
                }
                if (count == 0)
                {
                    return i;
                }
            }
            return -1;
        }

        private static int NaiveNextClearBit(bool[] bits, int pos)
        {
            while (pos < bits.Length && bits[pos])
            {
                pos++;
            }
            if (pos > bits.Length)
            {
                return bits.Length + 1;
            }
            else
            {
                return pos;
            }
        }

        private ulong[] BitsToWords(bool[] bits)
        {
            var words = new ulong[(bits.Length + 63) / 64];
            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i])
                {
                    words[i / 64] |= 1Lu << (i % 64);
                }
            }
            return words;
        }
    }
}
