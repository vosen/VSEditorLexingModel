using EditorChanges;
using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    class FakeLexer
    {
        internal static Func<IEnumerable<string>, int, IEnumerable<SpannedToken>> FromSpans(IEnumerable<Span> prepared)
        {
            var temp = prepared.Select(x => new SpannedToken(0, x)).ToArray();
            return  (_, __) => temp;
        }
    }
}
