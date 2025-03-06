using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using FluentValidation;
using FunSharp.Validation;


var resultado = BenchmarkRunner.Run<ValidateBechmark>();

[MemoryDiagnoser]
public class ValidateBechmark
{
    private SomeClass some = new SomeClass()
    {
        Name = "Rogério",
            BirthDate = new DateTime(1978, 1, 27),
            Age = 16
        };

    [Benchmark]
    public void ValidarComFluentValidation()
    {
        var validator = new SomeClassValidator();
        var result = validator.Validate(some);
    }

    [Benchmark]
    public void ValidarComValidate()
    {
        var result = new Validate()
                           .For(("Name", some.Name),
                                x => !string.IsNullOrWhiteSpace(x), "Nome é obrigatório.")
                           .For(("Name", some.Name),
                                x => x.Length < 20, "Nome não deve exceder 20 caracteres.")
                           .For(("BirthDate", some.BirthDate),
                                x => x < DateTime.Now, "Data Nascimento deve estar no passado.")
                           .For(("Age", some.Age),
                                x => x >= 18, "Deve ser maior de 18 anos.")
                           .Run();
    }
}

public class SomeClassValidator: AbstractValidator<SomeClass>
{
    public SomeClassValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Nome é obrigatório.");
        RuleFor(x => x.Name).MaximumLength(20).WithMessage("Nome não deve exceder 20 caracteres.");
        RuleFor(x => x.BirthDate).LessThan(DateTime.Now).WithMessage("Data Nascimento deve estar no passado.");
        RuleFor(x => x.Age).LessThan(18).WithMessage("Deve ser maior de 18 anos.");
    }
}

public class SomeClass
{
    public string Name { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public int Age { get; set; }
}