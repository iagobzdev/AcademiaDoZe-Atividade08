// Iago Barboza
using AcademiaDoZe.Domain.Common;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Exceptions;

namespace AcademiaDoZe.Domain.Tests.Common;

public class CommonTests
{
    private sealed class EntidadeTeste : Entity
    {
        public EntidadeTeste(int id) : base(id) { }
    }

    [Fact]
    public void Notification_Deve_Armazenar_Propriedade_E_Mensagem()
    {
        var notification = new Notification("Nome", "NOME_OBRIGATORIO");
        Assert.Equal("Nome", notification.Propriedade);
        Assert.Equal("NOME_OBRIGATORIO", notification.Mensagem);
    }

    [Fact]
    public void Notification_Deve_Usar_Igualdade_Por_Valor()
    {
        Assert.Equal(new Notification("Campo", "Erro"), new Notification("Campo", "Erro"));
    }

    [Fact]
    public void Result_Success_Deve_Ser_Sucesso()
    {
        var result = Result<string>.Success("ok");
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal("ok", result.Value);
        Assert.Empty(result.Notifications);
    }

    [Fact]
    public void Result_Failure_Com_Lista_Deve_Ser_Falha()
    {
        var result = Result<string>.Failure([new Notification("Campo", "ERRO")]);
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.Single(result.Notifications);
    }

    [Fact]
    public void Result_Failure_Com_Propriedade_E_Mensagem_Deve_Criar_Notificacao()
    {
        var result = Result<int>.Failure("Id", "ID_INVALIDO");
        Assert.Equal("Id", result.Notifications.Single().Propriedade);
        Assert.Equal("ID_INVALIDO", result.Notifications.Single().Mensagem);
    }

    [Fact]
    public void Result_Failure_Com_Notification_Deve_Preservar_Objeto()
    {
        var notification = new Notification("Cpf", "CPF_DIGITOS");
        var result = Result<int>.Failure(notification);
        Assert.Equal(notification, result.Notifications.Single());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(999)]
    public void Entity_Deve_Aceitar_Id_Nao_Negativo(int id)
    {
        var entity = new EntidadeTeste(id);
        Assert.Equal(id, entity.Id);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    [InlineData(-999)]
    public void Entity_Deve_Lancar_DomainException_Para_Id_Negativo(int id)
    {
        var ex = Assert.Throws<DomainException>(() => new EntidadeTeste(id));
        Assert.Equal("ID_NEGATIVO", ex.Message);
    }
}
