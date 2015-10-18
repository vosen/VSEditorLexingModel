using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorChanges
{
    static class SortedSetExtensions
    {
        static public IEnumerable<TrackingToken> CoveringTokens(this SortedSet<TrackingToken> tree, ITextSnapshot version, Span span)
        {
            List<TrackingToken> tokens = new List<TrackingToken>();
            if(tree.Root != null)
                FillCoveringTokens(tree.Root, version, span, tokens);
            return tokens;
        }

        private static void FillCoveringTokens(SortedSet<TrackingToken>.Node current, ITextSnapshot version, Span span, List<TrackingToken> tokens)
        {
            var currentSpan = current.Item.GetSpan(version);
            if (current.Left != null && span.Start <= currentSpan.Start)
                FillCoveringTokens(current.Left, version, span, tokens);
            if (RightInclusiveOverlap(currentSpan, span))
                tokens.Add(current.Item);
            if (current.Right != null && span.End >= currentSpan.End)
                FillCoveringTokens(current.Right, version, span, tokens);
        }

        static bool RightInclusiveOverlap(Span current, Span span)
        {
            if(span.End >= current.End)
                return span.Start <= current.End;
            if(span.Start <= current.Start)
                return span.End > current.Start;
            return true;
        }
    }
}
