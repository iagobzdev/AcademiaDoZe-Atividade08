// Iago Barboza
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Tests.ValueObjects;

public class ArquivoTests
{
    [Fact]
    public void Arquivo_Nulo_Deve_Falhar()
    {
        var result = Arquivo.Criar(null!);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "ARQUIVO_OBRIGATORIO");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(1024)]
    public void Arquivo_Ate_15Mb_Deve_Criar(int tamanho)
    {
        var conteudo = new byte[tamanho];
        var result = Arquivo.Criar(conteudo);
        Assert.True(result.IsSuccess);
        Assert.Equal(tamanho, result.Value!.Conteudo.Length);
    }

    [Fact]
    public void Arquivo_Exatamente_15Mb_Deve_Criar()
    {
        var result = Arquivo.Criar(new byte[15 * 1024 * 1024]);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Arquivo_Maior_Que_15Mb_Deve_Falhar()
    {
        var result = Arquivo.Criar(new byte[(15 * 1024 * 1024) + 1]);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "ARQUIVO_TIPO_TAMANHO");
    }
}
