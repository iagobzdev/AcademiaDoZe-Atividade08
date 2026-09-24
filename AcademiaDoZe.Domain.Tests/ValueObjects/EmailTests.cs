// Iago Barboza
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Tests.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("abc")]
    [InlineData("@dominio.com")]
    [InlineData("usuario@")]
    [InlineData("usuario@dominio")]
    [InlineData("usuario@.com")]
    [InlineData("usuario@dominio.")]
    [InlineData("usuario@dominio..com")]
    [InlineData("usuario@@dominio.com")]
    public void Email_Formato_Invalido_Deve_Falhar(string? input)
    {
        var result = Email.Criar(input!);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "EMAIL_FORMATO");
    }

    [Theory]
    [InlineData("usuario@dominio.com", "usuario@dominio.com")]
    [InlineData(" teste@exemplo.com ", "teste@exemplo.com")]
    [InlineData("nome.sobrenome@empresa.com.br", "nome.sobrenome@empresa.com.br")]
    [InlineData("A@B.COM", "A@B.COM")]
    public void Email_Valido_Deve_Criar_E_Normalizar_Espacos(string input, string expected)
    {
        var result = Email.Criar(input);
        Assert.True(result.IsSuccess);
        Assert.Equal(expected, result.Value!.Valor);
        Assert.Equal(expected, result.Value.ToString());
    }
}
