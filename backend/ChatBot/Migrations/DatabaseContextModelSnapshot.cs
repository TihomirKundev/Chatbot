﻿// <auto-generated />
using System;
using ChatBot.Repositories.EFC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChatBot.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ChatBot.Models.Conversation", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("ChatBot.Models.DTOs.TicketDTO", b =>
                {
                    b.Property<string>("ticketnumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.HasKey("ticketnumber");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("ChatBot.Models.Message", b =>
                {
                    b.Property<long?>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long?>("ID"));

                    b.Property<Guid>("AuthorID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ConversationID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Timestamp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("ID");

                    b.HasIndex("AuthorID");

                    b.HasIndex("ConversationID");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("ChatBot.Models.Participant", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ConversationID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("ConversationID");

                    b.ToTable("Participant");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Participant");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("ChatBot.Models.User", b =>
                {
                    b.HasBaseType("ChatBot.Models.Participant");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("User");
                });

            modelBuilder.Entity("ChatBot.Models.Message", b =>
                {
                    b.HasOne("ChatBot.Models.Participant", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChatBot.Models.Conversation", null)
                        .WithMany("Messages")
                        .HasForeignKey("ConversationID");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("ChatBot.Models.Participant", b =>
                {
                    b.HasOne("ChatBot.Models.Conversation", null)
                        .WithMany("Participants")
                        .HasForeignKey("ConversationID");
                });

            modelBuilder.Entity("ChatBot.Models.Conversation", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("Participants");
                });
#pragma warning restore 612, 618
        }
    }
}
