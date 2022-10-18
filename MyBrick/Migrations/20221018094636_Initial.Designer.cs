﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyBrick;

#nullable disable

namespace EntityFramework_Relations.Migrations
{
    [DbContext(typeof(BrickContext))]
    [Migration("20221018094636_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BrickTag", b =>
                {
                    b.Property<int>("BricksId")
                        .HasColumnType("int");

                    b.Property<int>("TagsId")
                        .HasColumnType("int");

                    b.HasKey("BricksId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("BrickTag");
                });

            modelBuilder.Entity("MyBrick.Brick", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("Color")
                        .HasColumnType("int");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.ToTable("Bricks");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Brick");
                });

            modelBuilder.Entity("MyBrick.BrickAvailability", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BrickId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(8,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("VendorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BrickId");

                    b.HasIndex("VendorId");

                    b.ToTable("BrickAvailabilities");
                });

            modelBuilder.Entity("MyBrick.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("MyBrick.Vendor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("VendorName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.ToTable("Vendors");
                });

            modelBuilder.Entity("MyBrick.BasePlate", b =>
                {
                    b.HasBaseType("MyBrick.Brick");

                    b.Property<int>("Length")
                        .HasColumnType("int");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("BasePlate");
                });

            modelBuilder.Entity("MyBrick.MinifigHead", b =>
                {
                    b.HasBaseType("MyBrick.Brick");

                    b.Property<bool>("IsDualSided")
                        .HasColumnType("bit");

                    b.HasDiscriminator().HasValue("MinifigHead");
                });

            modelBuilder.Entity("BrickTag", b =>
                {
                    b.HasOne("MyBrick.Brick", null)
                        .WithMany()
                        .HasForeignKey("BricksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyBrick.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyBrick.BrickAvailability", b =>
                {
                    b.HasOne("MyBrick.Brick", "Brick")
                        .WithMany("Availabilities")
                        .HasForeignKey("BrickId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyBrick.Vendor", "Vendor")
                        .WithMany("BrickAvailabilities")
                        .HasForeignKey("VendorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brick");

                    b.Navigation("Vendor");
                });

            modelBuilder.Entity("MyBrick.Brick", b =>
                {
                    b.Navigation("Availabilities");
                });

            modelBuilder.Entity("MyBrick.Vendor", b =>
                {
                    b.Navigation("BrickAvailabilities");
                });
#pragma warning restore 612, 618
        }
    }
}
