using System;
using System.Collections.Generic;

namespace FunSharp.Validation;

public struct Validate
{
    private readonly List<(string Property, string Error)> errors = new();

    public Validate() { }


    public readonly Validate For<TProp>((string Name, TProp Value) property,
                                     Func<TProp, bool> must,
                                     string message)
    {
        //if (property.Name is null) throw new ArgumentNullException(nameof(property));
        //if (must is null) throw new ArgumentNullException(nameof(must));
        //if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullException(nameof(message));

        if (!must(property.Value))
            errors.Add((property.Name, message));

        return this;
    }

    public readonly ValidationResult Run()
        => new ValidationResult(errors.Count == 0, errors);

    ////static string GetPropertyName<TProp>(Expression<Func<TObj, TProp>> expression)
    ////{
    ////    var lambda = expression as LambdaExpression;
    ////    MemberExpression memberExpression;
    ////    if (lambda.Body is UnaryExpression)
    ////    {
    ////        var unaryExpression = lambda.Body as UnaryExpression;
    ////        memberExpression = unaryExpression.Operand as MemberExpression;
    ////    }
    ////    else
    ////        memberExpression = lambda.Body as MemberExpression;

    ////    Debug.Assert(memberExpression != null,
    ////       "Please provide a lambda expression like 'n => n.PropertyName'");

    ////    if (memberExpression != null)
    ////    {
    ////        var propertyInfo = memberExpression.Member as PropertyInfo;

    ////        return propertyInfo.Name;
    ////    }

    ////    return null;
    ////}
}


////public struct PropertyInfo<TObj, TProp>
////{
////    public readonly TObj Object { get; }
////    public readonly TProp Property { get; }
////    public readonly string PropertyName { get; }

////    public PropertyInfo(TObj obj, TProp prop, string propName)
////    {
////        Object = obj;
////        Property = prop;
////        PropertyName = propName;
////    }
////}

////public readonly struct ValidationError
////{
////    public string Property { get; }
////    public string ErrorMessage { get; }

////    public ValidationError(string property, string errorMessage)
////    {
////        Property = property;
////        ErrorMessage = errorMessage;
////    }
////}

public readonly struct ValidationResult
{
    public bool IsValid { get; }
    public List<(string Property, string Error)> Errors { get; }

    public ValidationResult(bool isValid, List<(string Property, string Error)> errors)
    {
        IsValid = isValid;
        Errors = errors;
    }
}