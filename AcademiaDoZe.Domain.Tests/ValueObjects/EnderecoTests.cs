// Iago Barboza
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Tests.ValueObjects;

public class EnderecoTests
{
    [Theory]
    [InlineData("10", "", "10", "")]
    [InlineData(" 10 A ", " Bloco A ", "10 A", "Bloco A")]
    [InlineData("123", "Apto 4", "123", "Apto 4")]
    public void Endereco_Valido_Deve_Criar_E_Normalizar(string numero, string complemento, string numeroEsperado, string complementoEsperado)
    {
        var logradouro = TestData.LogradouroValido(7);
        var result = Endereco.Criar(logradouro, numero, complemento);

        Assert.True(result.IsSuccess);
        Assert.Equal(7, result.Value!.LogradouroId);
        Assert.Equal(numeroEsperado, result.Value.Numero);
        Assert.Equal(complementoEsperado, result.Value.Complemento);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Endereco_Numero_Obrigatorio_Deve_Falhar(string? numero)
    {
        var result = Endereco.Criar(TestData.LogradouroValido(), numero!, "");
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "NUMERO_OBRIGATORIO");
    }

    [Fact]
    public void Endereco_Logradouro_Nulo_Deve_Falhar()
    {
        var result = Endereco.Criar(null!, "10", "");
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "LOGRADOURO_OBRIGATORIO");
    }

    [Fact]
    public void Endereco_Pode_Acumular_Mais_De_Uma_Notificacao()
    {
        var result = Endereco.Criar(null!, "", "");
        Assert.True(result.IsFailure);
        Assert.Equal(2, result.Notifications.Count);
    }
}
