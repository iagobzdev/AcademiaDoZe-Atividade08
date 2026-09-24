// Iago Barboza
using AcademiaDoZe.Domain.Common;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Exceptions;

namespace AcademiaDoZe.Domain.Tests.Entities;

public class AlunoTests
{
    private static Result<Aluno> Criar(
        int id = 1,
        string nome = "João da Silva",
        string cpf = "529.982.247-25",
        int idade = 20,
        string telefone = "(49) 99999-9999",
        string email = "aluno@teste.com",
        Logradouro? logradouro = null,
        string numero = "123",
        string complemento = "",
        string senha = "SenhaA")
    {
        return Aluno.Criar(
            id,
            nome,
            cpf,
            DateOnly.FromDateTime(DateTime.Today.AddYears(-idade)),
            telefone,
            email,
            logradouro ?? TestData.LogradouroValido(),
            numero,
            complemento,
            senha,
            TestData.ArquivoValido());
    }

    [Theory]
    [InlineData(" João da Silva ", "João da Silva")]
    [InlineData("Maria", "Maria")]
    [InlineData("  João   Pedro  ", "João Pedro")]
    public void Aluno_Valido_Deve_Criar_E_Normalizar_Nome(string nome, string esperado)
    {
        var result = Criar(nome: nome);
        Assert.True(result.IsSuccess);
        Assert.Equal(esperado, result.Value!.Nome);
        Assert.IsAssignableFrom<IAggregateRoot>(result.Value);
    }

    [Theory]
    [InlineData(12)]
    [InlineData(13)]
    [InlineData(18)]
    [InlineData(25)]
    [InlineData(60)]
    public void Aluno_Com_Idade_Minima_Ou_Maior_Deve_Criar(int idade)
    {
        var result = Criar(idade: idade);
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(11)]
    [InlineData(10)]
    [InlineData(1)]
    public void Aluno_Com_Menos_De_12_Anos_Deve_Falhar(int idade)
    {
        var result = Criar(idade: idade);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "DATA_NASCIMENTO_MINIMA_INVALIDA");
    }

    [Fact]
    public void Aluno_Data_Nascimento_Default_Deve_Falhar()
    {
        var result = Aluno.Criar(1, "João", "52998224725", default, "49999999999", "a@b.com", TestData.LogradouroValido(), "1", "", "SenhaA", TestData.ArquivoValido());
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "DATA_NASCIMENTO_OBRIGATORIO");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Aluno_Nome_Vazio_Deve_Falhar(string nome)
    {
        var result = Criar(nome: nome);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "NOME_OBRIGATORIO");
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    public void Aluno_Cpf_Invalido_Deve_Falhar(string cpf)
    {
        var result = Criar(cpf: cpf);
        Assert.True(result.IsFailure);
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    public void Aluno_Telefone_Invalido_Deve_Falhar(string telefone)
    {
        var result = Criar(telefone: telefone);
        Assert.True(result.IsFailure);
    }

    [Theory]
    [InlineData("")]
    [InlineData("email")]
    [InlineData("a@b")]
    public void Aluno_Email_Invalido_Deve_Falhar(string email)
    {
        var result = Criar(email: email);
        Assert.True(result.IsFailure);
    }

    [Theory]
    [InlineData("")]
    [InlineData("abcdef")]
    [InlineData("Abc")]
    public void Aluno_Senha_Invalida_Deve_Falhar(string senha)
    {
        var result = Criar(senha: senha);
        Assert.True(result.IsFailure);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Aluno_Numero_Endereco_Obrigatorio_Deve_Falhar(string numero)
    {
        var result = Criar(numero: numero);
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Mensagem == "NUMERO_OBRIGATORIO");
    }

    [Fact]
    public void Aluno_Deve_Armazenar_Dados_Como_ValueObjects()
    {
        var result = Criar(id: 7);
        Assert.True(result.IsSuccess);
        Assert.Equal(7, result.Value!.Id);
        Assert.Equal("52998224725", result.Value.Cpf.Valor);
        Assert.Equal("49999999999", result.Value.Telefone.Valor);
        Assert.Equal("aluno@teste.com", result.Value.Email.Valor);
        Assert.Equal(1, result.Value.Endereco.LogradouroId);
    }

    [Fact]
    public void Aluno_Id_Negativo_Deve_Lancar_Excecao()
    {
        Assert.Throws<DomainException>(() => Criar(id: -1));
    }
}
