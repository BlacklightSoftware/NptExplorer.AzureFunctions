using System.Collections.Generic;
using System.Linq;
using NptExplorer.AzureFunctions.Models;
using NptExplorer.Dto.Models;

namespace NptExplorer.AzureFunctions.Helpers;

public static class TrailHelper
{
    public static List<TrailDto> BuildTrailDtoList(List<Trail> trails)
    {
        var trailDtoList = new List<TrailDto>();

        foreach (var trail in trails)
        {
            var lt = trail.LocationTrails.FirstOrDefault();
            if (lt == null) continue;

            var trailDto = new TrailDto
            {
                Id = trail.Id,
                NameEnglish = trail.NameEnglish,
                NameWelsh = trail.NameWelsh,
                TrailImage = trail.TrailImage,
                TrailMapImage = trail.TrailMapImage,
                Difficulty = trail.DifficultyId,
                DistanceMiles = trail.DistanceMiles,
                DistanceKm = trail.DistanceKm,
                TimeHours = trail.TimeHours,
                TimeMinutes = trail.TimeMinutes,
                StartingPosition = new LatLongDto
                { Latitude = (double)trail.StartLatitude, Longitude = (double)trail.StartLongitude },
                StartPointDetailsEnglish = trail.StartPointDetailsEnglish,
                StartPointDetailsWelsh = trail.StartPointDetailsWelsh,
                LocationId = lt.LocationId,
                LocationNameEnglish = lt.Location.NameEnglish,
                LocationNameWelsh = lt.Location.NameWelsh
            };

            trailDtoList.Add(trailDto);
        }

        return trailDtoList;
    }
    public static TrailDto BuildTrailDto(Trail trail, bool badgeCollected)
    {
        var trailDto = new TrailDto();

        var lt = trail.LocationTrails.FirstOrDefault();
        if (lt == null) return trailDto;

        trailDto.Id = trail.Id;
        trailDto.NameEnglish = trail.NameEnglish;
        trailDto.NameWelsh = trail.NameWelsh;
        trailDto.TrailImage = trail.TrailImage;
        trailDto.TrailMapImage = trail.TrailMapImage;
        trailDto.Difficulty = trail.DifficultyId;
        trailDto.DistanceMiles = trail.DistanceMiles;
        trailDto.DistanceKm = trail.DistanceKm;
        trailDto.TimeHours = trail.TimeHours;
        trailDto.TimeMinutes = trail.TimeMinutes;
        trailDto.StartingPosition = new LatLongDto
        { Latitude = (double)trail.StartLatitude, Longitude = (double)trail.StartLongitude };
        trailDto.StartPointDetailsEnglish = trail.StartPointDetailsEnglish;
        trailDto.StartPointDetailsWelsh = trail.StartPointDetailsWelsh;
        if (trail.Badges.Any())
        {
            trailDto.BadgeId = trail.Badges.First().Id;
        }
        trailDto.Collected = badgeCollected;

        trailDto.LocationId = lt.LocationId;
        trailDto.LocationNameEnglish = lt.Location.NameEnglish;
        trailDto.LocationNameWelsh = lt.Location.NameWelsh;

        return trailDto;
    }

    public static TrailRouteDto BuildTrailRouteDto(Trail trail, Models.Transient.TrailRouteResponse route)
    {
        var trailDto = new TrailRouteDto();

        var lt = trail.LocationTrails.FirstOrDefault();
        if (lt == null) return trailDto;

        trailDto.Id = trail.Id;
        trailDto.NameEnglish = trail.NameEnglish;
        trailDto.NameWelsh = trail.NameWelsh;
        trailDto.TrailImage = trail.TrailImage;
        trailDto.TrailMapImage = trail.TrailMapImage;
        trailDto.Difficulty = trail.DifficultyId;
        trailDto.DistanceMiles = trail.DistanceMiles;
        trailDto.DistanceKm = trail.DistanceKm;
        trailDto.TimeHours = trail.TimeHours;
        trailDto.TimeMinutes = trail.TimeMinutes;
        trailDto.StartingPosition = new LatLongDto
        { Latitude = (double)trail.StartLatitude, Longitude = (double)trail.StartLongitude };
        trailDto.StartPointDetailsEnglish = trail.StartPointDetailsEnglish;
        trailDto.StartPointDetailsWelsh = trail.StartPointDetailsWelsh;

        trailDto.LocationId = lt.LocationId;
        trailDto.LocationNameEnglish = lt.Location.NameEnglish;
        trailDto.LocationNameWelsh = lt.Location.NameWelsh;

        var routeList = new List<List<LatLongDto>>();
        foreach (var allGroups in route.geometry.coordinates)
        {
            var outerCords = new List<LatLongDto>();
            foreach (var cord in allGroups)
            {
                outerCords.Add(new LatLongDto(){ Longitude = cord[0], Latitude = cord[1] });
            }
            routeList.Add(outerCords);
        }

        trailDto.Route = routeList;

        return trailDto;
    }
}