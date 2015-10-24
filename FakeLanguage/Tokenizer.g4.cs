using Antlr4.Runtime;
using EditorChanges;
using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeLanguage
{
    partial class Tokenizer
    {
        public static IEnumerable<SpannedToken> Run(IEnumerable<string> arg, int offset)
        {
            var lexer = new Tokenizer(new AntlrInputStream(new TextSegmentsCharStream(arg)));
            while (true)
            {
                IToken current = lexer.NextToken();
                if (current.Type == Tokenizer.Eof)
                    break;
                yield return new SpannedToken(current.Type, new Span(current.StartIndex + offset, current.StopIndex - current.StartIndex + 1));
            }
        }
    }
}
