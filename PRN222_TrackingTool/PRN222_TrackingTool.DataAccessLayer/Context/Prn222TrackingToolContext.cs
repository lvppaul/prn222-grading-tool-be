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

    public virtual DbSet<LecturersTestsDetail> LecturersTestsDetails { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<TestsScore> TestsScores { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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

        modelBuilder.Entity<LecturersTestsDetail>(entity =>
        {
            entity.HasKey(e => new { e.TestId, e.LecturerId }).HasName("PK__Lecturer__FEB201A97B1CFBC9");

            entity.ToTable("Lecturers_Tests_Detail");

            entity.Property(e => e.TestId).HasColumnName("test_id");
            entity.Property(e => e.LecturerId).HasColumnName("lecturer_id");
            entity.Property(e => e.IsGrading).HasColumnName("is_grading");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.Score).HasColumnName("score");

            entity.HasOne(d => d.Lecturer).WithMany(p => p.LecturersTestsDetails)
                .HasForeignKey(d => d.LecturerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Lecturers__lectu__49C3F6B7");

            entity.HasOne(d => d.Test).WithMany(p => p.LecturersTestsDetails)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Lecturers__test___48CFD27E");
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
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");

            entity.HasOne(d => d.Exam).WithMany(p => p.Tests)
                .HasForeignKey(d => d.ExamId)
                .HasConstraintName("FK__Tests__exam_id__45F365D3");

            entity.HasOne(d => d.Student).WithMany(p => p.Tests)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Tests__student_i__44FF419A");
        });

        modelBuilder.Entity<TestsScore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tests_Sc__3213E83F5C14E709");

            entity.ToTable("Tests_Score");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsFinal)
                .HasDefaultValue(false)
                .HasColumnName("is_final");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.TestId).HasColumnName("test_id");

            entity.HasOne(d => d.Test).WithMany(p => p.TestsScores)
                .HasForeignKey(d => d.TestId)
                .HasConstraintName("FK__Tests_Sco__test___4D94879B");
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
