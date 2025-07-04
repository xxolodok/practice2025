using CommandLib;
using System.IO;

namespace FileSystemCommands;

    public class FindFilesCommand : ICommand
    {
        private readonly string _directoryPath;
        private readonly string _Mask;

        public FindFilesCommand(string directoryPath, string Mask)
        {
            _directoryPath = directoryPath;
            _Mask = Mask.Trim();
        }

        public void Execute()
        {
            string[] files;
        if (!string.IsNullOrEmpty(_Mask))
        {
            files = Directory.GetFiles(_directoryPath, _Mask);
        }
        else
        {
            files = Directory.GetFiles(_directoryPath);

        }

            Console.WriteLine($"найден {files.Length} файл:");

            files.Select(file => Path.GetFileName(file))
                .ToList()
                .ForEach(Console.WriteLine);
        }
    }
