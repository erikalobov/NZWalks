using AutoMapper;

namespace NZWalks.API.Profiles
{
    public class WalkDifficultyProfile : Profile
    {
        public WalkDifficultyProfile()
        {
            CreateMap<Models.Domains.WalkDifficulty, Models.DTOs.WalkDifficulty>().ReverseMap();
            CreateMap<Models.Domains.WalkDifficulty, Models.DTOs.WalkDifficultyRequest>().ReverseMap();
        }    
    }
}
