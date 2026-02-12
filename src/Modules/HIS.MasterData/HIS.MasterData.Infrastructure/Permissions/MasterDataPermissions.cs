using System.ComponentModel;

namespace HIS.MasterData.Infrastructure.Permissions;

/// <summary>
/// Master Data module permissions.
/// </summary>
public static partial class MasterDataPermissions
{
    [DisplayName("Country Permissions")]
    [Description("Set permissions for country management operations.")]
    public static class Countries
    {
        [Description("Allows viewing country details.")]
        public const string View = "Permissions.Countries.View";

        [Description("Allows creating country records.")]
        public const string Create = "Permissions.Countries.Create";

        [Description("Allows editing country details.")]
        public const string Edit = "Permissions.Countries.Edit";

        [Description("Allows deleting country records.")]
        public const string Delete = "Permissions.Countries.Delete";

        [Description("Allows searching country records.")]
        public const string Search = "Permissions.Countries.Search";

        [Description("Allows exporting country data.")]
        public const string Export = "Permissions.Countries.Export";

        [Description("Allows importing country data.")]
        public const string Import = "Permissions.Countries.Import";
    }

    [DisplayName("Nationality Permissions")]
    [Description("Set permissions for nationality management operations.")]
    public static class Nationalities
    {
        [Description("Allows viewing nationality details.")]
        public const string View = "Permissions.Nationalities.View";

        [Description("Allows creating nationality records.")]
        public const string Create = "Permissions.Nationalities.Create";

        [Description("Allows editing nationality details.")]
        public const string Edit = "Permissions.Nationalities.Edit";

        [Description("Allows deleting nationality records.")]
        public const string Delete = "Permissions.Nationalities.Delete";

        [Description("Allows searching nationality records.")]
        public const string Search = "Permissions.Nationalities.Search";

        [Description("Allows exporting nationality data.")]
        public const string Export = "Permissions.Nationalities.Export";

        [Description("Allows importing nationality data.")]
        public const string Import = "Permissions.Nationalities.Import";
    }

    [DisplayName("City Permissions")]
    [Description("Set permissions for city management operations.")]
    public static class Cities
    {
        [Description("Allows viewing city details.")]
        public const string View = "Permissions.Cities.View";

        [Description("Allows creating city records.")]
        public const string Create = "Permissions.Cities.Create";

        [Description("Allows editing city details.")]
        public const string Edit = "Permissions.Cities.Edit";

        [Description("Allows deleting city records.")]
        public const string Delete = "Permissions.Cities.Delete";

        [Description("Allows searching city records.")]
        public const string Search = "Permissions.Cities.Search";

        [Description("Allows exporting city data.")]
        public const string Export = "Permissions.Cities.Export";

        [Description("Allows importing city data.")]
        public const string Import = "Permissions.Cities.Import";
    }

    [DisplayName("Blood Group Permissions")]
    [Description("Set permissions for blood group management operations.")]
    public static class BloodGroups
    {
        [Description("Allows viewing blood group details.")]
        public const string View = "Permissions.BloodGroups.View";

        [Description("Allows creating blood group records.")]
        public const string Create = "Permissions.BloodGroups.Create";

        [Description("Allows editing blood group details.")]
        public const string Edit = "Permissions.BloodGroups.Edit";

        [Description("Allows deleting blood group records.")]
        public const string Delete = "Permissions.BloodGroups.Delete";

        [Description("Allows searching blood group records.")]
        public const string Search = "Permissions.BloodGroups.Search";

        [Description("Allows exporting blood group data.")]
        public const string Export = "Permissions.BloodGroups.Export";

        [Description("Allows importing blood group data.")]
        public const string Import = "Permissions.BloodGroups.Import";
    }

    [DisplayName("Marital Status Permissions")]
    [Description("Set permissions for marital status management operations.")]
    public static class MaritalStatuses
    {
        [Description("Allows viewing marital status details.")]
        public const string View = "Permissions.MaritalStatuses.View";

        [Description("Allows creating marital status records.")]
        public const string Create = "Permissions.MaritalStatuses.Create";

        [Description("Allows editing marital status details.")]
        public const string Edit = "Permissions.MaritalStatuses.Edit";

        [Description("Allows deleting marital status records.")]
        public const string Delete = "Permissions.MaritalStatuses.Delete";

        [Description("Allows searching marital status records.")]
        public const string Search = "Permissions.MaritalStatuses.Search";

        [Description("Allows exporting marital status data.")]
        public const string Export = "Permissions.MaritalStatuses.Export";

        [Description("Allows importing marital status data.")]
        public const string Import = "Permissions.MaritalStatuses.Import";
    }

    [DisplayName("Bed Permissions")]
    [Description("Set permissions for bed management operations.")]
    public static class Beds
    {
        [Description("Allows viewing bed details.")]
        public const string View = "Permissions.Beds.View";

