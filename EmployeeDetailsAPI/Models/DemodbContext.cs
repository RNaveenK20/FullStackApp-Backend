using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDetailsAPI.Models;

public partial class DemodbContext : DbContext
{
    public DemodbContext()
    {
    }

    public DemodbContext(DbContextOptions<DemodbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblDepartment> TblDepartments { get; set; }

    public virtual DbSet<TblEmployee> TblEmployees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("data source=localhost; integrated security = SSPI; database=demodb; TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblDepartment>(entity =>
        {
            entity.HasKey(e => e.Deptid).HasName("PK__Departme__BE2C1AEE82DCD87B");

            entity.ToTable("TblDepartment");

            entity.Property(e => e.Deptid)
                .ValueGeneratedNever()
                .HasColumnName("deptid");
            entity.Property(e => e.Dname)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("dname");
        });

        modelBuilder.Entity<TblEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TblEmplo__3213E83FB366328E");

            entity.ToTable("TblEmployee");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Deptno).HasColumnName("deptno");
            entity.Property(e => e.Location)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Salary).HasColumnName("salary");

            entity.HasOne(d => d.DeptnoNavigation).WithMany(p => p.TblEmployees)
                .HasForeignKey(d => d.Deptno)
                .HasConstraintName("fk_department");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
