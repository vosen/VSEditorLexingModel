using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorChanges
{
    class OnDemandEnumerator
    {
        readonly Func<IEnumerable<string>, IEnumerable<MultilineSpannedToken>> lexer;
        readonly ITextSnapshot snapshot;
        readonly int start;
        readonly int end;
        private int currentLine;

        public OnDemandEnumerator(Func<IEnumerable<string>, IEnumerable<MultilineSpannedToken>> lexer, ITextSnapshot snapshot, int start, int end)
        {
            this.lexer = lexer;
            this.end = end;
            this.start = start;
            this.snapshot = snapshot;
        }

        public IEnumerable<IList<MultilineSpannedToken>> Current { get; private set; }

        public bool MoveNext()
        {
            if (currentLine >= snapshot.LineCount)
                return false;
            lexer(ConsumeLines());
            return true;
        }

        private IEnumerable<string> ConsumeLines()
        {
            while (currentLine < snapshot.LineCount)
                yield return snapshot.GetLineFromLineNumber(currentLine++).GetTextIncludingLineBreak();
        }
    }
}
