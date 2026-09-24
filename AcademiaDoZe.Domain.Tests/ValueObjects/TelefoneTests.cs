// Iago Barboza
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Tests.ValueObjects;

public class TelefoneTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Telefone_Nulo_Vazio_Ou_Espacos_Deve_Falhar(string? input)
    {
        var result = Telefone.Criar(input!);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "TELEFONE_OBRIGATORIO");
    }

    [Theory]
    [InlineData("1234")]
    [InlineData("4999999999")]
    [InlineData("499999999999")]
    [InlineData("sem digitos")]
    public void Telefone_Com_Quantidade_Invalida_De_Digitos_Deve_Falhar(string input)
    {
        var result = Telefone.Criar(input);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "TELEFONE_DIGITOS");
    }

    [Theory]
    [InlineData("(49) 99999-9999", "49999999999")]
    [InlineData("49999999999", "49999999999")]
    [InlineData("49 99999 9999", "49999999999")]
    [InlineData("+55 (49) 99999-9999", "5549999999999")]
    public void Telefone_Deve_Normalizar_E_Validar_Quantidade(string input, string expected)
    {
        var result = Telefone.Criar(input);
        if (expected.Length == 11)
        {
            Assert.True(result.IsSuccess);
            Assert.Equal(expected, result.Value!.Valor);
            Assert.Equal(expected, result.Value.ToString());
        }
        else
        {
            Assert.True(result.IsFailure);
            Assert.Contains(result.Notifications, n => n.Mensagem == "TELEFONE_DIGITOS");
        }
    }
}
