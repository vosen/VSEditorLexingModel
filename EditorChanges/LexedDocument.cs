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

        public IList<TrackingToken> GetInvalidated(ITextSnapshot oldSnapshot, ITextChange change)
        {
            return tree.CoveringTokens(oldSnapshot, change.OldSpan);
        }

        public IList<TrackingToken> Rescan(ITextSnapshot oldSnapshot, IList<TrackingToken> invalid)
        {
            int invalidTokensStart = invalid[0].GetStart(oldSnapshot);
            Span invalidSpan = new Span(invalidTokensStart, invalid[invalid.Count - 1].GetEnd(comparer.Version) - invalidTokensStart);
            string initialText = comparer.Version.GetText(invalidSpan);
            TrackingToken lastConsumed = new TrackingToken();
            List<TrackingToken> rescanned = new List<TrackingToken>();
            foreach(var token in  lexer(new string[] { initialText }.Concat(tree.InOrderAfter(comparer.Version, invalidSpan.End).Select(t =>
            {
                lastConsumed = t;
                return t.GetText(comparer.Version);
            }))))
            {
                rescanned.Add(new TrackingToken(comparer.Version, token));
                if (token.Span.End == invalidSpan.End)
                    break;
                if (!lastConsumed.IsEmpty && token.Span.End == lastConsumed.GetEnd(comparer.Version))
                    break;
            }
            return rescanned;
        }

        public void ApplyVersion(ITextSnapshot currentSnapshot)
        {
            comparer.Version = currentSnapshot;
        }
    }
}
