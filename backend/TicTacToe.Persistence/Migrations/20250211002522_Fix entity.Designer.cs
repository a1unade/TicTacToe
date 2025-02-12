﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TicTacToe.Persistence.EfContext;

#nullable disable

namespace TicTacToe.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250211002522_Fix entity")]
    partial class Fixentity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TicTacToe.Domain.Entities.ChatHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoomId")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("ChatHistories");
                });

            modelBuilder.Entity("TicTacToe.Domain.Entities.ChatMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ChatHistoryId")
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<TimeOnly>("Time")
                        .HasColumnType("time without time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ChatHistoryId");

                    b.HasIndex("UserId");

                    b.ToTable("ChatMessages");
                });

            modelBuilder.Entity("TicTacToe.Domain.Entities.Match", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Board")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("character varying(9)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CurrentPlayerId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsStarted")
                        .HasColumnType("boolean");

                    b.Property<int>("MaxScore")
                        .HasColumnType("integer");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid?>("WinnerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoomId")
                        .IsUnique();

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("TicTacToe.Domain.Entities.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ChatId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("MatchId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Player1Id")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("Player2Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("Player1Id");

                    b.HasIndex("Player2Id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("TicTacToe.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ChatHistoryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Score")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TicTacToe.Domain.Entities.ChatHistory", b =>
                {
                    b.HasOne("TicTacToe.Domain.Entities.Room", "Room")
                        .WithOne("ChatHistory")
                        .HasForeignKey("TicTacToe.Domain.Entities.ChatHistory", "RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TicTacToe.Domain.Entities.User", "User")
                        .WithOne("ChatHistory")
                        .HasForeignKey("TicTacToe.Domain.Entities.ChatHistory", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TicTacToe.Domain.Entities.ChatMessage", b =>
                {
                    b.HasOne("TicTacToe.Domain.Entities.ChatHistory", "ChatHistory")
                        .WithMany("ChatMessages")
                        .HasForeignKey("ChatHistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TicTacToe.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ChatHistory");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TicTacToe.Domain.Entities.Match", b =>
                {
                    b.HasOne("TicTacToe.Domain.Entities.Room", "Room")
                        .WithOne("Match")
                        .HasForeignKey("TicTacToe.Domain.Entities.Match", "RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("TicTacToe.Domain.Entities.Room", b =>
                {
                    b.HasOne("TicTacToe.Domain.Entities.User", "Player1")
                        .WithMany()
                        .HasForeignKey("Player1Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TicTacToe.Domain.Entities.User", "Player2")
                        .WithMany()
                        .HasForeignKey("Player2Id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Player1");

                    b.Navigation("Player2");
                });

            modelBuilder.Entity("TicTacToe.Domain.Entities.ChatHistory", b =>
                {
                    b.Navigation("ChatMessages");
                });

            modelBuilder.Entity("TicTacToe.Domain.Entities.Room", b =>
                {
                    b.Navigation("ChatHistory")
                        .IsRequired();

                    b.Navigation("Match");
                });

            modelBuilder.Entity("TicTacToe.Domain.Entities.User", b =>
                {
                    b.Navigation("ChatHistory")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
