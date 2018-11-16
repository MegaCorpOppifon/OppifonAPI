﻿// <auto-generated />
using System;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DAL.Models.Appointment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndTime");

                    b.Property<int>("MaxParticipants");

                    b.Property<Guid>("OwnerId");

                    b.Property<DateTime>("StartTime");

                    b.Property<string>("Text");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("DAL.Models.Calendar", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<TimeSpan>("DefaultDuration");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Calendars");
                });

            modelBuilder.Entity("DAL.Models.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("DAL.Models.DayOff", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CalendarId");

                    b.Property<DateTime>("OffDay");

                    b.HasKey("Id");

                    b.HasIndex("CalendarId");

                    b.ToTable("DaysOff");
                });

            modelBuilder.Entity("DAL.Models.ManyToMany.CalendarAppointment", b =>
                {
                    b.Property<Guid>("CalendarId");

                    b.Property<Guid>("AppointmentId");

                    b.HasKey("CalendarId", "AppointmentId");

                    b.HasIndex("AppointmentId");

                    b.ToTable("CalendarAppointments");
                });

            modelBuilder.Entity("DAL.Models.ManyToMany.ExpertTag", b =>
                {
                    b.Property<Guid>("ExpertId");

                    b.Property<Guid>("TagId");

                    b.HasKey("ExpertId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("ExpertTags");
                });

            modelBuilder.Entity("DAL.Models.ManyToMany.MainFieldTag", b =>
                {
                    b.Property<Guid>("ExpertId");

                    b.Property<Guid>("TagId");

                    b.HasKey("ExpertId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("MainFieldTags");
                });

            modelBuilder.Entity("DAL.Models.ManyToMany.UserFavorites", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("ExpertId");

                    b.HasKey("UserId", "ExpertId");

                    b.HasIndex("ExpertId");

                    b.ToTable("UserFavorites");
                });

            modelBuilder.Entity("DAL.Models.ManyToMany.UserTag", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("TagId");

                    b.HasKey("UserId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("UserTags");
                });

            modelBuilder.Entity("DAL.Models.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Anonymity");

                    b.Property<Guid?>("ExpertId");

                    b.Property<string>("Name");

                    b.Property<int>("Rating");

                    b.Property<string>("ReviewText");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("ExpertId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("DAL.Models.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CategoryId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("DAL.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<DateTime>("Birthday");

                    b.Property<string>("City");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("Gender");

                    b.Property<byte[]>("Image");

                    b.Property<bool>("IsExpert");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("DAL.Models.WorkDay", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CalendarId");

                    b.Property<int>("DayOfWeek");

                    b.Property<DateTime>("EndHour");

                    b.Property<DateTime>("StartHour");

                    b.HasKey("Id");

                    b.HasIndex("CalendarId");

                    b.ToTable("WorkDays");
                });

            modelBuilder.Entity("DAL.Models.Expert", b =>
                {
                    b.HasBaseType("DAL.Models.User");

                    b.Property<string>("Description");

                    b.Property<Guid?>("ExpertCategoryId");

                    b.HasIndex("ExpertCategoryId");

                    b.ToTable("Expert");

                    b.HasDiscriminator().HasValue("Expert");
                });

            modelBuilder.Entity("DAL.Models.Calendar", b =>
                {
                    b.HasOne("DAL.Models.User", "User")
                        .WithOne("Calendar")
                        .HasForeignKey("DAL.Models.Calendar", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DAL.Models.DayOff", b =>
                {
                    b.HasOne("DAL.Models.Calendar")
                        .WithMany("DaysOff")
                        .HasForeignKey("CalendarId");
                });

            modelBuilder.Entity("DAL.Models.ManyToMany.CalendarAppointment", b =>
                {
                    b.HasOne("DAL.Models.Appointment", "Appointment")
                        .WithMany("Participants")
                        .HasForeignKey("AppointmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DAL.Models.Calendar", "Calendar")
                        .WithMany("Appointments")
                        .HasForeignKey("CalendarId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DAL.Models.ManyToMany.ExpertTag", b =>
                {
                    b.HasOne("DAL.Models.Expert", "Expert")
                        .WithMany("ExpertTags")
                        .HasForeignKey("ExpertId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DAL.Models.Tag", "Tag")
                        .WithMany("ExpertsTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DAL.Models.ManyToMany.MainFieldTag", b =>
                {
                    b.HasOne("DAL.Models.Expert", "Expert")
                        .WithMany("MainFields")
                        .HasForeignKey("ExpertId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DAL.Models.Tag", "Tag")
                        .WithMany("ExpertsMainField")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DAL.Models.ManyToMany.UserFavorites", b =>
                {
                    b.HasOne("DAL.Models.Expert", "Expert")
                        .WithMany("Subscribers")
                        .HasForeignKey("ExpertId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("DAL.Models.User", "User")
                        .WithMany("Favorites")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("DAL.Models.ManyToMany.UserTag", b =>
                {
                    b.HasOne("DAL.Models.Tag", "Tag")
                        .WithMany("UsersTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DAL.Models.User", "User")
                        .WithMany("InterestTags")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DAL.Models.Review", b =>
                {
                    b.HasOne("DAL.Models.Expert")
                        .WithMany("Reviews")
                        .HasForeignKey("ExpertId");
                });

            modelBuilder.Entity("DAL.Models.Tag", b =>
                {
                    b.HasOne("DAL.Models.Category")
                        .WithMany("Tags")
                        .HasForeignKey("CategoryId");
                });

            modelBuilder.Entity("DAL.Models.WorkDay", b =>
                {
                    b.HasOne("DAL.Models.Calendar")
                        .WithMany("WorkDays")
                        .HasForeignKey("CalendarId");
                });

            modelBuilder.Entity("DAL.Models.Expert", b =>
                {
                    b.HasOne("DAL.Models.Category", "ExpertCategory")
                        .WithMany("Experts")
                        .HasForeignKey("ExpertCategoryId")
                        .OnDelete(DeleteBehavior.SetNull);
                });
#pragma warning restore 612, 618
        }
    }
}
