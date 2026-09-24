// Iago Barboza
using AcademiaDoZe.Domain.Enums;

namespace AcademiaDoZe.Domain.Tests.Enums;

public class EnumsTests
{
    [Theory]
    [InlineData(ColaboradorTipo.Administrador, 0)]
    [InlineData(ColaboradorTipo.Atendente, 1)]
    [InlineData(ColaboradorTipo.Instrutor, 2)]
    public void ColaboradorTipo_Deve_Ter_Valores_Esperados(ColaboradorTipo valor, int esperado)
    {
        Assert.Equal(esperado, (int)valor);
    }

    [Theory]
    [InlineData(ColaboradorVinculo.CLT, 0)]
    [InlineData(ColaboradorVinculo.Estagio, 1)]
    public void ColaboradorVinculo_Deve_Ter_Valores_Esperados(ColaboradorVinculo valor, int esperado)
    {
        Assert.Equal(esperado, (int)valor);
    }

    [Theory]
    [InlineData(MatriculaPlano.Mensal, 0)]
    [InlineData(MatriculaPlano.Trimestral, 1)]
    [InlineData(MatriculaPlano.Semestral, 2)]
    [InlineData(MatriculaPlano.Anual, 3)]
    public void MatriculaPlano_Deve_Ter_Valores_Esperados(MatriculaPlano valor, int esperado)
    {
        Assert.Equal(esperado, (int)valor);
    }

    [Theory]
    [InlineData(MatriculaRestricoes.None, 0)]
    [InlineData(MatriculaRestricoes.Diabetes, 1)]
    [InlineData(MatriculaRestricoes.PressaoAlta, 2)]
    [InlineData(MatriculaRestricoes.Labirintite, 4)]
    [InlineData(MatriculaRestricoes.Alergias, 8)]
    [InlineData(MatriculaRestricoes.ProblemasRespiratorios, 16)]
    [InlineData(MatriculaRestricoes.RemedioContinuo, 32)]
    public void MatriculaRestricoes_Deve_Usar_Flags_Potencias_De_Dois(MatriculaRestricoes valor, int esperado)
    {
        Assert.Equal(esperado, (int)valor);
    }

    [Theory]
    [InlineData(MatriculaRestricoes.Diabetes, MatriculaRestricoes.Alergias)]
    [InlineData(MatriculaRestricoes.PressaoAlta, MatriculaRestricoes.RemedioContinuo)]
    [InlineData(MatriculaRestricoes.Labirintite, MatriculaRestricoes.ProblemasRespiratorios)]
    public void MatriculaRestricoes_Deve_Permitir_Combinacao_De_Flags(MatriculaRestricoes a, MatriculaRestricoes b)
    {
        var combinado = a | b;
        Assert.True(combinado.HasFlag(a));
        Assert.True(combinado.HasFlag(b));
    }
}
