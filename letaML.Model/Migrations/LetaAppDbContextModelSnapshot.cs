﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using leta.Model;

namespace leta.Model.Migrations
{
    [DbContext(typeof(LetaAppDbContext))]
    partial class LetaAppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("leta.Model.RouteTime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("DiaDaSemana")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("HoraDoDia")
                        .HasColumnType("datetime(6)");

                    b.Property<float>("Tempo")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("RouteTime");
                });
#pragma warning restore 612, 618
        }
    }
}