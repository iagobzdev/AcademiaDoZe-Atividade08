// Iago Barboza
using AcademiaDoZe.Domain.Common;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Exceptions;

namespace AcademiaDoZe.Domain.Tests.Entities;

public class ColaboradorTests
{
    private static Result<Colaborador> Criar(
        int id = 1,
        string nome = "Maria da Silva",
        int idade = 30,
        int diasAdmissao = -30,
        ColaboradorTipo tipo = ColaboradorTipo.Atendente,
        ColaboradorVinculo vinculo = ColaboradorVinculo.CLT,
        string cpf = "12345678901",
        string telefone = "49988888888",
        string email = "colaborador@teste.com",
        string senha = "SenhaB")
    {
        return Colaborador.Criar(
            id,
            nome,
            cpf,
            DateOnly.FromDateTime(DateTime.Today.AddYears(-idade)),
            telefone,
            email,
            TestData.LogradouroValido(),
            "321",
            "",
            senha,
            TestData.ArquivoValido(),
            DateOnly.FromDateTime(DateTime.Today.AddDays(diasAdmissao)),
            tipo,
            vinculo);
    }

    [Theory]
    [InlineData(ColaboradorTipo.Atendente, ColaboradorVinculo.CLT)]
    [InlineData(ColaboradorTipo.Atendente, ColaboradorVinculo.Estagio)]
    [InlineData(ColaboradorTipo.Instrutor, ColaboradorVinculo.CLT)]
    [InlineData(ColaboradorTipo.Instrutor, ColaboradorVinculo.Estagio)]
    [InlineData(ColaboradorTipo.Administrador, ColaboradorVinculo.CLT)]
    public void Colaborador_Com_Combinacao_Valida_Deve_Criar(ColaboradorTipo tipo, ColaboradorVinculo vinculo)
    {
        var result = Criar(tipo: tipo, vinculo: vinculo);
        Assert.True(result.IsSuccess);
        Assert.IsAssignableFrom<IAggregateRoot>(result.Value!);
    }

    [Fact]
    public void Administrador_Com_Estagio_Deve_Falhar()
    {
        var result = Criar(tipo: ColaboradorTipo.Administrador, vinculo: ColaboradorVinculo.Estagio);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "ADMINISTRADOR_CLT_INVALIDO");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-365)]
    public void Data_Admissao_Hoje_Ou_Passado_Deve_Criar(int dias)
    {
        var result = Criar(diasAdmissao: dias);
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(30)]
    [InlineData(365)]
    public void Data_Admissao_Futura_Deve_Falhar(int dias)
    {
        var result = Criar(diasAdmissao: dias);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "DATA_ADMISSAO_MAIOR_QUE_ATUAL");
    }

    [Fact]
    public void Data_Admissao_Default_Deve_Falhar()
    {
        var result = Colaborador.Criar(1, "Maria", "12345678901", DateOnly.FromDateTime(DateTime.Today.AddYears(-30)), "49988888888", "a@b.com", TestData.LogradouroValido(), "1", "", "SenhaB", TestData.ArquivoValido(), default, ColaboradorTipo.Atendente, ColaboradorVinculo.CLT);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "DATA_ADMISSAO_OBRIGATORIO");
    }

    [Theory]
    [InlineData(11)]
    [InlineData(10)]
    public void Colaborador_Com_Menos_De_12_Anos_Deve_Falhar(int idade)
    {
        var result = Criar(idade: idade);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "DATA_NASCIMENTO_MINIMA_INVALIDA");
    }

    [Theory]
    [InlineData(999, 0)]
    [InlineData(-1, 0)]
    public void Tipo_Invalido_Deve_Falhar(int tipo, int vinculo)
    {
        var result = Criar(tipo: (ColaboradorTipo)tipo, vinculo: (ColaboradorVinculo)vinculo);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "TIPO_COLABORADOR_INVALIDO");
    }

    [Theory]
    [InlineData(1, 999)]
    [InlineData(2, -1)]
    public void Vinculo_Invalido_Deve_Falhar(int tipo, int vinculo)
    {
        var result = Criar(tipo: (ColaboradorTipo)tipo, vinculo: (ColaboradorVinculo)vinculo);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "VINCULO_COLABORADOR_INVALIDO");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Nome_Obrigatorio_Deve_Falhar(string nome)
    {
        var result = Criar(nome: nome);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "NOME_OBRIGATORIO");
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    public void Cpf_Invalido_Deve_Falhar(string cpf)
    {
        Assert.True(Criar(cpf: cpf).IsFailure);
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    public void Telefone_Invalido_Deve_Falhar(string telefone)
    {
        Assert.True(Criar(telefone: telefone).IsFailure);
    }

    [Theory]
    [InlineData("")]
    [InlineData("abc")]
    public void Email_Invalido_Deve_Falhar(string email)
    {
        Assert.True(Criar(email: email).IsFailure);
    }

    [Theory]
    [InlineData("")]
    [InlineData("abcdef")]
    public void Senha_Invalida_Deve_Falhar(string senha)
    {
        Assert.True(Criar(senha: senha).IsFailure);
    }

    [Fact]
    public void Colaborador_Deve_Armazenar_Tipo_Vinculo_E_Data_Admissao()
    {
        var result = Criar(tipo: ColaboradorTipo.Instrutor, vinculo: ColaboradorVinculo.Estagio, diasAdmissao: -100);
        Assert.True(result.IsSuccess);
        Assert.Equal(ColaboradorTipo.Instrutor, result.Value!.Tipo);
        Assert.Equal(ColaboradorVinculo.Estagio, result.Value.Vinculo);
        Assert.Equal(DateOnly.FromDateTime(DateTime.Today.AddDays(-100)), result.Value.DataAdmissao);
    }

    [Fact]
    public void Colaborador_Id_Negativo_Deve_Lancar_Excecao()
    {
        Assert.Throws<DomainException>(() => Criar(id: -1));
    }
}
