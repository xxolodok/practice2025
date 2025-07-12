namespace task13;

public class Student
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public DateTime BirthDate { get; set; }
    
    public List<Subject> Grades { get; set; }

    public Student() { }

    public Student(string firstName, string lastName, DateTime birthDate, List<Subject> grades = null)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
        Grades = grades;
    }
}
