using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace HIS.Core.Licensing;

/// <summary>
/// Validates license files using RSA signature verification.
/// </summary>
public class LicenseValidator
{
    private readonly ILogger<LicenseValidator> _logger;
    
    // Public key for signature verification (embedded in application)
    // This should be the public key corresponding to the private key used to sign licenses
    // For production, load this from embedded resource or configuration
    private const string PublicKeyXml = @"<RSAKeyValue>
        <!-- PLACEHOLDER: Replace with actual public key -->
        <!-- Generate using: var rsa = RSA.Create(2048); var publicKey = rsa.ToXmlString(false); -->
    </RSAKeyValue>";

    public LicenseValidator(ILogger<LicenseValidator> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Validates a license payload's signature and expiry.
    /// </summary>
    public (bool IsValid, string? ErrorMessage) Validate(LicensePayload license)
    {
        try
        {
            // Check if license is expired
            if (license.ExpiryDate.HasValue && license.ExpiryDate.Value < DateTime.UtcNow)
            {
                return (false, $"License expired on {license.ExpiryDate.Value:yyyy-MM-dd}");
            }

            // Verify signature
            if (!VerifySignature(license))
            {
                return (false, "License signature is invalid. The license file may have been tampered with.");
            }

            // Validate required fields
            if (string.IsNullOrWhiteSpace(license.CustomerName))
            {
                return (false, "License is missing customer name");
            }

            if (license.LicensedModules == null || license.LicensedModules.Length == 0)
            {
                return (false, "License has no modules enabled");
            }

            if (license.MaxUsers <= 0)
            {
                return (false, "License has invalid max users value");
            }

            _logger.LogInformation("License validated successfully for customer: {Customer}", license.CustomerName);
            return (true, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating license");
            return (false, $"License validation error: {ex.Message}");
        }
    }

    /// <summary>
    /// Verifies the RSA signature of the license payload.
    /// </summary>
    private bool VerifySignature(LicensePayload license)
    {
        try
        {
            // Extract signature
            var signature= Convert.FromBase64String(license.Signature);

            // Create payload JSON without signature field for verification
            var payloadForSigning = new
            {
                customer = license.CustomerName,
                issued = license.IssuedDate,
                expires = license.ExpiryDate,
                maxUsers = license.MaxUsers,
                modules = license.LicensedModules,
                organizationId = license.OrganizationId,
                licenseType = license.LicenseType
            };

            var jsonPayload = JsonSerializer.Serialize(payloadForSigning, new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var dataBytes = Encoding.UTF8.GetBytes(jsonPayload);

            // For initial development, skip signature verification if public key is placeholder
            if (PublicKeyXml.Contains("PLACEHOLDER"))
            {
                _logger.LogWarning("License signature verification skipped - using placeholder public key");
                return true;
            }

            // Verify signature using RSA public key
            using var rsa = RSA.Create();
            rsa.FromXmlString(PublicKeyXml);
            
            return rsa.VerifyData(dataBytes, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying license signature");
            return false;
        }
    }

    /// <summary>
    /// Calculates days until expiration.
    /// Returns null for perpetual licenses, negative for expired licenses.
    /// </summary>
    public int? GetDaysUntilExpiration(LicensePayload license)
    {
        if (!license.ExpiryDate.HasValue)
        {
            return null; // Perpetual license
        }

        return (license.ExpiryDate.Value.Date - DateTime.UtcNow.Date).Days;
    }
}
