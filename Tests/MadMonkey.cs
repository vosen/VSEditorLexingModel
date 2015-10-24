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
    class MadMonkey
    {
        static string letters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";

        [Test]
        [Explicit]
        public void MashKeys()
        {
            var buffer = new MockTextBuffer("", (string)null);
            var doc = new LexedDocument(FakeLanguage.Tokenizer.Run, buffer.CurrentSnapshot);
            var rand = new Random();
            while(true)
                TypeStuff(buffer, doc, rand);
        }

        private void TypeStuff(ITextBuffer buffer, LexedDocument doc, Random rand)
        {
            string text1 = buffer.CurrentSnapshot.GetText();
            var snapshot = buffer.CurrentSnapshot;
            int insertPos = rand.Next(snapshot.Length + 1);
            int length = rand.Next(snapshot.Length - insertPos);
            Span replacedSpan = new Span(insertPos, length);
            string insertedText = GenerateText(rand);
            buffer.Replace(replacedSpan, insertedText);
            var args = new TextContentChangedEventArgs(snapshot, buffer.CurrentSnapshot, EditOptions.None, null);
            doc.ApplyTextChanges(args);
            var expectedtTokens = FakeLanguage.Tokenizer.Run(new [] { buffer.CurrentSnapshot.GetText() }, 0).Select(x => x.Span).ToArray();
            var actualTokens = doc.GetCoveringTokens(new Span(0, buffer.CurrentSnapshot.Length)).Select(x => x.GetSpan(buffer.CurrentSnapshot)).ToArray();
            if(expectedtTokens.Length != actualTokens.Length
               || expectedtTokens.Zip(actualTokens, (_1, _2) => new { _1 = _1, _2 = _2 }).Any(x => x._1 != x._2))
            {
                var error = String.Format("`{0}` replaced at {1} by `{2}`", text1, replacedSpan, insertedText);
                Assert.Fail(error);
            }
        }

        private string GenerateText(Random rand)
        {
            var length = rand.Next(0, 6) + 1;
            var sb = new StringBuilder(length);
            for(int i = 0; i <length; i++)
                sb.Append(GenerateTextSingle(rand));
            return sb.ToString();
        }
        

        static string GenerateTextSingle(Random r)
        {
            var sample = r.NextDouble();
            if(sample < 0.2)
                return letters[r.Next(0, letters.Length)].ToString();
            if(sample < 0.4)
                return " ";
            if(sample < 0.6)
                return "/*";
            if(sample < 0.8)
                return "*/";
            else
                return "\n";
        }
    }
}
