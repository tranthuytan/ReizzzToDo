﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReizzzToDo.DAL.Entities;

#nullable disable

namespace ReizzzToDo.DAL.Migrations
{
    [DbContext(typeof(ReizzzToDoContext))]
    [Migration("20250315003645_Add_User_Role")]
    partial class Add_User_Role
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ReizzzToDo.DAL.Entities.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Name = "Admin"
                        },
                        new
                        {
                            Id = 2L,
                            Name = "Registered"
                        });
                });

            modelBuilder.Entity("ReizzzToDo.DAL.Entities.TimeUnit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("time_units", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Seconds"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Minutes"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Hours"
                        });
                });

            modelBuilder.Entity("ReizzzToDo.DAL.Entities.ToDo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("TimeToComplete")
                        .HasColumnType("real");

                    b.Property<int?>("TimeUnitId")
                        .HasColumnType("int");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TimeUnitId");

                    b.HasIndex("UserId");

                    b.ToTable("to_dos", (string)null);
                });

            modelBuilder.Entity("ReizzzToDo.DAL.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreferredTimeUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("ReizzzToDo.DAL.Entities.UserRole", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("user_roles", (string)null);
                });

            modelBuilder.Entity("ReizzzToDo.DAL.Entities.ToDo", b =>
                {
                    b.HasOne("ReizzzToDo.DAL.Entities.TimeUnit", "TimeUnit")
                        .WithMany()
                        .HasForeignKey("TimeUnitId");

                    b.HasOne("ReizzzToDo.DAL.Entities.User", "User")
                        .WithMany("ToDos")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_todo_user");

                    b.Navigation("TimeUnit");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ReizzzToDo.DAL.Entities.UserRole", b =>
                {
                    b.HasOne("ReizzzToDo.DAL.Entities.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ReizzzToDo.DAL.Entities.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ReizzzToDo.DAL.Entities.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("ReizzzToDo.DAL.Entities.User", b =>
                {
                    b.Navigation("ToDos");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
