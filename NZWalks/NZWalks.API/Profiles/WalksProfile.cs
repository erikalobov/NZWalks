using AutoMapper;

namespace NZWalks.API.Profiles
{
    public class WalksProfile : Profile
    {
        public WalksProfile() { 
            CreateMap<Models.Domains.Walk, Models.DTOs.Walk>().ReverseMap();
            CreateMap<Models.Domains.Walk, Models.DTOs.AddWalkRequest>().ReverseMap();
            CreateMap<Models.Domains.Walk, Models.DTOs.UpdateWalkRequest>().ReverseMap();
            CreateMap<Models.Domains.WalkDifficulty, Models.DTOs.WalkDifficulty>().ReverseMap();
        }
    }
}
