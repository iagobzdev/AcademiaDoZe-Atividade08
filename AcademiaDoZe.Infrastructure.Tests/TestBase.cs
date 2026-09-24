// Iago Barboza
[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly, DisableTestParallelization = true)]

namespace AcademiaDoZe.Infrastructure.Tests;

// Classe base para os testes de integração da camada de Infraestrutura. Utiliza exclusivamente SQLite.
public abstract class TestBase
{
    private static readonly string DbPath = Path.Combine(AppContext.BaseDirectory, "db_academia_do_ze.db");

    protected string ConnectionString { get; } = $"Data Source={DbPath};Cache=Shared;";

    #region Geradores de dados aleatórios
    private static int _counter = 10000;
    protected static string GerarCep() => (80000000 + ((int)(DateTime.UtcNow.Ticks % 8000000)) + Interlocked.Increment(ref _counter)).ToString("D8")[..8];
    protected static string GerarCpf() => (10000000000L + (DateTime.UtcNow.Ticks % 8000000000L) + Interlocked.Increment(ref _counter)).ToString("D11")[..11];
    protected static string GerarEmail() => $"user_{Guid.NewGuid().ToString("N")[..8]}@test.com";
    protected static string GerarTelefone() => (49990000000L + (DateTime.UtcNow.Ticks % 8000000000L) + Interlocked.Increment(ref _counter)).ToString("D11")[..11];
    #endregion
}
