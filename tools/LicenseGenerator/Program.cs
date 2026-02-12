using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using HIS.Core.Licensing;
using Spectre.Console;

namespace LicenseGenerator;

internal class Program
{
    private const string PrivateKeyFileName = "license_private_key.xml";
    private const string PublicKeyFileName = "license_public_key.xml";
    
    private static readonly string[] AllModules =
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
    ];

    static async Task Main(string[] args)
    {
        AnsiConsole.Write(new FigletText("HIS License Generator").Color(Color.Blue));
        AnsiConsole.MarkupLine("[bold yellow]Generate RSA-signed license files for hospital modules[/]\n");

        try
        {
            // Step 1: Ensure RSA keys exist
            await EnsureKeysExist();

            // Step 2: Gather license information
            var licenseInfo = GatherLicenseInformation();

            // Step 3: Generate license file
            var licenseFilePath = await GenerateLicenseFile(licenseInfo);

            AnsiConsole.MarkupLine($"\n[bold green]✓ License generated successfully![/]");
            AnsiConsole.MarkupLine($"[grey]License file: {licenseFilePath}[/]");
            AnsiConsole.MarkupLine($"[grey]Valid until: {licenseInfo.ExpiryDate:yyyy-MM-dd}[/]");
            AnsiConsole.MarkupLine($"[grey]Modules: {licenseInfo.Modules.Count}[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[bold red]✗ Error: {ex.Message}[/]");
            Environment.Exit(1);
        }
    }

    private static async Task EnsureKeysExist()
    {
        if (File.Exists(PrivateKeyFileName) && File.Exists(PublicKeyFileName))
        {
            AnsiConsole.MarkupLine("[green]✓ RSA key pair found[/]");
            return;
        }

        var generateKeys = AnsiConsole.Confirm(
            "[yellow]RSA key pair not found. Generate new keys?[/]",
            defaultValue: true);

        if (!generateKeys)
        {
            throw new InvalidOperationException("Cannot generate license without RSA keys.");
        }

        await Task.Run(() =>
        {
            AnsiConsole.Status()
                .Start("[yellow]Generating RSA-2048 key pair...[/]", ctx =>
                {
                    using var rsa = RSA.Create(2048);
                    
                    // Export private key (keep secure!)
                    var privateKey = rsa.ToXmlString(includePrivateParameters: true);
                    File.WriteAllText(PrivateKeyFileName, privateKey);
                    
                    // Export public key (distribute with application)
                    var publicKey = rsa.ToXmlString(includePrivateParameters: false);
                    File.WriteAllText(PublicKeyFileName, publicKey);
                });
        });

        AnsiConsole.MarkupLine("[green]✓ RSA key pair generated successfully[/]");
        AnsiConsole.MarkupLine($"[yellow]⚠ IMPORTANT:[/] Keep [bold]{PrivateKeyFileName}[/] secure!");
        AnsiConsole.MarkupLine($"[grey]Public key saved to: {PublicKeyFileName}[/]\n");
    }

    private static LicenseInfo GatherLicenseInformation()
    {
        var info = new LicenseInfo();

        // Customer name
        info.CustomerName = AnsiConsole.Ask<string>("[cyan]Customer name:[/]");

        // License type
        info.LicenseType = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[cyan]License type:[/]")
                .AddChoices("Production", "Trial", "Development", "Education"));

        // Max users
        info.MaxUsers = AnsiConsole.Ask(
            "[cyan]Maximum concurrent users:[/]",
            defaultValue: 10);

        // Expiry date
        var expiryDays = AnsiConsole.Prompt(
            new SelectionPrompt<int>()
                .Title("[cyan]License validity period:[/]")
                .AddChoices(30, 90, 180, 365, 730, 1825)
                .UseConverter(days => days switch
                {
                    30 => "30 days (1 month)",
                    90 => "90 days (3 months)",
                    180 => "180 days (6 months)",
                    365 => "1 year",
                    730 => "2 years",
                    1825 => "5 years",
                    _ => $"{days} days"
                }));

        info.ExpiryDate = DateTime.UtcNow.AddDays(expiryDays);

        // Module selection
        var selectedModules = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("[cyan]Select licensed modules:[/]")
                .PageSize(20)
                .Required()
                .InstructionsText("[grey](Press [blue]<space>[/] to select, [green]<enter>[/] to confirm)[/]")
                .AddChoiceGroup("All Modules", AllModules));

        info.Modules = selectedModules.ToList();

        // Summary
        DisplaySummary(info);

        var confirm = AnsiConsole.Confirm(
            "[yellow]Generate license with these settings?[/]",
            defaultValue: true);

        if (!confirm)
        {
            throw new OperationCanceledException("License generation cancelled by user.");
        }

        return info;
    }

    private static void DisplaySummary(LicenseInfo info)
    {
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.AddColumn("[bold]Setting[/]");
        table.AddColumn("[bold]Value[/]");

        table.AddRow("Customer", info.CustomerName);
        table.AddRow("License Type", info.LicenseType);
        table.AddRow("Max Users", info.MaxUsers.ToString());
        table.AddRow("Expires", info.ExpiryDate.ToString("yyyy-MM-dd"));
        table.AddRow("Modules", string.Join(", ", info.Modules.Select(m => m.Replace("HIS.", ""))));

        AnsiConsole.Write(new Panel(table).Header("[bold yellow]License Summary[/]"));
    }

    private static async Task<string> GenerateLicenseFile(LicenseInfo info)
    {
        return await Task.Run(() =>
        {
            // Create license payload
            var payload = new LicensePayload
            {
                CustomerName = info.CustomerName,
                LicenseType = info.LicenseType,
                IssuedDate = DateTime.UtcNow,
                ExpiryDate = info.ExpiryDate,
                MaxUsers = info.MaxUsers,
                LicensedModules = info.Modules.ToArray()
            };

            // Sign payload
            using var rsa = RSA.Create();
            var privateKey = File.ReadAllText(PrivateKeyFileName);
            rsa.FromXmlString(privateKey);

            var payloadJson = JsonSerializer.Serialize(payload, new JsonSerializerOptions 
            { 
                WriteIndented = false 
            });
            var payloadBytes = Encoding.UTF8.GetBytes(payloadJson);
            var signature = rsa.SignData(payloadBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            payload.Signature = Convert.ToBase64String(signature);

            // Save license file
            var fileName = $"license_{info.CustomerName.Replace(" ", "_")}_{DateTime.UtcNow:yyyyMMdd}.json";
            var licenseJson = JsonSerializer.Serialize(payload, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
            File.WriteAllText(fileName, licenseJson);

            return Path.GetFullPath(fileName);
        });
    }

    private class LicenseInfo
    {
        public string CustomerName { get; set; } = string.Empty;
        public string LicenseType { get; set; } = "Production";
        public int MaxUsers { get; set; }
        public DateTime ExpiryDate { get; set; }
        public List<string> Modules { get; set; } = new();
    }
}
