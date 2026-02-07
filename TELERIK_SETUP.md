# Telerik Integration Setup Instructions

## Prerequisites
- Telerik UI for Blazor license (license file already present: `telerik-license.txt`)
- Telerik account credentials

## Setup Steps

### 1. Configure Telerik NuGet Source

**Option A: Update nuget.config manually**
1. Open `nuget.config` in the root directory
2. Replace `YOUR_TELERIK_EMAIL` with your Telerik account email
3. Replace `YOUR_TELERIK_PASSWORD` with your Telerik account password

**Option B: Use CLI command**
```bash
dotnet nuget add source https://nuget.telerik.com/v3/index.json \
  --name telerik \
  --username r.sanil@csh.ae \
  --password sanil0104\
  --store-password-in-clear-text
```

### 2. Restore NuGet Packages
```bash
dotnet restore
```

### 3. Build the Project
```bash
dotnet build src/Server.UI/Server.UI.csproj
```

### 4. Run the Application
```bash
dotnet run --project src/Server.UI
```

### 5. Access Telerik Data Page
Navigate to: `https://localhost:7152/pages/telerikdata`

## What's Been Configured

✅ **Telerik.UI.for.Blazor** package added to Server.UI.csproj
✅ **Telerik services** registered in DependencyInjection.cs
✅ **Global using statements** added for Telerik components
✅ **Namespace aliases** configured to resolve conflicts with MudBlazor:
   - `TelerikGridState` for Telerik Grid State
   - `MudGridState` for MudBlazor Grid State
✅ **TelerikData page** created with sample grid
✅ **Menu navigation** added under E-Commerce section
✅ **Theme integration** - Telerik grid styled to match MudBlazor theme

## Files Modified/Created

- ✏️ `src/Server.UI/Server.UI.csproj` - Added Telerik package
- ✏️ `src/Server.UI/DependencyInjection.cs` - Registered Telerik services
- ✏️ `src/Server.UI/_Imports.razor` - Added Telerik usings and aliases
- ✏️ `src/Server.UI/Services/Navigation/MenuService.cs` - Added menu item
- ➕ `src/Server.UI/Models/TelerikDataModel.cs` - Sample data model
- ➕ `src/Server.UI/Pages/TelerikData/TelerikData.razor` - Telerik grid page
- ➕ `nuget.config` - NuGet source configuration

## License Activation

The Telerik license from `telerik-license.txt` will be automatically used by the Telerik components when the application runs.

## Troubleshooting

### Error: NU1301 Unable to load the service index
**Solution**: Ensure your Telerik credentials are correctly set in `nuget.config`

### Error: GridState ambiguous reference
**Solution**: Already fixed - use `TelerikGridState` or `MudGridState` type aliases

### Build Errors
**Solution**: Run `dotnet clean` then `dotnet restore` then `dotnet build`

## Features of the Telerik Data Page

- ✨ Full CRUD operations (Create, Read, Update, Delete)
- 📊 Sorting on all columns
- 🔍 Filtering with filter row
- 📄 Pagination (15 items per page)
- ✅ Multi-row selection
- 🎨 Themed to match MudBlazor's design system
- 📱 Responsive layout
- 🔄 Refresh functionality
- 📤 Export button (ready for implementation)

## Next Steps

1. Add your Telerik credentials to `nuget.config`
2. Run `dotnet restore`
3. Build and run the application
4. Customize the TelerikData page for your specific needs
5. Implement export functionality if needed
6. Add real data sources instead of sample data

## Support

For Telerik-specific issues, visit: https://www.telerik.com/support
For project issues, check the main README.md
