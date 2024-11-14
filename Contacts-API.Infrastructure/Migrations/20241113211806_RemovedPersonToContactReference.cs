using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactsAPI.Infrastructure.Migrations
{
	/// <inheritdoc />
	public partial class RemovedPersonToContactReference : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_Contacts_PersonId",
				table: "Contacts");

			migrationBuilder.CreateIndex(
				name: "IX_Contacts_PersonId",
				table: "Contacts",
				column: "PersonId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_Contacts_PersonId",
				table: "Contacts");

			migrationBuilder.CreateIndex(
				name: "IX_Contacts_PersonId",
				table: "Contacts",
				column: "PersonId",
				unique: true);
		}
	}
}
