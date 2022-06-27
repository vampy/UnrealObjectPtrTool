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
            return Regex.Match(line, @"(?<=LogCompile: ).*(?=\()").Value;
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