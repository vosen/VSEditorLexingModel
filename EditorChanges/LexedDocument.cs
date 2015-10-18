using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorChanges
{
    public class LexedDocument
    {
        readonly TrackingToken.NonOverlappingComparer comparer = new TrackingToken.NonOverlappingComparer();
        SortedSet<TrackingToken> tree;
        readonly Func<IEnumerable<string>, IEnumerable<SpannedToken>> lexer;

        public LexedDocument(Func<IEnumerable<string>, IEnumerable<SpannedToken>> lexer, ITextSnapshot snapshot)
        {
            this.lexer = lexer;
            comparer.Version = snapshot;
            Initialize();
        }

        private void Initialize()
        {
            tree = new SortedSet<TrackingToken>(lexer(new string[] { comparer.Version.GetText() }).Select(t => new TrackingToken(comparer.Version, t)), comparer);
        }

        public IEnumerable<TrackingToken> GetInvalidated(ITextSnapshot oldSnapshot, ITextChange change)
        {
            return tree.CoveringTokens(oldSnapshot, change.OldSpan);
        }
    }
}
