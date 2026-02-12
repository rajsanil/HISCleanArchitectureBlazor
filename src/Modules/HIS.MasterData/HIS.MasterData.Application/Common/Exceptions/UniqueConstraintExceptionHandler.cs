using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace HIS.MasterData.Application.Common.Exceptions;

/// <summary>
/// Helper class for handling database unique constraint violations
/// </summary>
public static class UniqueConstraintExceptionHandler
{
    /// <summary>
    /// Checks if the exception is a unique constraint violation
    /// </summary>
    public static bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        // Check if it's a SQL Server unique constraint violation
        var sqlException = ex.InnerException as SqlException;
        if (sqlException != null)
        {
            // Error number 2601 = Cannot insert duplicate key row (unique index)
            // Error number 2627 = Violation of PRIMARY KEY or UNIQUE constraint
            return sqlException.Number == 2601 || sqlException.Number == 2627;
        }

        // Fallback to message check for other databases (PostgreSQL, SQLite, etc.)
        var message = ex.InnerException?.Message ?? ex.Message;
        return message.Contains("unique", StringComparison.OrdinalIgnoreCase) ||
               message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) ||
               message.Contains("constraint", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Extracts the constraint/index name from the exception message
    /// </summary>
    public static string? ExtractConstraintName(DbUpdateException ex)
    {
        var message = ex.InnerException?.Message ?? ex.Message;
        
        // SQL Server error 2601: "Cannot insert duplicate key row in object 'dbo.Countries' with unique index 'IX_Countries_Code'."
        // Match: unique index 'IX_...'  or  index 'IX_...'
        var match = System.Text.RegularExpressions.Regex.Match(
            message, 
            @"(?:unique\s+)?index\s+'([^']+)'", 
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        
        if (match.Success)
        {
            return match.Groups[1].Value;
        }

        // SQL Server error 2627: "Violation of UNIQUE KEY constraint 'IX_Countries_Code'."
        match = System.Text.RegularExpressions.Regex.Match(
            message, 
            @"constraint\s+'([^']+)'", 
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        
        if (match.Success)
        {
            return match.Groups[1].Value;
        }

        // PostgreSQL: "duplicate key value violates unique constraint "IX_Countries_Code""
        match = System.Text.RegularExpressions.Regex.Match(
            message, 
            @"constraint\s+""([^""]+)""", 
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        
        if (match.Success)
        {
            return match.Groups[1].Value;
        }

        return null;
    }

    /// <summary>
    /// Gets a user-friendly error message for a unique constraint violation
    /// </summary>
    public static string GetUserFriendlyMessage(string? constraintName, string defaultMessage = "A record with the same value already exists.")
    {
        if (string.IsNullOrEmpty(constraintName))
        {
            return defaultMessage;
        }

        // Parse constraint name to extract field name
        // Format: IX_TableName_FieldName or IX_TableName_Field1_Field2
        var parts = constraintName.Split('_', StringSplitOptions.RemoveEmptyEntries);
        
        if (parts.Length >= 3)
        {
            // Skip "IX" and table name, get field name(s)
            var fieldNames = parts.Skip(2).Select(FormatFieldName);
            var fields = string.Join(" and ", fieldNames);
            return $"A record with the same {fields} already exists.";
        }

        return defaultMessage;
    }

    private static string FormatFieldName(string fieldName)
    {
        // Convert PascalCase to readable format
        // e.g., "CountryId" -> "Country Id", "Iso2Code" -> "Iso2 Code"
        var result = System.Text.RegularExpressions.Regex.Replace(
            fieldName, 
            "([a-z])([A-Z])", 
            "$1 $2");
        
        return result.ToLowerInvariant();
    }
}
