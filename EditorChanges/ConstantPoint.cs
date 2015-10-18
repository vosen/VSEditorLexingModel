using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorChanges
{
    class ConstantPoint : ITrackingPoint
    {
        readonly int point;

        public ConstantPoint(int point)
        {
            this.point = point;
        }

        public ITextBuffer TextBuffer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public TrackingFidelityMode TrackingFidelity
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public PointTrackingMode TrackingMode
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public char GetCharacter(ITextSnapshot snapshot)
        {
            throw new NotImplementedException();
        }

        public SnapshotPoint GetPoint(ITextSnapshot snapshot)
        {
            throw new NotImplementedException();
        }

        public int GetPosition(ITextVersion version)
        {
            return point;
        }

        public int GetPosition(ITextSnapshot snapshot)
        {
            return point;
        }
    }
}
