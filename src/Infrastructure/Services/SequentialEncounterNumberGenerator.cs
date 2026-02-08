using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Blazor.Infrastructure.Services;

public class SequentialEncounterNumberGenerator : IEncounterNumberGenerator
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;

    public SequentialEncounterNumberGenerator(IApplicationDbContext context, IDateTime dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }

    public async Task<string> GenerateNextEncounterNumberAsync(CancellationToken cancellationToken = default)
    {
        var today = _dateTime.Now.Date;
        var datePrefix = today.ToString("yyyyMMdd");

        var lastNumber = await _context.Encounters
            .Where(e => e.EncounterNumber.StartsWith($"EN-{datePrefix}"))
            .OrderByDescending(e => e.EncounterNumber)
            .Select(e => e.EncounterNumber)
            .FirstOrDefaultAsync(cancellationToken);

        var nextNumber = 1;
        if (!string.IsNullOrEmpty(lastNumber))
        {
            var parts = lastNumber.Split('-');
            if (parts.Length == 3 && int.TryParse(parts[2], out var currentNumber))
            {
                nextNumber = currentNumber + 1;
            }
        }

        return $"EN-{datePrefix}-{nextNumber:D4}";
    }
}
