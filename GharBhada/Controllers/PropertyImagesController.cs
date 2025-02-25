using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using GharBhada.Repositories.GenericRepositories;
using GharBhada.Models;
using GharBhada.DTOs.PropertyImageDTOs;

namespace GharBhada.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyImagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepositories _genericRepositories;

        public PropertyImagesController(IMapper mapper, IGenericRepositories genericRepositories)
        {
            _mapper = mapper;
            _genericRepositories = genericRepositories;
        }

        // GET: api/PropertyImages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyImageReadDTO>>> GetPropertyImages()
        {
            var propertyImages = await _genericRepositories.SelectAll<PropertyImage>();
            return Ok(_mapper.Map<IEnumerable<PropertyImageReadDTO>>(propertyImages));
        }

        // GET: api/PropertyImages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyImageReadDTO>> GetPropertyImage(int id)
        {
            var propertyImage = await _genericRepositories.SelectbyId<PropertyImage>(id);
            if (propertyImage == null)
            {
                return NotFound(new { message = "PropertyImage not found." });
            }
            return Ok(_mapper.Map<PropertyImageReadDTO>(propertyImage));
        }

        // PUT: api/PropertyImages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPropertyImage(int id, PropertyImageUpdateDTO propertyImageUpdateDTO)
        {
            if (id != propertyImageUpdateDTO.PropertyImageId)
            {
                return BadRequest(new { message = "Mismatched property image ID." });
            }

            var existingPropertyImage = await _genericRepositories.SelectbyId<PropertyImage>(id);
            if (existingPropertyImage == null)
            {
                return NotFound(new { message = "PropertyImage not found." });
            }

            _mapper.Map(propertyImageUpdateDTO, existingPropertyImage);
            await _genericRepositories.UpdatebyId(id, existingPropertyImage);

            return NoContent();
        }

        // POST: api/PropertyImages
        [HttpPost]
        public async Task<ActionResult<PropertyImageReadDTO>> PostPropertyImage(PropertyImageCreateDTO propertyImageCreateDTO)
        {
            var propertyImage = _mapper.Map<PropertyImage>(propertyImageCreateDTO);
            await _genericRepositories.Create(propertyImage);
            return CreatedAtAction(nameof(GetPropertyImage), new { id = propertyImage.PropertyImageId }, _mapper.Map<PropertyImageReadDTO>(propertyImage));
        }

        // DELETE: api/PropertyImages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePropertyImage(int id)
        {
            var propertyImage = await _genericRepositories.SelectbyId<PropertyImage>(id);
            if (propertyImage == null)
            {
                return NotFound(new { message = "PropertyImage not found." });
            }

            await _genericRepositories.DeleteById<PropertyImage>(id);

            return NoContent();
        }
    }
}