using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Interfaces;
using AttributesLib;
namespace PluginLoader;
public static class PluginLoader
{
    public static void LoadAndExecutePlugins(string pluginsPath)
    {
        string _pluginsPath = pluginsPath;

        var loadedAssemblies = Directory
            .GetFiles(_pluginsPath, "*.dll")
            .Select(dllPath => new
            {
                Assembly = Assembly.LoadFrom(dllPath),
                Name = Path.GetFileNameWithoutExtension(dllPath)
            })
            .Where(x => x.Name != null)
            .ToDictionary(x => x.Name!, x => x.Assembly);

        var pluginTypes = loadedAssemblies.Values
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.GetCustomAttribute<PluginLoadAttribute>() != null)
            .ToList();

        if (!pluginTypes.Any())
        {
            Console.WriteLine("Не найдено ни одного плагина!");
            return;
        }

        try
        {
            var sortedPlugins = TopologicalSort(pluginTypes);

            Console.WriteLine("\nПорядок выполнения плагинов:");
            sortedPlugins
                .Select(type => new
                {
                    Type = type,
                    Plugin = Activator.CreateInstance(type) as IPlugin
                })
                .ToList()
                .ForEach(x =>
                {
                    if (x.Plugin != null)
                    {
                        Console.WriteLine($"\nВыполнение плагина: {x.Type.Name}");
                        x.Plugin.Execute();
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка: {x.Type.Name} не реализует IPlugin");
                    }
                });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сортировки плагинов: {ex.Message}");
        }
    }

    private static List<Type> TopologicalSort(List<Type> pluginTypes)
    {
        var graph = pluginTypes.ToDictionary(
            type => type,
            type => new List<Type>()
        );

        var inDegree = pluginTypes.ToDictionary(
            type => type,
            type => 0
        );

        pluginTypes
            .Where(type => type.GetCustomAttribute<PluginLoadAttribute>() != null)
            .ToList()
            .ForEach(type =>
            {
                type.GetCustomAttribute<PluginLoadAttribute>()?
                    .Dependencies
                    .Where(graph.ContainsKey)
                    .ToList()
                    .ForEach(dep =>
                    {
                        graph[dep].Add(type);
                        inDegree[type]++;
                    });
            });

        List<Type> ProcessQueue(Queue<Type> q, List<Type> acc)
        {
            if (!q.Any()) return acc;

            var current = q.Dequeue();
            var newAcc = acc.Append(current).ToList();

            graph[current].ForEach(neighbor =>
            {
                inDegree[neighbor]--;
                if (inDegree[neighbor] == 0)
                {
                    q.Enqueue(neighbor);
                }
            });

            return ProcessQueue(q, newAcc);
        }

        var initialQueue = new Queue<Type>(pluginTypes.Where(t => inDegree.GetValueOrDefault(t, -1) == 0));
        var result = ProcessQueue(initialQueue, new List<Type>());

        if (result.Count != pluginTypes.Count)
        {
            throw new Exception("Обнаружена циклическая зависимость между плагинами!");
        }

        return result;
    }
}
