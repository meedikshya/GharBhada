using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using GharBhada.Repositories.GenericRepositories;
using GharBhada.Models;
using GharBhada.DTOs.UserDetailDTOs;
using GharBhada.Repositories.SpecificRepositories.UserDetailRepositories;

namespace GharBhada.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepositories _genericRepositories;
        private readonly IUserDetailRepositories _userDetailRepositories;

        public UserDetailsController(IMapper mapper, IGenericRepositories genericRepositories, IUserDetailRepositories userDetailRepositories)
        {
            _mapper = mapper;
            _genericRepositories = genericRepositories;
            _userDetailRepositories = userDetailRepositories;
        }

        // GET: api/UserDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetailReadDTO>>> GetUserDetails()
        {
            var userDetails = await _genericRepositories.SelectAll<UserDetail>();
            return Ok(_mapper.Map<IEnumerable<UserDetailReadDTO>>(userDetails));
        }

        // GET: api/UserDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetailReadDTO>> GetUserDetail(int id)
        {
            var userDetail = await _genericRepositories.SelectbyId<UserDetail>(id);
            if (userDetail == null)
            {
                return NotFound(new { message = "UserDetail not found." });
            }
            return Ok(_mapper.Map<UserDetailReadDTO>(userDetail));
        }

        // GET: api/UserDetails/userId/28
        [HttpGet("userId/{id}")]
        public async Task<ActionResult<UserDetailReadDTO>> GetUserDetailByUserId(int id)
        {
            var userDetails = await _userDetailRepositories.GetUserDetailsByUserId(id);
            if (userDetails == null || !userDetails.Any())
            {
                return NotFound(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = $"No user detail found for userId {id}"
                });
            }

            return Ok(_mapper.Map<UserDetailReadDTO>(userDetails.First()));
        }

        // PUT: api/UserDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserDetail(int id, UserDetailUpdateDTO userDetailUpdateDTO)
        {
            if (id != userDetailUpdateDTO.UserDetailId)
            {
                return BadRequest(new { message = "Mismatched user detail ID." });
            }

            var existingUserDetail = await _genericRepositories.SelectbyId<UserDetail>(id);
            if (existingUserDetail == null)
            {
                return NotFound(new { message = "UserDetail not found." });
            }

            _mapper.Map(userDetailUpdateDTO, existingUserDetail);
            await _genericRepositories.UpdatebyId(id, existingUserDetail);

            return NoContent();
        }

        // POST: api/UserDetails
        [HttpPost]
        public async Task<ActionResult<UserDetailReadDTO>> PostUserDetail(UserDetailCreateDTO userDetailCreateDTO)
        {
            var userDetail = _mapper.Map<UserDetail>(userDetailCreateDTO);
            await _genericRepositories.Create(userDetail);
            return CreatedAtAction(nameof(GetUserDetail), new { id = userDetail.UserDetailId }, _mapper.Map<UserDetailReadDTO>(userDetail));
        }

        // DELETE: api/UserDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserDetail(int id)
        {
            var userDetail = await _genericRepositories.SelectbyId<UserDetail>(id);
            if (userDetail == null)
            {
                return NotFound(new { message = "UserDetail not found." });
            }

            await _genericRepositories.DeleteById<UserDetail>(id);

            return NoContent();
        }
    }
}