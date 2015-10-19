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
        internal static IEnumerable<SpannedToken> Run(IEnumerable<string> arg)
        {
            return RunInternal(String.Join("", arg));
        }

        static IEnumerable<SpannedToken> RunInternal(string text)
        {
            int idx = 0;
            char current;
            while (idx < text.Length)
            {
                current = text[idx];
                if (current == '\n')
                    yield return ConsumeNewline(ref idx);
                else if (char.IsWhiteSpace(current))
                    yield return ConsumeWhiteSpace(text, ref idx);
                else if (current == '/' && text[idx + 1] == '*')
                    yield return ConsumeComments(text, ref idx);
                else
                    yield return ConsumeToken(text, ref idx);
            }
        }

        private static SpannedToken ConsumeNewline(ref int idx)
        {
            return new SpannedToken(1, new Span(idx++, 1));
        }

        private static SpannedToken ConsumeWhiteSpace(string text, ref int idx)
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
            return new SpannedToken(2, new Span(start, idx - start));
        }

        private static SpannedToken ConsumeComments(string text, ref int idx)
        {
            int start = idx;
            char current;
            do
            {
                if (++idx >= text.Length)
                    break;
                current = text[idx];
            }
            while (current != '*' || text[idx+1] != '/');
            idx +=2 ;
            return new SpannedToken(3, new Span(start, idx - start));
        }

        private static SpannedToken ConsumeToken(string text, ref int idx)
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
            return new SpannedToken(4, new Span(start, idx - start));
        }
    }
}
