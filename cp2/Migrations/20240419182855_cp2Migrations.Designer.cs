﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oracle.EntityFrameworkCore.Metadata;
using cp2;

#nullable disable

namespace cp2.Migrations
{
    [DbContext(typeof(EncurtadorDbContext))]
    [Migration("20240419182855_cp2Migrations")]
    partial class cp2Migrations
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            OracleModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("cp2.Entities.EncurtadorUrl", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("RAW(16)");

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("NVARCHAR2(7)");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<string>("UrlCurta")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("UrlLonga")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("NVARCHAR2(255)");

                    b.HasKey("Id");

                    b.HasIndex("Codigo")
                        .IsUnique();

                    b.ToTable("EncurtadorUrls", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}