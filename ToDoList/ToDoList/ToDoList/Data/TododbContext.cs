using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using ToDoList.Models;
using TaskEntity = ToDoList.Models.Task;

namespace ToDoList.Data;

public partial class TododbContext : DbContext
{
    public TododbContext()
    {
    }

    public TododbContext(DbContextOptions<TododbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<List> Lists { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Recurringtask> Recurringtasks { get; set; }

    public virtual DbSet<Subtask> Subtasks { get; set; }

    public virtual DbSet<TaskEntity> Tasks { get; set; }

    public virtual DbSet<Taskattachment> Taskattachments { get; set; }

    public virtual DbSet<Taskcollaborator> Taskcollaborators { get; set; }

    public virtual DbSet<Taskreminder> Taskreminders { get; set; }

    public virtual DbSet<Tasktag> Tasktags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=tododb;user=milvin;password=1615", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.41-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<List>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("lists");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .HasColumnName("color");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Lists)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("lists_ibfk_1");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("notifications");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.IsRead)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_read");
            entity.Property(e => e.Message)
                .HasColumnType("text")
                .HasColumnName("message");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("notifications_ibfk_1");
        });

        modelBuilder.Entity<Recurringtask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("recurringtasks");

            entity.HasIndex(e => e.TaskId, "task_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NextOccurrence)
                .HasColumnType("datetime")
                .HasColumnName("next_occurrence");
            entity.Property(e => e.RecurrenceType)
                .HasMaxLength(50)
                .HasColumnName("recurrence_type");
            entity.Property(e => e.TaskId).HasColumnName("task_id");

            entity.HasOne(d => d.Task).WithMany(p => p.Recurringtasks)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("recurringtasks_ibfk_1");
        });

        modelBuilder.Entity<Subtask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("subtasks");

            entity.HasIndex(e => e.TaskId, "task_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Completed)
                .HasDefaultValueSql("'0'")
                .HasColumnName("completed");
            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Task).WithMany(p => p.Subtasks)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("subtasks_ibfk_1");
        });

        modelBuilder.Entity<TaskEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tasks");

            entity.HasIndex(e => e.ListId, "list_id");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Completed)
                .HasDefaultValueSql("'0'")
                .HasColumnName("completed");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.DueDate)
                .HasColumnType("datetime")
                .HasColumnName("due_date");
            entity.Property(e => e.ListId).HasColumnName("list_id");
            entity.Property(e => e.Priority).HasColumnName("priority");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.List).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ListId)
                .HasConstraintName("tasks_ibfk_1");

            entity.HasOne(d => d.User).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("tasks_ibfk_2");
        });

        modelBuilder.Entity<Taskattachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("taskattachments");

            entity.HasIndex(e => e.TaskId, "task_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .HasColumnName("file_name");
            entity.Property(e => e.FileUrl)
                .HasMaxLength(255)
                .HasColumnName("file_url");
            entity.Property(e => e.TaskId).HasColumnName("task_id");

            entity.HasOne(d => d.Task).WithMany(p => p.Taskattachments)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("taskattachments_ibfk_1");
        });

        modelBuilder.Entity<Taskcollaborator>(entity =>
        {
            entity.HasKey(e => new { e.ListId, e.UserId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("taskcollaborators");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.ListId).HasColumnName("list_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.PermissionLevel)
                .HasMaxLength(50)
                .HasColumnName("permission_level");

            entity.HasOne(d => d.List).WithMany(p => p.Taskcollaborators)
                .HasForeignKey(d => d.ListId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("taskcollaborators_ibfk_1");

            entity.HasOne(d => d.User).WithMany(p => p.Taskcollaborators)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("taskcollaborators_ibfk_2");
        });

        modelBuilder.Entity<Taskreminder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("taskreminders");

            entity.HasIndex(e => e.TaskId, "task_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ReminderTime)
                .HasColumnType("datetime")
                .HasColumnName("reminder_time");
            entity.Property(e => e.TaskId).HasColumnName("task_id");

            entity.HasOne(d => d.Task).WithMany(p => p.Taskreminders)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("taskreminders_ibfk_1");
        });

        modelBuilder.Entity<Tasktag>(entity =>
        {
            entity.HasKey(e => new { e.TaskId, e.Tag })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("tasktags");

            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.Tag).HasColumnName("tag");

            entity.HasOne(d => d.Task).WithMany(p => p.Tasktags)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tasktags_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
