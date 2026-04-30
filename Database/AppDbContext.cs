using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using breakincycleapi.Database.Models;

namespace breakincycleapi.Database;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chatroom> Chatrooms { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Progress> Progresses { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<TeacherCourse> TeacherCourses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chatroom>(entity =>
        {
            entity.HasKey(e => e.Roomid).HasName("PK_ChatRooms");
            entity.Property(e => e.MessageId)
                .HasColumnName("MessageID");

            entity.Property(e => e.Roomid)
                .HasDefaultValueSql("(newid())", "DF_Chatrooms")
                .HasColumnName("ROOMID");
            entity.Property(e => e.JoinedAt).HasPrecision(0);
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("NAME");
            entity.Property(e => e.Userid).HasColumnName("USERID");

            // Map the manually added columns
            entity.Property(e => e.MessageSenderName)
                .HasColumnName("MessageSenderName");
            entity.Property(e => e.MessageContent)
                .HasColumnName("MessageContent");
            entity.Property(e => e.Description)
                .HasColumnName("Description");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK_Courses");

            entity.ToTable("courses");

            entity.Property(e => e.CourseId)
                .HasDefaultValueSql("(newid())", "DF_Courses")
                .HasColumnName("CourseID");
            entity.Property(e => e.Createdat).HasPrecision(0);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Lastactive).HasPrecision(0);
            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.Property(e => e.MessageId).HasColumnName("MessageID");
            entity.Property(e => e.Createdat).HasPrecision(0);
            entity.Property(e => e.Message1)
                .HasMaxLength(2000)
                .HasColumnName("Message");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Roomid).HasColumnName("ROOMID");

            entity.HasOne(d => d.Room).WithMany(p => p.Messages)
                .HasForeignKey(d => d.Roomid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_messages_room");

            entity.HasOne(d => d.User).WithMany(p => p.Messages)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_room");
        });

        modelBuilder.Entity<Progress>(entity =>
        {
            entity.HasKey(e => e.ProgressId).HasName("PK_Progress");

            entity.ToTable("PROGRESS", tb => tb.HasTrigger("trg_Progress_UpdateLastUpdated"));

            entity.Property(e => e.ProgressId).HasColumnName("ProgressID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.LastUpdated).HasPrecision(0);
            entity.Property(e => e.ProgressStatus).HasMaxLength(100);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Student).WithMany(p => p.Progresses)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_progress_student");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK_Students");

            entity.ToTable("students");

            entity.HasIndex(e => e.Email, "UQ_Students_Email").IsUnique();

            entity.Property(e => e.StudentId)
                .HasDefaultValueSql("(newid())", "DF_Students")
                .HasColumnName("StudentID");
            entity.Property(e => e.Createdat).HasPrecision(0);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.Lastactive).HasPrecision(0);
            entity.Property(e => e.Location).HasMaxLength(250);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Phonenumber).HasMaxLength(250);
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK_Teachers");

            entity.ToTable("TEACHERS");

            entity.HasIndex(e => e.Email, "UQ_Teachers_Email").IsUnique();

            entity.Property(e => e.TeacherId)
                .HasDefaultValueSql("(newid())", "DF_Teachers")
                .HasColumnName("TeacherID");
            entity.Property(e => e.Createdat).HasPrecision(0);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.Lastactive).HasPrecision(0);
            entity.Property(e => e.Location).HasMaxLength(250);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Phonenumber).HasMaxLength(250);
        });

        modelBuilder.Entity<TeacherCourse>(entity =>
        {
            entity.HasKey(e => e.TeacherCourseId).HasName("PK_TeacherCourse");

            entity.ToTable("TEACHER_COURSE");

            entity.Property(e => e.TeacherCourseId).HasColumnName("TeacherCourseID");
            entity.Property(e => e.AssignedAt).HasPrecision(0);
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TeacherCourses)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_teacher_course_teacher");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email, "UQ_Users_Email").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())", "DF_Users_ID")
                .HasColumnName("ID");
            entity.Property(e => e.Createdat).HasPrecision(0);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.Lastactive).HasPrecision(0);
            entity.Property(e => e.Location).HasMaxLength(250);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(250)
                .HasColumnName("PasswordHASH");
            entity.Property(e => e.Phonenumbar).HasMaxLength(250);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
