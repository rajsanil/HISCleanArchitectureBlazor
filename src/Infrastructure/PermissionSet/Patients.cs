using System.ComponentModel;

namespace CleanArchitecture.Blazor.Infrastructure.PermissionSet;

public static partial class Permissions
{
    [DisplayName("Patient Permissions")]
    [Description("Set permissions for patient management operations.")]
    public static class Patients
    {
        [Description("Allows viewing patient details.")]
        public const string View = "Permissions.Patients.View";

        [Description("Allows creating patient records.")]
        public const string Create = "Permissions.Patients.Create";

        [Description("Allows editing patient details.")]
        public const string Edit = "Permissions.Patients.Edit";

        [Description("Allows deleting patient records.")]
        public const string Delete = "Permissions.Patients.Delete";

        [Description("Allows searching patient records.")]
        public const string Search = "Permissions.Patients.Search";

        [Description("Allows exporting patient data.")]
        public const string Export = "Permissions.Patients.Export";

        [Description("Allows importing patient data.")]
        public const string Import = "Permissions.Patients.Import";

        [Description("Allows viewing VIP patient details.")]
        public const string ViewVIP = "Permissions.Patients.ViewVIP";
    }
}
