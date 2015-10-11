﻿using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorChanges
{
    public struct LexedLine
    {
        readonly ITrackingPoint start; // will be null in case of empty lines
        readonly SpannedToken[] tokens; // will be null in case of empty lines
        internal SpannedToken[] Tokens { get { return tokens; } }

        internal LexedLine(ITextSnapshot snapshot, int startPoint, List<SpannedToken> line) : this()
        {
            Debug.Assert(line.Count > 0);
            start = snapshot.CreateTrackingPoint(startPoint, PointTrackingMode.Positive);
            tokens = line
              .Select(token => token.Move(-startPoint))
              .ToArray();
        }

        internal LexedLine(ITextSnapshot snapshot, List<SpannedToken> line)
            : this(snapshot, line[0].Span.Start, line)
        { }

        public bool IsEmpty { get { return start == null; } }

        internal SpannedToken GetContaining(ITextSnapshot snap, int absolutePosition)
        {
            int absoluteStart = start.GetPosition(snap);
            int relativePosition = absolutePosition - absoluteStart;
            int tokenIdx =  Array.BinarySearch<SpannedToken>(
                Tokens,
                new SpannedToken(0, new Span(relativePosition, 0)),
                SpannedTokenOverlapComparer.Instance);
            return Tokens[tokenIdx];
        }

        internal int GetStart(ITextSnapshot snaps)
        {
            return start.GetPosition(snaps);
        }
    }
}