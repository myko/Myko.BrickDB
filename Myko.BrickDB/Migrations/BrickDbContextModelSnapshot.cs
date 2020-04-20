﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Myko.BrickDB;

namespace Myko.BrickDB.Migrations
{
    [DbContext(typeof(BrickDbContext))]
    partial class BrickDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Myko.BrickDB.Color", b =>
                {
                    b.Property<string>("ColorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ColorId");

                    b.ToTable("Colors");
                });

            modelBuilder.Entity("Myko.BrickDB.Design", b =>
                {
                    b.Property<string>("DesignId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DesignId");

                    b.ToTable("Designs");
                });

            modelBuilder.Entity("Myko.BrickDB.Element", b =>
                {
                    b.Property<string>("ElementId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ColorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DesignId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ElementId");

                    b.HasIndex("ColorId");

                    b.HasIndex("DesignId");

                    b.ToTable("Elements");
                });

            modelBuilder.Entity("Myko.BrickDB.Element", b =>
                {
                    b.HasOne("Myko.BrickDB.Color", "Color")
                        .WithMany("Elements")
                        .HasForeignKey("ColorId");

                    b.HasOne("Myko.BrickDB.Design", "Design")
                        .WithMany("Elements")
                        .HasForeignKey("DesignId");
                });
#pragma warning restore 612, 618
        }
    }
}
