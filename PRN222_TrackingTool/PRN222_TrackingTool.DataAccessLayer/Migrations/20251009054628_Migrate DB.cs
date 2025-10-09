using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PRN222_TrackingTool.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class MigrateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Roles__3213E83F12FB5F9F", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    student_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    fullname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Students__3213E83F92C3D8B1", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    refresh_token = table.Column<string>(type: "text", nullable: true),
                    expired_refresh_token = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamptz", nullable: true, defaultValueSql: "NOW()"),
                    is_active = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    role_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__3213E83F76EF0D74", x => x.id);
                    table.ForeignKey(
                        name: "FK__Users__role_id__3C69FB99",
                        column: x => x.role_id,
                        principalTable: "Roles",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamptz", nullable: true, defaultValueSql: "NOW()"),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    examiner_id = table.Column<int>(type: "integer", nullable: true),
                    duration = table.Column<int>(type: "integer", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Exams__3213E83FF9197BD1", x => x.id);
                    table.ForeignKey(
                        name: "FK__Exams__examiner___412EB0B6",
                        column: x => x.examiner_id,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    original_filename = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    link = table.Column<string>(type: "text", nullable: true),
                    student_id = table.Column<int>(type: "integer", nullable: false),
                    exam_id = table.Column<int>(type: "integer", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tests__3213E83F8B8D01C8", x => x.id);
                    table.ForeignKey(
                        name: "FK__Tests__exam_id__45F365D3",
                        column: x => x.exam_id,
                        principalTable: "Exams",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Tests__student_id__44FF419A",
                        column: x => x.student_id,
                        principalTable: "Students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LecturerStudentAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LecturerId = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    TestId = table.Column<int>(type: "integer", nullable: false),
                    score = table.Column<double>(type: "double precision", nullable: true),
                    reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_re_exam = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    is_final = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    assigned_at = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LecturerStudentAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LecturerStudentAssignment_Lecturer",
                        column: x => x.LecturerId,
                        principalTable: "Users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_LecturerStudentAssignment_Student",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_LecturerStudentAssignment_Test",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exams_examiner_id",
                table: "Exams",
                column: "examiner_id");

            migrationBuilder.CreateIndex(
                name: "IX_LecturerStudentAssignments_LecturerId",
                table: "LecturerStudentAssignments",
                column: "LecturerId");

            migrationBuilder.CreateIndex(
                name: "IX_LecturerStudentAssignments_StudentId",
                table: "LecturerStudentAssignments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_LecturerStudentAssignments_TestId",
                table: "LecturerStudentAssignments",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_exam_id",
                table: "Tests",
                column: "exam_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_student_id",
                table: "Tests",
                column: "student_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_role_id",
                table: "Users",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LecturerStudentAssignments");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
