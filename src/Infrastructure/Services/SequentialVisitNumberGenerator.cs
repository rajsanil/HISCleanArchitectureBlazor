using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Blazor.Infrastructure.Services;

public class SequentialVisitNumberGenerator : IVisitNumberGenerator
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;

    public SequentialVisitNumberGenerator(IApplicationDbContext context, IDateTime dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }

    public async Task<string> GenerateNextVisitNumberAsync(CancellationToken cancellationToken = default)
    {
        var today = _dateTime.Now.Date;
        var datePrefix = today.ToString("yyyyMMdd");

        var lastVisitNumber = await _context.Visits
            .Where(v => v.VisitNumber.StartsWith($"VN-{datePrefix}"))
            .OrderByDescending(v => v.VisitNumber)
            .Select(v => v.VisitNumber)
            .FirstOrDefaultAsync(cancellationToken);

        var nextNumber = 1;
        if (!string.IsNullOrEmpty(lastVisitNumber))
        {
            var parts = lastVisitNumber.Split('-');
            if (parts.Length == 3 && int.TryParse(parts[2], out var currentNumber))
            {
                nextNumber = currentNumber + 1;
            }
        }

        return $"VN-{datePrefix}-{nextNumber:D4}";
    }
}
