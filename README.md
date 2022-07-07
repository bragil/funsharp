# FunSharp

FunSharp oferece uma abordagem funcional para o tratamento de erros em .Net, através de *RailWay Oriented Programming*. 

Ao invés de:

```csharp
public Pessoa ObterPessoa(int id)
{
    try
    {
        return repository.ObterPessoa(id);
    }
    catch(Exception ex)
    {
       return null;
    }
}

// Consumindo o método acima, não há como saber se o resultado é o esperado ou não ou se houve algum erro. 
// Se o desenvolvedor não verificar o valor retornado, podem ocorrer erros como NullReferenceException.
var pessoa = ObterPessoa(id);
```

Com FunSharp, basta você envolver o tipo de retorno em um tipo `Res<T>`. No caso abaixo, `Res<Pessoa>`, ou seja, o resultado da obtenção do objeto `Pessoa`:

```csharp
public Res<Pessoa> ObterPessoa(int id)
{
    try
    {
        return repository.ObterPessoa(id);
    }
    catch(Exception ex)
    {
        return new Error(ex, "Erro ao obter os dados da pessoa.");
    }
}

// Ao consumir o método acima, você pode ter uma lógica para cada situação através de pattern matching:
//   - retorno de valor (some);
//   - não retorno de valor (none);
//   - erro (error);

// O código abaixo mostra como consumir o método em um Action de um Controller Asp.Net Core Web API:

public IActionResult Get(int id)
{
    return ObterPessoa(id)
      .Match(
          some: pessoa => Ok(pessoa),
          none: _ => NotFound(),
          error: err => BadRequest(err.Message)
      );

}
```
