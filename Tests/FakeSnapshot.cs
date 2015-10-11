using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tests
{
  class FakeSnapshot : ITextSnapshot
  {
    readonly string txt;

    public FakeSnapshot(string txt)
    {
      this.txt = txt;
    }

    public char this[int position]
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public Microsoft.VisualStudio.Utilities.IContentType ContentType
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public int Length
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public int LineCount
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public IEnumerable<ITextSnapshotLine> Lines
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public ITextBuffer TextBuffer
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public ITextVersion Version
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
    {
      throw new NotImplementedException();
    }

    public ITrackingPoint CreateTrackingPoint(int position, PointTrackingMode trackingMode)
    {
      return new FakeTrackingPoint(position);
    }

    public ITrackingPoint CreateTrackingPoint(int position, PointTrackingMode trackingMode, TrackingFidelityMode trackingFidelity)
    {
      throw new NotImplementedException();
    }

    public ITrackingSpan CreateTrackingSpan(Span span, SpanTrackingMode trackingMode)
    {
      throw new NotImplementedException();
    }

    public ITrackingSpan CreateTrackingSpan(int start, int length, SpanTrackingMode trackingMode)
    {
      throw new NotImplementedException();
    }

    public ITrackingSpan CreateTrackingSpan(Span span, SpanTrackingMode trackingMode, TrackingFidelityMode trackingFidelity)
    {
      throw new NotImplementedException();
    }

    public ITrackingSpan CreateTrackingSpan(int start, int length, SpanTrackingMode trackingMode, TrackingFidelityMode trackingFidelity)
    {
      throw new NotImplementedException();
    }

    public ITextSnapshotLine GetLineFromLineNumber(int lineNumber)
    {
      throw new NotImplementedException();
    }

    public ITextSnapshotLine GetLineFromPosition(int position)
    {
      throw new NotImplementedException();
    }

    public int GetLineNumberFromPosition(int position)
    {
      throw new NotImplementedException();
    }

    public string GetText()
    {
      return this.txt;
    }

    public string GetText(Span span)
    {
      throw new NotImplementedException();
    }

    public string GetText(int startIndex, int length)
    {
      throw new NotImplementedException();
    }

    public char[] ToCharArray(int startIndex, int length)
    {
      throw new NotImplementedException();
    }

    public void Write(TextWriter writer)
    {
      throw new NotImplementedException();
    }

    public void Write(TextWriter writer, Span span)
    {
      throw new NotImplementedException();
    }
  }
}
