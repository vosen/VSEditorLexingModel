namespace EditorChanges
{
    public struct LineSpan
    {
        public int StartLine { get; set; }
        public int EndLine { get; set; }
        public int EndTokenIndex { get; set; }
    }
}