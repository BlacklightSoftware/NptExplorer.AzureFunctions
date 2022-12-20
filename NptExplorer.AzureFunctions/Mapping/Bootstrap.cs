using System.Linq;
using AutoMapper;
using NptExplorer.AzureFunctions.Models;
using NptExplorer.Dto.Models;

namespace NptExplorer.AzureFunctions.Mapping;

public class Bootstrap : Profile
{
    public Bootstrap()
    {
        this.LocationMapping();
        this.UserMapping();
        this.TokenMapping();
        this.ChallengeMapping();
    }

    private void LocationMapping()
    {
        CreateMap<Location, LocationDto>()
            .ForMember(dest => dest.Facilities, opt => opt.MapFrom((src => src.LocationFacilities.Select(x => x.FacilityId))))
            .ForMember(dest => dest.Activites, opt => opt.MapFrom((src => src.LocationActivities.Select(x => x.ActivityId))))
            .ForMember(dest => dest.Habitats, opt => opt.MapFrom((src => src.LocationHabitats.Select(x => x.HabitatId))))
            .ForMember(dest => dest.BusStops, opt => opt.MapFrom((src => src.LocationBusRoutes.Select(x => x.BusRoute.Name))))
			.ForMember(dest => dest.HighlightsList, opt => opt.MapFrom(src => src.LocationHighlights));
			
	        CreateMap<Location, LocationOverviewDto>()
            .ForMember(dest => dest.Facilities, opt => opt.MapFrom((src => src.LocationFacilities.Select(x => x.FacilityId))));

        CreateMap<LocationHighlight, HighlightDto>()
            .ForMember(dest => dest.Sequence, opt => opt.MapFrom((src => src.Sequence)))
            .ForMember(dest => dest.HighlightEnglish, opt => opt.MapFrom((src => src.HighlightEnglish)))
            .ForMember(dest => dest.HighlightWelsh, opt => opt.MapFrom((src => src.HighlightWelsh)));

    }

    private void ChallengeMapping()
    {
        CreateMap<PointOfInterest, PointOfInterestOverviewDto>();
        CreateMap<Trail, TrailOverviewDto>();
    }

    private void UserMapping()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Badges, opt => opt.MapFrom((src => src.UserBadges.Select(x => x.Badge))))
            .ForMember(dest => dest.BadgeCollection, opt => opt.MapFrom((src => src.UserBadges.Select(x => x.BadgeId))))
            .ForMember(dest => dest.Friends, opt => opt.MapFrom((src => src.UserFriendUsers.Select(x => x.FriendId))));

        CreateMap<Badge, ChallengeBadgeDto>();
    }

    private void TokenMapping()
    {
        CreateMap<BadgeType, BadgeTypeDto>();
        CreateMap<BadgePoint, BadgePointDto>();
        CreateMap<Badge, BadgeDto>()
            .ForMember(dest => dest.Badges, opt => opt.MapFrom((src => src.UserBadges.Select(x => x.Badge))));
        CreateMap<CategoryPoint, CategoryPointsDto>()
           .ForMember(dest => dest.BadgeType, opt => opt.MapFrom((src => src.CategoryPointBadgeTypes.Select(x => x.BadgeTypeId))));
    }

}