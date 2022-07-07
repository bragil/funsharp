# FunSharp

FunSharp oferece uma abordagem funcional para o tratamento de erros e valores nulos em .Net, através de *RailWay Oriented Programming*. 

# Índice

[1. Motivação](#motivação)\
[2. Instalação](#instalação)

# Motivação

Primeiro, vamos ver como fazemos **sem o FunSharp**, e como ele pode nos ajudar.

Segue abaixo o exemplo de um método muito comum em qualquer projeto, a obtenção de dados de um repositório:

```csharp

public class PessoaService
{
    // Demais implementações da classe ...


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
}
```
Consumindo o método acima, não há como saber se o resultado é o esperado ou não, ou se houve algum erro. Se o desenvolvedor não verificar o valor retornado, podem ocorrer erros como `NullReferenceException`.

```csharp
var pessoa = pessoaService.ObterPessoa(id);
```
No caso acima, podem ocorrer 2 situações:
- O objeto Pessoa é retornado com os dados da pessoa;
- O objeto Pessoa é retornado com o valor NULL.

E caso o valor seja **NULL**, pode ter sido por não existir no banco de dados **OU** por ter ocorrido uma `Exception`.

**FunSharp** oferece uma interface fluída para que possamos tratar estas questões de uma forma muito simples, usando o *pattern* **Monad**. Reescrevendo o método acima, basta você envolver o tipo de retorno em um tipo `Res<T>`, no caso, `Res<Pessoa>`, ou seja, o resultado da obtenção do objeto `Pessoa`:

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
```
Se ocorrer uma Exception, basta retornar um objeto `Error`, ele será convertido para o tipo `Res<Pessoa>`.

Ao consumir o método acima, você pode ter uma lógica para cada situação através de *pattern matching*:
- Retorno de valor (some);
- Não retorno de valor (none);
- Erro (error);

O código abaixo mostra como consumir o método em um Action de um Controller **Asp.Net Core Web API**:

```csharp
public IActionResult Get(int id)
{
    return pessoaService.ObterPessoa(id)
              .Match<IActionResult>(
                  some: pessoa => Ok(pessoa),
                  none: _ =>      NotFound(),
                  error: err =>   BadRequest(err.Message)
              );

}
```
Veja que o método `Match` retornou o objeto `IActionResult` mais adequado para cada situação, sem o uso de `if .. else`, sem o uso de `switch`, de forma bastante simplificada e elegante. 

Este é apenas um dos inúmeros recursos de **FunSharp**.

# Instalação

## Visual Studio

No **Package Manager Console** (Visual Studio) digite `Install-Package FunSharp` e tecle Enter.

Ou clique com o botão direito do mouse sobre o projeto onde será instalado o **FunSharp** (ou sobre a Solution), depois clique na opção **Manage NuGet Packages...**. Na aba **Browse**, digite no campo de pesquisa **FunSharp** e tecle Enter. Na listagem, clique sobre o **FunSharp** e no painel lateral direito clique no botão **Install**.

## CLI

Dentro da pasta do projeto onde o **FunSharp** será instalado, digite `dotnet add package FunSharp`.
