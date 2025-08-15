using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OfflineTicketingSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("65258150-e28c-44e5-9dc5-a1586771f664"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d07443a3-87d2-4156-9417-abb8591bd56a"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("65258150-e28c-44e5-9dc5-a1586771f664"), 0, "df114fa2-0127-4789-961b-c8bd0b6582a3", "admin@bargheto.com", false, "Admin User", false, null, null, null, "$2a$11$0rK0qFavEUsbtfetjYl6gO3dFDAQoGA9PYxdCXp0RmSrDpizi7.Te", null, false, 1, null, false, null },
                    { new Guid("d07443a3-87d2-4156-9417-abb8591bd56a"), 0, "c8e67ae4-bd82-4089-a419-302c5e572585", "p.sharifi@bargheto.com", false, "Peyman Sharifi", false, null, null, null, "$2a$11$C64AbzPVsZpJRWWl7kr7/.UeyCU9vngMJso2F92GF1JHyb4dkgn5W", null, false, 0, null, false, null }
                });
        }
    }
}
