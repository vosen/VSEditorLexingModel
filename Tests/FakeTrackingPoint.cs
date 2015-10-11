using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
  class FakeTrackingPoint : ITrackingPoint
  {
    private readonly int position;

    public FakeTrackingPoint(int position)
    {
      this.position = position;
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
      throw new NotImplementedException();
    }

    public int GetPosition(ITextSnapshot snapshot)
    {
      throw new NotImplementedException();
    }
  }
}
