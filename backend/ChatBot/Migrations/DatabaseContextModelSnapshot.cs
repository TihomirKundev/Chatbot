﻿// <auto-generated />
using System;
using ChatBot.Repositories.EFC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ChatBot.Models.Conversation", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("ChatBot.Models.DTOs.TicketDTO", b =>
                {
                    b.Property<string>("ticketnumber")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("longtext");

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

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("ConversationID")
                        .HasColumnType("char(36)");

                    b.HasKey("ID");

                    b.HasIndex("ConversationID");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("ChatBot.Models.Participant", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("ConversationID")
                        .HasColumnType("char(36)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.HasIndex("ConversationID");

                    b.ToTable("Participant");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Participant");
                });

            modelBuilder.Entity("ChatBot.Models.User", b =>
                {
                    b.HasBaseType("ChatBot.Models.Participant");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasDiscriminator().HasValue("User");
                });

            modelBuilder.Entity("ChatBot.Models.Message", b =>
                {
                    b.HasOne("ChatBot.Models.Conversation", null)
                        .WithMany("Messages")
                        .HasForeignKey("ConversationID");
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
