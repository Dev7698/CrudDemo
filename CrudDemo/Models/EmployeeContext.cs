using System;
using System.Collections.Generic;
using CrudDemo.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CrudDemo.Models;

public partial class EmployeeContext : DbContext
{
    public static void SetConnectionString(string connectionString)
    {
        if (ConnectionString == null)
        {
            ConnectionString = connectionString;
        }
        else
        {
            throw new Exception();
        }
    }
    private static string ConnectionString { get; set; }
    public EmployeeContext()
    {
    }

    public EmployeeContext(DbContextOptions<EmployeeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblEmployee> TblEmployees { get; set; }

    public virtual DbSet<EmployeeModel> EmployeeModel { get; set; } 

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblEmployee>(entity =>
        {
            entity.HasKey(e => e.EmpId);

            entity.ToTable("tblEmployee");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Department).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Image).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
        });
        modelBuilder.Entity<EmployeeModel>(entity => entity.HasNoKey());
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
