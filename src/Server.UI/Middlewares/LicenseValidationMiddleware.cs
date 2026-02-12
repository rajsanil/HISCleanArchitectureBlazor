using HIS.Core.Abstractions;
using Microsoft.AspNetCore.Components;

namespace CleanArchitecture.Blazor.Server.UI.Middlewares;

/// <summary>
/// Middleware to validate module licenses and prevent access to unlicensed features.
/// </summary>
public class LicenseValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LicenseValidationMiddleware> _logger;

    public LicenseValidationMiddleware(
        RequestDelegate next,
        ILogger<LicenseValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ILicenseService licenseService,
        IModuleLoader moduleLoader)
    {
        // Skip validation for static files, API endpoints, and system pages
        var path = context.Request.Path.Value ?? "";
        if (ShouldSkipValidation(path))
        {
            await _next(context);
            return;
        }

        // Extract module from route (e.g., /pages/countries -> HIS.MasterData)
        var moduleId = TryGetModuleFromPath(path, moduleLoader);
        
        if (!string.IsNullOrEmpty(moduleId))
        {
            // Check if module is licensed
            if (!licenseService.IsModuleLicensed(moduleId))
            {
                _logger.LogWarning(
                    "Access denied to unlicensed module. Path: {Path}, Module: {ModuleId}",
                    path,
                    moduleId);

                // Redirect to license error page
                context.Response.Redirect($"/system/license-error?module={Uri.EscapeDataString(moduleId)}");
                return;
            }
        }

        await _next(context);
    }

    private static bool ShouldSkipValidation(string path)
    {
        // Skip validation for:
        // - Static files (wwwroot)
        // - Authentication pages
        // - System/admin pages
        // - API endpoints
        // - SignalR hubs
        
        if (string.IsNullOrEmpty(path)) return true;

        var skipPrefixes = new[]
        {
            "/_",           // Blazor internal
            "/css/",
            "/js/",
            "/images/",
            "/files/",
            "/identity/",   // Login, logout, etc.
            "/system/",     // System pages (including license info)
            "/api/",
            "/hubs/",
            "/health",
            "/swagger"
        };

        return skipPrefixes.Any(prefix => path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
    }

    private static string? TryGetModuleFromPath(string path, IModuleLoader moduleLoader)
    {
        // Map common route patterns to modules
        // This is a simplified mapping - in production, you might want a more sophisticated approach
        
        var routeToModuleMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // Master Data module pages
            { "/pages/countries", "HIS.MasterData" },
            { "/pages/cities", "HIS.MasterData" },
            { "/pages/nationalities", "HIS.MasterData" },
            { "/pages/bloodgroups", "HIS.MasterData" },
            { "/pages/maritalstatuses", "HIS.MasterData" },
            
            // Foundation module pages (future)
            { "/pages/facilities", "HIS.Foundation" },
            { "/pages/departments", "HIS.Foundation" },
            { "/pages/specialties", "HIS.Foundation" },
            { "/pages/locations", "HIS.Foundation" },
            { "/pages/rooms", "HIS.Foundation" },
            { "/pages/beds", "HIS.Foundation" },
            { "/pages/staff", "HIS.Foundation" },
            
            // Patient module pages (future)
            { "/pages/patients", "HIS.Patient" },
            { "/pages/visits", "HIS.Patient" },
            
            // Clinical module pages (future)
            { "/pages/encounters", "HIS.Clinical" },
            { "/pages/bedboard", "HIS.Clinical" },
        };

        foreach (var (route, moduleId) in routeToModuleMap)
        {
            if (path.StartsWith(route, StringComparison.OrdinalIgnoreCase))
            {
                return moduleId;
            }
        }

        return null; // Unknown route or no module mapping
    }
}
