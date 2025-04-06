using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GharBhada.Repositories.GenericRepositories;
using GharBhada.Models;
using AutoMapper;
using GharBhada.DTOs.BookingDTOs;
using GharBhada.Repositories.SpecificRepositories.BookingRepositories;

namespace GharBhada.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepositories _genericRepositories;
        private readonly IBookingRepositories _bookingRepositories;

        public BookingsController(IMapper mapper, IGenericRepositories genericRepositories, IBookingRepositories bookingRepositories)
        {
            _mapper = mapper;
            _genericRepositories = genericRepositories;
            _bookingRepositories = bookingRepositories;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingReadDTO>>> GetBookings()
        {
            var bookings = await _genericRepositories.SelectAll<Booking>();
            return Ok(_mapper.Map<IEnumerable<BookingReadDTO>>(bookings));
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingReadDTO>> GetBooking(int id)
        {
            var booking = await _genericRepositories.SelectbyId<Booking>(id);
            if (booking == null)
            {
                return NotFound(new { message = "Booking not found." });
            }
            return Ok(_mapper.Map<BookingReadDTO>(booking));
        }

        // GET: api/Bookings/User/5
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<BookingReadDTO>>> GetBookingsByUserId(int userId)
        {
            var bookings = await _bookingRepositories.GetBookingsByUserId(userId);
            if (bookings == null || bookings.Count == 0)
            {
                return Ok(new { message = "No bookings found for this user." });
            }
            return Ok(_mapper.Map<IEnumerable<BookingReadDTO>>(bookings));
        }

        // GET: api/Bookings/accepted-count
        [HttpGet("accepted-count")]
        public async Task<ActionResult<int>> GetAcceptedBookingCount()
        {
            var count = await _bookingRepositories.GetAcceptedBookingCountAsync();
            return Ok(count);
        }

        // GET: api/Bookings/total-count
        [HttpGet("total-count")]
        public async Task<ActionResult<int>> GetTotalBookingCount()
        {
            var count = await _bookingRepositories.GetTotalBookingCountAsync();
            return Ok(count);
        }

        // PUT: api/Bookings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, BookingUpdateDTO bookingUpdateDTO)
        {
            if (id != bookingUpdateDTO.BookingId)
            {
                return BadRequest(new { message = "Mismatched booking ID." });
            }

            var existingBooking = await _genericRepositories.SelectbyId<Booking>(id);
            if (existingBooking == null)
            {
                return NotFound(new { message = "Booking not found." });
            }

            _mapper.Map(bookingUpdateDTO, existingBooking);
            await _genericRepositories.UpdatebyId(id, existingBooking);

            return NoContent();
        }

        // POST: api/Bookings
        [HttpPost]
        public async Task<ActionResult<BookingReadDTO>> PostBooking(BookingCreateDTO bookingCreateDTO)
        {
            var booking = _mapper.Map<Booking>(bookingCreateDTO);
            await _genericRepositories.Create(booking);
            return CreatedAtAction(nameof(GetBooking), new { id = booking.BookingId }, _mapper.Map<BookingReadDTO>(booking));
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _genericRepositories.SelectbyId<Booking>(id);
            if (booking == null)
            {
                return NotFound(new { message = "Booking not found." });
            }

            await _genericRepositories.DeleteById<Booking>(id);

            return NoContent();
        }
    }
}