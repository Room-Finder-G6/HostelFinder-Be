﻿// <auto-generated />
using System;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HostelFinder.Infrastructure.Migrations
{
    [DbContext(typeof(HostelFinderDbContext))]
    [Migration("20241031071443_Update-Context_v1.2")]
    partial class UpdateContext_v12
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HostelFinder.Domain.Entities.Address", b =>
                {
                    b.Property<Guid>("HostelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DetailAddress")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("District")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Province")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("commune")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("HostelId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Amenity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AmenityName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("Amenities");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.BlackListToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BlackListTokens");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Hostel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Coordinates")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("HostelName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid?>("LandlordId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("NumberOfRooms")
                        .HasColumnType("int");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<float?>("Size")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("LandlordId");

                    b.ToTable("Hostels");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.HostelService", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HostelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("HostelId");

                    b.HasIndex("ServiceId");

                    b.ToTable("HostelServices");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("HostelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("PostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("HostelId");

                    b.HasIndex("PostId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Invoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<Guid>("ServiceCostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("InVoices");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Membership", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("Memberships");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.MembershipServices", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("MembershipId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MembershipId");

                    b.ToTable("MembershipServices");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTime>("DateAvailable")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<Guid>("HostelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("MembershipServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("HostelId");

                    b.HasIndex("MembershipServiceId");

                    b.HasIndex("RoomId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("HostelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal>("MonthlyRentCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("RoomName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoomType")
                        .HasColumnType("int");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("HostelId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.RoomAmenities", b =>
                {
                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AmenityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RoomId", "AmenityId");

                    b.HasIndex("AmenityId");

                    b.ToTable("RoomAmenities");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.RoomDetails", b =>
                {
                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BathRooms")
                        .HasColumnType("int");

                    b.Property<int>("BedRooms")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("Kitchen")
                        .HasColumnType("int");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<int>("Size")
                        .HasColumnType("int");

                    b.HasKey("RoomId");

                    b.ToTable("RoomDetails");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Service", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.ServiceCost", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("CurrentReading")
                        .HasColumnType("int");

                    b.Property<Guid?>("InVoiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("PreviousReading")
                        .HasColumnType("int");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("unitCost")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("InVoiceId");

                    b.HasIndex("RoomId");

                    b.ToTable("ServiceCosts");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AvatarUrl")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsEmailConfirmed")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("bit");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordResetToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("PasswordResetTokenExpires")
                        .HasColumnType("datetime2");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.UserMembership", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MembershipId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("UserId", "MembershipId");

                    b.HasIndex("MembershipId");

                    b.ToTable("UserMemberships");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Wishlist", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Wishlists");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.WishlistPost", b =>
                {
                    b.Property<Guid>("WishlistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("WishlistId", "PostId");

                    b.HasIndex("PostId");

                    b.ToTable("WishlistPosts");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Address", b =>
                {
                    b.HasOne("HostelFinder.Domain.Entities.Hostel", "Hostel")
                        .WithOne("Address")
                        .HasForeignKey("HostelFinder.Domain.Entities.Address", "HostelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hostel");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Hostel", b =>
                {
                    b.HasOne("HostelFinder.Domain.Entities.User", "Landlord")
                        .WithMany("Hostels")
                        .HasForeignKey("LandlordId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Landlord");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.HostelService", b =>
                {
                    b.HasOne("HostelFinder.Domain.Entities.Hostel", "Hostel")
                        .WithMany("HostelServices")
                        .HasForeignKey("HostelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HostelFinder.Domain.Entities.Service", "Services")
                        .WithMany("HostelServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hostel");

                    b.Navigation("Services");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Image", b =>
                {
                    b.HasOne("HostelFinder.Domain.Entities.Hostel", "Hostel")
                        .WithMany("Images")
                        .HasForeignKey("HostelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HostelFinder.Domain.Entities.Post", "Post")
                        .WithMany("Images")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Hostel");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.MembershipServices", b =>
                {
                    b.HasOne("HostelFinder.Domain.Entities.Membership", "Membership")
                        .WithMany("MembershipServices")
                        .HasForeignKey("MembershipId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Membership");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Post", b =>
                {
                    b.HasOne("HostelFinder.Domain.Entities.Hostel", "Hostel")
                        .WithMany("Posts")
                        .HasForeignKey("HostelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HostelFinder.Domain.Entities.MembershipServices", "MembershipServices")
                        .WithMany("Posts")
                        .HasForeignKey("MembershipServiceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HostelFinder.Domain.Entities.Room", "Room")
                        .WithMany("Posts")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Hostel");

                    b.Navigation("MembershipServices");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Room", b =>
                {
                    b.HasOne("HostelFinder.Domain.Entities.Hostel", "Hostel")
                        .WithMany("Rooms")
                        .HasForeignKey("HostelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hostel");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.RoomAmenities", b =>
                {
                    b.HasOne("HostelFinder.Domain.Entities.Amenity", "Amenity")
                        .WithMany("RoomAmenities")
                        .HasForeignKey("AmenityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HostelFinder.Domain.Entities.Room", "Room")
                        .WithMany("RoomAmenities")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Amenity");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.RoomDetails", b =>
                {
                    b.HasOne("HostelFinder.Domain.Entities.Room", "Room")
                        .WithOne("RoomDetails")
                        .HasForeignKey("HostelFinder.Domain.Entities.RoomDetails", "RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.ServiceCost", b =>
                {
                    b.HasOne("HostelFinder.Domain.Entities.Invoice", "Invoice")
                        .WithMany("ServiceCost")
                        .HasForeignKey("InVoiceId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("HostelFinder.Domain.Entities.Room", "Room")
                        .WithMany("ServiceCost")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Invoice");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.UserMembership", b =>
                {
                    b.HasOne("HostelFinder.Domain.Entities.Membership", "Membership")
                        .WithMany("UserMemberships")
                        .HasForeignKey("MembershipId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HostelFinder.Domain.Entities.User", "User")
                        .WithMany("UserMemberships")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Membership");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Wishlist", b =>
                {
                    b.HasOne("HostelFinder.Domain.Entities.User", "User")
                        .WithOne("Wishlists")
                        .HasForeignKey("HostelFinder.Domain.Entities.Wishlist", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.WishlistPost", b =>
                {
                    b.HasOne("HostelFinder.Domain.Entities.Post", "Post")
                        .WithMany("WishlistPosts")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HostelFinder.Domain.Entities.Wishlist", "Wishlist")
                        .WithMany("WishlistPosts")
                        .HasForeignKey("WishlistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("Wishlist");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Amenity", b =>
                {
                    b.Navigation("RoomAmenities");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Hostel", b =>
                {
                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("HostelServices");

                    b.Navigation("Images");

                    b.Navigation("Posts");

                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Invoice", b =>
                {
                    b.Navigation("ServiceCost");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Membership", b =>
                {
                    b.Navigation("MembershipServices");

                    b.Navigation("UserMemberships");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.MembershipServices", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Post", b =>
                {
                    b.Navigation("Images");

                    b.Navigation("WishlistPosts");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Room", b =>
                {
                    b.Navigation("Posts");

                    b.Navigation("RoomAmenities");

                    b.Navigation("RoomDetails")
                        .IsRequired();

                    b.Navigation("ServiceCost");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Service", b =>
                {
                    b.Navigation("HostelServices");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.User", b =>
                {
                    b.Navigation("Hostels");

                    b.Navigation("UserMemberships");

                    b.Navigation("Wishlists");
                });

            modelBuilder.Entity("HostelFinder.Domain.Entities.Wishlist", b =>
                {
                    b.Navigation("WishlistPosts");
                });
#pragma warning restore 612, 618
        }
    }
}
