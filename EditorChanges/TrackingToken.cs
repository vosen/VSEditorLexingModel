﻿using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorChanges
{
    public struct TrackingToken
    {
        public class NonOverlappingComparer : IComparer<TrackingToken>
        {
            public ITextSnapshot Version;

            public int Compare(TrackingToken x, TrackingToken y)
            {
                return x.Start.GetPosition(Version).CompareTo(y.Start.GetPosition(Version));
            }
        }

        public ITrackingPoint Start;
        public int Length;
        public int Type;

        internal TrackingToken(ITextSnapshot snapshot, SpannedToken arg) : this()
        {
            Start = snapshot.CreateTrackingPoint(arg.Span.Start, PointTrackingMode.Negative);
            Length = arg.Span.Length;
            Type = arg.Type;
        }

        public bool Contains(ITextSnapshot snap, int pos)
        {
            int realStart = Start.GetPosition(snap);
            return realStart >= pos && realStart + Length < pos;
        }

        public Span GetSpan(ITextSnapshot snap)
        {
            return new Span(Start.GetPosition(snap), Length);
        }
    }

}