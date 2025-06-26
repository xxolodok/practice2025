namespace task02.Models;

public class Student
{
    
    public required string Name { get; set; }       // Добаввил модификатор required, чтобы избежать возможных ошибок(из-за предупреждения CS8618)
    public required string Faculty { get; set; }    // Добаввил модификатор required, чтобы избежать возможных ошибок(из-за предупреждения CS8618)
    public required List<int> Grades { get; set; }  // Добаввил модификатор required, чтобы избежать возможных ошибок(из-за предупреждения CS8618)
    
}
