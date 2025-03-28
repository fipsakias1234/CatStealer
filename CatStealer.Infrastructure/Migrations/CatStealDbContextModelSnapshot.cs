﻿// <auto-generated />
using System;
using CatStealer.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CatStealer.Infrastructure.Migrations
{
    [DbContext(typeof(CatStealDbContext))]
    partial class CatStealDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CatStealer.Domain.Cats.CatEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CatId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<double>("Height")
                        .HasColumnType("float");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("StolenCats");
                });

            modelBuilder.Entity("CatStealer.Domain.CatsTagsBridge.CatTags", b =>
                {
                    b.Property<int>("CatsId")
                        .HasColumnType("int");

                    b.Property<int>("TagsId")
                        .HasColumnType("int");

                    b.HasKey("CatsId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("CatTags", (string)null);
                });

            modelBuilder.Entity("CatStealer.Domain.Tags.TagEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("CatStealer.Domain.CatsTagsBridge.CatTags", b =>
                {
                    b.HasOne("CatStealer.Domain.Cats.CatEntity", "Cat")
                        .WithMany("CatTags")
                        .HasForeignKey("CatsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CatStealer.Domain.Tags.TagEntity", "Tag")
                        .WithMany("CatTags")
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cat");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("CatStealer.Domain.Cats.CatEntity", b =>
                {
                    b.Navigation("CatTags");
                });

            modelBuilder.Entity("CatStealer.Domain.Tags.TagEntity", b =>
                {
                    b.Navigation("CatTags");
                });
#pragma warning restore 612, 618
        }
    }
}
