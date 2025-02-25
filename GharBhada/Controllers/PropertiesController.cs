using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using GharBhada.Repositories.GenericRepositories;
using GharBhada.Models;
using GharBhada.DTOs.PropertyDTOs;

namespace GharBhada.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepositories _genericRepositories;

        public PropertiesController(IMapper mapper, IGenericRepositories genericRepositories)
        {
            _mapper = mapper;
            _genericRepositories = genericRepositories;
        }

        // GET: api/Properties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyReadDTO>>> GetProperties()
        {
            var properties = await _genericRepositories.SelectAll<Property>();
            return Ok(_mapper.Map<IEnumerable<PropertyReadDTO>>(properties));
        }

        // GET: api/Properties/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyReadDTO>> GetProperty(int id)
        {
            var property = await _genericRepositories.SelectbyId<Property>(id);
            if (property == null)
            {
                return NotFound(new { message = "Property not found." });
            }
            return Ok(_mapper.Map<PropertyReadDTO>(property));
        }

        // PUT: api/Properties/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProperty(int id, PropertyUpdateDTO propertyUpdateDTO)
        {
            if (id != propertyUpdateDTO.PropertyId)
            {
                return BadRequest(new { message = "Mismatched property ID." });
            }

            var existingProperty = await _genericRepositories.SelectbyId<Property>(id);
            if (existingProperty == null)
            {
                return NotFound(new { message = "Property not found." });
            }

            _mapper.Map(propertyUpdateDTO, existingProperty);
            await _genericRepositories.UpdatebyId(id, existingProperty);

            return NoContent();
        }

        // POST: api/Properties
        [HttpPost]
        public async Task<ActionResult<PropertyReadDTO>> PostProperty(PropertyCreateDTO propertyCreateDTO)
        {
            var property = _mapper.Map<Property>(propertyCreateDTO);
            await _genericRepositories.Create(property);
            return CreatedAtAction(nameof(GetProperty), new { id = property.PropertyId }, _mapper.Map<PropertyReadDTO>(property));
        }

        // DELETE: api/Properties/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            var property = await _genericRepositories.SelectbyId<Property>(id);
            if (property == null)
            {
                return NotFound(new { message = "Property not found." });
            }

            await _genericRepositories.DeleteById<Property>(id);

            return NoContent();
        }
    }
}