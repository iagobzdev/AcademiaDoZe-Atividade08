// Iago Barboza
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Tests.ValueObjects;

public class CepTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Cep_Nulo_Vazio_Ou_Espacos_Deve_Falhar(string? input)
    {
        var result = Cep.Criar(input!);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "CEP_OBRIGATORIO");
    }

    [Theory]
    [InlineData("123")]
    [InlineData("1234567")]
    [InlineData("123456789")]
    [InlineData("abc")]
    public void Cep_Com_Quantidade_Invalida_De_Digitos_Deve_Falhar(string input)
    {
        var result = Cep.Criar(input);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "CEP_DIGITOS");
    }

    [Theory]
    [InlineData("88500-001", "88500001")]
    [InlineData("88500001", "88500001")]
    [InlineData("88.500-001", "88500001")]
    [InlineData(" 88500-001 ", "88500001")]
    public void Cep_Valido_Deve_Normalizar(string input, string expected)
    {
        var result = Cep.Criar(input);
        Assert.True(result.IsSuccess);
        Assert.Equal(expected, result.Value!.Valor);
        Assert.Equal(expected, result.Value.ToString());
    }
}