        [Description("Allows creating bed records.")]
        public const string Create = "Permissions.Beds.Create";

        [Description("Allows editing bed details.")]
        public const string Edit = "Permissions.Beds.Edit";

        [Description("Allows deleting bed records.")]
        public const string Delete = "Permissions.Beds.Delete";

        [Description("Allows searching bed records.")]
        public const string Search = "Permissions.Beds.Search";

        [Description("Allows exporting bed data.")]
        public const string Export = "Permissions.Beds.Export";

        [Description("Allows importing bed data.")]
        public const string Import = "Permissions.Beds.Import";

        [Description("Allows managing bed status.")]
        public const string ManageStatus = "Permissions.Beds.ManageStatus";
    }

    [DisplayName("Contact Permissions")]
    [Description("Set permissions for contact management operations.")]
    public static class Contacts
    {
        [Description("Allows viewing contact details.")]
        public const string View = "Permissions.Contacts.View";

        [Description("Allows creating contact records.")]
        public const string Create = "Permissions.Contacts.Create";

        [Description("Allows editing contact details.")]
        public const string Edit = "Permissions.Contacts.Edit";

        [Description("Allows deleting contact records.")]
        public const string Delete = "Permissions.Contacts.Delete";

        [Description("Allows searching contact records.")]
        public const string Search = "Permissions.Contacts.Search";

        [Description("Allows exporting contact data.")]
        public const string Export = "Permissions.Contacts.Export";

        [Description("Allows importing contact data.")]
        public const string Import = "Permissions.Contacts.Import";
    }

    [DisplayName("Department Permissions")]
    [Description("Set permissions for department management operations.")]
    public static class Departments
    {
        [Description("Allows viewing department details.")]
        public const string View = "Permissions.Departments.View";

        [Description("Allows creating department records.")]
        public const string Create = "Permissions.Departments.Create";

        [Description("Allows editing department details.")]
        public const string Edit = "Permissions.Departments.Edit";

        [Description("Allows deleting department records.")]
        public const string Delete = "Permissions.Departments.Delete";

        [Description("Allows searching department records.")]
        public const string Search = "Permissions.Departments.Search";

        [Description("Allows exporting department data.")]
        public const string Export = "Permissions.Departments.Export";

        [Description("Allows importing department data.")]
        public const string Import = "Permissions.Departments.Import";
    }

    [DisplayName("Location Permissions")]
    [Description("Set permissions for location management operations.")]
    public static class Locations
    {
        [Description("Allows viewing location details.")]
        public const string View = "Permissions.Locations.View";

        [Description("Allows creating location records.")]
        public const string Create = "Permissions.Locations.Create";

        [Description("Allows editing location details.")]
        public const string Edit = "Permissions.Locations.Edit";

        [Description("Allows deleting location records.")]
        public const string Delete = "Permissions.Locations.Delete";

        [Description("Allows searching location records.")]
        public const string Search = "Permissions.Locations.Search";

        [Description("Allows exporting location data.")]
        public const string Export = "Permissions.Locations.Export";

        [Description("Allows importing location data.")]
        public const string Import = "Permissions.Locations.Import";
    }

    [DisplayName("Specialty Permissions")]
    [Description("Set permissions for specialty management operations.")]
    public static class Specialties
    {
        [Description("Allows viewing specialty details.")]
        public const string View = "Permissions.Specialties.View";

        [Description("Allows creating specialty records.")]
        public const string Create = "Permissions.Specialties.Create";

        [Description("Allows editing specialty details.")]
        public const string Edit = "Permissions.Specialties.Edit";

        [Description("Allows deleting specialty records.")]
        public const string Delete = "Permissions.Specialties.Delete";

        [Description("Allows searching specialty records.")]
        public const string Search = "Permissions.Specialties.Search";

        [Description("Allows exporting specialty data.")]
        public const string Export = "Permissions.Specialties.Export";

        [Description("Allows importing specialty data.")]
        public const string Import = "Permissions.Specialties.Import";
    }

    [DisplayName("Shift Permissions")]
    [Description("Set permissions for shift management operations.")]
    public static class Shifts
    {
        [Description("Allows viewing shift details.")]
        public const string View = "Permissions.Shifts.View";

        [Description("Allows creating shift records.")]
        public const string Create = "Permissions.Shifts.Create";

        [Description("Allows editing shift details.")]
        public const string Edit = "Permissions.Shifts.Edit";

        [Description("Allows deleting shift records.")]
        public const string Delete = "Permissions.Shifts.Delete";

        [Description("Allows searching shift records.")]
        public const string Search = "Permissions.Shifts.Search";

        [Description("Allows exporting shift data.")]
        public const string Export = "Permissions.Shifts.Export";

        [Description("Allows importing shift data.")]
        public const string Import = "Permissions.Shifts.Import";
    }
}
