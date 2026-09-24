// Iago Barboza
using AcademiaDoZe.Infrastructure.Data;
using AcademiaDoZe.Infrastructure.Exceptions;
using System.Data;
using System.Data.Common;

namespace AcademiaDoZe.Infrastructure.Repositories;

/*
BaseRepository é uma classe utilitária de infraestrutura focada em gerenciamento de conexões (GetOpenConnectionAsync e Dispose/DisposeAsync).
As operações de CRUD e mapeamento são implementadas diretamente nas classes filhas (AlunoRepository, ColaboradorRepository, etc.).
*/
public abstract class BaseRepository : IDisposable, IAsyncDisposable
{
    protected readonly string _connectionString;
    private DbConnection? _connection;
    private bool _disposed;

    protected BaseRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new InfrastructureException("STRING_CONEXAO_NULA", $"String de conexão não pode ser nula: {nameof(connectionString)}");
    }

    protected virtual async Task<DbConnection> GetOpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        try
        {
            // cria o banco e as tabelas do banco de dados se não existir
            await DbInitializer.InicializarAsync(_connectionString, cancellationToken);

            if (_connection == null)
            {
                _connection = DbProvider.CreateConnection(_connectionString);
                await _connection.OpenAsync(cancellationToken);
                await HabilitarChavesEstrangeirasAsync(_connection, cancellationToken);
            }
            else if (_connection.State == ConnectionState.Broken)
            {
                await _connection.CloseAsync();
                await _connection.OpenAsync(cancellationToken);
                await HabilitarChavesEstrangeirasAsync(_connection, cancellationToken);
            }
            else if (_connection.State == ConnectionState.Closed)
            {
                await _connection.OpenAsync(cancellationToken);
                await HabilitarChavesEstrangeirasAsync(_connection, cancellationToken);
            }

            return _connection;
        }
        catch (DbException ex)
        {
            throw new InfrastructureException("FALHA_ABRIR_CONEXAO", "Falha ao abrir conexão com o banco de dados.", ex);
        }
    }

    private static async Task HabilitarChavesEstrangeirasAsync(DbConnection connection, CancellationToken cancellationToken)
    {
        await using var command = DbProvider.CreateCommand("PRAGMA foreign_keys = ON;", connection);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    protected virtual async Task<DbCommand> CreateCommandAsync(string commandText, CancellationToken cancellationToken = default)
    {
        var connection = await GetOpenConnectionAsync(cancellationToken);
        return DbProvider.CreateCommand(commandText, connection);
    }

    protected static string FormatInsertQuery(string insertSql) => DbProvider.FormatInsertQuery(insertSql);
    protected static string GetCurrentDateFunction() => DbProvider.GetCurrentDateFunction();
    protected static string GetDateAddDaysExpression(string dateExpr, string daysParam) => DbProvider.GetDateAddDaysExpression(dateExpr, daysParam);
    protected static string GetDateHourExpression(string dateColumn) => DbProvider.GetDateHourExpression(dateColumn);
    protected static string GetDateMonthExpression(string dateColumn) => DbProvider.GetDateMonthExpression(dateColumn);
    protected static string GetDateDayExpression(string dateColumn) => DbProvider.GetDateDayExpression(dateColumn);

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _connection?.Dispose();
                _connection = null;
            }
            _disposed = true;
        }
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_connection != null)
        {
            await _connection.DisposeAsync().ConfigureAwait(false);
            _connection = null;
        }
    }
}
