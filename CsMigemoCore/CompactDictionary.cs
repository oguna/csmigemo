using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CsMigemo
{
    class CompactDictionary
    {
        private readonly LoudsTrie KeyTrie;
        private readonly LoudsTrie ValueTrie;
        private readonly BitVector MappingBitVector;
        private readonly uint[] Mapping;

        public CompactDictionary(Stream stream)
        {
            var br = new BinaryReader(stream);
            KeyTrie = ReadTrie(br, true);
            ValueTrie = ReadTrie(br, false);
            var mappingBitVectorSize = Swap(br.ReadUInt32());
            var mappingBitVectorWords = new ulong[(mappingBitVectorSize + 63) / 64];
            for (var i = 0; i < mappingBitVectorWords.Length; i++)
            {
                mappingBitVectorWords[i] = Swap(br.ReadUInt64());
            }
            MappingBitVector = new BitVector(mappingBitVectorWords, (int)mappingBitVectorSize);
            var mappingSize = Swap(br.ReadUInt32());
            Mapping = new uint[mappingSize];
            for (var i = 0; i < mappingSize; i++)
            {
                Mapping[i] = Swap(br.ReadUInt32());
            }
            if (br.BaseStream.Position != br.BaseStream.Length)
            {
                throw new Exception();
            }
        }

        private static LoudsTrie ReadTrie(BinaryReader br, bool compactHiragana)
        {
            var keyTrieEdgeSize = Swap(br.ReadUInt32());
            var keyTrieEdges = new char[keyTrieEdgeSize];
            for (var i = 0; i < keyTrieEdgeSize; i++)
            {
                char c;
                if (compactHiragana)
                {
                    c = Decode(br.ReadByte());
                }
                else
                {
                    c = (char)Swap(br.ReadUInt16());
                }
                keyTrieEdges[i] = c;
            }
            var keyTrieBitVectorSize = Swap(br.ReadUInt32());
            var keyTrieBitVectorWords = new ulong[(keyTrieBitVectorSize + 63) / 64];
            for (var i = 0; i < keyTrieBitVectorWords.Length; i++)
            {
                keyTrieBitVectorWords[i] = Swap(br.ReadUInt64());
            }
            return new LoudsTrie(new BitVector(keyTrieBitVectorWords, (int)keyTrieBitVectorSize), keyTrieEdges);
        }

        private static uint Swap(uint x)
        {
            x = (x >> 16) | (x << 16);
            return ((x & 0xFF00FF00) >> 8) | ((x & 0x00FF00FF) << 8);
        }

        private static ulong Swap(ulong x)
        {
            x = (x >> 32) | (x << 32);
            x = ((x & 0xFFFF0000FFFF0000) >> 16) | ((x & 0x0000FFFF0000FFFF) << 16);
            return ((x & 0xFF00FF00FF00FF00) >> 8) | ((x & 0x00FF00FF00FF00FF) << 8);
        }

        private static ushort Swap(ushort x)
        {
            return (ushort)((x & 0xFF) << 8 | (x >> 8) & 0xFF);
        }

        private static char Decode(byte c)
        {
            if (0x20 <= c && c <= 0x7e)
            {
                return (char)c;
            }
            if (0xa1 <= c && c <= 0xf6)
            {
                return (char)(c + 0x3040 - 0xa0);
            }
            throw new Exception();
        }

        public IEnumerable<string> Search(string key)
        {
            var keyIndex = KeyTrie.Get(key);
            if (keyIndex != -1)
            {
                var valueStartPos = MappingBitVector.Select(keyIndex, false);
                var valueEndPos = MappingBitVector.NextClearBit(valueStartPos + 1);
                var size = valueEndPos - valueStartPos - 1;
                if (size > 0)
                {
                    var offset = MappingBitVector.Rank(valueStartPos, false);
                    for (var i = 0; i < size; i++)
                    {
                        yield return ValueTrie.GetKey((int)Mapping[valueStartPos - offset + i]);
                    }
                }
            }
        }

        public IEnumerable<string> PredictiveSearch(string key)
        {
            var keyIndex = KeyTrie.Get(key);
            if (keyIndex > 1)
            {
                foreach (var i in KeyTrie.Iterator(keyIndex))
                {
                    var valueStartPos = MappingBitVector.Select(i, false);
                    var valueEndPos = MappingBitVector.NextClearBit(valueStartPos + 1);
                    var size = valueEndPos - valueStartPos - 1;
                    var offset = MappingBitVector.Rank(valueStartPos, false);
                    for (var j = 0; j < size; j++)
                    {
                        yield return ValueTrie.GetKey((int)Mapping[valueStartPos - offset + j]);
                    }
                }
            }
        }
    }
}
