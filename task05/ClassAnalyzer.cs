using System.Reflection;

namespace task05;

public class ClassAnalyzer
{
    private readonly Type _type;
    public ClassAnalyzer(Type type)
    {
        _type = type;
    }
    public IEnumerable<string> GetPublicMethods()
    {
        return _type
                    .GetMethods()
                    .Where(s => s.IsPublic == true)
                    .Select(x => x.Name)
                    .ToList();
    }
    public IEnumerable<string> GetMethodParams(string methodname)
    {
        var method = _type.GetMethod(methodname);
        return method?
                    .GetParameters()
                    .Select(p => p.Name ?? string.Empty) 
                    .ToList() 
                    ?? Enumerable.Empty<string>();
    }
    public IEnumerable<string> GetAllFields()
    {
        var bindigFlags = BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.Static;

        return _type
                    .GetFields(bindigFlags)
                    .Select(f => f.Name)
                    .ToList();
    }
    public IEnumerable<string> GetProperties()
    {
        return _type.GetProperties().Select(p => p.Name).ToList();
    }
    public bool HasAttribute<T>() where T : Attribute
    {
        return Attribute.IsDefined(_type, typeof(T));
    }
}
