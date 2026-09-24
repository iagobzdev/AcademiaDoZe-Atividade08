namespace AcademiaDoZe.Application.DependencyInjection;

// Adaptação: o projeto AcademiaDoZe.Infrastructure utiliza exclusivamente SQLite (Microsoft.Data.Sqlite)
// e seus repositórios possuem construtor com um único parâmetro (connectionString).
// Não existe um enum DatabaseType na Infrastructure desta solução.
public class RepositoryConfig
{
    public required string ConnectionString { get; set; }
}
