// Iago Barboza
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Tests.ValueObjects;

public class CpfTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Cpf_Nulo_Vazio_Ou_Espacos_Deve_Falhar(string? input)
    {
        var result = Cpf.Criar(input!);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "CPF_OBRIGATORIO");
    }

    [Theory]
    [InlineData("123")]
    [InlineData("1234567890")]
    [InlineData("123456789012")]
    [InlineData("sem digitos")]
    public void Cpf_Com_Quantidade_Invalida_De_Digitos_Deve_Falhar(string input)
    {
        var result = Cpf.Criar(input);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "CPF_DIGITOS");
    }

    [Theory]
    [InlineData("529.982.247-25", "52998224725")]
    [InlineData("52998224725", "52998224725")]
    [InlineData("123.456.789-01", "12345678901")]
    [InlineData("11111111111", "11111111111")]
    [InlineData(" 123.456.789-01 ", "12345678901")]
    public void Cpf_Com_11_Digitos_Deve_Criar_Na_Versao_Simplificada(string input, string expected)
    {
        var result = Cpf.Criar(input);
        Assert.True(result.IsSuccess);
        Assert.Equal(expected, result.Value!.Valor);
        Assert.Equal(expected, result.Value.ToString());
    }
}
