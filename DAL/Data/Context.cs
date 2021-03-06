﻿using DAL.Models;
using DAL.Models.ManyToMany;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        { }

        public Context(string connectionString) : base(GetOptions(connectionString))
        { }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return new DbContextOptionsBuilder().UseSqlServer(connectionString)
                .Options;
        } 

        public DbSet<User> Users { get; set; }
        public DbSet<Expert> Experts { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<WorkDay> WorkDays { get; set; }
        public DbSet<DayOff> DaysOff { get; set; }
        public DbSet<CalendarAppointment> CalendarAppointments { get; set; }
        public DbSet<ExpertTag> ExpertTags { get; set; }
        public DbSet<UserTag> UserTags { get; set; }
        public DbSet<MainFieldTag> MainFieldTags { get; set; }
        public DbSet<UserFavorites> UserFavorites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // One to One User and Calendar
            modelBuilder.Entity<User>()
                .HasOne(u => u.Calendar)
                .WithOne(c => c.User)
                .HasForeignKey<Calendar>(u => u.UserId);

            // Expert inherit from user
            modelBuilder.Entity<Expert>()
                .HasBaseType<User>();

            // One to May Category and Expert
            modelBuilder.Entity<Expert>()
                .HasOne(x => x.ExpertCategory)
                .WithMany(x => x.Experts)
                .OnDelete(DeleteBehavior.SetNull);

            // Many to many User and Expert
            modelBuilder.Entity<UserFavorites>()
                .HasKey(uf => new {uf.UserId, uf.ExpertId});

            modelBuilder.Entity<UserFavorites>()
                .HasOne(e => e.Expert)
                .WithMany(e => e.Subscribers)
                .HasForeignKey(e => e.ExpertId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserFavorites>()
                .HasOne(e => e.User)
                .WithMany(e => e.Favorites)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Many to many experts and tags
            modelBuilder.Entity<MainFieldTag>()
                .HasKey(e => new { e.ExpertId, e.TagId });

            modelBuilder.Entity<MainFieldTag>()
                .HasOne(et => et.Expert)
                .WithMany(e => e.MainFields)
                .HasForeignKey(et => et.ExpertId);

            modelBuilder.Entity<MainFieldTag>()
                .HasOne(et => et.Tag)
                .WithMany(t => t.ExpertsMainField)
                .HasForeignKey(et => et.TagId);

            // Many to many experts and tags
            modelBuilder.Entity<ExpertTag>()
               .HasKey(e => new { e.ExpertId, e.TagId });

            modelBuilder.Entity<ExpertTag>()
                .HasOne(et => et.Expert)
                .WithMany(e => e.ExpertTags)
                .HasForeignKey(et => et.ExpertId);

            modelBuilder.Entity<ExpertTag>()
                .HasOne(et => et.Tag)
                .WithMany(t => t.ExpertsTags)
                .HasForeignKey(et => et.TagId);

            // Many to many users and tags
            modelBuilder.Entity<UserTag>()
                .HasKey(u => new { u.UserId, u.TagId });

            modelBuilder.Entity<UserTag>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.InterestTags)
                .HasForeignKey(ut => ut.UserId);

            modelBuilder.Entity<UserTag>()
                .HasOne(ut => ut.Tag)
                .WithMany(t => t.UsersTags)
                .HasForeignKey(et => et.TagId);

            // Many to many Calendars and Appointments
            modelBuilder.Entity<CalendarAppointment>()
                .HasKey(u => new { u.CalendarId, u.AppointmentId });

            modelBuilder.Entity<CalendarAppointment>()
                .HasOne(ua => ua.Calendar)
                .WithMany(u => u.Appointments)
                .HasForeignKey(ut => ut.CalendarId);

            modelBuilder.Entity<CalendarAppointment>()
                .HasOne(ua => ua.Appointment)
                .WithMany(t => t.Participants)
                .HasForeignKey(et => et.AppointmentId);
        }
    }
}
