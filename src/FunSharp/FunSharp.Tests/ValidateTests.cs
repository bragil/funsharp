using FunSharp.Validation;

namespace FunSharp.Tests;

[TestFixture]
public class ValidateTests
{

    [Test]
    public void Should_validate_the_object()
    {
        var some = new SomeClass()
        {
            Name = "Rogério",
            BirthDate = new DateTime(2025, 1, 27),
            Age = 46
        };

        var n = nameof(some.Name);

        var val = new Validate()
                           .For((nameof(some.Name), some.Name), 
                                x => !string.IsNullOrWhiteSpace(x), "Name is required.")
                           .For((nameof(some.BirthDate), some.BirthDate), 
                                x => x < DateTime.Now, "Birth Date must be in the past.")
                           .For((nameof(some.Age), some.Age), 
                                x => x < 30, "Age must be lower then 30.")
                           .Run();

        Assert.That(val.IsValid, Is.False);
        Assert.That(val.Errors.Count, Is.EqualTo(2));
    }
}

public class SomeClass
{
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public int Age { get; set; }
}
