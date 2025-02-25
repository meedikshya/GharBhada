using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GharBhada.Repositories.GenericRepositories;
using GharBhada.Repositories.SpecificRepositories.FavouriteRepositories;
using GharBhada.Models;
using AutoMapper;
using GharBhada.DTOs.FavouriteDTOs;

namespace GharBhada.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FavouritesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepositories _genericRepositories;
        private readonly IFavouriteRepositories _favouriteRepositories;


        public FavouritesController(IMapper mapper, IGenericRepositories genericRepositories, IFavouriteRepositories favouriteRepositories)
        {
            _mapper = mapper;
            _genericRepositories = genericRepositories;
            _favouriteRepositories = favouriteRepositories;
        }

        // GET: api/Favourites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FavouriteReadDTO>>> GetFavourites()
        {
            var favourites = await _genericRepositories.SelectAll<Favourite>();
            return Ok(_mapper.Map<IEnumerable<FavouriteReadDTO>>(favourites));
        }

        // GET: api/Favourites/user/27
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<FavouriteReadDTO>>> GetFavouritesByUserId(int userId)
        {
            var favourites = await _genericRepositories.SelectAll<Favourite>(f => f.UserId == userId);
            return Ok(_mapper.Map<IEnumerable<FavouriteReadDTO>>(favourites));
        }

        // GET: api/Favourites/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FavouriteReadDTO>> GetFavourite(int id)
        {
            var favourite = await _genericRepositories.SelectbyId<Favourite>(id);
            if (favourite == null)
            {
                return NotFound(new { message = "Favourite not found." });
            }
            return Ok(_mapper.Map<FavouriteReadDTO>(favourite));
        }

        // PUT: api/Favourites/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFavourite(int id, FavouriteUpdateDTO favouriteUpdateDTO)
        {
            if (id != favouriteUpdateDTO.FavouriteId)
            {
                return BadRequest(new { message = "Mismatched favourite ID." });
            }

            var existingFavourite = await _genericRepositories.SelectbyId<Favourite>(id);
            if (existingFavourite == null)
            {
                return NotFound(new { message = "Favourite not found." });
            }

            _mapper.Map(favouriteUpdateDTO, existingFavourite);
            await _genericRepositories.UpdatebyId(id, existingFavourite);

            return NoContent();
        }

        // POST: api/Favourites
        [HttpPost]
        public async Task<ActionResult<FavouriteReadDTO>> PostFavourite(FavouriteCreateDTO favouriteCreateDTO)
        {
            var favourite = _mapper.Map<Favourite>(favouriteCreateDTO);
            await _genericRepositories.Create(favourite);
            return CreatedAtAction(nameof(GetFavourite), new { id = favourite.FavouriteId }, _mapper.Map<FavouriteReadDTO>(favourite));
        }

        // DELETE: api/Favourites/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFavourite(int id)
        {
            var favourite = await _genericRepositories.SelectbyId<Favourite>(id);
            if (favourite == null)
            {
                return NotFound(new { message = "Favourite not found." });
            }

            await _genericRepositories.DeleteById<Favourite>(id);

            return NoContent();
        }

        // DELETE: api/Favourites/remove?userId=27&propertyId=1
        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFavourite(int userId, int propertyId)
        {
            var favourite = await _favouriteRepositories.GetFavouriteByUserIdAndPropertyIdAsync(userId, propertyId);
            if (favourite == null)
            {
                return NotFound(new { message = "Favourite not found." });
            }

            await _favouriteRepositories.RemoveFavouriteAsync(favourite);

            return NoContent();
        }
    }
}