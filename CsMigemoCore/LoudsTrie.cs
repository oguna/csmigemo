using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsMigemo
{
    class LoudsTrie
    {
        internal BitVector BitVector;
        internal char[] Edges;

        public LoudsTrie(BitVector bitVector, char[] edges)
        {
            BitVector = bitVector;
            Edges = edges;
        }

        public string GetKey(int index)
        {
            if (index <= 0 || Edges.Length <= index)
            {
                throw new IndexOutOfRangeException();
            }
            var sb = new StringBuilder();
            while (index > 1)
            {
                sb.Append(Edges[index]);
                index = Parent(index);
            }
            var array = sb.ToString().ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        public int Parent(int x)
        {
            return BitVector.Rank(BitVector.Select(x, true), false);
        }

        public int FirstChild(int x)
        {
            var y = BitVector.Select(x, false) + 1;
            if (BitVector.Get(y))
            {
                return BitVector.Rank(y, true) + 1;
            }
            else
            {
                return -1;
            }
        }

        public int Traverse(int index, char c)
        {
            var firstChild = FirstChild(index);
            if (firstChild == -1)
            {
                return -1;
            }
            var childStartBit = BitVector.Select(firstChild, true);
            var childEndBit = BitVector.NextClearBit(childStartBit);
            var childSize = childEndBit - childStartBit;
            var result = Array.BinarySearch(Edges, firstChild, childSize, c);
            return result >= 0 ? result : -1;
        }

        public int Get(string key)
        {
            var nodeIndex = 1;
            for (var i = 0; i < key.Length; i++)
            {
                var c = key[i];
                nodeIndex = Traverse(nodeIndex, c);
                if (nodeIndex == -1)
                {
                    break;
                }
            }
            return (nodeIndex >= 0) ? nodeIndex : -1;
        }

        public IEnumerable<int> Iterator(int index)
        {
            yield return index;
            var child = FirstChild(index);
            if (child == -1)
            {
                yield break;
            }
            var childPos = BitVector.Select(child, true);
            while (BitVector.Get(childPos))
            {
                foreach (var e in Iterator(child))
                {
                    yield return e;
                }
                child++;
                childPos++;
            }
        }

        public int Size()
        {
            return Edges.Length - 2;
        }

        public static LoudsTrie Build(string[] keys)
        {
            return Build(keys, out _);
        }

        public static LoudsTrie Build(string[] keys, out int[] indexes)
        {
            var memo = new int[keys.Length];
            for (int i = 0; i < memo.Length; i++)
            {
                memo[i] = 1;
            }
            var offset = 0;
            var current_node = 1;
            var edges = new List<char> { ' ', ' ' };
            var child_sizes = new uint[128];
            while (true) {
                var last_char = 0;
                var last_parent = 0;
                var rest_keys = 0;
                for (var i = 0; i < keys.Length; i++) {
                    if (memo[i] < 0) {
                        continue;
                    }
                    if (keys[i].Length <= offset) {
                        memo[i] = -memo[i];
                        continue;
                    }
                    var current_char = keys[i][offset];
                    var current_parent = memo[i];
                    if (last_char != current_char || last_parent != current_parent) {
                        if (child_sizes.Length <= memo[i]) {
                            var newChildSizes = new uint[child_sizes.Length * 2];
                            Array.Copy(child_sizes, 0, newChildSizes, 0, child_sizes.Length);
                            child_sizes = newChildSizes;
                        }
                        child_sizes[memo[i]] = child_sizes[memo[i]] + 1;
                        current_node += 1;
                        edges.Add(current_char);
                        last_char = current_char;
                        last_parent = current_parent;
                    }
                    memo[i] = current_node;
                    rest_keys = rest_keys + 1;
                }
                if (rest_keys == 0) {
                    break;
                }
                offset += 1;
            }
            for (var i = 0; i < memo.Length; i++) {
                memo[i] = -memo[i];
            }

            var num_of_children = 0;
            for (var i = 0; i <= current_node; i++)
            {
                num_of_children += (int)child_sizes[i];
            }

            var num_of_nodes = current_node;
            var bit_vector_words = new ulong[((num_of_children + num_of_nodes + 63 + 1) / 64)];
            var bit_vector_index = 1;
            bit_vector_words[0] = 1;
            for (var i = 1; i <= current_node; i++) {
                bit_vector_index += 1;
                var child_size = child_sizes[i];
                for (var j = 0; j < child_size; j++) {
                    bit_vector_words[bit_vector_index >> 5] =
                        bit_vector_words[bit_vector_index >> 5] | (1UL << (bit_vector_index & 31));
                    bit_vector_index = bit_vector_index + 1;
                }
            }
            var bit_vector = new BitVector(bit_vector_words, bit_vector_index);
            indexes = memo;
            return new LoudsTrie(bit_vector, edges.ToArray());
        }
    }
}
