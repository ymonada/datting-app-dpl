﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebSocket.db;

#nullable disable

namespace WebSocket.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250115193841_Init2")]
    partial class Init2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WebSocket.Entity.Like", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("UserFromId")
                        .HasColumnType("integer");

                    b.Property<int>("UserToId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserFromId");

                    b.HasIndex("UserToId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("WebSocket.Entity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("integer");

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebSocket.Entity.Like", b =>
                {
                    b.HasOne("WebSocket.Entity.User", "UserFrom")
                        .WithMany("SendLikes")
                        .HasForeignKey("UserFromId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebSocket.Entity.User", "UserTo")
                        .WithMany("ReceiveLikes")
                        .HasForeignKey("UserToId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserFrom");

                    b.Navigation("UserTo");
                });

            modelBuilder.Entity("WebSocket.Entity.User", b =>
                {
                    b.Navigation("ReceiveLikes");

                    b.Navigation("SendLikes");
                });
#pragma warning restore 612, 618
        }
    }
}
