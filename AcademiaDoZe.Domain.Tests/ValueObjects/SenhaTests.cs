// Iago Barboza
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Tests.ValueObjects;

public class SenhaTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Senha_Nula_Vazia_Ou_Espacos_Deve_Falhar(string? input)
    {
        var result = Senha.Criar(input!);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "SENHA_OBRIGATORIO");
    }

    [Theory]
    [InlineData("abcde")]
    [InlineData("abcdef")]
    [InlineData("123456")]
    [InlineData("abc123")]
    public void Senha_Sem_Requisitos_Deve_Falhar(string input)
    {
        var result = Senha.Criar(input);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "SENHA_FORMATO");
    }

    [Theory]
    [InlineData("Abcdef", "Abcdef")]
    [InlineData("SenhaA", "SenhaA")]
    [InlineData("  SenhaA  ", "SenhaA")]
    [InlineData("A12345", "A12345")]
    [InlineData("Ab cde", "Ab cde")]
    public void Senha_Valida_Deve_Criar(string input, string expected)
    {
        var result = Senha.Criar(input);
        Assert.True(result.IsSuccess);
        Assert.Equal(expected, result.Value!.Valor);
        Assert.Equal(expected, result.Value.ToString());
    }
}
