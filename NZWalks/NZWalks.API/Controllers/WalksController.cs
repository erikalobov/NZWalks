using AutoMapper;
using AutoMapper.Configuration.Annotations;
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
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
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
            if (!await ValidateAddWalkAsync(addWalkRequest))
            {
                return BadRequest(ModelState);
            }
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
            if (!await ValidateUpdateWalkAsync(updateWalkRequest))
            {
                return BadRequest(ModelState);
            }
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

        #region private methods
        private async Task<bool> ValidateAddWalkAsync(Models.DTOs.AddWalkRequest addWalkRequest)
        {
            if (addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest), $"Add Region Data is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)} cannot be null or empty.");
            }
            if (addWalkRequest.Lenght <= 0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Lenght), $"{nameof(addWalkRequest.Lenght)} cannot be less than or equal to zero.");
            }
            var region = await regionRepository.GetAsync(addWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)} is invalid.");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId), $"{nameof(addWalkRequest.WalkDifficultyId)} is invalid.");
            }


            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTOs.UpdateWalkRequest updateWalkRequest)
        {
            if (updateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest), $"Add Region Data is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name), $"{nameof(updateWalkRequest.Name)} cannot be null or empty.");
            }
            if (updateWalkRequest.Lenght <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Lenght), $"{nameof(updateWalkRequest.Lenght)} cannot be less than or equal to zero.");
            }
            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId), $"{nameof(updateWalkRequest.RegionId)} is invalid.");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId), $"{nameof(updateWalkRequest.WalkDifficultyId)} is invalid.");
            }


            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
