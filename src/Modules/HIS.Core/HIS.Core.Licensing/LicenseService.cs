using System.Text.Json;
using HIS.Core.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HIS.Core.Licensing;

/// <summary>
/// License service options for configuration.
/// </summary>
public class LicenseOptions
{
    /// <summary>
    /// Path to the license file (absolute or relative to application root)
    /// </summary>
    public string LicenseFilePath { get; set; } = "license.json";

    /// <summary>
    /// Whether to allow running without a valid license (development mode)
    /// </summary>
    public bool AllowUnlicensed { get; set; } = false;
}

/// <summary>
/// Implementation of ILicenseService that reads and validates license files.
/// </summary>
public class LicenseService : ILicenseService
{
    private readonly LicenseValidator _validator;
    private readonly ILogger<LicenseService> _logger;
    private readonly LicenseOptions _options;
    private LicensePayload? _cachedLicense;
    private bool _cacheInitialized;
    private readonly object _cacheLock = new();

    public LicenseService(
        LicenseValidator validator,
        ILogger<LicenseService> logger,
        IOptions<LicenseOptions> options)
    {
        _validator = validator;
        _logger = logger;
        _options = options.Value;
    }

    /// <summary>
    /// Loads the license from file and caches it.
    /// </summary>
    private LicensePayload? LoadLicense()
    {
        lock (_cacheLock)
        {
            if (_cacheInitialized)
            {
                return _cachedLicense;
            }

            try
            {
                var licensePath = _options.LicenseFilePath;
                
                if (!File.Exists(licensePath))
                {
                    _logger.LogWarning("License file not found at: {Path}", licensePath);
                    
                    if (_options.AllowUnlicensed)
                    {
                        _logger.LogWarning("Running in unlicensed mode (development only)");
                        _cachedLicense = CreateDevelopmentLicense();
                    }
                    else
                    {
                        _cachedLicense = null;
                    }
                    
                    _cacheInitialized = true;
                    return _cachedLicense;
                }

                var json = File.ReadAllText(licensePath);
                var license = JsonSerializer.Deserialize<LicensePayload>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (license == null)
                {
                    _logger.LogError("Failed to deserialize license file");
                    _cachedLicense = null;
                }
                else
                {
                    var (isValid, errorMessage) = _validator.Validate(license);
                    
                    if (!isValid)
                    {
                        _logger.LogError("License validation failed: {Error}", errorMessage);
                        _cachedLicense = null;
                    }
                    else
                    {
                        _logger.LogInformation("License loaded successfully for: {Customer}", license.CustomerName);
                        _cachedLicense = license;
                    }
                }

                _cacheInitialized = true;
                return _cachedLicense;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading license file");
                _cachedLicense = null;
                _cacheInitialized = true;
                return null;
            }
        }
    }

    /// <summary>
    /// Creates a development-only license with all modules enabled.
    /// </summary>
    private LicensePayload CreateDevelopmentLicense()
    {
        return new LicensePayload
        {
            CustomerName = "Development Environment",
            IssuedDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddYears(1),
            MaxUsers = 999,
            LicenseType = "Development",
            LicensedModules = 
            [
                "HIS.Core",
                "HIS.MasterData",
                "HIS.Foundation",
                "HIS.Patient",
                "HIS.Outpatient",
                "HIS.Inpatient",
                "HIS.Emergency",
                "HIS.Clinical",
                "HIS.Laboratory",
                "HIS.Radiology",
                "HIS.Pharmacy",
                "HIS.Nursing",
                "HIS.Inventory",
                "HIS.Insurance",
                "HIS.MRD",
                "HIS.Support"
            ],
            Signature = "DEV-MODE-NO-SIGNATURE"
        };
    }

    public bool IsModuleLicensed(string moduleId)
    {
        var license = LoadLicense();
        return license?.LicensedModules?.Contains(moduleId, StringComparer.OrdinalIgnoreCase) ?? false;
    }

    public IReadOnlyList<string> GetLicensedModules()
    {
        var license = LoadLicense();
        return license?.LicensedModules ?? Array.Empty<string>();
    }

    public LicenseInfo GetLicenseInfo()
    {
        var license = LoadLicense();
        
        if (license == null)
        {
            return new LicenseInfo
            {
                CustomerName = "Unlicensed",
                LicenseType = null,
                IsValid = false,
                ValidationError = "No valid license found"
            };
        }

        var (isValid, errorMessage) = _validator.Validate(license);

        return new LicenseInfo
        {
            CustomerName = license.CustomerName,
            LicenseType = license.LicenseType,
            IssuedDate = license.IssuedDate,
            ExpiryDate = license.ExpiryDate,
            MaxUsers = license.MaxUsers,
            LicensedModules = license.LicensedModules,
            IsValid = isValid,
            ValidationError = errorMessage
        };
    }

    public int? GetDaysUntilExpiration()
    {
        var license = LoadLicense();
        return license == null ? null : _validator.GetDaysUntilExpiration(license);
    }

    public bool IsLicenseValid()
    {
        var license = LoadLicense();
        if (license == null) return false;

        var (isValid, _) = _validator.Validate(license);
        return isValid;
    }

    public bool IsApproachingExpiration(int warningDays = 30)
    {
        var daysUntilExpiration = GetDaysUntilExpiration();
        
        if (!daysUntilExpiration.HasValue)
        {
            return false; // Perpetual license
        }

        return daysUntilExpiration.Value >= 0 && daysUntilExpiration.Value <= warningDays;
    }
}
