// Iago Barboza
using AcademiaDoZe.Domain.Common;
using AcademiaDoZe.Domain.Entities;

namespace AcademiaDoZe.Domain.Repositories;

// Interface genérica para repositórios. Restrita apenas a Raízes de Agregado (Aggregate Roots) no DDD.
public interface IRepository<TEntity> where TEntity : Entity, IAggregateRoot
{
    Task<TEntity?> ObterPorId(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> ObterTodos(CancellationToken cancellationToken = default);
    Task<TEntity> Adicionar(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity> Atualizar(TEntity entity, CancellationToken cancellationToken = default);
    Task<bool> Remover(int id, CancellationToken cancellationToken = default);
}
