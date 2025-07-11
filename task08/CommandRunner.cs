using CommandLib;
using System.Reflection;
using System.Linq;

namespace task08;

public static class CommandRunner
{
    static void Main()
    {
        bool shouldContinue = true;
        while (shouldContinue)
        {
            Console.WriteLine("Выберите команду");
            Console.WriteLine("1. Вычислить размер директории\n2.Найти файл\n3.Выход");
            var option = int.TryParse(Console.ReadLine(), out int result);
            if (option)
            {
                switch (result)
                {
                    case 1:
                        InvokeDirectorySizeCommand();
                        break;
                    case 2:
                        InvokeFindFilesCommand();
                        break;
                    case 3:
                        shouldContinue = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор");
                        break;
                }
                ;
            }
        }
    }
    private static void InvokeDirectorySizeCommand()
    {
        string testDir = CreateDirectory();

        Assembly assembly = LoadAssembly();

        Type commandType = assembly.GetType("FileSystemCommands.DirectorySizeCommand");
        object commandInstance = Activator.CreateInstance(commandType, testDir);
        MethodInfo executeMethod = commandType.GetMethod("Execute");
        executeMethod.Invoke(commandInstance, null);
    }

    private static void InvokeFindFilesCommand()
    {
        Console.WriteLine("Введите маску для поиска файла или оставьте пустой");
        string? Mask = Console.ReadLine();
        string testDir = CreateDirectory();

        Assembly assembly = LoadAssembly();

        var commandType = assembly.GetType("FileSystemCommands.FindFilesCommand");
        var commandInstance = Activator.CreateInstance(commandType, new[] { testDir, Mask });
        var executeMethod = commandType.GetMethod("Execute");
        executeMethod.Invoke(commandInstance, null);
    }
    private static Assembly LoadAssembly()
    {
        string assemblyPath = "C:/practice2025/FileSystemCommands/bin/Debug/net9.0/FileSystemCommands.dll";
        Assembly assembly = Assembly.LoadFile(assemblyPath);
        return assembly;
    }
    private static string CreateDirectory()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");

        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

        return testDir;

    }
}
