#nullable disable

using Microsoft.EntityFrameworkCore;
using NptExplorer.AzureFunctions.Models;

namespace NptExplorer.AzureFunctions.Context
{
    public partial class NptExplorerContext : DbContext
    {
        public NptExplorerContext()
        {
        }

        public NptExplorerContext(DbContextOptions<NptExplorerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<Badge> Badges { get; set; }
        public virtual DbSet<BadgeType> BadgeTypes { get; set; }
        public virtual DbSet<BadgePoint> BadgePoints { get; set; }
        public virtual DbSet<BusRoute> BusRoutes { get; set; }
        public virtual DbSet<CategoryPoint> CategoryPoints { get; set; }
        public virtual DbSet<CategoryPointBadgeType> CategoryPointBadgeTypes { get; set; }
        public virtual DbSet<DefaultLocation> DefaultLocations { get; set; }
        public virtual DbSet<Difficulty> Difficulties { get; set; }
        public virtual DbSet<Distance> Distances { get; set; }
        public virtual DbSet<Facility> Facilities { get; set; }
        public virtual DbSet<Habitat> Habitats { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LocationActivity> LocationActivities { get; set; }
        public virtual DbSet<LocationBusRoute> LocationBusRoutes { get; set; }
        public virtual DbSet<LocationFacility> LocationFacilities { get; set; }
        public virtual DbSet<LocationHabitat> LocationHabitats { get; set; }
        public virtual DbSet<LocationHighlight> LocationHighlights { get; set; }
        public virtual DbSet<LocationPointOfInterest> LocationPointOfInterests { get; set; }
        public virtual DbSet<LocationRating> LocationRatings { get; set; }
        public virtual DbSet<LocationTrail> LocationTrails { get; set; }
        public virtual DbSet<PointOfInterest> PointOfInterests { get; set; }
        public virtual DbSet<Time> Times { get; set; }
        public virtual DbSet<Trail> Trails { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserBadge> UserBadges { get; set; }
        public virtual DbSet<UserFriend> UserFriends { get; set; }
        public virtual DbSet<TrophyPoint> TrophyPoints { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Badge>(entity =>
            {
                entity.HasOne(d => d.BadgeType)
                    .WithMany(p => p.Badges)
                    .HasForeignKey(d => d.BadgeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Badges_BadgeTypes");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Badges)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Badges_Locations");

                entity.HasOne(d => d.PointOfInterest)
                    .WithMany(p => p.Badges)
                    .HasForeignKey(d => d.PointOfInterestId)
                    .HasConstraintName("FK_Badges_PointOfInterests");

                entity.HasOne(d => d.Trail)
                    .WithMany(p => p.Badges)
                    .HasForeignKey(d => d.TrailId)
                    .HasConstraintName("FK_Badges_Trails");
            });

            modelBuilder.Entity<BadgePoint>(entity =>
            {
                entity.Property(e => e.BadgeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BadgeType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CategoryPointBadgeType>(entity =>
            {
                entity.ToTable("CategoryPointBadgeType");

                entity.HasOne(d => d.BadgeType)
                    .WithMany(p => p.CategoryPointBadgeTypes)
                    .HasForeignKey(d => d.BadgeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoryPointsBadgeType_BadgeType");

                entity.HasOne(d => d.CategoryPoint)
                    .WithMany(p => p.CategoryPointBadgeTypes)
                    .HasForeignKey(d => d.CategoryPointId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoryPointsCategoryPoints_CategoryPoints");
            });

            modelBuilder.Entity<DefaultLocation>(entity =>
            {
                entity.HasOne(d => d.Location)
                    .WithMany(p => p.DefaultLocations)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DefaultLocation_Location");
            });

            modelBuilder.Entity<BusRoute>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Difficulty>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Distance>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Facility>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Habitat>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.Businesses).IsUnicode(false);

                entity.Property(e => e.DescriptionEnglish).IsUnicode(false);

                entity.Property(e => e.DescriptionWelsh).IsUnicode(false);

                entity.Property(e => e.GeneralInformation).IsUnicode(false);

                entity.Property(e => e.GetInvolved).IsUnicode(false);

                entity.Property(e => e.GetInvolvedLink)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Latitude).HasColumnType("decimal(12, 9)");

                entity.Property(e => e.LearnMore).IsUnicode(false);

                entity.Property(e => e.LearnMoreLink)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Longitude).HasColumnType("decimal(12, 9)");

                entity.Property(e => e.MapImage)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NameEnglish)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NameWelsh)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NearestBusStop)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Parking).IsUnicode(false);

                entity.Property(e => e.ParkingCharge)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PrimaryImage)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ResourceLink)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Website)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.What3Words)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LocationActivity>(entity =>
            {
                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.LocationActivities)
                    .HasForeignKey(d => d.ActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LocationActivities_Activities");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.LocationActivities)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LocationActivities_Locations");
            });

            modelBuilder.Entity<LocationBusRoute>(entity =>
            {
                entity.HasOne(d => d.BusRoute)
                    .WithMany(p => p.LocationBusRoutes)
                    .HasForeignKey(d => d.BusRouteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LocationBusRoutes_BusRoutes");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.LocationBusRoutes)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LocationBusRoutes_Locations");
            });

            modelBuilder.Entity<LocationFacility>(entity =>
            {
                entity.HasOne(d => d.Facility)
                    .WithMany(p => p.LocationFacilities)
                    .HasForeignKey(d => d.FacilityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LocationFacilities_Facilities");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.LocationFacilities)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LocationFacilities_Locations");
            });

            modelBuilder.Entity<LocationHabitat>(entity =>
            {
                entity.HasOne(d => d.Habitat)
                            .WithMany(p => p.LocationHabitats)
                            .HasForeignKey(d => d.HabitatId)
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_LocationHabitats_Habitats");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.LocationHabitats)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LocationHabitats_Locations");
            });

            modelBuilder.Entity<LocationHighlight>(entity =>
            {
                entity.Property(e => e.HighlightEnglish)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.HighlightWelsh)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Sequence).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.LocationHighlights)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LocationHighlights_Locations");
            });

            modelBuilder.Entity<LocationPointOfInterest>(entity =>
            {
                entity.ToTable("LocationPointOfInterest");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.LocationPointOfInterests)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LocationPointOfInterest_Locations");

                entity.HasOne(d => d.PointOfInterest)
                    .WithMany(p => p.LocationPointOfInterests)
                    .HasForeignKey(d => d.PointOfInterestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LocationPointOfInterest_PointOfInterests");
            });

            modelBuilder.Entity<LocationRating>(entity =>
            {
                entity.HasOne(d => d.Location)
                    .WithMany(p => p.LocationRatings)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LocationRatings_Locations");
            });

            modelBuilder.Entity<LocationTrail>(entity =>
            {
                entity.HasOne(d => d.Location)
                    .WithMany(p => p.LocationTrails)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LocationTrails_Locations");

                entity.HasOne(d => d.Trail)
                    .WithMany(p => p.LocationTrails)
                    .HasForeignKey(d => d.TrailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LocationTrails_Trails");
            });

            modelBuilder.Entity<PointOfInterest>(entity =>
            {
                entity.Property(e => e.DescriptionEnglish).IsUnicode(false);

                entity.Property(e => e.DescriptionWelsh).IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Latitude).HasColumnType("decimal(12, 9)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(12, 9)");

                entity.Property(e => e.NameEnglish)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NameWelsh)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Time>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Trail>(entity =>
            {
                entity.Property(e => e.DistanceKm).HasColumnType("numeric(5, 2)");

                entity.Property(e => e.DistanceMiles).HasColumnType("numeric(5, 2)");

                entity.Property(e => e.NameEnglish)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NameWelsh)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.StartLatitude).HasColumnType("decimal(12, 9)");

                entity.Property(e => e.StartLongitude).HasColumnType("decimal(12, 9)");

                entity.Property(e => e.StartPointDetailsEnglish)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.StartPointDetailsWelsh)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.TrailImage)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TrailMapImage)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TrailRouteApi).IsUnicode(false);

                entity.HasOne(d => d.Difficulty)
                    .WithMany(p => p.Trails)
                    .HasForeignKey(d => d.DifficultyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trails_Difficulties");

                entity.HasOne(d => d.Distance)
                    .WithMany(p => p.Trails)
                    .HasForeignKey(d => d.DistanceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trails_Distances");

                entity.HasOne(d => d.Time)
                    .WithMany(p => p.Trails)
                    .HasForeignKey(d => d.TimeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trails_Times");
            });

            modelBuilder.Entity<TrophyPoint>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserBadge>(entity =>
            {
                entity.HasOne(d => d.Badge)
                      .WithMany(p => p.UserBadges)
                      .HasForeignKey(d => d.BadgeId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_UserBadges_Badges");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserBadges)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserBadges_User");
            });

            modelBuilder.Entity<UserFriend>(entity =>
            {
                entity.HasOne(d => d.Friend)
                    .WithMany(p => p.UserFriendFriends)
                    .HasForeignKey(d => d.FriendId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserFriends_FriendId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserFriendUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserFriends_UserId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
