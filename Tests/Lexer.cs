using EditorChanges;
using Microsoft.VisualStudio.Text;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    static class Lexer
    {
        internal static IEnumerable<SpannedToken> Run(IEnumerable<string> arg, int offset)
        {
            return RunInternal(String.Join("", arg), offset);
        }

        static IEnumerable<SpannedToken> RunInternal(string text, int offset)
        {
            int idx = 0;
            char current;
            while (idx < text.Length)
            {
                current = text[idx];
                if (current == '\n')
                    yield return ConsumeNewline(offset, ref idx);
                else if (char.IsWhiteSpace(current))
                    yield return ConsumeWhiteSpace(text, offset, ref idx);
                else if (current == '/' && text[idx + 1] == '*')
                    yield return ConsumeComments(text, offset, ref idx);
                else
                    yield return ConsumeToken(text, offset, ref idx);
            }
        }

        private static SpannedToken ConsumeNewline(int offset, ref int idx)
        {
            return new SpannedToken(1, new Span(offset + idx++, 1));
        }

        private static SpannedToken ConsumeWhiteSpace(string text, int offset, ref int idx)
        {
            int start = idx;
            char current;
            do
            {
                if (++idx >= text.Length)
                    break;
                current = text[idx];
            }
            while (current != '\n' && char.IsWhiteSpace(current));
            return new SpannedToken(2, new Span(start + offset, idx - start));
        }

        private static SpannedToken ConsumeComments(string text, int offset, ref int idx)
        {
            int start = idx;
            char current;
            do
            {
                if (++idx >= text.Length)
                    break;
                current = text[idx];
            }
            while (current != '*' || text[idx + 1] != '/');
            if (idx < text.Length)
                idx += 2;
            return new SpannedToken(3, new Span(start + offset, idx - start));
        }

        private static SpannedToken ConsumeToken(string text, int offset, ref int idx)
        {
            int start = idx;
            char current;
            do
            {
                if (++idx >= text.Length)
                    break;
                current = text[idx];
            }
            while (!char.IsWhiteSpace(current));
            return new SpannedToken(4, new Span(start + offset, idx - start));
        }
    }
}
