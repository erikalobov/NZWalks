using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await regionRepository.GetAllAsync();

            var regionsDTO = mapper.Map<List<Models.DTOs.Region>>(regions);

            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            var regionDTO = mapper.Map<Models.DTOs.Region>(region);

            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTOs.AddRegionRequest addRegionRequest)
        {
            //Request DTO to Domain Model
            var region = new Models.Domains.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Lon = addRegionRequest.Lon,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population
            };

            //Call to repository
            region = await regionRepository.AddAsync(region);

            //Conversion to DTO
            var regionDTO = mapper.Map<Models.DTOs.Region>(region);

            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            var region = await regionRepository.DeleteAsync(id);

            if (region == null) { 
                return NotFound();
            }

            var regionDTO = mapper.Map<Models.DTOs.Region>(region);

            return Ok(regionDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute]Guid id, [FromBody] UpdateRegionRequest updateRegionRequest)
        {
            var region = mapper.Map<Models.Domains.Region>(updateRegionRequest);

            region = await regionRepository.UpdateAsync(id, region);

            if (region == null)
            {
                return NotFound();
            }

            var regionDTO = mapper.Map<Models.DTOs.Region>(region);

            return Ok(regionDTO);
        }

    }
}

