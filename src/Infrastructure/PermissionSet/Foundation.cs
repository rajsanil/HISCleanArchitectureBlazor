using System.ComponentModel;

namespace CleanArchitecture.Blazor.Infrastructure.PermissionSet;

public static partial class Permissions
{
    [DisplayName("Facility Permissions")]
    [Description("Set permissions for facility operations.")]
    public static class Facilities
    {
        [Description("Allows viewing facility details.")]
        public const string View = "Permissions.Facilities.View";

        [Description("Allows creating facility records.")]
        public const string Create = "Permissions.Facilities.Create";

        [Description("Allows editing facility details.")]
        public const string Edit = "Permissions.Facilities.Edit";

        [Description("Allows deleting facility records.")]
        public const string Delete = "Permissions.Facilities.Delete";

        [Description("Allows searching facility records.")]
        public const string Search = "Permissions.Facilities.Search";

        [Description("Allows exporting facility data.")]
        public const string Export = "Permissions.Facilities.Export";

        [Description("Allows importing facility data.")]
        public const string Import = "Permissions.Facilities.Import";
    }

    [DisplayName("Department Permissions")]
    [Description("Set permissions for department operations.")]
    public static class Departments
    {
        public const string View = "Permissions.Departments.View";
        public const string Create = "Permissions.Departments.Create";
        public const string Edit = "Permissions.Departments.Edit";
        public const string Delete = "Permissions.Departments.Delete";
        public const string Search = "Permissions.Departments.Search";
        public const string Export = "Permissions.Departments.Export";
    }

    [DisplayName("Bed Permissions")]
    [Description("Set permissions for bed management operations.")]
    public static class Beds
    {
        public const string View = "Permissions.Beds.View";
        public const string Create = "Permissions.Beds.Create";
        public const string Edit = "Permissions.Beds.Edit";
        public const string Delete = "Permissions.Beds.Delete";
        public const string Search = "Permissions.Beds.Search";
        public const string ManageStatus = "Permissions.Beds.ManageStatus";
    }

    [DisplayName("Staff Permissions")]
    [Description("Set permissions for staff management operations.")]
    public static class StaffMembers
    {
        public const string View = "Permissions.StaffMembers.View";
        public const string Create = "Permissions.StaffMembers.Create";
        public const string Edit = "Permissions.StaffMembers.Edit";
        public const string Delete = "Permissions.StaffMembers.Delete";
        public const string Search = "Permissions.StaffMembers.Search";
        public const string Export = "Permissions.StaffMembers.Export";
        public const string Import = "Permissions.StaffMembers.Import";
    }

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
}
