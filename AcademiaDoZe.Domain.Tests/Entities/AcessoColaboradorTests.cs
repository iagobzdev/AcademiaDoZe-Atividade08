// Iago Barboza
using AcademiaDoZe.Domain.Common;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Exceptions;

namespace AcademiaDoZe.Domain.Tests.Entities;

public class AcessoColaboradorTests
{
    [Theory]
    [InlineData(6, 0)]
    [InlineData(8, 0)]
    [InlineData(12, 30)]
    [InlineData(17, 45)]
    [InlineData(22, 0)]
    public void Horario_Entre_06_E_22_Deve_Criar(int hora, int minuto)
    {
        var colaborador = TestData.ColaboradorValido(id: 9);
        var dataHora = DateTime.Today.AddHours(hora).AddMinutes(minuto);
        var result = AcessoColaborador.Criar(1, colaborador, dataHora);
        Assert.True(result.IsSuccess);
        Assert.Equal(9, result.Value!.ColaboradorId);
        Assert.Equal(dataHora, result.Value.DataHora);
        Assert.IsAssignableFrom<IAggregateRoot>(result.Value);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(5, 59)]
    [InlineData(22, 1)]
    [InlineData(23, 59)]
    public void Horario_Fora_Do_Intervalo_Deve_Falhar(int hora, int minuto)
    {
        var result = AcessoColaborador.Criar(1, TestData.ColaboradorValido(), DateTime.Today.AddHours(hora).AddMinutes(minuto));
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "DATA_HORA_INTERVALO_INVALIDO");
    }

    [Fact]
    public void Colaborador_Nulo_Deve_Falhar()
    {
        var result = AcessoColaborador.Criar(1, null!, DateTime.Today.AddHours(10));
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "COLABORADOR_INVALIDO");
    }

    [Fact]
    public void AcessoColaborador_Id_Negativo_Deve_Lancar_Excecao()
    {
        Assert.Throws<DomainException>(() => AcessoColaborador.Criar(-1, TestData.ColaboradorValido(), DateTime.Today.AddHours(10)));
    }
}
