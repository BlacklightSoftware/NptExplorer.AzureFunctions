using System.Collections.Generic;

#nullable disable

namespace NptExplorer.AzureFunctions.Models
{
    public partial class Location
    {
        public Location()
        {
            Badges = new HashSet<Badge>();
            DefaultLocations = new HashSet<DefaultLocation>();
            LocationActivities = new HashSet<LocationActivity>();
            LocationBusRoutes = new HashSet<LocationBusRoute>();
            LocationFacilities = new HashSet<LocationFacility>();
            LocationHabitats = new HashSet<LocationHabitat>();
            LocationHighlights = new HashSet<LocationHighlight>();
            LocationPointOfInterests = new HashSet<LocationPointOfInterest>();
            LocationRatings = new HashSet<LocationRating>();
            LocationTrails = new HashSet<LocationTrail>();
        }

        public int Id { get; set; }
        public string NameEnglish { get; set; }
        public string NameWelsh { get; set; }
        public string DescriptionEnglish { get; set; }
        public string DescriptionWelsh { get; set; }
        public string PrimaryImage { get; set; }
        public string MapImage { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string What3Words { get; set; }
        public string NearestBusStop { get; set; }
        public string Website { get; set; }
        public string Parking { get; set; }
        public string ParkingCharge { get; set; }
        public string GeneralInformation { get; set; }
        public string ResourceLink { get; set; }
        public string GetInvolved { get; set; }
        public string GetInvolvedLink { get; set; }
        public string LearnMore { get; set; }
        public string LearnMoreLink { get; set; }
        public string Businesses { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public bool ExploreDefaultSelection { get; set; }
        public bool ChallengeDefaultSelection { get; set; }

        public virtual ICollection<Badge> Badges { get; set; }
        public virtual ICollection<DefaultLocation> DefaultLocations { get; set; }
        public virtual ICollection<LocationActivity> LocationActivities { get; set; }
        public virtual ICollection<LocationBusRoute> LocationBusRoutes { get; set; }
        public virtual ICollection<LocationFacility> LocationFacilities { get; set; }
        public virtual ICollection<LocationHabitat> LocationHabitats { get; set; }
        public virtual ICollection<LocationHighlight> LocationHighlights { get; set; }
        public virtual ICollection<LocationPointOfInterest> LocationPointOfInterests { get; set; }
        public virtual ICollection<LocationRating> LocationRatings { get; set; }
        public virtual ICollection<LocationTrail> LocationTrails { get; set; }
    }
}
