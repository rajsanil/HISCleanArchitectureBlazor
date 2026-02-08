namespace CleanArchitecture.Blazor.Application.Common.Interfaces;

public interface IEncounterNumberGenerator
{
    Task<string> GenerateNextEncounterNumberAsync(CancellationToken cancellationToken = default);
}
