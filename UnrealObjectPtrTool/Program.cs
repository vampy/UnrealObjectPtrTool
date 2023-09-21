using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace UnrealObjectPtrTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // No need to get username, %AppData% would do the trick.
            string filePath = @"%AppData%\Local\UnrealBuildTool\Log_UHT.txt";
            if (args.Length > 0)
            {
                filePath = args[0];
            }

            var pointersFound = ReadUHTFile(filePath);
            if (pointersFound == null)
                return;

            var pointers = GetNativePointersFound(pointersFound);
            PreviewNativePointersFound(pointers);

            WriteToFiles(pointers);

            Console.WriteLine("The files have been updated. Press any key to exit...");
            Console.ReadKey();
        }

        static List<string> ReadUHTFile(string FilePath)
        {
            if (!File.Exists(FilePath))
            {
                Console.WriteLine();
                Console.WriteLine($"File could not be found -> {FilePath}");
                Console.ReadKey();

                return null;
            }


            var pointersFound = new List<string>();
            var lines = File.ReadAllLines(FilePath);
            var search = new Regex("Consider TObjectPtr as an alternative.");

            foreach (var line in lines)
            {
                if (search.IsMatch(line))
                    pointersFound.Add(line);
            }

            if (pointersFound.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("No native pointer usage detected");
                Console.ReadKey();

                return null;
            }

            return pointersFound;
        }


        static Dictionary<string, List<NativePointerItem>> GetNativePointersFound(List<string> pointersFound)
        {
            Console.WriteLine();
            Console.WriteLine("Files that will be changed:");

            var pointers = new Dictionary<string, List<NativePointerItem>>();

            foreach (var pointerLine in pointersFound)
            {
                var pointer = new NativePointerItem(pointerLine);

                if (!pointers.ContainsKey(pointer.File))
                {
                    pointers.Add(pointer.File, new List<NativePointerItem>());
                }

                pointers[pointer.File].Add(pointer);
            }

            return pointers;
        }


        static void PreviewNativePointersFound(Dictionary<string, List<NativePointerItem>> pointers)
        {
            foreach (var item in pointers)
            {
                Console.WriteLine();
                Console.WriteLine($"{item.Key}");

                foreach (var info in item.Value)
                {
                    Console.WriteLine($"Line {info.Line} :: {info.Pointer}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }


        static void WriteToFiles(Dictionary<string, List<NativePointerItem>> pointers)
        {
            foreach (var item in pointers)
            {
                var file = item.Key;

                if (!File.Exists(file))
                {
                    Console.WriteLine();
                    Console.WriteLine($"File could not be found -> {file}");
                    continue;
                }


                Console.WriteLine();
                Console.WriteLine($"{item.Key}");

                var lines = File.ReadAllLines(file);

                foreach (var info in item.Value)
                {
                    lines = ReplaceLine(lines, info.Line, info.Pointer);
                }

                File.WriteAllLines(file, lines);
            }
        }


        static string[] ReplaceLine(string[] lines, int line, string pointer)
        {
            var lineIndex = line - 1;

            if (lines.Length < lineIndex)
            {
                Console.WriteLine();
                Console.WriteLine($"Line {line} is invalid");

                return lines;
            }

            var value = lines[lineIndex];

            var newPointer = $"TObjectPtr<{pointer}>".Replace("*", "") + AddSpace(value, pointer);

            var oldValue = value;
            var newValue = value.Replace(pointer, newPointer);

            lines[lineIndex] = newValue;

            Console.WriteLine($"({line}) {oldValue}");
            Console.WriteLine($"({line}) {newValue}");
            Console.WriteLine();

            return lines;
        }


        static string AddSpace(string line, string pointer)
        {
            var indexOfPointer = line.IndexOf(pointer);
            var indexOfPointerNextChar = indexOfPointer + pointer.Length;

            if (indexOfPointer >= 0 && line.Length > indexOfPointerNextChar)
            {
                var value = line[indexOfPointerNextChar];
                var chars = new List<char> { ' ', '>' };

                return chars.Any(x => value == x) ? string.Empty : " ";
            }

            return string.Empty;
        }
    }
}
