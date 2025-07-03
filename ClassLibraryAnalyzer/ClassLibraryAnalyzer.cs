using System;
using System.IO;
using System.Reflection;

namespace AssemblyMetadataViewer
{
    class Program
    {
        static void Main(string[] args)
        {
            string assemblyPath = args[0];

            PrintAssemblyMetadata(assemblyPath);
        }

        static void PrintAssemblyMetadata(string assemblyPath)
        {
            var assembly = Assembly.LoadFrom(assemblyPath);

            Console.WriteLine($"Сборка: {assembly.FullName}");

            Console.WriteLine("====================================");

            assembly
                    .GetTypes()
                    .ToList()
                    .ForEach(
                        PrintTypeMetadata
                    );

        }

        static void PrintTypeMetadata(Type type)
        {

            Console.WriteLine($"\nКласс: {type.FullName}");

            var attributes = type.GetCustomAttributesData();

            if (attributes.Count > 0)
            {
                Console.WriteLine("  Атрибуты:");
                attributes
                        .ToList()
                        .ForEach(
                            attr
                            => Console.WriteLine($"    {attr.AttributeType.Name}")
                        );
            }

            var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);

            if (constructors.Length > 0)
            {
                Console.WriteLine("  Конструкторы:");

                constructors
                            .ToList()
                            .ForEach(
                                ctor
                                =>
                                {
                                    Console.Write($"    {type.Name}(");
                                    var parameters = ctor.GetParameters();
                                    parameters
                                            .ToList()
                                            .ForEach(
                                                param
                                                => Console.Write($"{param.ParameterType.Name} {param.Name}, ")
                                                );
                                    Console.WriteLine(")");
                                }
                            );
            }

            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
            if (methods.Length > 0)
            {
                Console.WriteLine("  Методы:");

                methods
                        .ToList()
                        .ForEach(
                            method
                            =>
                            {
                                Console.Write($"    {method.ReturnType.Name} {method.Name}(");
                                var parameters = method.GetParameters();

                                parameters
                                        .ToList()
                                        .ForEach(
                                            param
                                            => Console.Write($"{param.ParameterType.Name} {param.Name}, ")
                                        );
                                Console.WriteLine(")");
                            }
                        );
            }
        }
    }
}