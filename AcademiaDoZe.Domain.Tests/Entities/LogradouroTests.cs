// Iago Barboza
using AcademiaDoZe.Domain.Common;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Exceptions;

namespace AcademiaDoZe.Domain.Tests.Entities;

public class LogradouroTests
{
    [Theory]
    [InlineData("Rua Teste", "Centro", "Lages", "SC", "Brasil")]
    [InlineData(" Av. Brasil ", " Centro ", " Lages ", " s c ", " Brasil ")]
    [InlineData("Rua A", "Bairro B", "Cidade C", "sp", "Brasil")]
    public void Logradouro_Valido_Deve_Criar(string nome, string bairro, string cidade, string estado, string pais)
    {
        var result = Logradouro.Criar(1, "88500-001", nome, bairro, cidade, estado, pais);
        Assert.True(result.IsSuccess);
        Assert.IsAssignableFrom<IAggregateRoot>(result.Value!);
        Assert.Equal(1, result.Value!.Id);
    }

    [Theory]
    [InlineData(" s c ", "SC")]
    [InlineData("sp", "SP")]
    [InlineData(" r j ", "RJ")]
    public void Logradouro_Deve_Normalizar_Estado(string input, string expected)
    {
        var result = Logradouro.Criar(1, "88500-001", "Rua", "Centro", "Cidade", input, "Brasil");
        Assert.True(result.IsSuccess);
        Assert.Equal(expected, result.Value!.Estado);
    }

    [Theory]
    [InlineData("", "NOME_OBRIGATORIO")]
    [InlineData(" ", "NOME_OBRIGATORIO")]
    public void Logradouro_Nome_Vazio_Deve_Falhar(string nome, string mensagem)
    {
        var result = Logradouro.Criar(1, "88500-001", nome, "Centro", "Lages", "SC", "Brasil");
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == mensagem);
    }

    [Theory]
    [InlineData("", "BAIRRO_OBRIGATORIO")]
    [InlineData(" ", "BAIRRO_OBRIGATORIO")]
    public void Logradouro_Bairro_Vazio_Deve_Falhar(string bairro, string mensagem)
    {
        var result = Logradouro.Criar(1, "88500-001", "Rua", bairro, "Lages", "SC", "Brasil");
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == mensagem);
    }

    [Theory]
    [InlineData("", "CIDADE_OBRIGATORIO")]
    [InlineData(" ", "CIDADE_OBRIGATORIO")]
    public void Logradouro_Cidade_Vazia_Deve_Falhar(string cidade, string mensagem)
    {
        var result = Logradouro.Criar(1, "88500-001", "Rua", "Centro", cidade, "SC", "Brasil");
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == mensagem);
    }

    [Theory]
    [InlineData("", "ESTADO_OBRIGATORIO")]
    [InlineData(" ", "ESTADO_OBRIGATORIO")]
    public void Logradouro_Estado_Vazio_Deve_Falhar(string estado, string mensagem)
    {
        var result = Logradouro.Criar(1, "88500-001", "Rua", "Centro", "Lages", estado, "Brasil");
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == mensagem);
    }

    [Theory]
    [InlineData("", "PAIS_OBRIGATORIO")]
    [InlineData(" ", "PAIS_OBRIGATORIO")]
    public void Logradouro_Pais_Vazio_Deve_Falhar(string pais, string mensagem)
    {
        var result = Logradouro.Criar(1, "88500-001", "Rua", "Centro", "Lages", "SC", pais);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == mensagem);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("")]
    public void Logradouro_Cep_Invalido_Deve_Falhar(string cep)
    {
        var result = Logradouro.Criar(1, cep, "Rua", "Centro", "Lages", "SC", "Brasil");
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Logradouro_Deve_Acumular_Erros_De_Campos_Obrigatorios()
    {
        var result = Logradouro.Criar(1, "", "", "", "", "", "");
        Assert.True(result.IsFailure);
        Assert.True(result.Notifications.Count >= 6);
    }

    [Fact]
    public void Logradouro_Id_Negativo_Deve_Lancar_Excecao()
    {
        var ex = Assert.Throws<DomainException>(() => Logradouro.Criar(-1, "88500-001", "Rua", "Centro", "Lages", "SC", "Brasil"));
        Assert.Equal("ID_NEGATIVO", ex.Message);
    }
}
