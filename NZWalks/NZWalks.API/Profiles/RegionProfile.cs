using AutoMapper;

namespace NZWalks.API.Profiles
{
public class RegionProfile : Profile
{
    public RegionProfile()
    {
        CreateMap<Models.Domains.Region, Models.DTOs.Region>()
            .ReverseMap();
        CreateMap<Models.Domains.Region, Models.DTOs.UpdateRegionRequest>()
            .ReverseMap();
    }
}
}
