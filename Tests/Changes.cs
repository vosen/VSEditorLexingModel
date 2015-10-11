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
            var buffer = new MockTextBuffer("ad ada \n das 12", (string)null);
            var snapshot = buffer.CurrentSnapshot;
            var doc = new LexedDocument(Lexer.Run, snapshot);
            buffer.Insert(0, "z");
            doc.Update(new TextContentChangedEventArgs(snapshot, buffer.CurrentSnapshot, EditOptions.None, null));
        }

        [Test]
        public void Multilines()
        {
            var buffer = new MockTextBuffer("12  /*\n\ndsad*/ 23", (string)null);
            var snapshot = buffer.CurrentSnapshot;
            var doc = new LexedDocument(Lexer.Run, snapshot);
            Assert.AreEqual(3, doc.Lines.Count);
            buffer.Insert(0, "z");
            doc.Update(new TextContentChangedEventArgs(snapshot, buffer.CurrentSnapshot, EditOptions.None, null));
        }

        [Test]
        public void InsideEmptyLine()
        {
            var buffer = new MockTextBuffer("/*\n\naz*/ ", (string)null);
            var snapshot = buffer.CurrentSnapshot;
            var doc = new LexedDocument(Lexer.Run, snapshot);
            buffer.Insert(3, "x");
            doc.Update(new TextContentChangedEventArgs(snapshot, buffer.CurrentSnapshot, EditOptions.None, null));
        }

        [Test]
        public void HangingMultiline()
        {
            var buffer = new MockTextBuffer("/*\n\n*/ a ", (string)null);
            var snapshot = buffer.CurrentSnapshot;
            var doc = new LexedDocument(Lexer.Run, snapshot);
            buffer.Insert(6, "x");
            doc.Update(new TextContentChangedEventArgs(snapshot, buffer.CurrentSnapshot, EditOptions.None, null));
        }
    }
}
