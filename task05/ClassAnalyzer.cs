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
        return _type.GetMethods().Where(s => s.IsPublic == true).Select(x => x.Name).AsEnumerable();
    }

    public IEnumerable<string> GetMethodParams(string methodname)
    {
        return _type.GetMethod(methodname).GetParameters().Select(p => p.Name).AsEnumerable();
    }
    public IEnumerable<string> GetAllFields()
    {
        var bindigFlags = BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.Static;

        return _type
                    .GetFields(bindigFlags)
                    .Select(f => f.Name)
                    .AsEnumerable();
    }
    // Список имен свойств
    public IEnumerable<string> GetProperties()
    {
        return _type.GetProperties().Select(p => p.Name).AsEnumerable();
    }
    // // Наличие атрибута указанного типа у класса
    public bool HasAttribute<T>() where T : Attribute
    {
        // Проверяем, есть ли у типа атрибут T
        return Attribute.IsDefined(_type, typeof(T));
    }
}
