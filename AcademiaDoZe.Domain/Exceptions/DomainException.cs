// Iago Barboza
namespace AcademiaDoZe.Domain.Exceptions;

// Classe base para exceções de domínio, permitindo exceções específicas de regras de negócio.
public sealed class DomainException(string message) : Exception(message)
{
}
