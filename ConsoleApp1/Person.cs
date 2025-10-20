namespace ConsoleApp1;

public class Person(string name, string firstName, int age, string? email)
{
  public string Name { get; } = name;
  public string FirstName { get; } = firstName;
  public int Age { get; } = age;
  public string? Email { get; } = email;

  public override string ToString()
  {
    return $"Name: {Name}, FirstName: {FirstName}, Age: {Age}, Email: {Email ?? "N/A"}";
  }


  public static implicit operator int(Person data)
  {
    return data.Age;
  }

  public static implicit operator string(Person data)
  {
    return data.Name + " " + data.FirstName;
  }

  public string Category()
  {
    return Age switch
    {
      < 13 => "Child",
      < 20 => "Teenager",
      < 65 => "Adult",
      _ => "Senior"
    };
  }
}