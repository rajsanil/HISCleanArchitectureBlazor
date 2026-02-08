using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Blazor.Infrastructure.Services;

public class SequentialMrnGenerator : IMrnGenerator
{
    private readonly IApplicationDbContext _context;

    public SequentialMrnGenerator(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateNextMrnAsync(CancellationToken cancellationToken = default)
    {
        var lastMrn = await _context.Patients
            .OrderByDescending(p => p.Id)
            .Select(p => p.MRN)
            .FirstOrDefaultAsync(cancellationToken);

        var nextNumber = 1;
        if (!string.IsNullOrEmpty(lastMrn))
        {
            var parts = lastMrn.Split('-');
            if (parts.Length == 2 && int.TryParse(parts[1], out var currentNumber))
            {
                nextNumber = currentNumber + 1;
            }
        }

        return $"MRN-{nextNumber:D6}";
    }
}
