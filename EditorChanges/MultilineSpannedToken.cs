using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorChanges
{
    public struct MultilineSpannedToken
    {
        public MultilineSpannedToken(SpannedToken spannedToken)
            : this(spannedToken, 1)
        { }

        public MultilineSpannedToken(SpannedToken spannedToken, int lines) : this()
        {
            this.Token = spannedToken;
            Lines = lines;
        }

        public SpannedToken Token { get; private set; }
        public int Lines { get; private set; }
    }
}
