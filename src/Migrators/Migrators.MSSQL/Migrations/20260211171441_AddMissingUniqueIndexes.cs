using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Blazor.Migrators.MSSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_Beds_BedId",
                table: "Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_Locations_LocationId",
                table: "Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Encounters_Departments_DepartmentId",
                table: "Encounters");

            migrationBuilder.DropForeignKey(
                name: "FK_Encounters_Locations_LocationId",
                table: "Encounters");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_BloodGroups_BloodGroupId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Cities_CityId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Countries_CountryId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_MaritalStatuses_MaritalStatusId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Nationalities_NationalityId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Locations_LocationId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Staff_Departments_DepartmentId",
                table: "Staff");

            migrationBuilder.DropForeignKey(
                name: "FK_Staff_Specialties_SpecialtyId",
                table: "Staff");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Beds_FromBedId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Beds_ToBedId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Locations_FromLocationId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Locations_ToLocationId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Visits_Departments_DepartmentId",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Visits_DepartmentId",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_FromBedId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_FromLocationId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_ToBedId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_ToLocationId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Staff_DepartmentId",
                table: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Staff_SpecialtyId",
                table: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Patients_BloodGroupId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_CityId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_CountryId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_MaritalStatusId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_NationalityId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Encounters_DepartmentId",
                table: "Encounters");

            migrationBuilder.DropIndex(
                name: "IX_Encounters_LocationId",
                table: "Encounters");

            migrationBuilder.DropIndex(
                name: "IX_Admissions_BedId",
                table: "Admissions");

            migrationBuilder.DropIndex(
                name: "IX_Admissions_LocationId",
                table: "Admissions");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Contacts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Contacts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Contacts",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Contacts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Contacts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BedStatus",
                table: "Beds",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValue: "Available");

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_Name",
                table: "Specialties",
                column: "Name",
                unique: true,
                filter: "[Deleted] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_Name",
                table: "Nationalities",
                column: "Name",
                unique: true,
                filter: "[Deleted] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MaritalStatuses_Name",
                table: "MaritalStatuses",
                column: "Name",
                unique: true,
                filter: "[Deleted] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Name_FacilityId",
                table: "Locations",
                columns: new[] { "Name", "FacilityId" },
                unique: true,
                filter: "[Deleted] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Name_FacilityId",
                table: "Departments",
                columns: new[] { "Name", "FacilityId" },
                unique: true,
                filter: "[Deleted] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Iso3Code",
                table: "Countries",
                column: "Iso3Code",
                unique: true,
                filter: "[Deleted] IS NULL AND [Iso3Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Name",
                table: "Countries",
                column: "Name",
                unique: true,
                filter: "[Deleted] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Email",
                table: "Contacts",
                column: "Email",
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_Name_CountryId",
                table: "Cities",
                columns: new[] { "Name", "CountryId" },
                unique: true,
                filter: "[Deleted] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BloodGroups_Name",
                table: "BloodGroups",
                column: "Name",
                unique: true,
                filter: "[Deleted] IS NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Locations_LocationId",
                table: "Rooms",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Locations_LocationId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Specialties_Name",
                table: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_Nationalities_Name",
                table: "Nationalities");

            migrationBuilder.DropIndex(
                name: "IX_MaritalStatuses_Name",
                table: "MaritalStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Locations_Name_FacilityId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Departments_Name_FacilityId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Countries_Iso3Code",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_Name",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_Email",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Cities_Name_CountryId",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_BloodGroups_Name",
                table: "BloodGroups");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Contacts",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Contacts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Contacts",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Contacts",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Contacts",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BedStatus",
                table: "Beds",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "Available",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.CreateIndex(
                name: "IX_Visits_DepartmentId",
                table: "Visits",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_FromBedId",
                table: "Transfers",
                column: "FromBedId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_FromLocationId",
                table: "Transfers",
                column: "FromLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_ToBedId",
                table: "Transfers",
                column: "ToBedId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_ToLocationId",
                table: "Transfers",
                column: "ToLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_DepartmentId",
                table: "Staff",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_SpecialtyId",
                table: "Staff",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_BloodGroupId",
                table: "Patients",
                column: "BloodGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_CityId",
                table: "Patients",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_CountryId",
                table: "Patients",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_MaritalStatusId",
                table: "Patients",
                column: "MaritalStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_NationalityId",
                table: "Patients",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_DepartmentId",
                table: "Encounters",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_LocationId",
                table: "Encounters",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_BedId",
                table: "Admissions",
                column: "BedId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_LocationId",
                table: "Admissions",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_Beds_BedId",
                table: "Admissions",
                column: "BedId",
                principalTable: "Beds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_Locations_LocationId",
                table: "Admissions",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Encounters_Departments_DepartmentId",
                table: "Encounters",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Encounters_Locations_LocationId",
                table: "Encounters",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_BloodGroups_BloodGroupId",
                table: "Patients",
                column: "BloodGroupId",
                principalTable: "BloodGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Cities_CityId",
                table: "Patients",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Countries_CountryId",
                table: "Patients",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_MaritalStatuses_MaritalStatusId",
                table: "Patients",
                column: "MaritalStatusId",
                principalTable: "MaritalStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Nationalities_NationalityId",
                table: "Patients",
                column: "NationalityId",
                principalTable: "Nationalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Locations_LocationId",
                table: "Rooms",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_Departments_DepartmentId",
                table: "Staff",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_Specialties_SpecialtyId",
                table: "Staff",
                column: "SpecialtyId",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Beds_FromBedId",
                table: "Transfers",
                column: "FromBedId",
                principalTable: "Beds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Beds_ToBedId",
                table: "Transfers",
                column: "ToBedId",
                principalTable: "Beds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Locations_FromLocationId",
                table: "Transfers",
                column: "FromLocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Locations_ToLocationId",
                table: "Transfers",
                column: "ToLocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Departments_DepartmentId",
                table: "Visits",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
