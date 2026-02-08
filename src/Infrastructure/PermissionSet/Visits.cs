using System.ComponentModel;

namespace CleanArchitecture.Blazor.Infrastructure.PermissionSet;

public static partial class Permissions
{
    [DisplayName("Visit Permissions")]
    [Description("Set permissions for visit management operations.")]
    public static class Visits
    {
        [Description("Allows viewing visit details.")]
        public const string View = "Permissions.Visits.View";

        [Description("Allows creating visit records.")]
        public const string Create = "Permissions.Visits.Create";

        [Description("Allows editing visit details.")]
        public const string Edit = "Permissions.Visits.Edit";

        [Description("Allows cancelling visits.")]
        public const string Cancel = "Permissions.Visits.Cancel";

        [Description("Allows searching visit records.")]
        public const string Search = "Permissions.Visits.Search";

        [Description("Allows exporting visit data.")]
        public const string Export = "Permissions.Visits.Export";

        [Description("Allows admitting patients.")]
        public const string Admit = "Permissions.Visits.Admit";

        [Description("Allows transferring patients.")]
        public const string Transfer = "Permissions.Visits.Transfer";

        [Description("Allows discharging patients.")]
        public const string Discharge = "Permissions.Visits.Discharge";
    }

    [DisplayName("Encounter Permissions")]
    [Description("Set permissions for encounter management operations.")]
    public static class Encounters
    {
        [Description("Allows viewing encounter details.")]
        public const string View = "Permissions.Encounters.View";

        [Description("Allows creating encounter records.")]
        public const string Create = "Permissions.Encounters.Create";

        [Description("Allows editing encounter details.")]
        public const string Edit = "Permissions.Encounters.Edit";

        [Description("Allows deleting encounter records.")]
        public const string Delete = "Permissions.Encounters.Delete";

        [Description("Allows searching encounter records.")]
        public const string Search = "Permissions.Encounters.Search";
    }
}
