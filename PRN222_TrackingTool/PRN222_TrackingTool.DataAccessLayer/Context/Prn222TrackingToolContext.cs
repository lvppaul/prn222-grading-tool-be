using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PRN222_TrackingTool.DataAccessLayer.Entities;

namespace PRN222_TrackingTool.DataAccessLayer.Context;

public partial class Prn222TrackingToolContext : DbContext
{
    public Prn222TrackingToolContext()
    {
    }

    public Prn222TrackingToolContext(DbContextOptions<Prn222TrackingToolContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Students> Students { get; set; }

    public virtual DbSet<LecturerStudentAssignment> LecturerStudentAssignments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exams__3213E83FF9197BD1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.ExaminerId).HasColumnName("examiner_id");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Examiner).WithMany(p => p.Exams)
                .HasForeignKey(d => d.ExaminerId)
                .HasConstraintName("FK__Exams__examiner___412EB0B6");
        });

        modelBuilder.Entity<LecturerStudentAssignment>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_LecturerStudentAssignment");

                entity.Property(e => e.Score)
                .HasColumnName("score");

                entity.Property(e => e.Reason)
                    .HasMaxLength(500)
                    .HasColumnName("reason");

                entity.Property(e => e.AssignedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("assigned_at");

                entity.Property(e => e.IsReExam)
                .HasDefaultValue(false)
                .HasColumnName("is_re_exam");

                entity.Property(e => e.IsFinal)
                    .HasDefaultValue(false)
                    .HasColumnName("is_final");

                entity.HasOne(d => d.Lecturer)
                .WithMany(p => p.LecturerStudentAssignments) // User cần có ICollection<LecturerStudentAssignment>
                .HasForeignKey(d => d.LecturerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LecturerStudentAssignment_Lecturer");

                entity.HasOne(d => d.Student)
                .WithMany(p => p.LecturerStudentAssignments) // Student cũng có ICollection<LecturerStudentAssignment>
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LecturerStudentAssignment_Student");

                entity.HasOne(d => d.Test)
                .WithMany(p => p.LecturerStudentAssignments) // Test cũng có ICollection<LecturerStudentAssignment>
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LecturerStudentAssignment_Test");
            });
        

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3213E83F12FB5F9F");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Students>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Students__3213E83F92C3D8B1");
            entity.ToTable("Students");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StudentCode)
                .HasDefaultValue(false)
                .HasColumnName("student_code");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("fullname");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tests__3213E83F8B8D01C8");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.ExamId).HasColumnName("exam_id");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.Link).HasColumnName("link");
            entity.Property(e => e.OriginalFilename)
                .HasMaxLength(100)
                .HasColumnName("original_filename");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");

            entity.HasOne(d => d.Exam).WithMany(p => p.Tests)
                .HasForeignKey(d => d.ExamId)
                .HasConstraintName("FK__Tests__exam_id__45F365D3");

            entity.HasOne(d => d.Student)
            .WithOne(p => p.Test)   // sửa lại đây
            .HasForeignKey<Test>(d => d.StudentId) // khóa ngoại nằm ở Test
            .HasConstraintName("FK__Tests__student_id__44FF419A");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3213E83F76EF0D74");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.ExpiredRefreshToken)
                .HasColumnType("datetime")
                .HasColumnName("expired_refresh_token");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.RefreshToken).HasColumnName("refresh_token");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Users__role_id__3C69FB99");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
