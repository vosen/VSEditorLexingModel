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
            Assert.AreEqual(0, invalid.StartLine);
            Assert.AreEqual(0, invalid.EndLine);
            Assert.AreEqual(0, invalid.EndTokenIndex);
        }

        [Test]
        public void Multilines()
        {
            var buffer = new MockTextBuffer("12  /*\n\ndsad*/ 23", (string)null);
            var snapshot = buffer.CurrentSnapshot;
            var doc = new LexedDocument(Lexer.Run, snapshot);
            buffer.Insert(0, "z");
            var args = new TextContentChangedEventArgs(snapshot, buffer.CurrentSnapshot, EditOptions.None, null);
            var invalid = doc.GetInvalidated(snapshot, args.Changes.First());
            Assert.AreEqual(0, invalid.StartLine);
            Assert.AreEqual(0, invalid.EndLine);
            Assert.AreEqual(0, invalid.EndTokenIndex);
        }

        [Test]
        public void InsideEmptyLine()
        {
            var buffer = new MockTextBuffer("/*\n\naz*/ ", (string)null);
            var snapshot = buffer.CurrentSnapshot;
            var doc = new LexedDocument(Lexer.Run, snapshot);
            buffer.Insert(3, "x");
            var args = new TextContentChangedEventArgs(snapshot, buffer.CurrentSnapshot, EditOptions.None, null);
            var invalid = doc.GetInvalidated(snapshot, args.Changes.First());
            Assert.AreEqual(0, invalid.StartLine);
            Assert.AreEqual(0, invalid.EndLine);
            Assert.AreEqual(0, invalid.EndTokenIndex);
        }

        [Test]
        public void HangingMultiline()
        {
            var buffer = new MockTextBuffer("/*\n\n*/ a ", (string)null);
            var snapshot = buffer.CurrentSnapshot;
            var doc = new LexedDocument(Lexer.Run, snapshot);
            buffer.Insert(6, "x");
            var args = new TextContentChangedEventArgs(snapshot, buffer.CurrentSnapshot, EditOptions.None, null);
            var invalid = doc.GetInvalidated(snapshot, args.Changes.First());
            Assert.AreEqual(2, invalid.StartLine);
            Assert.AreEqual(2, invalid.EndLine);
            Assert.AreEqual(0, invalid.EndTokenIndex);
        }

        [Test]
        public void InsertIntoEmpty()
        {
            var buffer = new MockTextBuffer("", (string)null);
            var snapshot = buffer.CurrentSnapshot;
            var doc = new LexedDocument(Lexer.Run, snapshot);
            buffer.Insert(0, "x");
            var args = new TextContentChangedEventArgs(snapshot, buffer.CurrentSnapshot, EditOptions.None, null);
            var invalid = doc.GetInvalidated(snapshot, args.Changes.First());
            Assert.AreEqual(0, invalid.StartLine);
            Assert.AreEqual(0, invalid.EndLine);
            Assert.AreEqual(0, invalid.EndTokenIndex);
        }

        [Test]
        public void InsertInTheMiddle()
        {
            var buffer = new MockTextBuffer("ab\ncd\nef", (string)null);
            var snapshot = buffer.CurrentSnapshot;
            var doc = new LexedDocument(Lexer.Run, snapshot);
            buffer.Insert(4, "x");
            var args = new TextContentChangedEventArgs(snapshot, buffer.CurrentSnapshot, EditOptions.None, null);
            var invalid = doc.GetInvalidated(snapshot, args.Changes.First());
            Assert.AreEqual(1, invalid.StartLine);
            Assert.AreEqual(1, invalid.EndLine);
            Assert.AreEqual(0, invalid.EndTokenIndex);
        }

        [Test]
        public void NewlineInsert()
        {
            var buffer = new MockTextBuffer("ab\ncd\nef", (string)null);
            var snapshot = buffer.CurrentSnapshot;
            var doc = new LexedDocument(Lexer.Run, snapshot);
            buffer.Replace(new Span(3,2), "1\n2");
            var args = new TextContentChangedEventArgs(snapshot, buffer.CurrentSnapshot, EditOptions.None, null);
            var invalid = doc.GetInvalidated(snapshot, args.Changes.First());
            Assert.AreEqual(1, invalid.StartLine);
            Assert.AreEqual(1, invalid.EndLine);
            Assert.AreEqual(0, invalid.EndTokenIndex);
        }

        [Test]
        public void ReplaceLast()
        {
            var buffer = new MockTextBuffer("ab\ncd ef\ngh", (string)null);
            var snapshot = buffer.CurrentSnapshot;
            var doc = new LexedDocument(Lexer.Run, snapshot);
            buffer.Replace(new Span(6, 2), "ef");
            var args = new TextContentChangedEventArgs(snapshot, buffer.CurrentSnapshot, EditOptions.None, null);
            var invalid = doc.GetInvalidated(snapshot, args.Changes.First());
            Assert.AreEqual(1, invalid.StartLine);
            Assert.AreEqual(1, invalid.EndLine);
            Assert.AreEqual(2, invalid.EndTokenIndex);
        }
    }
}
