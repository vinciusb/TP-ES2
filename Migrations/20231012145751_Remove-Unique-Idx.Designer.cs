﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TwitterAPI.Infrastructure.Persistence;

#nullable disable

namespace TwitterAPI.Migrations
{
    [DbContext(typeof(PostgresTwitterRepository))]
    [Migration("20231012145751_Remove-Unique-Idx")]
    partial class RemoveUniqueIdx
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TweetTweet", b =>
                {
                    b.Property<int>("RepliesId")
                        .HasColumnType("integer")
                        .HasColumnName("ReplyId");

                    b.Property<int>("TweetId")
                        .HasColumnType("integer")
                        .HasColumnName("ParentId");

                    b.HasKey("RepliesId", "TweetId");

                    b.HasIndex("TweetId");

                    b.ToTable("TweetsReplies", (string)null);
                });

            modelBuilder.Entity("TweetUser", b =>
                {
                    b.Property<int>("LikeHistoryId")
                        .HasColumnType("integer")
                        .HasColumnName("TweetId");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("LikeHistoryId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("LikeHistory", (string)null);
                });

            modelBuilder.Entity("TwitterAPI.Application.Domain.Tweet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Likes")
                        .HasColumnType("integer");

                    b.Property<int>("OwnerId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("PostTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("ReplyToId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(144)
                        .HasColumnType("character varying(144)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ReplyToId");

                    b.ToTable("Tweets", (string)null);
                });

            modelBuilder.Entity("TwitterAPI.Application.Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("At")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasMaxLength(144)
                        .HasColumnType("character varying(144)");

                    b.Property<DateOnly?>("BirthDate")
                        .HasColumnType("date");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("character varying(25)");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("TweetTweet", b =>
                {
                    b.HasOne("TwitterAPI.Application.Domain.Tweet", null)
                        .WithMany()
                        .HasForeignKey("RepliesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TwitterAPI.Application.Domain.Tweet", null)
                        .WithMany()
                        .HasForeignKey("TweetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TweetUser", b =>
                {
                    b.HasOne("TwitterAPI.Application.Domain.Tweet", null)
                        .WithMany()
                        .HasForeignKey("LikeHistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TwitterAPI.Application.Domain.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TwitterAPI.Application.Domain.Tweet", b =>
                {
                    b.HasOne("TwitterAPI.Application.Domain.User", "Owner")
                        .WithMany("Tweets")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TwitterAPI.Application.Domain.Tweet", "ReplyTo")
                        .WithMany()
                        .HasForeignKey("ReplyToId");

                    b.Navigation("Owner");

                    b.Navigation("ReplyTo");
                });

            modelBuilder.Entity("TwitterAPI.Application.Domain.User", b =>
                {
                    b.Navigation("Tweets");
                });
#pragma warning restore 612, 618
        }
    }
}
