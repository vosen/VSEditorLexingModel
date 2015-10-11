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
        internal static IEnumerable<MultilineSpannedToken> Run(IEnumerable<string> arg)
        {
            return RunInternal(arg.First());
        }

        static IEnumerable<MultilineSpannedToken> RunInternal(string text)
        {
            int idx = 0;
            char current;
            while (idx < text.Length)
            {
                current = text[idx];
                if (current == '\n')
                    yield return new MultilineSpannedToken(ConsumeNewline(ref idx));
                else if (char.IsWhiteSpace(current))
                    yield return new MultilineSpannedToken(ConsumeWhiteSpace(text, ref idx));
                else if (current == '/' && text[idx + 1] == '*')
                    yield return ConsumeComments(text, ref idx);
                else
                    yield return new MultilineSpannedToken(ConsumeToken(text, ref idx));
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

        private static MultilineSpannedToken ConsumeComments(string text, ref int idx)
        {
            int lines = 1;
            int start = idx;
            char current;
            do
            {
                if (++idx >= text.Length)
                    break;
                current = text[idx];
                if(current == '\n')
                    lines++;
            }
            while (current != '*' || text[idx+1] != '/');
            idx +=2 ;
            return new MultilineSpannedToken(new SpannedToken(3, new Span(start, idx - start)), lines);
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

        class Test
        {
            [Test]
            public void LexEmpty()
            {
                var result = Lexer.RunInternal("").ToArray();
                Assert.AreEqual(0, result.Length);
            }

            [Test]
            public void LexSingle()
            {
                var result = Lexer.RunInternal("a").ToArray();
                Assert.AreEqual(new Span(0, 1), result[0].Token.Span);
            }

            [Test]
            public void LexWhitespace()
            {
                var result = Lexer.RunInternal("a ").ToArray();
                Assert.AreEqual(new Span(0, 1), result[0].Token.Span);
                Assert.AreEqual(new Span(1, 1), result[1].Token.Span);
            }

            [Test]
            public void LexLines()
            {
                var result = Lexer.RunInternal("ab  ad  \n  ab  ra  abg").ToArray();
                Assert.AreEqual(new Span(0, 2), result[0].Token.Span);
                Assert.AreEqual(new Span(2, 2), result[1].Token.Span);
                Assert.AreEqual(new Span(4, 2), result[2].Token.Span);
                Assert.AreEqual(new Span(6, 2), result[3].Token.Span);
                Assert.AreEqual(new Span(8, 1), result[4].Token.Span);
                Assert.AreEqual(new Span(9, 2), result[5].Token.Span);
                Assert.AreEqual(new Span(11, 2), result[6].Token.Span);
                Assert.AreEqual(new Span(13, 2), result[7].Token.Span);
                Assert.AreEqual(new Span(15, 2), result[8].Token.Span);
                Assert.AreEqual(new Span(17, 2), result[9].Token.Span);
                Assert.AreEqual(new Span(19, 3), result[10].Token.Span);
            }

            [Test]
            public void LexMultilineComment()
            {
                var result = Lexer.RunInternal("12  /*\n\ndsad*/ 23").ToArray();
                Assert.AreEqual(new Span(0, 2), result[0].Token.Span);
                Assert.AreEqual(new Span(2, 2), result[1].Token.Span);
                Assert.AreEqual(new Span(4, 10), result[2].Token.Span);
                Assert.AreEqual(3, result[2].Lines);
                Assert.AreEqual(new Span(14, 1), result[3].Token.Span);
                Assert.AreEqual(new Span(15, 2), result[4].Token.Span);
            }
        }
    }
}
