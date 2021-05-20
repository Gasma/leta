﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using leta.Data;

namespace leta.Data.Migrations
{
    [DbContext(typeof(LetaAppDbContext))]
    [Migration("20210517222214_CriaInfoModel")]
    partial class CriaInfoModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("leta.Data.Entities.InfoModelo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Mensagem")
                        .HasColumnType("longtext");

                    b.Property<int>("QuantDados")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UltimoTreinamento")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("InfoModelo");
                });

            modelBuilder.Entity("leta.Data.RouteTime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DiaDaSemana")
                        .HasColumnType("int");

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