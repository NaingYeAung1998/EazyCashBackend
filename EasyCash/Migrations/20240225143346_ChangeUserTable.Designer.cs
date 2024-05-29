﻿// <auto-generated />
using System;
using EasyCash.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EasyCash.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240225143346_ChangeUserTable")]
    partial class ChangeUserTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("EasyCash.Entities.Advertisment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AdvertismentStatus")
                        .HasColumnType("int");

                    b.Property<int>("AdvertismentType")
                        .HasColumnType("int");

                    b.Property<decimal>("AmountPerWatch")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Advertisments");
                });

            modelBuilder.Entity("EasyCash.Entities.Membership", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("RenewalDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("SubscriptionAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("SubscriptionDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Memberships");
                });

            modelBuilder.Entity("EasyCash.Entities.PaymentMethod", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PaymentMethods");
                });

            modelBuilder.Entity("EasyCash.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TransactionStatus")
                        .HasColumnType("int");

                    b.Property<int>("TransactionType")
                        .HasColumnType("int");

                    b.Property<Guid?>("WalletId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("WalletPaymentMethodId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("WalletId");

                    b.HasIndex("WalletPaymentMethodId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("EasyCash.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EasyCash.Entities.UserAdvertismentView", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AdvertismentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("EarnedAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("WatchDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AdvertismentId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAdvertismentViews");
                });

            modelBuilder.Entity("EasyCash.Entities.Wallet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("EasyCash.Entities.WalletPaymentMethod", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("PaymentMethodId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("WalletId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PaymentMethodId");

                    b.HasIndex("WalletId");

                    b.ToTable("WalletPaymentMethods");
                });

            modelBuilder.Entity("EasyCash.Entities.Membership", b =>
                {
                    b.HasOne("EasyCash.Entities.User", "User")
                        .WithOne("Membership")
                        .HasForeignKey("EasyCash.Entities.Membership", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EasyCash.Entities.Transaction", b =>
                {
                    b.HasOne("EasyCash.Entities.Wallet", null)
                        .WithMany("Transactions")
                        .HasForeignKey("WalletId");

                    b.HasOne("EasyCash.Entities.WalletPaymentMethod", "WalletPaymentMethod")
                        .WithMany()
                        .HasForeignKey("WalletPaymentMethodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WalletPaymentMethod");
                });

            modelBuilder.Entity("EasyCash.Entities.UserAdvertismentView", b =>
                {
                    b.HasOne("EasyCash.Entities.Advertisment", "Advertisment")
                        .WithMany()
                        .HasForeignKey("AdvertismentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EasyCash.Entities.User", "User")
                        .WithMany("UserAdvertismentViews")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Advertisment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EasyCash.Entities.Wallet", b =>
                {
                    b.HasOne("EasyCash.Entities.User", "User")
                        .WithOne("Wallet")
                        .HasForeignKey("EasyCash.Entities.Wallet", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EasyCash.Entities.WalletPaymentMethod", b =>
                {
                    b.HasOne("EasyCash.Entities.PaymentMethod", "PaymentMethods")
                        .WithMany("WalletPaymentMethods")
                        .HasForeignKey("PaymentMethodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EasyCash.Entities.Wallet", "Wallet")
                        .WithMany("WalletPaymentMethods")
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PaymentMethods");

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("EasyCash.Entities.PaymentMethod", b =>
                {
                    b.Navigation("WalletPaymentMethods");
                });

            modelBuilder.Entity("EasyCash.Entities.User", b =>
                {
                    b.Navigation("Membership");

                    b.Navigation("UserAdvertismentViews");

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("EasyCash.Entities.Wallet", b =>
                {
                    b.Navigation("Transactions");

                    b.Navigation("WalletPaymentMethods");
                });
#pragma warning restore 612, 618
        }
    }
}
