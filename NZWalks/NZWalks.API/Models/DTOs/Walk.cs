﻿using NZWalks.API.Models.Domains;

namespace NZWalks.API.Models.DTOs
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Lenght { get; set; }
        public Guid RegionId { get; set; }
        public Guid WalkDifficultyId { get; set; }

        //Navigation Properties
        public Region Region { get; set; }
        public WalkDifficulty WalkDifficulty { get; set; }
    }
}