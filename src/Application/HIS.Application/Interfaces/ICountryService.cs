using HIS.Application.DTOs;

namespace HIS.Application.Interfaces;

public interface ICountryService
{
    Task<IEnumerable<CountryDto>> GetAllCountriesAsync(CancellationToken cancellationToken = default);
    Task<CountryDto?> GetCountryByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CountryDto> CreateCountryAsync(CreateCountryDto dto, CancellationToken cancellationToken = default);
    Task UpdateCountryAsync(Guid id, CreateCountryDto dto, CancellationToken cancellationToken = default);
    Task DeleteCountryAsync(Guid id, CancellationToken cancellationToken = default);
}
