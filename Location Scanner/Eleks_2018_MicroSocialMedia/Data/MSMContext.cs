using Eleks_2018_MicroSocialMedia.WriteModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Data
{
    public class MSMContext : IdentityDbContext<AppUser>
    {
        public MSMContext(DbContextOptions options) 
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Profile>()
                .Property(p => p.ExternalId)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasValueGenerator<ProfileIdValueGenerator>();

            builder.Entity<Friend>()
                .HasOne(a => a.RequestedBy)
                .WithMany(b => b.SentFriendRequests)
                .HasForeignKey(c => c.RequestedById);

            builder.Entity<Friend>()
                .HasOne(a => a.RequestedTo)
                .WithMany(b => b.ReceievedFriendRequests)
                .HasForeignKey(c => c.RequestedToId);


            builder.Entity<Device>()
                .HasOne(p => p.Profile)
                .WithMany(p => p.Devices)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Profile>()
                .HasOne(a => a.User)
                .WithOne(b => b.Profile)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Geolocation>()
                .HasOne(p => p.Profile)
                .WithOne(p => p.Geolocation)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Meeting>()
                .HasOne(p => p.Profile)
                .WithMany(p => p.Meetings);

            builder.Entity<MessageGroupProfile>()
                .HasKey(bc => new { bc.MessageGroupId, bc.ProfileId });

            builder.Entity<MessageGroupProfile>()
                .HasOne(p => p.Profile)
                .WithMany(p => p.MessageGroups)
                .HasForeignKey(p => p.ProfileId);

            builder.Entity<MessageGroupProfile>()
                .HasOne(p => p.MessageGroup)
                .WithMany(p => p.Members)
                .HasForeignKey(p => p.MessageGroupId);

            base.OnModelCreating(builder);
        }
    }
}
