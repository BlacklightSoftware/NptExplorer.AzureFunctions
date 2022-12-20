using NptExplorer.AzureFunctions.Models;
using NptExplorer.Dto.Models;
using System.Collections.Generic;
using System.Linq;

namespace NptExplorer.AzureFunctions.Helpers;

public static class ChallengeHelper
{
    public static List<ChallengeOverviewDto> BuildChallengeOverviewDtoList(List<Location> locations, List<UserBadge> userBadges)
    {
        var challengeDtoList = new List<ChallengeOverviewDto>();

        foreach (var location in locations)
        {
            var challengeDto = new ChallengeOverviewDto();
            challengeDto.LocationId = location.Id;
            challengeDto.LocationNameEnglish = location.NameEnglish;
            challengeDto.LocationNameWelsh = location.NameWelsh;
            challengeDto.ChallengeImage = location.PrimaryImage;
            challengeDto.Badges = new List<ChallengeBadgeDto>();

            foreach (var badge in location.Badges)
            {
                var badgeDto = new ChallengeBadgeDto();
                badgeDto.BadgeId = badge.Id;
                badgeDto.BadgeTypeId = badge.BadgeTypeId;
                if (badge.TrailId > 0)
                {
                    badgeDto.TrailId = badge.TrailId;
                }
                else
                {
                    badgeDto.PointOfInterestId = badge.PointOfInterestId;
                }
                if (userBadges.Any(x => x.BadgeId == badge.Id))
                {
                    badgeDto.Collected = true;
                }

                challengeDto.Badges.Add(badgeDto);
            }
            challengeDtoList.Add(challengeDto);
        }

        return challengeDtoList;
    }

    public static ChallengeDto BuildChallengeDto(Location location, List<UserBadge> userBadges)
    {
        var challengeDto = new ChallengeDto
        {
            LocationId = location.Id,
            LocationNameEnglish = location.NameEnglish,
            LocationNameWelsh = location.NameWelsh,
            Position = new LatLongDto { Latitude = (double)location.Latitude, Longitude = (double)location.Longitude },
            PointsOfInterest = new List<PointOfInterestDto>()
        };

        foreach (var badge in location.Badges.Where(x => x.PointOfInterestId != null))
        {
            var poiDto = new PointOfInterestDto
                {
                    Id = badge.PointOfInterest.Id,
                    NameEnglish = badge.PointOfInterest.NameEnglish,
                    NameWelsh = badge.PointOfInterest.NameWelsh,
                    DescriptionEnglish = badge.PointOfInterest.DescriptionEnglish,
                    DescriptionWelsh = badge.PointOfInterest.DescriptionWelsh,
                    Image = badge.PointOfInterest.Image,
                    Position = new LatLongDto{ Latitude = (double)badge.PointOfInterest.Latitude, Longitude = (double)badge.PointOfInterest.Longitude},
                    BadgeId = badge.Id,
                    BadgeTypeId = badge.BadgeTypeId
                };

            if (userBadges.Any(x => x.BadgeId == badge.Id))
            {
                poiDto.Collected = true;
            }

            challengeDto.PointsOfInterest.Add(poiDto);
        }

        return challengeDto;
    }
}