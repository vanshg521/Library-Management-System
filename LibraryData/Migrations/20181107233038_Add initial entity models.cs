using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryData.Migrations
{
    public partial class Addinitialentitymodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HomeLibraryBranchId",
                table: "Patrons",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LibraryCardId",
                table: "Patrons",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "libraryBranches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Address = table.Column<string>(nullable: false),
                    Telephone = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    OpenDate = table.Column<DateTime>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_libraryBranches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "libraryCards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Fees = table.Column<decimal>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_libraryCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "statuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BranchHours",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<int>(nullable: true),
                    DayOfWeek = table.Column<int>(nullable: false),
                    OpenTime = table.Column<int>(nullable: false),
                    CloseTime = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchHours_libraryBranches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "libraryBranches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "libraryAssets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    Cost = table.Column<decimal>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    NumberOfCopies = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    ISBN = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    DeweyIndex = table.Column<string>(nullable: true),
                    Director = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_libraryAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_libraryAssets_libraryBranches_LocationId",
                        column: x => x.LocationId,
                        principalTable: "libraryBranches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_libraryAssets_statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "checkoutHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LibraryAssetId = table.Column<int>(nullable: false),
                    LibraryCardId = table.Column<int>(nullable: false),
                    CheckOut = table.Column<DateTime>(nullable: false),
                    CheckIn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checkoutHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_checkoutHistories_libraryAssets_LibraryAssetId",
                        column: x => x.LibraryAssetId,
                        principalTable: "libraryAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_checkoutHistories_libraryCards_LibraryCardId",
                        column: x => x.LibraryCardId,
                        principalTable: "libraryCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "checkouts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    libraryAssetId = table.Column<int>(nullable: false),
                    libraryCardId = table.Column<int>(nullable: true),
                    Since = table.Column<DateTime>(nullable: false),
                    Until = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checkouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_checkouts_libraryAssets_libraryAssetId",
                        column: x => x.libraryAssetId,
                        principalTable: "libraryAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_checkouts_libraryCards_libraryCardId",
                        column: x => x.libraryCardId,
                        principalTable: "libraryCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Holds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LibraryAssetId = table.Column<int>(nullable: true),
                    LibraryCardId = table.Column<int>(nullable: true),
                    HoldPlaced = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Holds_libraryAssets_LibraryAssetId",
                        column: x => x.LibraryAssetId,
                        principalTable: "libraryAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Holds_libraryCards_LibraryCardId",
                        column: x => x.LibraryCardId,
                        principalTable: "libraryCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patrons_HomeLibraryBranchId",
                table: "Patrons",
                column: "HomeLibraryBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Patrons_LibraryCardId",
                table: "Patrons",
                column: "LibraryCardId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchHours_BranchId",
                table: "BranchHours",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_checkoutHistories_LibraryAssetId",
                table: "checkoutHistories",
                column: "LibraryAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_checkoutHistories_LibraryCardId",
                table: "checkoutHistories",
                column: "LibraryCardId");

            migrationBuilder.CreateIndex(
                name: "IX_checkouts_libraryAssetId",
                table: "checkouts",
                column: "libraryAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_checkouts_libraryCardId",
                table: "checkouts",
                column: "libraryCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Holds_LibraryAssetId",
                table: "Holds",
                column: "LibraryAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Holds_LibraryCardId",
                table: "Holds",
                column: "LibraryCardId");

            migrationBuilder.CreateIndex(
                name: "IX_libraryAssets_LocationId",
                table: "libraryAssets",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_libraryAssets_StatusId",
                table: "libraryAssets",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patrons_libraryBranches_HomeLibraryBranchId",
                table: "Patrons",
                column: "HomeLibraryBranchId",
                principalTable: "libraryBranches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patrons_libraryCards_LibraryCardId",
                table: "Patrons",
                column: "LibraryCardId",
                principalTable: "libraryCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patrons_libraryBranches_HomeLibraryBranchId",
                table: "Patrons");

            migrationBuilder.DropForeignKey(
                name: "FK_Patrons_libraryCards_LibraryCardId",
                table: "Patrons");

            migrationBuilder.DropTable(
                name: "BranchHours");

            migrationBuilder.DropTable(
                name: "checkoutHistories");

            migrationBuilder.DropTable(
                name: "checkouts");

            migrationBuilder.DropTable(
                name: "Holds");

            migrationBuilder.DropTable(
                name: "libraryAssets");

            migrationBuilder.DropTable(
                name: "libraryCards");

            migrationBuilder.DropTable(
                name: "libraryBranches");

            migrationBuilder.DropTable(
                name: "statuses");

            migrationBuilder.DropIndex(
                name: "IX_Patrons_HomeLibraryBranchId",
                table: "Patrons");

            migrationBuilder.DropIndex(
                name: "IX_Patrons_LibraryCardId",
                table: "Patrons");

            migrationBuilder.DropColumn(
                name: "HomeLibraryBranchId",
                table: "Patrons");

            migrationBuilder.DropColumn(
                name: "LibraryCardId",
                table: "Patrons");
        }
    }
}
