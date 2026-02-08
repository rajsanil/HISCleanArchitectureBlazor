using HIS.Application.DTOs;
using HIS.Application.Interfaces;
using HIS.Domain.Entities;
using HIS.Domain.Interfaces;

namespace HIS.Application.Services;

public class CountryService : ICountryService
{
    private readonly IRepository<Country> _countryRepository;

    public CountryService(IRepository<Country> countryRepository)
    {
        _countryRepository = countryRepository;
    }

    public async Task<IEnumerable<CountryDto>> GetAllCountriesAsync(CancellationToken cancellationToken = default)
    {
        var countries = await _countryRepository.GetAllAsync(cancellationToken);
        return countries.Select(MapToDto).OrderBy(c => c.Name);
    }

    public async Task<CountryDto?> GetCountryByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var country = await _countryRepository.GetByIdAsync(id, cancellationToken);
        return country != null ? MapToDto(country) : null;
    }

    public async Task<CountryDto> CreateCountryAsync(CreateCountryDto dto, CancellationToken cancellationToken = default)
    {
        var country = new Country
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Code = dto.Code.ToUpperInvariant(),
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };

        var createdCountry = await _countryRepository.AddAsync(country, cancellationToken);
        return MapToDto(createdCountry);
    }

    public async Task UpdateCountryAsync(Guid id, CreateCountryDto dto, CancellationToken cancellationToken = default)
    {
        var country = await _countryRepository.GetByIdAsync(id, cancellationToken);
        if (country == null)
            throw new InvalidOperationException($"Country with ID {id} not found.");

        country.Name = dto.Name;
        country.Code = dto.Code.ToUpperInvariant();
        country.Description = dto.Description;
        country.UpdatedAt = DateTime.UtcNow;

        await _countryRepository.UpdateAsync(country, cancellationToken);
    }

    public async Task DeleteCountryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _countryRepository.DeleteAsync(id, cancellationToken);
    }

    private static CountryDto MapToDto(Country country)
    {
        return new CountryDto
        {
            Id = country.Id,
            Name = country.Name,
            Code = country.Code,
            Description = country.Description
        };
    }
}
