using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorChanges
{
    public struct SpannedToken
    {
        public Span Span { get; private set; }
        public int Type { get; private set; }

        public SpannedToken(int token, Span span) : this()
        {
            this.Span = span;
            this.Type = token;
        }
    }
}
