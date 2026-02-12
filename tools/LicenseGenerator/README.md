# HIS License Generator

A command-line tool for generating RSA-signed license files for the Hospital Information System (HIS) modular architecture.

## Overview

This tool creates cryptographically signed license files that control which HIS modules are available to specific customers. The license system uses **RSA-2048 digital signatures** to prevent tampering and ensure authenticity.

## Features

- **Interactive CLI**: Guided prompts for all license parameters using Spectre.Console
- **RSA Key Management**: Automatic generation and secure storage of RSA key pairs
- **Module Selection**: Multi-select interface for choosing licensed modules
- **Flexible Expiry**: Preset validity periods (30 days to 5 years)
- **License Types**: Production, Trial, Development, Education
- **Tamper-Proof**: Cryptographic signatures prevent modification

## Prerequisites

- .NET 10.0 SDK or later
- Windows, Linux, or macOS

## Building

```bash
cd tools/LicenseGenerator
dotnet build
```

## Usage

### Basic Usage

```bash
dotnet run
```

The tool will guide you through an interactive wizard:

1. **RSA Key Generation** (first run only)
   - Generates a 2048-bit RSA key pair
   - Saves `license_private_key.xml` (keep secure!)
   - Saves `license_public_key.xml` (distribute with app)

2. **Customer Information**
   - Customer name
   - License type (Production/Trial/Development/Education)
   - Maximum concurrent users

3. **Validity Period**
   - Choose from preset durations or custom days
   - License automatically expires after the selected period

4. **Module Selection**
   - Multi-select from all 16 HIS modules:
     - HIS.Core
     - HIS.MasterData
     - HIS.Foundation
     - HIS.Patient
     - HIS.Outpatient
     - HIS.Inpatient
     - HIS.Emergency
     - HIS.Clinical
     - HIS.Laboratory
     - HIS.Radiology
     - HIS.Pharmacy
     - HIS.Nursing
     - HIS.Inventory
     - HIS.Insurance
     - HIS.MRD
     - HIS.Support

5. **Confirmation**
   - Review summary
   - Confirm or cancel

6. **Output**
   - License file: `license_{CustomerName}_{Date}.json`
   - Ready to deploy to customer environment

### Example Session

```
╔════════════════════════════════════════════╗
║   HIS License Generator                     ║
╚════════════════════════════════════════════╝
Generate RSA-signed license files for hospital modules

✓ RSA key pair found

Customer name: City General Hospital
License type: Production
Maximum concurrent users: 50
License validity period: 1 year

Select licensed modules:
[X] HIS.Core
[X] HIS.MasterData
[X] HIS.Foundation
[X] HIS.Patient
[X] HIS.Outpatient
[ ] HIS.Inpatient
[ ] HIS.Emergency
[X] HIS.Clinical
[X] HIS.Laboratory
[ ] HIS.Radiology
[X] HIS.Pharmacy
[ ] HIS.Nursing
[ ] HIS.Inventory
[ ] HIS.Insurance
[ ] HIS.MRD
[ ] HIS.Support

╭─────────── License Summary ────────────╮
│ Customer     │ City General Hospital   │
│ License Type │ Production              │
│ Max Users    │ 50                      │
│ Expires      │ 2026-02-08             │
│ Modules      │ Core, MasterData, ...   │
╰────────────────────────────────────────╯

Generate license with these settings? (y/n): y

✓ License generated successfully!
License file: C:\...\license_City_General_Hospital_20250208.json
Valid until: 2026-02-08
Modules: 8
```

## License File Format

Generated licenses are JSON files with the following structure:

```json
{
  "customer": "City General Hospital",
  "licenseType": "Production",
  "issuedDate": "2025-02-08T10:30:00Z",
  "expirationDate": "2026-02-08T10:30:00Z",
  "maximumUsers": 50,
  "modules": [
    "HIS.Core",
    "HIS.MasterData",
    "HIS.Foundation",
    "HIS.Patient",
    "HIS.Outpatient",
    "HIS.Clinical",
    "HIS.Laboratory",
    "HIS.Pharmacy"
  ],
  "signature": "Base64EncodedRSASignature..."
}
```

## Deployment

### For Development Teams

1. **Generate Keys** (once per organization):
   ```bash
   dotnet run
   # Let the tool generate keys on first run
   ```

