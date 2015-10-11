using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorChanges
{
    class SpannedTokenOverlapComparer : IComparer<SpannedToken>
    {
        internal static readonly SpannedTokenOverlapComparer Instance = new SpannedTokenOverlapComparer();

        public int Compare(SpannedToken x, SpannedToken y)
        {
            if(x.Span.OverlapsWith(y.Span))
                return 0;
            return x.Span.Start.CompareTo(y.Span.Start);
        }
    }
}
