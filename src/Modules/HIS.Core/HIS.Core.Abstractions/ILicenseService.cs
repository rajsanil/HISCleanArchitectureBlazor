namespace HIS.Core.Abstractions;

/// <summary>
/// Service for validating and querying license information.
/// </summary>
public interface ILicenseService
{
    /// <summary>
    /// Checks if a specific module is licensed for use.
    /// </summary>
    /// <param name="moduleId">The module ID to check (e.g., "HIS.Patient")</param>
    /// <returns>True if the module is licensed, false otherwise</returns>
    bool IsModuleLicensed(string moduleId);

    /// <summary>
    /// Gets the list of all licensed module IDs.
    /// </summary>
    IReadOnlyList<string> GetLicensedModules();

    /// <summary>
    /// Gets comprehensive license information including customer name, expiry, etc.
    /// </summary>
    LicenseInfo GetLicenseInfo();

    /// <summary>
    /// Gets the number of days until license expiration.
    /// Returns null if the license has no expiry or is perpetual.
    /// Returns negative values if already expired.
    /// </summary>
    int? GetDaysUntilExpiration();

    /// <summary>
    /// Checks if the license is valid (not expired and properly signed).
    /// </summary>
    bool IsLicenseValid();

    /// <summary>
    /// Checks if the license is approaching expiration (within warning threshold).
    /// </summary>
    /// <param name="warningDays">Number of days before expiry to start warning (default: 30)</param>
    bool IsApproachingExpiration(int warningDays = 30);
}

/// <summary>
/// Contains license information for display and validation.
/// </summary>
public class LicenseInfo
{
    public string CustomerName { get; set; } = "Unlicensed";
    public string? LicenseType { get; set; }
    public DateTime? IssuedDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public int MaxUsers { get; set; }
    public IReadOnlyList<string> LicensedModules { get; set; } = [];
    public bool IsValid { get; set; }
    public string? ValidationError { get; set; }
}
