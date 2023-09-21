using System;
using System.Text.RegularExpressions;

namespace UnrealObjectPtrTool
{
    public class NativePointerItem
    {
        public string File { get; set; }
        public int Line { get; set; }
        public string Pointer { get; set; }


        public NativePointerItem(string line)
        {
            File = GetFile(line);
            Line = GetLine(line);
            Pointer = GetPointer(line);
        }


        private string GetFile(string line)
        {
            // Changed regex because sometimes logs do not contain LogCompile as initial start and we cannot assume this.
            return Regex.Match(line, @"^[A-z]:.*(?=\()").Value;
        }


        private int GetLine(string line)
        {
            return Convert.ToInt32(Regex.Match(line, @"(?<=\()\d+(?=\))").Value);
        }


        private string GetPointer(string line)
        {
            return Regex.Match(line, @"(?<=\[\[\[).*(?=\]\]\])").Value;
        }
    }
}