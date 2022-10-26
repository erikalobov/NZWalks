﻿namespace NZWalks.API.Models.DTOs
{
    public class Region
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Area { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public long Population { get; set; }
    }
}
