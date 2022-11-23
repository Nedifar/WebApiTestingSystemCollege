using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace webApiipAweb.Migrations
{
    public partial class onNew1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Achivments",
                columns: table => new
                {
                    idAchivment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Term = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achivments", x => x.idAchivment);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    firstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    middlleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    levelStuding = table.Column<int>(type: "int", nullable: false),
                    point = table.Column<double>(type: "float", nullable: false),
                    spendPoint = table.Column<double>(type: "float", nullable: false),
                    passRecoveryCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    imagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LevelStudings",
                columns: table => new
                {
                    idLevelStuding = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nameLevel = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelStudings", x => x.idLevelStuding);
                });

            migrationBuilder.CreateTable(
                name: "ThingPacks",
                columns: table => new
                {
                    idThingPack = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    namePack = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    imageOfPack = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThingPacks", x => x.idThingPack);
                });

            migrationBuilder.CreateTable(
                name: "TypeAppeals",
                columns: table => new
                {
                    idTypeAppeal = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    typeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeAppeals", x => x.idTypeAppeal);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AchivmentExecutions",
                columns: table => new
                {
                    idAchivmentExecution = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idAchivment = table.Column<int>(type: "int", nullable: false),
                    ChildId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchivmentExecutions", x => x.idAchivmentExecution);
                    table.ForeignKey(
                        name: "FK_AchivmentExecutions_Achivments_idAchivment",
                        column: x => x.idAchivment,
                        principalTable: "Achivments",
                        principalColumn: "idAchivment",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AchivmentExecutions_AspNetUsers_ChildId",
                        column: x => x.ChildId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LevelStudingExecutions",
                columns: table => new
                {
                    idLevelStudingExecution = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChildId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    idLevelStuding = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelStudingExecutions", x => x.idLevelStudingExecution);
                    table.ForeignKey(
                        name: "FK_LevelStudingExecutions_AspNetUsers_ChildId",
                        column: x => x.ChildId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LevelStudingExecutions_LevelStudings_idLevelStuding",
                        column: x => x.idLevelStuding,
                        principalTable: "LevelStudings",
                        principalColumn: "idLevelStuding",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    idSubject = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nameSubject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idLevelStuding = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.idSubject);
                    table.ForeignKey(
                        name: "FK_Subjects_LevelStudings_idLevelStuding",
                        column: x => x.idLevelStuding,
                        principalTable: "LevelStudings",
                        principalColumn: "idLevelStuding",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThingPackExecutions",
                columns: table => new
                {
                    idThingPackExecution = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChildId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    idThingPack = table.Column<int>(type: "int", nullable: false),
                    isCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThingPackExecutions", x => x.idThingPackExecution);
                    table.ForeignKey(
                        name: "FK_ThingPackExecutions_AspNetUsers_ChildId",
                        column: x => x.ChildId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThingPackExecutions_ThingPacks_idThingPack",
                        column: x => x.idThingPack,
                        principalTable: "ThingPacks",
                        principalColumn: "idThingPack",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Things",
                columns: table => new
                {
                    idThing = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idthingPack = table.Column<int>(type: "int", nullable: false),
                    xPosition = table.Column<double>(type: "float", nullable: false),
                    yPosition = table.Column<double>(type: "float", nullable: false),
                    urlImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Things", x => x.idThing);
                    table.ForeignKey(
                        name: "FK_Things_ThingPacks_idthingPack",
                        column: x => x.idthingPack,
                        principalTable: "ThingPacks",
                        principalColumn: "idThingPack",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appeals",
                columns: table => new
                {
                    idAppeal = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChildId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    dateAppeal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    idTypeAppeal = table.Column<int>(type: "int", nullable: false),
                    textAppeal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<byte>(type: "tinyint", nullable: false),
                    inArchive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appeals", x => x.idAppeal);
                    table.ForeignKey(
                        name: "FK_Appeals_AspNetUsers_ChildId",
                        column: x => x.ChildId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appeals_TypeAppeals_idTypeAppeal",
                        column: x => x.idTypeAppeal,
                        principalTable: "TypeAppeals",
                        principalColumn: "idTypeAppeal",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chapters",
                columns: table => new
                {
                    idChapter = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idSubject = table.Column<int>(type: "int", nullable: false),
                    award = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapters", x => x.idChapter);
                    table.ForeignKey(
                        name: "FK_Chapters_Subjects_idSubject",
                        column: x => x.idSubject,
                        principalTable: "Subjects",
                        principalColumn: "idSubject",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectExecutions",
                columns: table => new
                {
                    idSubjectExecution = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idSubject = table.Column<int>(type: "int", nullable: true),
                    idLevelStudingExecution = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectExecutions", x => x.idSubjectExecution);
                    table.ForeignKey(
                        name: "FK_SubjectExecutions_LevelStudingExecutions_idLevelStudingExecution",
                        column: x => x.idLevelStudingExecution,
                        principalTable: "LevelStudingExecutions",
                        principalColumn: "idLevelStudingExecution",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectExecutions_Subjects_idSubject",
                        column: x => x.idSubject,
                        principalTable: "Subjects",
                        principalColumn: "idSubject",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubjectThingPack",
                columns: table => new
                {
                    SubjectsidSubject = table.Column<int>(type: "int", nullable: false),
                    ThingPacksidThingPack = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectThingPack", x => new { x.SubjectsidSubject, x.ThingPacksidThingPack });
                    table.ForeignKey(
                        name: "FK_SubjectThingPack_Subjects_SubjectsidSubject",
                        column: x => x.SubjectsidSubject,
                        principalTable: "Subjects",
                        principalColumn: "idSubject",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectThingPack_ThingPacks_ThingPacksidThingPack",
                        column: x => x.ThingPacksidThingPack,
                        principalTable: "ThingPacks",
                        principalColumn: "idThingPack",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThingExecutions",
                columns: table => new
                {
                    idThingExecution = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idThing = table.Column<int>(type: "int", nullable: false),
                    isFinished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThingExecutions", x => x.idThingExecution);
                    table.ForeignKey(
                        name: "FK_ThingExecutions_Things_idThing",
                        column: x => x.idThing,
                        principalTable: "Things",
                        principalColumn: "idThing",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestPacks",
                columns: table => new
                {
                    idTestPack = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    header = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idChapter = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestPacks", x => x.idTestPack);
                    table.ForeignKey(
                        name: "FK_TestPacks_Chapters_idChapter",
                        column: x => x.idChapter,
                        principalTable: "Chapters",
                        principalColumn: "idChapter",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TheoreticalMaterial",
                columns: table => new
                {
                    idTheoreticalMaterial = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    header = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idChapter = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheoreticalMaterial", x => x.idTheoreticalMaterial);
                    table.ForeignKey(
                        name: "FK_TheoreticalMaterial_Chapters_idChapter",
                        column: x => x.idChapter,
                        principalTable: "Chapters",
                        principalColumn: "idChapter",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChapterExecutions",
                columns: table => new
                {
                    idChapterExecution = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idChapter = table.Column<int>(type: "int", nullable: false),
                    idSubjectExecution = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterExecutions", x => x.idChapterExecution);
                    table.ForeignKey(
                        name: "FK_ChapterExecutions_Chapters_idChapter",
                        column: x => x.idChapter,
                        principalTable: "Chapters",
                        principalColumn: "idChapter",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChapterExecutions_SubjectExecutions_idSubjectExecution",
                        column: x => x.idSubjectExecution,
                        principalTable: "SubjectExecutions",
                        principalColumn: "idSubjectExecution",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ThingExecutionThingPackExecution",
                columns: table => new
                {
                    ThingExecutionsidThingExecution = table.Column<int>(type: "int", nullable: false),
                    ThingPackExecutionsidThingPackExecution = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThingExecutionThingPackExecution", x => new { x.ThingExecutionsidThingExecution, x.ThingPackExecutionsidThingPackExecution });
                    table.ForeignKey(
                        name: "FK_ThingExecutionThingPackExecution_ThingExecutions_ThingExecutionsidThingExecution",
                        column: x => x.ThingExecutionsidThingExecution,
                        principalTable: "ThingExecutions",
                        principalColumn: "idThingExecution",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThingExecutionThingPackExecution_ThingPackExecutions_ThingPackExecutionsidThingPackExecution",
                        column: x => x.ThingPackExecutionsidThingPackExecution,
                        principalTable: "ThingPackExecutions",
                        principalColumn: "idThingPackExecution",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskWithOpenAnsws",
                columns: table => new
                {
                    idTaskWithOpenAnsw = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    textQuestion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    answear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    theme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idTestPack = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskWithOpenAnsws", x => x.idTaskWithOpenAnsw);
                    table.ForeignKey(
                        name: "FK_TaskWithOpenAnsws_TestPacks_idTestPack",
                        column: x => x.idTestPack,
                        principalTable: "TestPacks",
                        principalColumn: "idTestPack",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestTasks",
                columns: table => new
                {
                    idTestTask = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    textQuestion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idTestPack = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestTasks", x => x.idTestTask);
                    table.ForeignKey(
                        name: "FK_TestTasks_TestPacks_idTestPack",
                        column: x => x.idTestPack,
                        principalTable: "TestPacks",
                        principalColumn: "idTestPack",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestPackExecutions",
                columns: table => new
                {
                    idTestPackExecution = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idChapterExecution = table.Column<int>(type: "int", nullable: false),
                    idTestPack = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestPackExecutions", x => x.idTestPackExecution);
                    table.ForeignKey(
                        name: "FK_TestPackExecutions_ChapterExecutions_idChapterExecution",
                        column: x => x.idChapterExecution,
                        principalTable: "ChapterExecutions",
                        principalColumn: "idChapterExecution",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestPackExecutions_TestPacks_idTestPack",
                        column: x => x.idTestPack,
                        principalTable: "TestPacks",
                        principalColumn: "idTestPack",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Solutions",
                columns: table => new
                {
                    idSolution = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idTaskWithOpenAnsw = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solutions", x => x.idSolution);
                    table.ForeignKey(
                        name: "FK_Solutions_TaskWithOpenAnsws_idTaskWithOpenAnsw",
                        column: x => x.idTaskWithOpenAnsw,
                        principalTable: "TaskWithOpenAnsws",
                        principalColumn: "idTaskWithOpenAnsw",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswearOnTasks",
                columns: table => new
                {
                    idAnswearOnTask = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    textAnswear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accuracy = table.Column<bool>(type: "bit", nullable: false),
                    idTestTask = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswearOnTasks", x => x.idAnswearOnTask);
                    table.ForeignKey(
                        name: "FK_AnswearOnTasks_TestTasks_idTestTask",
                        column: x => x.idTestTask,
                        principalTable: "TestTasks",
                        principalColumn: "idTestTask",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskWithOpenAnswsExecutions",
                columns: table => new
                {
                    idTaskWithOpenAnswsExecution = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idTestPackExecution = table.Column<int>(type: "int", nullable: true),
                    idTaskWithOpenAnsws = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    timeExecutionInSecond = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskWithOpenAnswsExecutions", x => x.idTaskWithOpenAnswsExecution);
                    table.ForeignKey(
                        name: "FK_TaskWithOpenAnswsExecutions_TaskWithOpenAnsws_idTaskWithOpenAnsws",
                        column: x => x.idTaskWithOpenAnsws,
                        principalTable: "TaskWithOpenAnsws",
                        principalColumn: "idTaskWithOpenAnsw",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskWithOpenAnswsExecutions_TestPackExecutions_idTestPackExecution",
                        column: x => x.idTestPackExecution,
                        principalTable: "TestPackExecutions",
                        principalColumn: "idTestPackExecution",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TryingTestTasks",
                columns: table => new
                {
                    idTryingTestTask = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    result = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idTestPackExecution = table.Column<int>(type: "int", nullable: false),
                    timeExecutionInSecond = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TryingTestTasks", x => x.idTryingTestTask);
                    table.ForeignKey(
                        name: "FK_TryingTestTasks_TestPackExecutions_idTestPackExecution",
                        column: x => x.idTestPackExecution,
                        principalTable: "TestPackExecutions",
                        principalColumn: "idTestPackExecution",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestTaskExecutions",
                columns: table => new
                {
                    idTestTaskExecution = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idTestTask = table.Column<int>(type: "int", nullable: false),
                    idTryingTestTask = table.Column<int>(type: "int", nullable: false),
                    idAnswearOnTask = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestTaskExecutions", x => x.idTestTaskExecution);
                    table.ForeignKey(
                        name: "FK_TestTaskExecutions_AnswearOnTasks_idAnswearOnTask",
                        column: x => x.idAnswearOnTask,
                        principalTable: "AnswearOnTasks",
                        principalColumn: "idAnswearOnTask",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestTaskExecutions_TestTasks_idTestTask",
                        column: x => x.idTestTask,
                        principalTable: "TestTasks",
                        principalColumn: "idTestTask",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestTaskExecutions_TryingTestTasks_idTryingTestTask",
                        column: x => x.idTryingTestTask,
                        principalTable: "TryingTestTasks",
                        principalColumn: "idTryingTestTask",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AchivmentExecutions_ChildId",
                table: "AchivmentExecutions",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_AchivmentExecutions_idAchivment",
                table: "AchivmentExecutions",
                column: "idAchivment");

            migrationBuilder.CreateIndex(
                name: "IX_AnswearOnTasks_idTestTask",
                table: "AnswearOnTasks",
                column: "idTestTask");

            migrationBuilder.CreateIndex(
                name: "IX_Appeals_ChildId",
                table: "Appeals",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_Appeals_idTypeAppeal",
                table: "Appeals",
                column: "idTypeAppeal");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterExecutions_idChapter",
                table: "ChapterExecutions",
                column: "idChapter");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterExecutions_idSubjectExecution",
                table: "ChapterExecutions",
                column: "idSubjectExecution");

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_idSubject",
                table: "Chapters",
                column: "idSubject");

            migrationBuilder.CreateIndex(
                name: "IX_LevelStudingExecutions_ChildId",
                table: "LevelStudingExecutions",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_LevelStudingExecutions_idLevelStuding",
                table: "LevelStudingExecutions",
                column: "idLevelStuding");

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_idTaskWithOpenAnsw",
                table: "Solutions",
                column: "idTaskWithOpenAnsw");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectExecutions_idLevelStudingExecution",
                table: "SubjectExecutions",
                column: "idLevelStudingExecution");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectExecutions_idSubject",
                table: "SubjectExecutions",
                column: "idSubject");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_idLevelStuding",
                table: "Subjects",
                column: "idLevelStuding");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectThingPack_ThingPacksidThingPack",
                table: "SubjectThingPack",
                column: "ThingPacksidThingPack");

            migrationBuilder.CreateIndex(
                name: "IX_TaskWithOpenAnsws_idTestPack",
                table: "TaskWithOpenAnsws",
                column: "idTestPack");

            migrationBuilder.CreateIndex(
                name: "IX_TaskWithOpenAnswsExecutions_idTaskWithOpenAnsws",
                table: "TaskWithOpenAnswsExecutions",
                column: "idTaskWithOpenAnsws");

            migrationBuilder.CreateIndex(
                name: "IX_TaskWithOpenAnswsExecutions_idTestPackExecution",
                table: "TaskWithOpenAnswsExecutions",
                column: "idTestPackExecution");

            migrationBuilder.CreateIndex(
                name: "IX_TestPackExecutions_idChapterExecution",
                table: "TestPackExecutions",
                column: "idChapterExecution");

            migrationBuilder.CreateIndex(
                name: "IX_TestPackExecutions_idTestPack",
                table: "TestPackExecutions",
                column: "idTestPack");

            migrationBuilder.CreateIndex(
                name: "IX_TestPacks_idChapter",
                table: "TestPacks",
                column: "idChapter");

            migrationBuilder.CreateIndex(
                name: "IX_TestTaskExecutions_idAnswearOnTask",
                table: "TestTaskExecutions",
                column: "idAnswearOnTask");

            migrationBuilder.CreateIndex(
                name: "IX_TestTaskExecutions_idTestTask",
                table: "TestTaskExecutions",
                column: "idTestTask");

            migrationBuilder.CreateIndex(
                name: "IX_TestTaskExecutions_idTryingTestTask",
                table: "TestTaskExecutions",
                column: "idTryingTestTask");

            migrationBuilder.CreateIndex(
                name: "IX_TestTasks_idTestPack",
                table: "TestTasks",
                column: "idTestPack");

            migrationBuilder.CreateIndex(
                name: "IX_TheoreticalMaterial_idChapter",
                table: "TheoreticalMaterial",
                column: "idChapter");

            migrationBuilder.CreateIndex(
                name: "IX_ThingExecutions_idThing",
                table: "ThingExecutions",
                column: "idThing");

            migrationBuilder.CreateIndex(
                name: "IX_ThingExecutionThingPackExecution_ThingPackExecutionsidThingPackExecution",
                table: "ThingExecutionThingPackExecution",
                column: "ThingPackExecutionsidThingPackExecution");

            migrationBuilder.CreateIndex(
                name: "IX_ThingPackExecutions_ChildId",
                table: "ThingPackExecutions",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_ThingPackExecutions_idThingPack",
                table: "ThingPackExecutions",
                column: "idThingPack");

            migrationBuilder.CreateIndex(
                name: "IX_Things_idthingPack",
                table: "Things",
                column: "idthingPack");

            migrationBuilder.CreateIndex(
                name: "IX_TryingTestTasks_idTestPackExecution",
                table: "TryingTestTasks",
                column: "idTestPackExecution");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AchivmentExecutions");

            migrationBuilder.DropTable(
                name: "Appeals");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Solutions");

            migrationBuilder.DropTable(
                name: "SubjectThingPack");

            migrationBuilder.DropTable(
                name: "TaskWithOpenAnswsExecutions");

            migrationBuilder.DropTable(
                name: "TestTaskExecutions");

            migrationBuilder.DropTable(
                name: "TheoreticalMaterial");

            migrationBuilder.DropTable(
                name: "ThingExecutionThingPackExecution");

            migrationBuilder.DropTable(
                name: "Achivments");

            migrationBuilder.DropTable(
                name: "TypeAppeals");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "TaskWithOpenAnsws");

            migrationBuilder.DropTable(
                name: "AnswearOnTasks");

            migrationBuilder.DropTable(
                name: "TryingTestTasks");

            migrationBuilder.DropTable(
                name: "ThingExecutions");

            migrationBuilder.DropTable(
                name: "ThingPackExecutions");

            migrationBuilder.DropTable(
                name: "TestTasks");

            migrationBuilder.DropTable(
                name: "TestPackExecutions");

            migrationBuilder.DropTable(
                name: "Things");

            migrationBuilder.DropTable(
                name: "ChapterExecutions");

            migrationBuilder.DropTable(
                name: "TestPacks");

            migrationBuilder.DropTable(
                name: "ThingPacks");

            migrationBuilder.DropTable(
                name: "SubjectExecutions");

            migrationBuilder.DropTable(
                name: "Chapters");

            migrationBuilder.DropTable(
                name: "LevelStudingExecutions");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "LevelStudings");
        }
    }
}
