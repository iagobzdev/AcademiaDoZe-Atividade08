// Iago Barboza
using AcademiaDoZe.Domain.Common;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Exceptions;

namespace AcademiaDoZe.Domain.Tests.Entities;

public class AcessoAlunoTests
{
    [Theory]
    [InlineData(6, 0)]
    [InlineData(7, 30)]
    [InlineData(12, 0)]
    [InlineData(18, 45)]
    [InlineData(22, 0)]
    public void Horario_Entre_06_E_22_Deve_Criar(int hora, int minuto)
    {
        var aluno = TestData.AlunoValido(id: 7);
        var dataHora = DateTime.Today.AddHours(hora).AddMinutes(minuto);
        var result = AcessoAluno.Criar(1, aluno, dataHora);
        Assert.True(result.IsSuccess);
        Assert.Equal(7, result.Value!.AlunoId);
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
        var result = AcessoAluno.Criar(1, TestData.AlunoValido(), DateTime.Today.AddHours(hora).AddMinutes(minuto));
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "DATA_HORA_INTERVALO_INVALIDO");
    }

    [Fact]
    public void Aluno_Nulo_Deve_Falhar()
    {
        var result = AcessoAluno.Criar(1, null!, DateTime.Today.AddHours(10));
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "ALUNO_INVALIDO");
    }

    [Fact]
    public void AcessoAluno_Id_Negativo_Deve_Lancar_Excecao()
    {
        Assert.Throws<DomainException>(() => AcessoAluno.Criar(-1, TestData.AlunoValido(), DateTime.Today.AddHours(10)));
    }
}
