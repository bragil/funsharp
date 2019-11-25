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

// Consumindo o método acima:
var pessoa = ObterPessoa(id);
if (pessoa == null)
{
   // lógica para null
}
else
{
   // lógica para diferente de null
}
```

Com FunSharp, você faz assim:

```csharp
using static FunSharp.TryFunctions;

public Res<Pessoa> ObterPessoa(int id)
    => Try(() => repository.ObterPessoa(id), "Erro ao obter pessoa.");

// Consumindo o método acima:
ObterPessoa(id)
  .Match(
      some: p => 
      {
         // lógica para quando há retorno de valor.
      },
      error: err =>
      {
         // Tratamento do erro (mensagens, log, etc)
      },
      none: _ =>
      {
         // Lógica para quando não retorna valor (nulo).
      }
  );
```
