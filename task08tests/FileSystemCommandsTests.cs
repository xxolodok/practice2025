using task08;
using FileSystemCommands;
using CommandLib;
using Xunit;
using System.IO;
using System;

public class FileSystemCommandsTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");



        var command = new DirectorySizeCommand(testDir);
        
        using var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        command.Execute();
        var output = consoleOutput.ToString().Trim();

        Assert.Equal($"Размер директории: 10 байтов", output);



        Directory.Delete(testDir, true);
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");



        var command = new FindFilesCommand(testDir, "*.txt");
        
        using var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        command.Execute();
        var output = consoleOutput.ToString().Trim();

        var lines = output.Split(Environment.NewLine);
        Assert.Equal(2, lines.Length);
        Assert.Equal("найден 1 файл:", lines[0]);
        Assert.Equal("file1.txt", lines[1]);



        Directory.Delete(testDir, true);
    }
}

