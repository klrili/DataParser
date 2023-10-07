﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SteamBigData.Data;

#nullable disable

namespace SteamBigData.Migrations
{
    [DbContext(typeof(SteamBigDataDbContext))]
    partial class SteamBigDataDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SteamBigData.Data.ItemInfo", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("itemId")
                        .HasColumnType("int");

                    b.Property<int>("itemName")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("ItemInfo");
                });

            modelBuilder.Entity("SteamBigData.Data.SoldInfo", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int?>("ItemInfoid")
                        .HasColumnType("int");

                    b.Property<string>("buyerAvatarUrl")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("buyerUserName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("itemNameId")
                        .HasColumnType("int");

                    b.Property<decimal>("price")
                        .HasColumnType("decimal(6 ,2)");

                    b.Property<string>("sellerAvatarUrl")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("sellerUserName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("timestamp")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("ItemInfoid");

                    b.ToTable("SoldInfos");
                });

            modelBuilder.Entity("SteamBigData.Data.SoldInfo", b =>
                {
                    b.HasOne("SteamBigData.Data.ItemInfo", "ItemInfo")
                        .WithMany()
                        .HasForeignKey("ItemInfoid");

                    b.Navigation("ItemInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
