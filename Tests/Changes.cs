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
            AssertInsertCorrectness("ad daa \n das 12", new Span(0, 0), "z");
        }

        [Test]
        public void InsertInsideFirstToken()
        {
            AssertInsertCorrectness("ad daa \n das 12", new Span(1, 0), "z");
        }

        [Test]
        public void InsertAtTheEndOfTheFirstToken()
        {
            AssertInsertCorrectness("ad daa \n das 12", new Span(2, 0), "z");
        }

        [Test]
        public void InsertIntoEmpty()
        {
            AssertInsertCorrectness("", new Span(), "z");
        }

        [Test]
        public void ReplaceMidTokens()
        {
            AssertInsertCorrectness("ab  cd  ", new Span(1, 4), "bc");
        }

        [Test]
        public void RemoveComment()
        {
            AssertInsertCorrectness("/*  ab  cd  fg", new Span(0, 2), "");
        }

        [Test]
        public void AddComment()
        {
            AssertInsertCorrectness("   ab  */cd  fg", new Span(0, 1), "/*");
        }

        [Test]
        public void ChangeMiddleOfToken()
        {
            AssertInsertCorrectness("ab  cd  ef  ", new Span(5, 0), "z");
        }

        [Test]
        public void ReplaceAll()
        {
            AssertInsertCorrectness("ab  cd  ef  ", new Span(0, 12), "x");
        }

        [Test]
        public void ReplaceMost()
        {
            AssertInsertCorrectness("ab  cd  ef", new Span(0, 7), "");
        }

        [Test]
        public void ReplaceMiddle()
        {
            AssertInsertCorrectness("ab  cd  ef  ", new Span(5, 4), "/*");
        }

        [Test]
        public void InsertNewAtTheEnd()
        {
            AssertInsertCorrectness(" a  ", new Span(3, 0), "b");
        }

        [Test]
        public void ReplaceMulti()
        {
            AssertInsertCorrectness(" a ", new Span(0, 1), "ab  cd  ef  gh");
        }

        private void AssertInsertCorrectness(string text, Span replaced, string inserted)
        {
            var buffer = new MockTextBuffer(text, (string)null);
            var snapshot = buffer.CurrentSnapshot;
            var doc = new LexedDocument(FakeLanguage.Tokenizer.Run, snapshot);
            buffer.Replace(replaced, inserted);
            var args = new TextContentChangedEventArgs(snapshot, buffer.CurrentSnapshot, EditOptions.None, null);
            doc.ApplyTextChanges(args);
            var expectedtTokens = FakeLanguage.Tokenizer.Run(new [] { buffer.CurrentSnapshot.GetText() }, 0).Select(x => x.Span);
            var actualTokens = doc.GetTokens(new Span(0, buffer.CurrentSnapshot.Length)).Select(x => x.GetSpan(buffer.CurrentSnapshot));
            CollectionAssert.AreEqual(expectedtTokens, actualTokens);
        }
    }
}
