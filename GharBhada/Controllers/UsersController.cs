using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using GharBhada.Repositories.GenericRepositories;
using GharBhada.Models;
using GharBhada.DTOs.UserDTOs;
using Microsoft.Extensions.Logging;
using GharBhada.Repositories.SpecificRepositories.UserRepositories;

namespace GharBhada.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepositories _genericRepositories;
        private readonly IUserRepositories _userRepositories;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMapper mapper, IGenericRepositories genericRepositories, IUserRepositories userRepositories, ILogger<UsersController> logger)
        {
            _mapper = mapper;
            _genericRepositories = genericRepositories;
            _userRepositories = userRepositories;
            _logger = logger;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDTO>>> GetUsers()
        {
            var users = await _genericRepositories.SelectAll<User>();
            return Ok(_mapper.Map<IEnumerable<UserReadDTO>>(users));
        }

        // GET: api/Users/firebase/{firebaseUserId}
        [HttpGet("firebase/{firebaseUserId}")]
        public async Task<ActionResult<int>> GetUserIdByFirebaseId(string firebaseUserId)
        {
            var userId = await _userRepositories.GetUserIdByFirebaseId(firebaseUserId);

            if (userId == 0)
            {
                return NotFound(new { message = "User not found for the provided Firebase User ID." });
            }

            return Ok(userId);
        }

        // GET: api/Users/firebaseByUserId/{id}
        [HttpGet("firebaseByUserId/{id}")]
        public async Task<ActionResult<string>> GetFirebaseUserIdByUserId(int id)
        {
            var firebaseUserId = await _userRepositories.GetFirebaseUserIdByUserId(id);

            if (firebaseUserId == null)
            {
                return NotFound(new { message = "Firebase User ID not found for the provided User ID." });
            }

            return Ok(firebaseUserId);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDTO>> GetUser(int id)
        {
            var user = await _genericRepositories.SelectbyId<User>(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserReadDTO>(user));
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserUpdateDTO userUpdateDTO)
        {
            if (id != userUpdateDTO.UserId)
            {
                return BadRequest(new { message = "Mismatched user ID." });
            }

            var existingUser = await _genericRepositories.SelectbyId<User>(id);
            if (existingUser == null)
            {
                return NotFound(new { message = "User not found." });
            }

            _mapper.Map(userUpdateDTO, existingUser);
            await _genericRepositories.UpdatebyId(id, existingUser);

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserReadDTO>> PostUser(UserCreateDTO userCreateDTO)
        {
            var user = _mapper.Map<User>(userCreateDTO);

            // Ensure the role is set to 'renter' by default if not provided
            if (string.IsNullOrEmpty(user.UserRole))
            {
                user.UserRole = "renter"; // Default role
            }

            await _genericRepositories.Create(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, _mapper.Map<UserReadDTO>(user));
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _genericRepositories.SelectbyId<User>(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            await _genericRepositories.DeleteById<User>(id);

            return NoContent();
        }
    }
}