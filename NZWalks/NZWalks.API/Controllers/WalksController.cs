using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;
using System.Collections.Generic;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync() {
            var walks = await walkRepository.GetAllAsync();

            var walksDTO = mapper.Map<List<Models.DTOs.Walk>>(walks);

            return Ok(walksDTO);   
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //get domain
            var walk = await walkRepository.GetAsync(id);

            //convert domain to dto
            var walkDTO = mapper.Map<Models.DTOs.Walk>(walk);

            //return response
            if (walkDTO == null) { 
                return NotFound();
            }
            return Ok(walkDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody]Models.DTOs.AddWalkRequest addWalkRequest)
        {
            //convert DTO to domain
            var walkDomain = mapper.Map<Models.Domains.Walk>(addWalkRequest);

            //call repository
            walkDomain = await walkRepository.AddAsync(walkDomain);

            //convert back to DTO
            var walkDTO = mapper.Map<Models.DTOs.Walk>(walkDomain);

            //send response
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTOs.UpdateWalkRequest updateWalkRequest)
        {

            //convert dto to domain
            var walk = mapper.Map<Models.Domains.Walk>(updateWalkRequest);

            //call repository
            walk = await walkRepository.UpdateAsync(id, walk);

            //convert back to DTO
            var walkDTO = mapper.Map<Models.DTOs.Walk>(walk);

            //return response
            if (walkDTO == null) {
                return NotFound();
            }
            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            //call repository
            var walk = await walkRepository.DeleteAsync(id);

            //return response
            if (walk == null)
            {
                return NotFound();
            }
            mapper.Map<Models.DTOs.Walk>(walk);
            return Ok(walk);
        }
    }
}
