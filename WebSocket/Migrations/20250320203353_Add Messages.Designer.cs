﻿// <auto-generated />
using System;
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
    [Migration("20250320203353_Add Messages")]
    partial class AddMessages
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

            modelBuilder.Entity("WebSocket.Entity.Match", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("FirstUserId")
                        .HasColumnType("integer");

                    b.Property<int>("SecondUserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SecondUserId");

                    b.HasIndex("FirstUserId", "SecondUserId")
                        .IsUnique();

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("WebSocket.Entity.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsRead")
                        .HasColumnType("boolean");

                    b.Property<int>("MatchId")
                        .HasColumnType("integer");

                    b.Property<int>("SenderId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("WebSocket.Entity.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("WebSocket.Entity.ProfileHistory", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("WatchId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "WatchId");

                    b.ToTable("ProfileHistories");
                });

            modelBuilder.Entity("WebSocket.Entity.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");
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
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<int>("GenderPreference")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastActiveTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebSocket.Entity.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("WebSocket.Entity.Like", b =>
                {
                    b.HasOne("WebSocket.Entity.User", "UserFrom")
                        .WithMany()
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

            modelBuilder.Entity("WebSocket.Entity.Match", b =>
                {
                    b.HasOne("WebSocket.Entity.User", "FirstUser")
                        .WithMany("Matches")
                        .HasForeignKey("FirstUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebSocket.Entity.User", "SecondUser")
                        .WithMany()
                        .HasForeignKey("SecondUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("FirstUser");

                    b.Navigation("SecondUser");
                });

            modelBuilder.Entity("WebSocket.Entity.Message", b =>
                {
                    b.HasOne("WebSocket.Entity.Match", "Match")
                        .WithMany("MessagesHistory")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebSocket.Entity.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Match");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("WebSocket.Entity.Photo", b =>
                {
                    b.HasOne("WebSocket.Entity.User", "User")
                        .WithMany("Photos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebSocket.Entity.ProfileHistory", b =>
                {
                    b.HasOne("WebSocket.Entity.User", "User")
                        .WithMany("ProfileHistory")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebSocket.Entity.UserRole", b =>
                {
                    b.HasOne("WebSocket.Entity.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebSocket.Entity.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebSocket.Entity.Match", b =>
                {
                    b.Navigation("MessagesHistory");
                });

            modelBuilder.Entity("WebSocket.Entity.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("WebSocket.Entity.User", b =>
                {
                    b.Navigation("Matches");

                    b.Navigation("Photos");

                    b.Navigation("ProfileHistory");

                    b.Navigation("ReceiveLikes");

                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
