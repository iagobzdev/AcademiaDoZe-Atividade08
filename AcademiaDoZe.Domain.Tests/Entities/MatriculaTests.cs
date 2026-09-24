// Iago Barboza
using AcademiaDoZe.Domain.Common;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Tests.Entities;

public class MatriculaTests
{
    private static Result<Matricula> Criar(
        Aluno? aluno = null,
        MatriculaPlano plano = MatriculaPlano.Mensal,
        DateOnly? inicio = null,
        string objetivo = "Melhorar condicionamento",
        MatriculaRestricoes restricoes = MatriculaRestricoes.None,
        Arquivo? laudo = null,
        string observacoes = "",
        int id = 1)
    {
        return Matricula.Criar(
            id,
            aluno ?? TestData.AlunoValido(),
            plano,
            inicio ?? DateOnly.FromDateTime(DateTime.Today),
            objetivo,
            restricoes,
            laudo,
            observacoes);
    }

    [Theory]
    [InlineData(MatriculaPlano.Mensal, 1)]
    [InlineData(MatriculaPlano.Trimestral, 3)]
    [InlineData(MatriculaPlano.Semestral, 6)]
    [InlineData(MatriculaPlano.Anual, 12)]
    public void Plano_Deve_Calcular_DataFim(MatriculaPlano plano, int meses)
    {
        var inicio = new DateOnly(2026, 1, 15);
        var result = Criar(plano: plano, inicio: inicio);
        Assert.True(result.IsSuccess);
        Assert.Equal(inicio.AddMonths(meses), result.Value!.DataFim);
        Assert.IsAssignableFrom<IAggregateRoot>(result.Value);
    }

    [Theory]
    [InlineData(999)]
    [InlineData(-1)]
    [InlineData(100)]
    public void Plano_Invalido_Deve_Falhar(int plano)
    {
        var result = Criar(plano: (MatriculaPlano)plano);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "PLANO_INVALIDO");
    }

    [Fact]
    public void DataInicio_Default_Deve_Falhar()
    {
        var result = Matricula.Criar(1, TestData.AlunoValido(), MatriculaPlano.Mensal, default, "Objetivo", MatriculaRestricoes.None, null);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "DATA_INICIO_OBRIGATORIO");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Objetivo_Obrigatorio_Deve_Falhar(string objetivo)
    {
        var result = Criar(objetivo: objetivo);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "OBJETIVO_OBRIGATORIO");
    }

    [Theory]
    [InlineData("  Ganhar   massa  ", "Ganhar massa")]
    [InlineData("Perder peso", "Perder peso")]
    public void Objetivo_Deve_Ser_Normalizado(string input, string expected)
    {
        var result = Criar(objetivo: input);
        Assert.True(result.IsSuccess);
        Assert.Equal(expected, result.Value!.Objetivo);
    }

    [Theory]
    [InlineData(MatriculaRestricoes.Diabetes)]
    [InlineData(MatriculaRestricoes.PressaoAlta)]
    [InlineData(MatriculaRestricoes.Alergias)]
    [InlineData(MatriculaRestricoes.ProblemasRespiratorios)]
    [InlineData(MatriculaRestricoes.RemedioContinuo)]
    public void Restricao_Sem_Laudo_Deve_Falhar(MatriculaRestricoes restricao)
    {
        var result = Criar(restricoes: restricao, laudo: null);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "RESTRICOES_LAUDO_OBRIGATORIO");
    }

    [Theory]
    [InlineData(MatriculaRestricoes.Diabetes)]
    [InlineData(MatriculaRestricoes.Diabetes | MatriculaRestricoes.Alergias)]
    [InlineData(MatriculaRestricoes.PressaoAlta | MatriculaRestricoes.RemedioContinuo)]
    public void Restricao_Com_Laudo_Deve_Criar(MatriculaRestricoes restricao)
    {
        var result = Criar(restricoes: restricao, laudo: TestData.ArquivoValido(), observacoes: "Observação");
        Assert.True(result.IsSuccess);
        Assert.Equal(restricao, result.Value!.RestricoesMedicas);
        Assert.NotNull(result.Value.LaudoMedico);
    }

    [Theory]
    [InlineData(15)]
    [InlineData(14)]
    [InlineData(13)]
    [InlineData(12)]
    public void Menor_De_16_Sem_Laudo_Deve_Falhar(int idade)
    {
        var aluno = TestData.AlunoValido(idade: idade);
        var result = Criar(aluno: aluno);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "MENOR_16_LAUDO_OBRIGATORIO");
    }

    [Theory]
    [InlineData(16)]
    [InlineData(17)]
    [InlineData(20)]
    [InlineData(40)]
    public void Aluno_Com_16_Ou_Mais_Sem_Restricao_Nao_Exige_Laudo(int idade)
    {
        var aluno = TestData.AlunoValido(idade: idade);
        var result = Criar(aluno: aluno);
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData("  usa   medicamento  ", "usa medicamento")]
    [InlineData("obs", "obs")]
    [InlineData("", "")]
    public void Observacoes_Deve_Ser_Normalizada(string input, string expected)
    {
        var result = Criar(restricoes: MatriculaRestricoes.Diabetes, laudo: TestData.ArquivoValido(), observacoes: input);
        Assert.True(result.IsSuccess);
        Assert.Equal(expected, result.Value!.ObservacoesRestricoes);
    }

    [Fact]
    public void Matricula_Deve_Referenciar_Aluno_Apenas_Por_Id()
    {
        var aluno = TestData.AlunoValido(id: 25);
        var result = Criar(aluno: aluno);
        Assert.True(result.IsSuccess);
        Assert.Equal(25, result.Value!.AlunoId);
        Assert.Null(typeof(Matricula).GetProperty("Aluno"));
    }

    [Fact]
    public void Aluno_Nulo_Deve_Falhar()
    {
        var result = Matricula.Criar(1, null!, MatriculaPlano.Mensal, DateOnly.FromDateTime(DateTime.Today), "Objetivo", MatriculaRestricoes.None, null);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "ALUNO_INVALIDO");
    }

    [Fact]
    public void Matricula_Id_Negativo_Deve_Lancar_Excecao()
    {
        Assert.Throws<DomainException>(() => Criar(id: -1));
    }
}