2. **Secure Private Key**:
   - Store `license_private_key.xml` in a secure location
   - **NEVER** commit to version control
   - Use a password manager or HSM for production
   - Back up securely

3. **Distribute Public Key**:
   - Copy `license_public_key.xml` to `src/Infrastructure/Resources/`
   - Commit to version control
   - Gets embedded in the application

### For Customers

1. Receive license file: `license_CustomerName_Date.json`
2. Copy to application directory: `Server.UI/license.json`
3. Restart application
4. Licensed modules will be available

## Security Considerations

### Private Key Security ⚠️

The `license_private_key.xml` file is **CRITICALLY SENSITIVE**:

- **DO NOT** commit to Git or any public repository
- **DO NOT** share via email or unsecured channels
- **DO NOT** store in cloud storage without encryption
- **DO** use hardware security modules (HSM) for production
- **DO** implement access controls and audit logging
- **DO** back up securely with encryption

### Key Rotation

To rotate keys (e.g., after compromise):

1. Generate new key pair:
   ```bash
   rm license_private_key.xml license_public_key.xml
   dotnet run
   ```

2. Update public key in application codebase
3. Regenerate all customer licenses with new key
4. Deploy updated application + new licenses

### Signature Verification

The application validates licenses by:
1. Reading `license.json`
2. Extracting signature
3. Verifying signature using embedded public key
4. Checking expiration date
5. Enabling only licensed modules

Tampered licenses will **fail signature verification** and be rejected.

## Troubleshooting

### "Cannot generate license without RSA keys"

- Run the tool and allow it to generate keys
- Or copy existing keys to the working directory

### "Signature verification failed" in application

- License file was modified after generation
- Public key in application doesn't match private key used for signing
- Regenerate license or update public key

### "License has expired"

- Generate new license with extended validity period
- Or update expiration date (requires regeneration)

## Advanced Usage

### Programmatic Generation

For automated license provisioning, reference the `HIS.Core.Licensing` library directly:

```csharp
using HIS.Core.Licensing;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

var payload = new LicensePayload
{
    Customer = "Automated Customer",
    LicenseType = "Production",
    IssuedDate = DateTime.UtcNow,
    ExpirationDate = DateTime.UtcNow.AddYears(1),
    MaximumUsers = 100,
    Modules = new[] { "HIS.Core", "HIS.MasterData", ... }
};

using var rsa = RSA.Create();
rsa.FromXmlString(File.ReadAllText("license_private_key.xml"));
var json = JsonSerializer.Serialize(payload);
var bytes = Encoding.UTF8.GetBytes(json);
var signature = rsa.SignData(bytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
payload.Signature = Convert.ToBase64String(signature);

File.WriteAllText("license.json", JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true }));
```

## License Types

- **Production**: For live hospital environments
- **Trial**: Time-limited evaluation licenses (30-90 days recommended)
- **Development**: For development teams (set `AllowUnlicensed: true` in app config)
- **Education**: For training and academic institutions

## Module Reference

| Module | Description | Dependencies |
|--------|-------------|--------------|
| HIS.Core | Core infrastructure | None |
| HIS.MasterData | Countries, cities, reference data | Core |
| HIS.Foundation | Facilities, departments, staff | Core, MasterData |
| HIS.Patient | Patient registration, demographics | Core, MasterData |
| HIS.Outpatient | OPD visits, appointments | Patient, Foundation |
| HIS.Inpatient | IPD admissions, bed management | Patient, Foundation |
| HIS.Emergency | ER triage, trauma care | Patient, Foundation |
| HIS.Clinical | Encounters, vitals, diagnoses | Patient |
| HIS.Laboratory | Lab orders, results | Clinical |
| HIS.Radiology | Imaging orders, PACS | Clinical |
| HIS.Pharmacy | Medications, dispensing | Clinical |
| HIS.Nursing | Nursing notes, care plans | Inpatient, Clinical |
| HIS.Inventory | Stock management | Foundation |
| HIS.Insurance | Claims, billing | Patient |
| HIS.MRD | Medical records | Patient, Clinical |
| HIS.Support | Help desk, tickets | Foundation |

## Support

For issues or questions:
- Check application logs for license validation errors
- Verify public key matches the one used for signing
- Ensure license file is valid JSON
- Contact HIS support team with license details (never share private key!)

---

**Version**: 1.0  
**Last Updated**: February 8, 2025  
**Maintained By**: HIS Development Team
