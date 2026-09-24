// Iago Barboza
using AcademiaDoZe.Domain.Services;

namespace AcademiaDoZe.Domain.Tests.Services;

public class NormalizacaoServiceTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData(" ", true)]
    [InlineData("   ", true)]
    [InlineData("texto", false)]
    [InlineData(" texto ", false)]
    public void TextoVazioOuNulo_Deve_Retornar_Esperado(string? input, bool expected)
    {
        Assert.Equal(expected, NormalizacaoService.TextoVazioOuNulo(input));
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData(" ", "")]
    [InlineData(" a b c ", "a b c")]
    [InlineData("a   b    c", "a b c")]
    [InlineData("a\tb\nc", "a b c")]
    [InlineData("  João   da   Silva  ", "João da Silva")]
    public void LimparEspacos_Deve_Normalizar(string? input, string expected)
    {
        Assert.Equal(expected, NormalizacaoService.LimparEspacos(input));
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData(" ", "")]
    [InlineData("a b c", "abc")]
    [InlineData(" s p ", "sp")]
    [InlineData("1 2 3", "123")]
    public void LimparTodosEspacos_Deve_Remover_Espacos(string? input, string expected)
    {
        Assert.Equal(expected, NormalizacaoService.LimparTodosEspacos(input));
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("abc", "ABC")]
    [InlineData("sp", "SP")]
    [InlineData("áéíõç", "ÁÉÍÕÇ")]
    [InlineData("Teste 123", "TESTE 123")]
    public void ParaMaiusculo_Deve_Converter(string? input, string expected)
    {
        Assert.Equal(expected, NormalizacaoService.ParaMaiusculo(input));
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("a1b2c3", "123")]
    [InlineData("(49) 99999-9999", "49999999999")]
    [InlineData("88500-001", "88500001")]
    [InlineData("sem digitos", "")]
    [InlineData("1 2 3 4 5", "12345")]
    public void LimparEDigitos_Deve_Manter_Somente_Digitos(string? input, string expected)
    {
        Assert.Equal(expected, NormalizacaoService.LimparEDigitos(input));
    }
}
