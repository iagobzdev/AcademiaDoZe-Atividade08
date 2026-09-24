// Iago Barboza
using AcademiaDoZe.Domain.Common;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Repositories;

namespace AcademiaDoZe.Domain.Tests.Repositories;

public class RepositoryContractsTests
{
    [Theory]
    [InlineData(typeof(Logradouro))]
    [InlineData(typeof(Aluno))]
    [InlineData(typeof(Colaborador))]
    [InlineData(typeof(Matricula))]
    [InlineData(typeof(AcessoAluno))]
    [InlineData(typeof(AcessoColaborador))]
    public void Agregados_Raiz_Devem_Implementar_IAggregateRoot(Type tipo)
    {
        Assert.True(typeof(IAggregateRoot).IsAssignableFrom(tipo));
    }

    [Theory]
    [InlineData(typeof(ILogradouroRepository), typeof(Logradouro))]
    [InlineData(typeof(IAlunoRepository), typeof(Aluno))]
    [InlineData(typeof(IColaboradorRepository), typeof(Colaborador))]
    [InlineData(typeof(IMatriculaRepository), typeof(Matricula))]
    [InlineData(typeof(IAcessoAlunoRepository), typeof(AcessoAluno))]
    [InlineData(typeof(IAcessoColaboradorRepository), typeof(AcessoColaborador))]
    public void Repositorio_Especifico_Deve_Herdar_IRepository_Correto(Type repositorio, Type entidade)
    {
        var contrato = typeof(IRepository<>).MakeGenericType(entidade);
        Assert.True(contrato.IsAssignableFrom(repositorio));
    }

    [Theory]
    [InlineData("ObterPorId")]
    [InlineData("ObterTodos")]
    [InlineData("Adicionar")]
    [InlineData("Atualizar")]
    [InlineData("Remover")]
    public void IRepository_Deve_Conter_Metodos_Basicos(string metodo)
    {
        Assert.NotNull(typeof(IRepository<>).GetMethod(metodo));
    }

    [Theory]
    [InlineData(typeof(ILogradouroRepository), "ObterPorCep")]
    [InlineData(typeof(ILogradouroRepository), "CepJaExiste")]
    [InlineData(typeof(ILogradouroRepository), "ObterPorCidade")]
    [InlineData(typeof(ILogradouroRepository), "ObterPorBairro")]
    [InlineData(typeof(IAlunoRepository), "ObterPorCpf")]
    [InlineData(typeof(IAlunoRepository), "ObterPorEmail")]
    [InlineData(typeof(IAlunoRepository), "CpfJaExiste")]
    [InlineData(typeof(IAlunoRepository), "EmailJaExiste")]
    [InlineData(typeof(IAlunoRepository), "ObterPorNome")]
    [InlineData(typeof(IAlunoRepository), "TrocarSenha")]
    [InlineData(typeof(IColaboradorRepository), "ObterPorCpf")]
    [InlineData(typeof(IColaboradorRepository), "ObterPorEmail")]
    [InlineData(typeof(IColaboradorRepository), "CpfJaExiste")]
    [InlineData(typeof(IColaboradorRepository), "EmailJaExiste")]
    [InlineData(typeof(IColaboradorRepository), "ObterPorTipo")]
    [InlineData(typeof(IColaboradorRepository), "ObterPorVinculo")]
    [InlineData(typeof(IColaboradorRepository), "TrocarSenha")]
    [InlineData(typeof(IMatriculaRepository), "ObterPorAluno")]
    [InlineData(typeof(IMatriculaRepository), "ObterMatriculaAtivaPorAluno")]
    [InlineData(typeof(IMatriculaRepository), "PossuiMatriculaAtiva")]
    [InlineData(typeof(IMatriculaRepository), "ObterAtivas")]
    [InlineData(typeof(IMatriculaRepository), "ObterVencendoEmDias")]
    [InlineData(typeof(IMatriculaRepository), "ObterPorPlano")]
    [InlineData(typeof(IAcessoAlunoRepository), "ObterAcessosPorAlunoPeriodo")]
    [InlineData(typeof(IAcessoAlunoRepository), "ObterUltimoAcesso")]
    [InlineData(typeof(IAcessoAlunoRepository), "EstaNaAcademia")]
    [InlineData(typeof(IAcessoAlunoRepository), "ObterHorarioMaisProcuradoPorMes")]
    [InlineData(typeof(IAcessoAlunoRepository), "ObterPermanenciaMediaPorMes")]
    [InlineData(typeof(IAcessoAlunoRepository), "ObterAlunosSemAcessoNosUltimosDias")]
    [InlineData(typeof(IAcessoColaboradorRepository), "ObterAcessosPorColaboradorPeriodo")]
    [InlineData(typeof(IAcessoColaboradorRepository), "ObterUltimoAcesso")]
    [InlineData(typeof(IAcessoColaboradorRepository), "ObterHorasTrabalhadasNoDia")]
    public void Repositorios_Devem_Conter_Metodos_Especificos(Type repositorio, string metodo)
    {
        Assert.NotNull(repositorio.GetMethod(metodo));
    }
}
