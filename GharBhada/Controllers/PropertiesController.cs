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
using GharBhada.Repositories.SpecificRepositories.PropertyRepositories;
using GharBhada.Repositories.SpecificRepositories.PaymentRepositories;

namespace GharBhada.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepositories _genericRepositories;
        private readonly IPropertyRepositories _propertyRepositories;
        private readonly IPaymentRepositories _paymentRepositories;

        public PropertiesController(IMapper mapper, IGenericRepositories genericRepositories, IPropertyRepositories propertyRepositories, IPaymentRepositories paymentRepositories)
        {
            _mapper = mapper;
            _genericRepositories = genericRepositories;
            _propertyRepositories = propertyRepositories;
            _paymentRepositories = paymentRepositories;
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
                return Ok(new { message = "Property not found." });
            }
            return Ok(_mapper.Map<PropertyReadDTO>(property));
        }

        // GET: api/Properties/Landlord/5
        [HttpGet("Landlord/{landlordId}")]
        public ActionResult<IEnumerable<PropertyReadDTO>> GetPropertiesByLandlordId(int landlordId)
        {
            var properties = _propertyRepositories.GetPropertiesByLandlordId(landlordId);
            if (properties == null || !properties.Any())
            {
                return Ok(new { message = "No properties found for this landlord." });
            }
            return Ok(_mapper.Map<IEnumerable<PropertyReadDTO>>(properties));
        }

        // GET: api/Properties/total-count
        [HttpGet("total-count")]
        public async Task<ActionResult<int>> GetTotalPropertyCount()
        {
            var totalCount = await _propertyRepositories.GetTotalPropertyCountAsync();
            return Ok(totalCount);
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
                return Ok(new { message = "Property not found." });
            }

            _mapper.Map(propertyUpdateDTO, existingProperty);
            await _genericRepositories.UpdatebyId(id, existingProperty);

            return NoContent();
        }

        // PUT: api/Properties/updateStatus/5
        [HttpPut("updateStatus/{propertyId}")]
        public async Task<IActionResult> UpdatePropertyStatus(int propertyId)
        {
            var isPaymentCompleted = await _paymentRepositories.IsPaymentCompletedForPropertyAsync(propertyId);
            if (isPaymentCompleted)
            {
                await _propertyRepositories.UpdatePropertyStatusAsync(propertyId, "Rented");
                return Ok(new { message = "Property status updated to Rented." });
            }
            return BadRequest(new { message = "No completed payment found for this property." });
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
                return Ok(new { message = "Property not found." });
            }

            await _genericRepositories.DeleteById<Property>(id);

            return NoContent();
        }
    }
}