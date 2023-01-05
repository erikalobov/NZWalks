using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var walkDifficultiesDomain = await walkDifficultyRepository.GetAllAsync();

            return Ok(mapper.Map<List<Models.DTOs.WalkDifficulty>>(walkDifficultiesDomain));
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetWalkDifficulty(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.GetAsync(id);

            if (walkDifficulty == null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = mapper.Map<Models.DTOs.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficulty(WalkDifficultyRequest walkDifficultyRequest)
        {
            if (!ValidateWalkDifficulty(walkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }

            var walkDifficulty = mapper.Map<Models.Domains.WalkDifficulty>(walkDifficultyRequest);

            walkDifficulty = await walkDifficultyRepository.AddAsync(walkDifficulty);

            var walkDifficultyDTO = mapper.Map<Models.DTOs.WalkDifficulty>(walkDifficulty);

            return CreatedAtAction(nameof(GetWalkDifficulty), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateWalkDifficulty(Guid id, WalkDifficultyRequest walkDifficultyRequest)
        {
            if (!ValidateWalkDifficulty(walkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }

            var walkDifficulty = mapper.Map<Models.Domains.WalkDifficulty>(walkDifficultyRequest);

            walkDifficulty = await walkDifficultyRepository.UpdateAsync(id, walkDifficulty);

            if (walkDifficulty == null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = mapper.Map<Models.DTOs.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficulty(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.DeleteAsync(id);

            if (walkDifficulty == null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = mapper.Map<Models.DTOs.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }

        #region private methods
        private bool ValidateWalkDifficulty(WalkDifficultyRequest walkDifficultyRequest)
        {
            if (walkDifficultyRepository == null)
            {
                ModelState.AddModelError(nameof(walkDifficultyRequest), $"is required.");
                return false;   
            }

            if (string.IsNullOrWhiteSpace(walkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(walkDifficultyRequest.Code), $"{nameof(walkDifficultyRequest.Code)} cannot be null or empty.");
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
