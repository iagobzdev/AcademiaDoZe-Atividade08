using AcademiaDoZe.Application.Interfaces;
using AcademiaDoZe.Application.Services;
using AcademiaDoZe.Domain.Repositories;
using AcademiaDoZe.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AcademiaDoZe.Application.DependencyInjection;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Registra os serviços da camada de aplicação
        services.AddTransient<ILogradouroService, LogradouroService>();
        services.AddTransient<IColaboradorService, ColaboradorService>();
        services.AddTransient<IAlunoService, AlunoService>();
        services.AddTransient<IMatriculaService, MatriculaService>();
        // TODO Avaliação 03: registrar quando IAcessoAlunoService/AcessoAlunoService existirem
        // services.AddTransient<IAcessoAlunoService, AcessoAlunoService>();
        // TODO Avaliação 03: registrar quando IAcessoColaboradorService/AcessoColaboradorService existirem
        // services.AddTransient<IAcessoColaboradorService, AcessoColaboradorService>();

        // AddScoped: cria uma instância do serviço por requisição HTTP.
        // AddSingleton: cria uma única instância do serviço durante toda a vida útil da aplicação.
        // AddTransient: cria uma nova instância do serviço toda vez que ele é solicitado.

        // Registra as fábricas Func<IRepo> para criar instâncias sob demanda nos services
        // Adaptação: AcademiaDoZe.Infrastructure é exclusivamente SQLite, os repositórios recebem
        // apenas a connectionString no construtor (não existe um enum DatabaseType nesta solução).
        services.AddTransient(provider =>
        {
            var config = provider.GetRequiredService<RepositoryConfig>();
            return (Func<ILogradouroRepository>)(() => new LogradouroRepository(config.ConnectionString));
        });

        services.AddTransient(provider =>
        {
            var config = provider.GetRequiredService<RepositoryConfig>();
            return (Func<IColaboradorRepository>)(() => new ColaboradorRepository(config.ConnectionString));
        });

        services.AddTransient(provider =>
        {
            var config = provider.GetRequiredService<RepositoryConfig>();
            return (Func<IAlunoRepository>)(() => new AlunoRepository(config.ConnectionString));
        });

        services.AddTransient(provider =>
        {
            var config = provider.GetRequiredService<RepositoryConfig>();
            return (Func<IMatriculaRepository>)(() => new MatriculaRepository(config.ConnectionString));
        });

        // TODO Avaliação 03: registrar a fábrica Func<IAcessoAlunoRepository> quando AcessoAlunoRepository existir
        // services.AddTransient(provider =>
        // {
        //     var config = provider.GetRequiredService<RepositoryConfig>();
        //     return (Func<IAcessoAlunoRepository>)(() => new AcessoAlunoRepository(config.ConnectionString));
        // });

        // TODO Avaliação 03: registrar a fábrica Func<IAcessoColaboradorRepository> quando AcessoColaboradorRepository existir
        // services.AddTransient(provider =>
        // {
        //     var config = provider.GetRequiredService<RepositoryConfig>();
        //     return (Func<IAcessoColaboradorRepository>)(() => new AcessoColaboradorRepository(config.ConnectionString));
        // });

        return services;
    }
}
