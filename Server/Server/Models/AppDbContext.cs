using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Server.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Goal> Goals { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<PerfomanceReview> PerfomanceReviews { get; set; }

    public virtual DbSet<PredefinedCriterion> PredefinedCriteria { get; set; }

    public virtual DbSet<RoleTable> RoleTables { get; set; }

    public virtual DbSet<StatusTable> StatusTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=PC0596\\MSSQL2019;database=PMS;trusted_connection=True; Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__AuditLog__7839F64DDDB54D17");

            entity.Property(e => e.LogId).HasColumnName("logId");
            entity.Property(e => e.Action)
                .HasMaxLength(255)
                .HasColumnName("action");
            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("changedAt");
            entity.Property(e => e.ChangedBy).HasColumnName("changedBy");
            entity.Property(e => e.EmpId).HasColumnName("empId");
            entity.Property(e => e.NewValue).HasColumnName("newValue");
            entity.Property(e => e.OldValue).HasColumnName("oldValue");
            entity.Property(e => e.TableName)
                .HasMaxLength(50)
                .HasColumnName("tableName");

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.AuditLogChangedByNavigations)
                .HasForeignKey(d => d.ChangedBy)
                .HasConstraintName("FK__AuditLogs__chang__17036CC0");

            entity.HasOne(d => d.Emp).WithMany(p => p.AuditLogEmps)
                .HasForeignKey(d => d.EmpId)
                .HasConstraintName("FK__AuditLogs__empId__160F4887");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DeptId).HasName("PK__Departme__BE2D26B650EEA62A");

            entity.ToTable("Department");

            entity.Property(e => e.DeptId).HasColumnName("deptId");
            entity.Property(e => e.DeptName)
                .HasMaxLength(50)
                .HasColumnName("deptName");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmpId).HasName("PK__Employee__AFB3EC0D6498B7DF");

            entity.HasIndex(e => e.UserName, "UQ__Employee__66DCF95CA484119B").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Employee__AB6E6164B3862E06").IsUnique();

            entity.Property(e => e.EmpId).HasColumnName("empId");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.DeptId).HasColumnName("deptId");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.EmpName)
                .HasMaxLength(50)
                .HasColumnName("empName");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.JoiningDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("joiningDate");
            entity.Property(e => e.ManagerId).HasColumnName("managerId");
            entity.Property(e => e.ModiftyBy).HasColumnName("modiftyBy");
            entity.Property(e => e.ModifyAt).HasColumnName("modifyAt");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(50)
                .HasColumnName("passwordHash");
            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("userName");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InverseCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Employees__creat__236943A5");

            entity.HasOne(d => d.Dept).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DeptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employees__deptI__75A278F5");

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__Employees__manag__04E4BC85");

            entity.HasOne(d => d.ModiftyByNavigation).WithMany(p => p.InverseModiftyByNavigation)
                .HasForeignKey(d => d.ModiftyBy)
                .HasConstraintName("FK__Employees__modif__245D67DE");

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employees__roleI__76969D2E");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__2613FD2479D46E07");

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId).HasColumnName("feedbackId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Message)
                .HasMaxLength(255)
                .HasColumnName("message");
            entity.Property(e => e.ReceiverId).HasColumnName("receiverId");
            entity.Property(e => e.SenderId).HasColumnName("senderId");

            entity.HasOne(d => d.Receiver).WithMany(p => p.FeedbackReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__receiv__1BC821DD");

            entity.HasOne(d => d.Sender).WithMany(p => p.FeedbackSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__sender__1AD3FDA4");
        });

        modelBuilder.Entity<Goal>(entity =>
        {
            entity.HasKey(e => e.GoalId).HasName("PK__Goal__7E225EB155547F10");

            entity.ToTable("Goal");

            entity.Property(e => e.GoalId).HasColumnName("goalId");
            entity.Property(e => e.CreatedAt).HasColumnName("createdAt");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.Detail)
                .HasMaxLength(255)
                .HasColumnName("detail");
            entity.Property(e => e.DueDate).HasColumnName("dueDate");
            entity.Property(e => e.EmpId).HasColumnName("empId");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.ModifyAt).HasColumnName("modifyAt");
            entity.Property(e => e.ModifyBy).HasColumnName("modifyBy");
            entity.Property(e => e.StatusId).HasColumnName("statusId");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.GoalCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Goal__createdBy__46B27FE2");

            entity.HasOne(d => d.Emp).WithMany(p => p.GoalEmps)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Goal__empId__44CA3770");

            entity.HasOne(d => d.ModifyByNavigation).WithMany(p => p.GoalModifyByNavigations)
                .HasForeignKey(d => d.ModifyBy)
                .HasConstraintName("FK__Goal__modifyBy__489AC854");

            entity.HasOne(d => d.Status).WithMany(p => p.Goals)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Goal__statusId__45BE5BA9");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__4BA5CEA9EB499135");

            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId).HasColumnName("notificationId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.EmpId).HasColumnName("empId");
            entity.Property(e => e.IsRead)
                .HasDefaultValue(false)
                .HasColumnName("isRead");
            entity.Property(e => e.Message)
                .HasMaxLength(255)
                .HasColumnName("message");

            entity.HasOne(d => d.Emp).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__empId__07C12930");
        });

        modelBuilder.Entity<PerfomanceReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Perfoman__2ECD6E0489CEBCAD");

            entity.ToTable("PerfomanceReview");

            entity.Property(e => e.ReviewId).HasColumnName("reviewId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.CriteriaId).HasColumnName("criteriaId");
            entity.Property(e => e.EmpId).HasColumnName("empId");
            entity.Property(e => e.EndDate).HasColumnName("endDate");
            entity.Property(e => e.Improvement)
                .HasMaxLength(255)
                .HasColumnName("improvement");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.ManagerFeedback)
                .HasMaxLength(255)
                .HasColumnName("managerFeedback");
            entity.Property(e => e.ManagerId).HasColumnName("managerId");
            entity.Property(e => e.ManagerRating).HasColumnName("managerRating");
            entity.Property(e => e.ModifyAt)
                .HasColumnType("datetime")
                .HasColumnName("modifyAt");
            entity.Property(e => e.ModifyBy).HasColumnName("modifyBy");
            entity.Property(e => e.SelfRating).HasColumnName("selfRating");
            entity.Property(e => e.StartDate).HasColumnName("startDate");
            entity.Property(e => e.StatusId).HasColumnName("statusId");
            entity.Property(e => e.Strength)
                .HasMaxLength(255)
                .HasColumnName("strength");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PerfomanceReviewCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Perfomanc__creat__5AB9788F");

            entity.HasOne(d => d.Criteria).WithMany(p => p.PerfomanceReviews)
                .HasForeignKey(d => d.CriteriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Perfomanc__crite__57DD0BE4");

            entity.HasOne(d => d.Emp).WithMany(p => p.PerfomanceReviewEmps)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Perfomanc__empId__55F4C372");

            entity.HasOne(d => d.Manager).WithMany(p => p.PerfomanceReviewManagers)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Perfomanc__manag__56E8E7AB");

            entity.HasOne(d => d.ModifyByNavigation).WithMany(p => p.PerfomanceReviewModifyByNavigations)
                .HasForeignKey(d => d.ModifyBy)
                .HasConstraintName("FK__Perfomanc__modif__5CA1C101");

            entity.HasOne(d => d.Status).WithMany(p => p.PerfomanceReviews)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Perfomanc__statu__58D1301D");
        });

        modelBuilder.Entity<PredefinedCriterion>(entity =>
        {
            entity.HasKey(e => e.CriteriaId).HasName("PK__Predefin__A5275073617C5C2C");

            entity.Property(e => e.CriteriaId).HasColumnName("criteriaId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("createdAt");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.CriteriaName)
                .HasMaxLength(255)
                .HasColumnName("criteriaName");
            entity.Property(e => e.Details)
                .HasMaxLength(255)
                .HasColumnName("details");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.ModifyAt).HasColumnName("modifyAt");
            entity.Property(e => e.ModifyBy).HasColumnName("modifyBy");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PredefinedCriterionCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Predefine__creat__3A4CA8FD");

            entity.HasOne(d => d.ModifyByNavigation).WithMany(p => p.PredefinedCriterionModifyByNavigations)
                .HasForeignKey(d => d.ModifyBy)
                .HasConstraintName("FK__Predefine__modif__3B40CD36");
        });

        modelBuilder.Entity<RoleTable>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__RoleTabl__CD98462ADD063385");

            entity.ToTable("RoleTable");

            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<StatusTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StatusTa__3214EC07209CEE73");

            entity.ToTable("StatusTable");

            entity.HasIndex(e => e.Status, "UQ__StatusTa__A858923CD8287A58").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("createdAt");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.StatusTables)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StatusTab__creat__40058253");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
