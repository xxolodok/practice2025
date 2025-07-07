using FileSystemCommands;
using CommandRunner;
using CommandLib;
using Xunit;
using System.IO;
using System;
using Moq;

public class FileSystemCommandsTests
{

    private readonly Mock<TextReader> _mockInput;
    private readonly Mock<TextWriter> _mockOutput;
    private readonly StringWriter _outputWriter;

    public FileSystemCommandsTests()
    {
        _mockInput = new Mock<TextReader>();
        _mockOutput = new Mock<TextWriter>();
        _outputWriter = new StringWriter();
        Console.SetOut(_mockOutput.Object);
        Console.SetIn(_mockInput.Object);
    }

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

    [Fact]
    public void Main_ShouldExit_WhenOption3Selected()
    {
        var inputSequence = new Queue<string>();
        inputSequence.Enqueue("3");

        _mockInput.SetupSequence(x => x.ReadLine())
            .Returns(inputSequence.Dequeue());

        CommandRunner.CommandRunner.Main();

        _mockOutput.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("Произошел выход из программы"))));
    }

    [Fact]
    public void Main_ShouldShowError_WhenInvalidOptionSelected()
    {
        var inputSequence = new Queue<string>();
        inputSequence.Enqueue("99");
        inputSequence.Enqueue("3");

        _mockInput.SetupSequence(x => x.ReadLine())
            .Returns(inputSequence.Dequeue())
            .Returns(inputSequence.Dequeue());


        CommandRunner.CommandRunner.Main();

        _mockOutput.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("Неверный выбор"))));
    }

    [Fact]
    public void Main_ShouldShowError_WhenFirstOptiionSelected()
    {
        var inputSequence = new Queue<string>();
        inputSequence.Enqueue("99");
        inputSequence.Enqueue("1");
        inputSequence.Enqueue("3");

        _mockInput.SetupSequence(x => x.ReadLine())
            .Returns(inputSequence.Dequeue())
            .Returns(inputSequence.Dequeue())
            .Returns(inputSequence.Dequeue());


        CommandRunner.CommandRunner.Main();


        _mockOutput.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("Размер директории: 10 байтов"))));
    }


    [Fact]
    public void Main_ShouldInvokeFindFiles_WhenOption2Selected()
    {
        // Arrange
        var inputSequence = new Queue<string>();
        inputSequence.Enqueue("2");
        inputSequence.Enqueue("*.txt"); 
        inputSequence.Enqueue("3");

        _mockInput.SetupSequence(x => x.ReadLine())
            .Returns(inputSequence.Dequeue())
            .Returns(inputSequence.Dequeue())
            .Returns(inputSequence.Dequeue());


        CommandRunner.CommandRunner.Main();

        _mockOutput.Verify(x => x.WriteLine(It.Is<string>(s =>
            s.Contains("Введите маску для поиска файла или оставьте пустой"))));
        _mockOutput.Verify(x => x.WriteLine(It.Is<string>(s =>
            s.Contains("найден") && s.Contains("файл"))));

    }
}

