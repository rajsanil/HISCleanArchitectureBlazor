using System.Text.Json.Serialization;

namespace HIS.Core.Licensing;

/// <summary>
/// Represents the payload of a license file before signature verification.
/// This gets serialized to JSON and signed with RSA private key.
/// </summary>
public class LicensePayload
{
    /// <summary>
    /// Customer or organization name
    /// </summary>
    [JsonPropertyName("customer")]
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Date the license was issued
    /// </summary>
    [JsonPropertyName("issued")]
    public DateTime IssuedDate { get; set; }

    /// <summary>
    /// Date the license expires (null for perpetual licenses)
    /// </summary>
    [JsonPropertyName("expires")]
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// Maximum number of concurrent users allowed
    /// </summary>
    [JsonPropertyName("maxUsers")]
    public int MaxUsers { get; set; }

    /// <summary>
    /// List of licensed module IDs
    /// </summary>
    [JsonPropertyName("modules")]
    public string[] LicensedModules { get; set; } = [];

    /// <summary>
    /// Optional: Customer's organization ID for multi-tenant scenarios
    /// </summary>
    [JsonPropertyName("organizationId")]
    public string? OrganizationId { get; set; }

    /// <summary>
    /// Optional: License type (Trial, Standard, Enterprise, etc.)
    /// </summary>
    [JsonPropertyName("licenseType")]
    public string? LicenseType { get; set; }

    /// <summary>
    /// RSA signature of the JSON payload (base64 encoded)
    /// This field is NOT included in the signature calculation itself.
    /// </summary>
    [JsonPropertyName("signature")]
    public string Signature { get; set; } = string.Empty;
}
