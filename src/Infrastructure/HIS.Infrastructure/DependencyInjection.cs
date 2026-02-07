using HIS.Application.Interfaces;
using HIS.Application.Services;
using HIS.Domain.Interfaces;
using HIS.Infrastructure.Data;
using HIS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HIS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Add DbContext with InMemory database for demonstration
        services.AddDbContext<HISDbContext>(options =>
            options.UseInMemoryDatabase("HISDatabase"));

        // Register repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Register application services
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IAppointmentService, AppointmentService>();

        return services;
    }
}
