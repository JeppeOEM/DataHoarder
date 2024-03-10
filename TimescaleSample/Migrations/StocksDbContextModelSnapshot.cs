﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TimescaleSample.Data;

#nullable disable

namespace TimescaleSample.Migrations
{
    [DbContext(typeof(StocksDbContext))]
    partial class StocksDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-preview.6.22329.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TimescaleSample.Models.Company", b =>
                {
                    b.Property<string>("Symbol")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Symbol");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("TimescaleSample.Models.IntervalResult", b =>
                {
                    b.Property<decimal>("Average")
                        .HasColumnType("numeric");

                    b.Property<decimal>("End")
                        .HasColumnType("numeric");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Start")
                        .HasColumnType("numeric");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("text");

                    b.ToTable("IntervalResult");
                });

            modelBuilder.Entity("TimescaleSample.Models.Stock", b =>
                {
                    b.Property<int?>("DayVolume")
                        .HasColumnType("integer");

                    b.Property<decimal?>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("Time")
                        .HasColumnType("timestamp with time zone");

                    b.HasIndex("Symbol");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("TimescaleSample.Models.Stock", b =>
                {
                    b.HasOne("TimescaleSample.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("Symbol")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });
#pragma warning restore 612, 618
        }
    }
}
