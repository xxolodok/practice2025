using CommandLib;
using System.IO;

namespace FileSystemCommands;

    public class DirectorySizeCommand : ICommand
    {
        private readonly string _directoryPath;

        public DirectorySizeCommand(string directoryPath)
        {
            _directoryPath = directoryPath;
        }

        public void Execute()
        {
            long size = CalculateDirectorySize(_directoryPath);
            Console.WriteLine($"Размер директории: {size} байтов");
        }

        private long CalculateDirectorySize(string path)
        {
            return  Directory.EnumerateFiles(path).Sum(file => new FileInfo(file).Length) +
                    Directory.EnumerateDirectories(path).Sum(dir => CalculateDirectorySize(dir));
        }
    }
    