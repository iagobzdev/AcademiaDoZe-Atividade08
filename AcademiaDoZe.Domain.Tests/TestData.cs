// Iago Barboza
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Tests;

internal static class TestData
{
    public static Logradouro LogradouroValido(int id = 1) =>
        Logradouro.Criar(id, "88500-001", "Rua Teste", "Centro", "Lages", "SC", "Brasil").Value!;

    public static Arquivo ArquivoValido() => Arquivo.Criar([1, 2, 3]).Value!;

    public static Aluno AlunoValido(int id = 1, int idade = 20) =>
        Aluno.Criar(
            id,
            "João da Silva",
            "529.982.247-25",
            DateOnly.FromDateTime(DateTime.Today.AddYears(-idade)),
            "(49) 99999-9999",
            "aluno@teste.com",
            LogradouroValido(),
            "123",
            "",
            "SenhaA",
            ArquivoValido()).Value!;

    public static Colaborador ColaboradorValido(
        int id = 1,
        ColaboradorTipo tipo = ColaboradorTipo.Atendente,
        ColaboradorVinculo vinculo = ColaboradorVinculo.CLT) =>
        Colaborador.Criar(
            id,
            "Maria da Silva",
            "123.456.789-01",
            DateOnly.FromDateTime(DateTime.Today.AddYears(-30)),
            "(49) 98888-8888",
            "colaborador@teste.com",
            LogradouroValido(),
            "321",
            "Sala 2",
            "SenhaB",
            ArquivoValido(),
            DateOnly.FromDateTime(DateTime.Today.AddYears(-1)),
            tipo,
            vinculo).Value!;
}
