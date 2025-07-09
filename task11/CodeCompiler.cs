namespace task11;

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Reflection;
using System.IO;

static public class CodeCompiler
{
    static public dynamic CodeGenerate(string code)
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
        MetadataReference[] references =
        [
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location)
        ];

        CSharpCompilation compilation = CSharpCompilation.Create(
            "GeneratedAssembly",
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var ms = new MemoryStream();

        Assembly assembly = Assembly.Load(ms.ToArray());

        Type calculatorType = assembly.GetType("Calculator")!;

        return Activator.CreateInstance(calculatorType)!;
    }
}
