﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ThAmCo.Catering.Data;

#nullable disable

namespace ThAmCo.Catering.Migrations
{
    [DbContext(typeof(CateringDbContext))]
    [Migration("20231020210819_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.12");

            modelBuilder.Entity("ThAmCo.Catering.Data.FoodBooking", b =>
                {
                    b.Property<int>("FoodBookingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClientReferenceId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MenuId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberOfGuests")
                        .HasColumnType("INTEGER");

                    b.HasKey("FoodBookingId");

                    b.HasIndex("MenuId");

                    b.ToTable("FoodBookings");
                });

            modelBuilder.Entity("ThAmCo.Catering.Data.FoodItem", b =>
                {
                    b.Property<int>("FoodItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<float>("UnitPrice")
                        .HasColumnType("REAL");

                    b.HasKey("FoodItemId");

                    b.ToTable("FoodItems");
                });

            modelBuilder.Entity("ThAmCo.Catering.Data.Menu", b =>
                {
                    b.Property<int>("MenuId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("MenuName")
                        .HasColumnType("TEXT");

                    b.HasKey("MenuId");

                    b.ToTable("Menus");
                });

            modelBuilder.Entity("ThAmCo.Catering.Data.MenuFoodItem", b =>
                {
                    b.Property<int>("MenuId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FoodItemId")
                        .HasColumnType("INTEGER");

                    b.HasKey("MenuId", "FoodItemId");

                    b.HasIndex("FoodItemId");

                    b.ToTable("MenuFoodItems");
                });

            modelBuilder.Entity("ThAmCo.Catering.Data.FoodBooking", b =>
                {
                    b.HasOne("ThAmCo.Catering.Data.Menu", "Menu")
                        .WithMany()
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Menu");
                });

            modelBuilder.Entity("ThAmCo.Catering.Data.MenuFoodItem", b =>
                {
                    b.HasOne("ThAmCo.Catering.Data.FoodItem", "FoodItem")
                        .WithMany()
                        .HasForeignKey("FoodItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThAmCo.Catering.Data.Menu", "Menu")
                        .WithMany("MenuFoodItems")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FoodItem");

                    b.Navigation("Menu");
                });

            modelBuilder.Entity("ThAmCo.Catering.Data.Menu", b =>
                {
                    b.Navigation("MenuFoodItems");
                });
#pragma warning restore 612, 618
        }
    }
}
