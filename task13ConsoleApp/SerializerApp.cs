using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web; 
using System.IO;
using task13;
using System.Data.SqlTypes;
class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("1. Создаем объект Student...");
        Student student = new Student()
        {
            FirstName = "Шарик",
            LastName = "Кубиков",
            BirthDate = DateTime.Now,
            Grades = new List<Subject>()
        };
        Console.WriteLine($"   Создан студент: {student.FirstName} {student.LastName}, дата рождения: {student.BirthDate:yyyy-MM-dd}");
        
        Console.WriteLine("\n2. Сериализуем объект в JSON...");
        var json = SerializerApp.SerializeInstance(student);
        Console.WriteLine("   Результат сериализации:");
        Console.WriteLine(json);

        Console.WriteLine("\n3. Десериализуем JSON обратно в объект...");
        var deserializeStudent = SerializerApp.DeserializeInstance(json);
        Console.WriteLine($"   Десериализованный студент: {deserializeStudent.FirstName} {deserializeStudent.LastName}, дата рождения: {deserializeStudent.BirthDate:yyyy-MM-dd}");

        var filePath = "Student.json";
        Console.WriteLine($"\n4. Сохраняем объект в файл {filePath}...");
        SerializerApp.SaveToFile(student, filePath);
        Console.WriteLine("   Файл успешно сохранен");

        Console.WriteLine($"\n5. Загружаем объект из файла {filePath}...");
        deserializeStudent = SerializerApp.LoadFromFile(filePath);
        Console.WriteLine($"   Загруженный студент: {deserializeStudent.FirstName} {deserializeStudent.LastName}");
        Console.WriteLine($"   Дата рождения: {deserializeStudent.BirthDate:yyyy-MM-dd}");
        Console.WriteLine($"   Количество оценок: {deserializeStudent.Grades?.Count ?? 0}");

        Console.WriteLine("\nПрограмма завершена.");
    }
}

public static class SerializerApp
{
    private static readonly JsonSerializerOptions Options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new DateFormatConverter() },
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public static string SerializeInstance(dynamic instance)
        => JsonSerializer.Serialize(instance, Options);

    public static Student DeserializeInstance(string json)
        => JsonSerializer.Deserialize<Student>(json, Options)!;
    
    public static void SaveToFile(dynamic instance, string filePath)
    {
        string json = SerializeInstance(instance);
        File.WriteAllText(filePath, json);
    }

    public static Student LoadFromFile(string filePath)
        => DeserializeInstance(File.ReadAllText(filePath));
    
}

public class DateFormatConverter : JsonConverter<DateTime>
{
    private const string Format = "yyyy-MM-dd";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return DateTime.ParseExact(reader.GetString()!, Format, null);
        }
        catch (FormatException ex)
        {
            throw new JsonException("Invalid date format", ex);
        }
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format));
    }
}
