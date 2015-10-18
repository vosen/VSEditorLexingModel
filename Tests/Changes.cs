using EditorChanges;
using Microsoft.VisualStudio.Text;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtilities.Mocks;

namespace Tests
{
    class Changes
    {
        [Test]
        public void InsertAtStart()
        {
            var buffer = new MockTextBuffer("ad daa \n das 12", (string)null);
            var snapshot = buffer.CurrentSnapshot;
            var doc = new LexedDocument(Lexer.Run, snapshot);
            buffer.Insert(0, "z");
            var args = new TextContentChangedEventArgs(snapshot, buffer.CurrentSnapshot, EditOptions.None, null);
            var invalid = doc.GetInvalidated(snapshot, args.Changes.First());
            Assert.AreEqual(0, invalid.Count());
        }

        [Test]
        public void InsertInsideFirstToken()
        {
            var buffer = new MockTextBuffer("ad daa \n das 12", (string)null);
            var snapshot = buffer.CurrentSnapshot;
            var doc = new LexedDocument(Lexer.Run, snapshot);
            buffer.Insert(1, "z");
            var args = new TextContentChangedEventArgs(snapshot, buffer.CurrentSnapshot, EditOptions.None, null);
            var invalid = doc.GetInvalidated(snapshot, args.Changes.First());
            Assert.AreEqual(1, invalid.Count());
        }

        [Test]
        public void InsertAtTheEndOfTheFirstToken()
        {
            var buffer = new MockTextBuffer("ad daa \n das 12", (string)null);
            var snapshot = buffer.CurrentSnapshot;
            var doc = new LexedDocument(Lexer.Run, snapshot);
            buffer.Insert(2, "z");
            var args = new TextContentChangedEventArgs(snapshot, buffer.CurrentSnapshot, EditOptions.None, null);
            var invalid = doc.GetInvalidated(snapshot, args.Changes.First());
            Assert.AreEqual(1, invalid.Count());
            Assert.AreEqual(new Span(0, 2), invalid.First().GetSpan(snapshot));
        }

        [Test]
        public void InsertIntoEmpty()
        {
            var buffer = new MockTextBuffer("", (string)null);
            var snapshot = buffer.CurrentSnapshot;
            var doc = new LexedDocument(FakeLexer.FromSpans(new [] { new Span() }), snapshot);
            buffer.Insert(0, "z");
            var args = new TextContentChangedEventArgs(snapshot, buffer.CurrentSnapshot, EditOptions.None, null);
            var invalid = doc.GetInvalidated(snapshot, args.Changes.First());
            Assert.AreEqual(1, invalid.Count());
        }
    }
}
