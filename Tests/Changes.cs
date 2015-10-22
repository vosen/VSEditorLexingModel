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

        [Test]
        public void ReplaceMidTokens()
        {
            var buffer = new MockTextBuffer("ab  cd  ", (string)null);
            var snapshot = buffer.CurrentSnapshot;
            var doc = new LexedDocument(Lexer.Run, snapshot);
            buffer.Replace(new Span(1, 4), "bc");
            doc.ApplyVersion(buffer.CurrentSnapshot);
            var args = new TextContentChangedEventArgs(snapshot, buffer.CurrentSnapshot, EditOptions.None, null);
            var invalid = doc.GetInvalidated(snapshot, args.Changes.First());
            Assert.AreEqual(3, invalid.Count());
            var newTokens = doc.Rescan(snapshot, invalid, args.Changes.First().Delta);
            Assert.AreEqual(1, newTokens.Count);
            Assert.AreEqual(0, newTokens[0].GetStart(buffer.CurrentSnapshot));
            Assert.AreEqual(4, newTokens[0].Length);
        }

        [Test]
        public void RemoveComment()
        {
            var buffer = new MockTextBuffer("/*  ab  cd  fg", (string)null);
            var snapshot = buffer.CurrentSnapshot;
            var doc = new LexedDocument(Lexer.Run, snapshot);
            buffer.Replace(new Span(0, 2), "");
            doc.ApplyVersion(buffer.CurrentSnapshot);
            var args = new TextContentChangedEventArgs(snapshot, buffer.CurrentSnapshot, EditOptions.None, null);
            var invalid = doc.GetInvalidated(snapshot, args.Changes.First());
            Assert.AreEqual(1, invalid.Count());
            var newTokens = doc.Rescan(snapshot, invalid, args.Changes.First().Delta);
            Assert.AreEqual(6, newTokens.Count);
            Assert.AreEqual(0, newTokens[0].GetStart(buffer.CurrentSnapshot));
            Assert.AreEqual(2, newTokens[0].Length);
            Assert.AreEqual(2, newTokens[1].GetStart(buffer.CurrentSnapshot));
            Assert.AreEqual(2, newTokens[1].Length);
            Assert.AreEqual(4, newTokens[2].GetStart(buffer.CurrentSnapshot));
            Assert.AreEqual(2, newTokens[2].Length);
            Assert.AreEqual(6, newTokens[3].GetStart(buffer.CurrentSnapshot));
            Assert.AreEqual(2, newTokens[3].Length);
            Assert.AreEqual(8, newTokens[4].GetStart(buffer.CurrentSnapshot));
            Assert.AreEqual(2, newTokens[4].Length);
            Assert.AreEqual(10, newTokens[5].GetStart(buffer.CurrentSnapshot));
            Assert.AreEqual(2, newTokens[5].Length);
        }
        
        [Test]
        public void AddComment()
        {
            var buffer = new MockTextBuffer("   ab  */cd  fg", (string)null);
            var snapshot = buffer.CurrentSnapshot;
            var doc = new LexedDocument(Lexer.Run, snapshot);
            buffer.Replace(new Span(0, 1), "/*");
            doc.ApplyVersion(buffer.CurrentSnapshot);
            var args = new TextContentChangedEventArgs(snapshot, buffer.CurrentSnapshot, EditOptions.None, null);
            var invalid = doc.GetInvalidated(snapshot, args.Changes.First());
            Assert.AreEqual(1, invalid.Count());
            var newTokens = doc.Rescan(snapshot, invalid, args.Changes.First().Delta);
            Assert.AreEqual(4, newTokens.Count);
            Assert.AreEqual(0, newTokens[0].GetStart(buffer.CurrentSnapshot));
            Assert.AreEqual(10, newTokens[0].Length);
            Assert.AreEqual(10, newTokens[1].GetStart(buffer.CurrentSnapshot));
            Assert.AreEqual(2, newTokens[1].Length);
            Assert.AreEqual(12, newTokens[2].GetStart(buffer.CurrentSnapshot));
            Assert.AreEqual(2, newTokens[2].Length);
            Assert.AreEqual(14, newTokens[3].GetStart(buffer.CurrentSnapshot));
            Assert.AreEqual(2, newTokens[3].Length);
        }
    }
}
