using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EmployerAPI.Models
{
    public partial class DemodbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DemodbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DemodbContext(DbContextOptions<DemodbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<TblDepartment> TblDepartments { get; set; }
        public virtual DbSet<TblEmployee> TblEmployees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("demodbCS");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

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
                entity.HasKey(e => e.Id).HasName("PK_TblEmployee");

                entity.Property(e => e.Deptno).HasColumnName("deptno");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Location)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("location");
                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("name");
                entity.Property(e => e.Salary).HasColumnName("salary");

                entity.HasOne(d => d.DeptnoNavigation).WithMany()
                    .HasForeignKey(d => d.Deptno)
                    .HasConstraintName("fk_department");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
