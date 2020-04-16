using System;
using System.Collections.Generic;
using System.Text;

namespace CsMigemo
{
    class RegexGenerator
    {
        private class Node
        {
            public char Code;
            public Node Child;
            public Node Next;
            public Node(char c)
            {
                Code = c;
            }
        }

        private Node Root = null;

        public void Add(string word)
        {
            if (word == null || word.Length == 0)
            {
                return;
            }
            Root = Add(Root, word, 0);
        }

        static Node Add(Node node, string word, int offset)
        {
            if (node == null)
            {
                if (offset >= word.Length)
                {
                    return null;
                }
                node = new Node(word[offset]);
                if (offset < word.Length - 1)
                {
                    node.Child = Add(null, word, offset + 1);
                }
                return node;
            }
            var thisNode = node;
            var code = word[offset];
            if (code < node.Code)
            {
                var newNode = new Node(code)
                {
                    Next = node
                };
                node = newNode;
                if (offset < word.Length)
                {
                    node.Child = Add(null, word, offset + 1);
                }
                thisNode = node;
            } else {
                while (node.Next != null && node.Next.Code <= code)
                {
                    node = node.Next;
                }
                if (node.Code == code)
                {
                    if (node.Child == null)
                    {
                        return thisNode;
                    }
                } else
                {
                    var newNode = new Node(code)
                    {
                        Next = node.Next
                    };
                    node.Next = newNode;
                    node = newNode;
                }
                if (word.Length == offset + 1)
                {
                    node.Child = null;
                    return thisNode;
                }
                node.Child = Add(node.Child, word, offset + 1);
            }
            return thisNode;

        }

        public string Generate(RegexOperator rxop)
        {
            if (rxop == null)
            {
                return "";
            } else
            {
                var sb = new StringBuilder();
                GenerateStub(Root, rxop, sb);
                return sb.ToString();
            }
        }

        static void GenerateStub(Node node, RegexOperator rxop, StringBuilder buf)
        {
            var escapeCharacters = "\\.[]{}()*+-?^$|".ToCharArray();
            var brother = 1;
            var haschild = 0;
            for (var tmp = node; tmp != null; tmp = tmp.Next)
            {
                if (tmp.Next != null)
                {
                    brother++;
                }
                if (tmp.Child != null)
                {
                    haschild++;
                }
            }
            var nochild = brother - haschild;

            if (brother > 1 && haschild > 0)
            {
                buf.Append(rxop.BeginGroup);
            }

            if (nochild > 0)
            {
                if (nochild > 1)
                {
                    buf.Append(rxop.BeginClass);
                }
                for (var tmp = node; tmp != null; tmp = tmp.Next)
                {
                    if (tmp.Child != null)
                    {
                        continue;
                    }
                    if (Array.IndexOf(escapeCharacters, tmp.Code) != -1)
                    {
                        buf.Append("\\");
                    }
                    buf.Append(tmp.Code);
                }
                if (nochild > 1)
                {
                    buf.Append(rxop.EndClass);
                }
            }

            if (haschild > 0)
            {
                if (nochild > 0)
                {
                    buf.Append(rxop.Or);
                }
                Node tmp;
                for (tmp = node; tmp.Child == null; tmp = tmp.Next)
                {
                }
                while (true)
                {
                    if (Array.IndexOf(escapeCharacters, tmp.Code) != -1)
                    {
                        buf.Append("\\");
                    }
                    buf.Append(tmp.Code);
                    if (rxop.Newline != null)
                    {
                        buf.Append(rxop.Newline);
                    }
                    GenerateStub(tmp.Child, rxop, buf);
                    for (tmp = tmp.Next; tmp != null && tmp.Child == null; tmp = tmp.Next)
                    {
                    }
                    if (tmp == null)
                    {
                        break;
                    }
                    if (haschild > 1)
                    {
                        buf.Append(rxop.Or);
                    }
                }
            }
            if (brother > 1 && haschild > 0)
            {
                buf.Append(rxop.EndGroup);
            }
        }
    }
}
