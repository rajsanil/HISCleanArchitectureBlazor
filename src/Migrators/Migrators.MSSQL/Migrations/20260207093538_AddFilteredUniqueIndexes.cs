using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Blazor.Migrators.MSSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddFilteredUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Visits_VisitNumber",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Staff_EmployeeCode",
                table: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Specialties_Code",
                table: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_Code_LocationId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Patients_MRN",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Locations_Code_FacilityId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Facilities_Code",
                table: "Facilities");

            migrationBuilder.DropIndex(
                name: "IX_Encounters_EncounterNumber",
                table: "Encounters");

            migrationBuilder.DropIndex(
                name: "IX_Departments_Code_FacilityId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Beds_Code_RoomId",
                table: "Beds");

            migrationBuilder.DropIndex(
                name: "IX_Admissions_VisitId",
                table: "Admissions");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_VisitNumber",
                table: "Visits",
                column: "VisitNumber",
                unique: true,
                filter: "[Deleted] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_EmployeeCode",
                table: "Staff",
                column: "EmployeeCode",
                unique: true,
                filter: "[Deleted] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_Code",
                table: "Specialties",
                column: "Code",
                unique: true,
                filter: "[Deleted] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_Code_LocationId",
                table: "Rooms",
                columns: new[] { "Code", "LocationId" },
                unique: true,
                filter: "[Deleted] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_MRN",
                table: "Patients",
                column: "MRN",
                unique: true,
                filter: "[Deleted] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Code_FacilityId",
                table: "Locations",
                columns: new[] { "Code", "FacilityId" },
                unique: true,
                filter: "[Deleted] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_Code",
                table: "Facilities",
                column: "Code",
                unique: true,
                filter: "[Deleted] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_EncounterNumber",
                table: "Encounters",
                column: "EncounterNumber",
                unique: true,
                filter: "[Deleted] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Code_FacilityId",
                table: "Departments",
                columns: new[] { "Code", "FacilityId" },
                unique: true,
                filter: "[Deleted] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Beds_Code_RoomId",
                table: "Beds",
                columns: new[] { "Code", "RoomId" },
                unique: true,
                filter: "[Deleted] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_VisitId",
                table: "Admissions",
                column: "VisitId",
                unique: true,
                filter: "[Deleted] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Visits_VisitNumber",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Staff_EmployeeCode",
                table: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Specialties_Code",
                table: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_Code_LocationId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Patients_MRN",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Locations_Code_FacilityId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Facilities_Code",
                table: "Facilities");

            migrationBuilder.DropIndex(
                name: "IX_Encounters_EncounterNumber",
                table: "Encounters");

            migrationBuilder.DropIndex(
                name: "IX_Departments_Code_FacilityId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Beds_Code_RoomId",
                table: "Beds");

            migrationBuilder.DropIndex(
                name: "IX_Admissions_VisitId",
                table: "Admissions");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_VisitNumber",
                table: "Visits",
                column: "VisitNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Staff_EmployeeCode",
                table: "Staff",
                column: "EmployeeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_Code",
                table: "Specialties",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_Code_LocationId",
                table: "Rooms",
                columns: new[] { "Code", "LocationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_MRN",
                table: "Patients",
                column: "MRN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Code_FacilityId",
                table: "Locations",
                columns: new[] { "Code", "FacilityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_Code",
                table: "Facilities",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_EncounterNumber",
                table: "Encounters",
                column: "EncounterNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Code_FacilityId",
                table: "Departments",
                columns: new[] { "Code", "FacilityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Beds_Code_RoomId",
                table: "Beds",
                columns: new[] { "Code", "RoomId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_VisitId",
                table: "Admissions",
                column: "VisitId",
                unique: true);
        }
    }
}
