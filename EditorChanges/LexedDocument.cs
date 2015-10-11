﻿using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorChanges
{
    public class LexedDocument
    {
        readonly Func<IEnumerable<string>, IEnumerable<MultilineSpannedToken>> lexer;
        ITextSnapshot current;
        List<LexedLine> lines;

        public IReadOnlyList<LexedLine> Lines { get { return lines; } }

        public LexedDocument(
          Func<IEnumerable<string>, IEnumerable<MultilineSpannedToken>> lexer,
          ITextSnapshot snapshot)
        {
            this.lexer = lexer;
            Initialize(snapshot);
        }

        static bool IsNewline(int type)
        {
            return type == 1;
        }

        public void Update(TextContentChangedEventArgs change)
        {
            if (this.current.Version.VersionNumber != change.BeforeVersion.VersionNumber)
                Initialize(change.After);
            else
                UpdateCore(change);
        }

        void UpdateCore(TextContentChangedEventArgs change)
        {
            this.current = change.After;
            foreach (ITextChange tc in change.Changes)
                ApplyChange(tc);
        }

        void ApplyChange(ITextChange change)
        {
            int realChangeStartLineIndex = LineContaining(change.NewPosition);
        }

        // Since the tokens can be multiline, some lines will contain no tokens. eg.
        // 1:/* This is a comment
        // 2: * spanning three lines
        // 3: */ let x = 1;
        // in this case line 2 contains no tokens, so we have to go back
        // and find the first non-empty line
        int LineContaining(int pos)
        {
            int curr = current.GetLineNumberFromPosition(pos);
            // In the face of multi-line tokens we are not guaranteed that
            // the first token in the line starts at the start of the line.
            // Eg. in the above example the first token in the line 3
            // is whitespace with the span [3,4)
            if (!lines[curr].IsEmpty && IsInsideFirstToken(lines[curr], pos))
                return curr;
            for (; curr > 0; curr--)
            {
                if (!lines[curr].IsEmpty)
                    return curr;
            }
            return curr;
        }

        private bool IsInsideFirstToken(LexedLine line, int absPos)
        {
            if(line.Tokens[0].Span.Start == 0)
                return true;
            int absLineStart = line.GetStart(current);
            return absPos - absLineStart >= line.Tokens[0].Span.Start;
        }

        private Range LinesRange(ITextSnapshot change, Span span)
        {
            int start = current.GetLineNumberFromPosition(span.Start);
            int end = current.GetLineNumberFromPosition(span.End);
            return new Range
            {
                Start = start,
                Count = end - start + 1
            };
        }

        IEnumerable<string> GetLines(int startLine, int count)
        {
            for (int i = startLine; i < startLine + count; i++)
            {
                yield return current.GetLineFromLineNumber(i).GetTextIncludingLineBreak();
            }
        }

        void Initialize(ITextSnapshot snapshot)
        {
            this.current = snapshot;
            lines = new List<LexedLine>();
            foreach (var line in SplitByNewline(lexer(new[] { snapshot.GetText() })))
            {
                if (line == null)
                    lines.Add(new LexedLine());
                else if (!line.Item2)
                    lines.Add(new LexedLine(current, current.GetLineFromPosition(line.Item1[0].Span.Start).Start.Position, line.Item1));
                else
                    lines.Add(new LexedLine(current, line.Item1));
            }
        }

        // bool part of the tuple is a flag for the cases where the first token doesn't start
        // at start of the line. eg. in the following example:
        // 1: /* something
        // 2:  */let x = 1;
        // Line 2 will be marked with false, because first token (keyword let) starts,
        // relativly to the line start, at position 2
        static IEnumerable<Tuple<List<SpannedToken>, bool>> SplitByNewline(IEnumerable<MultilineSpannedToken> source)
        {
            bool firstTokenStartsWithLine = true;
            var list = new List<SpannedToken>();
            foreach (var token in source)
            {
                list.Add(token.Token);
                // Newline token
                if (IsNewline(token.Token.Type))
                {
                    yield return Tuple.Create(list, firstTokenStartsWithLine);
                    firstTokenStartsWithLine = true;
                    list = new List<SpannedToken>();
                }
                // Multiline token
                else if (token.Lines > 1)
                {
                    yield return Tuple.Create(list, firstTokenStartsWithLine);
                    firstTokenStartsWithLine = false;
                    for (int i = 0; i < token.Lines - 2; i++) // emit empty lines
                        yield return null;
                    list = new List<SpannedToken>();
                }
            }
            if (list.Count > 0)
                yield return Tuple.Create(list, firstTokenStartsWithLine);
            else
                yield return null;
        }
    }
}