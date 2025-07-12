namespace task13tests;

using Xunit;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using task13;

public class SerializerAppTests
{
    private readonly Student _testStudent = new Student
    {
        FirstName = "Тест",
        LastName = "Студент",
        BirthDate = new DateTime(2000, 1, 1),
        Grades = new List<Subject>
        {
            new Subject { Name = "Математика", Grade = 5 },
            new Subject { Name = "Физика", Grade = 4 }
        }
    };

    [Fact]
    public void SerializeInstance_ShouldReturnValidJson()
    {
        // Act
        var json = SerializerApp.SerializeInstance(_testStudent);

        // Assert
        Assert.NotNull(json);
        Assert.Contains("\"FirstName\":\"Тест\"", json);
        Assert.Contains("\"LastName\":\"Студент\"", json);
        Assert.Contains("\"2000-01-01\"", json);
        Assert.Contains("\"Математика\"", json);
    }

    [Fact]
    public void DeserializeInstance_ShouldReturnValidStudent()
    {
        // Arrange
        var json = SerializerApp.SerializeInstance(_testStudent);

        // Act
        var student = SerializerApp.DeserializeInstance(json);

        // Assert
        Assert.Equal(_testStudent.FirstName, student.FirstName);
        Assert.Equal(_testStudent.LastName, student.LastName);
        Assert.Equal(_testStudent.BirthDate, student.BirthDate);
        Assert.Equal(2, student.Grades.Count);
    }

    [Fact]
    public void SerializeDeserialize_ShouldReturnEquivalentObject()
    {
        // Act
        var json = SerializerApp.SerializeInstance(_testStudent);
        var deserialized = SerializerApp.DeserializeInstance(json);

        // Assert
        Assert.Equal(_testStudent.FirstName, deserialized.FirstName);
        Assert.Equal(_testStudent.LastName, deserialized.LastName);
        Assert.Equal(_testStudent.BirthDate, deserialized.BirthDate);
        Assert.Equal(_testStudent.Grades.Count, deserialized.Grades.Count);
    }

    [Fact]
    public void SaveAndLoadFromFile_ShouldWorkCorrectly()
    {
        // Arrange
        var filePath = "test_student.json";

        try
        {
            // Act
            SerializerApp.SaveToFile(_testStudent, filePath);
            var loaded = SerializerApp.LoadFromFile(filePath);

            // Assert
            Assert.Equal(_testStudent.FirstName, loaded.FirstName);
            Assert.Equal(_testStudent.LastName, loaded.LastName);
            Assert.Equal(_testStudent.BirthDate, loaded.BirthDate);
            Assert.Equal(_testStudent.Grades.Count, loaded.Grades.Count);
        }
        finally
        {
            // Cleanup
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [Fact]
    public void DeserializeInstance_ShouldThrowOnInvalidJson()
    {
        // Arrange
        var invalidJson = "{invalid json}";

        // Act & Assert
        Assert.Throws<JsonException>(() => SerializerApp.DeserializeInstance(invalidJson));
    }

    [Fact]
    public void SerializeInstance_ShouldIgnoreNullValues()
    {
        // Arrange
        var studentWithNulls = new Student
        {
            FirstName = "Null",
            LastName = "Student",
            BirthDate = DateTime.Now,
            Grades = null
        };

        // Act
        var json = SerializerApp.SerializeInstance(studentWithNulls);

        // Assert
        Assert.DoesNotContain("Grades", json);
    }

    [Fact]
    public void DateFormatConverter_ShouldFormatDateCorrectly()
    {
        // Arrange
        var testDate = new DateTime(2023, 12, 31);
        var studentWithDate = new Student
        {
            FirstName = "Date",
            LastName = "Test",
            BirthDate = testDate
        };

        // Act
        var json = SerializerApp.SerializeInstance(studentWithDate);

        // Assert
        Assert.Contains("\"2023-12-31\"", json);
    }
}
