using task02.Models;

namespace task02;

public class StudentService
{
    private readonly List<Student> _students;

    public StudentService(List<Student> students) => _students = students;

    // 1. Возвращает студентов указанного факультета
    public IEnumerable<Student> GetStudentsByFaculty(string faculty)
    => _students.Where(student => student.Faculty.ToLower() == faculty.ToLower());


    // 2. Возвращает студентов со средним баллом >= minAverageGrade
    public IEnumerable<Student> GetStudentsWithMinAverageGrade(double minAverageGrade)
    => _students.Where(student => student.Grades.Average() >= minAverageGrade);

    // 3. Возвращает студентов, отсортированных по имени (A-Z)
    public IEnumerable<Student> GetStudentsOrderedByName()
        => _students.OrderBy(student => student.Name);

    // 4. Группировка по факультету
    public ILookup<string, Student> GroupStudentsByFaculty()
        => _students.ToLookup(student => student.Faculty);

    // 5. Находит факультет с максимальным средним баллом
    public string GetFacultyWithHighestAverageGrade()
         => _students
             .GroupBy(s => s.Faculty)
             .Select(g => new
             {
                 Faculty = g.Key,
                 AvgGrade = g.SelectMany(s => s.Grades)
                             .Average()
             })
             .OrderBy(x => x.AvgGrade)
             .Last()
             .Faculty;
}