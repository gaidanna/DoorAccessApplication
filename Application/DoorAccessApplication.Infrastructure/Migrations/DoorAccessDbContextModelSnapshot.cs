// <auto-generated />
using System;
using DoorAccessApplication.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DoorAccessApplication.Infrastructure.Migrations
{
    [DbContext(typeof(DoorAccessDbContext))]
    partial class DoorAccessDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DoorAccessApplication.Core.Models.Lock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("UniqueIdentifier")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Locks");
                });

            modelBuilder.Entity("DoorAccessApplication.Core.Models.LockHistoryEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("LockId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("LockId");

                    b.HasIndex("UserId");

                    b.ToTable("LockHistoryEntries");
                });

            modelBuilder.Entity("DoorAccessApplication.Core.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LockUser", b =>
                {
                    b.Property<int>("LocksId")
                        .HasColumnType("int");

                    b.Property<string>("UsersId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LocksId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("LockUser");
                });

            modelBuilder.Entity("DoorAccessApplication.Core.Models.LockHistoryEntry", b =>
                {
                    b.HasOne("DoorAccessApplication.Core.Models.Lock", null)
                        .WithMany("HistoryEntries")
                        .HasForeignKey("LockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DoorAccessApplication.Core.Models.User", null)
                        .WithMany("HistoryEntries")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LockUser", b =>
                {
                    b.HasOne("DoorAccessApplication.Core.Models.Lock", null)
                        .WithMany()
                        .HasForeignKey("LocksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DoorAccessApplication.Core.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DoorAccessApplication.Core.Models.Lock", b =>
                {
                    b.Navigation("HistoryEntries");
                });

            modelBuilder.Entity("DoorAccessApplication.Core.Models.User", b =>
                {
                    b.Navigation("HistoryEntries");
                });
#pragma warning restore 612, 618
        }
    }
}
