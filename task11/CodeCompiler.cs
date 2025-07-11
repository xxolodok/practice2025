namespace task11;

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Reflection;
using System.IO;
using Interfaces;

static public class CodeCompiler
{
    static public ICalculator CodeGenerate(string code)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(code);
        
        var references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ICalculator).Assembly.Location)
        };

        var compilation = CSharpCompilation.Create(
            "GeneratedAssembly",
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using var ms = new MemoryStream();
        var result = compilation.Emit(ms);

        if (!result.Success)
        {
            throw new InvalidOperationException("Compilation failed");
        }

        ms.Seek(0, SeekOrigin.Begin);
        var assembly = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromStream(ms);
        var calculatorType = assembly.GetType("Calculator");
        return (ICalculator)Activator.CreateInstance(calculatorType);
    }
}
