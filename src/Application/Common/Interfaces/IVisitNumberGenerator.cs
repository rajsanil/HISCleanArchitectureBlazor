namespace CleanArchitecture.Blazor.Application.Common.Interfaces;

public interface IVisitNumberGenerator
{
    Task<string> GenerateNextVisitNumberAsync(CancellationToken cancellationToken = default);
}
