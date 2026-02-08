namespace CleanArchitecture.Blazor.Application.Common.Interfaces;

public interface IMrnGenerator
{
    Task<string> GenerateNextMrnAsync(CancellationToken cancellationToken = default);
}
