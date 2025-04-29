using GharBhada.Data;
using GharBhada.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GharBhada.Repositories.SpecificRepositories.AgreementRepositories
{
    public class AgreementRepositories : IAgreementRepositories
    {
        private readonly GharBhadaContext _context;
        private readonly ILogger<AgreementRepositories> _logger;


        public AgreementRepositories(GharBhadaContext context, ILogger<AgreementRepositories> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Agreement> GetAgreementByBookingIdAsync(int bookingId)
        {
            return await _context.Agreements.FirstOrDefaultAsync(a => a.BookingId == bookingId);
        }

        public async Task<List<Agreement>> GetAgreementsByUserIdAsync(int userId)
        {
            return await _context.Agreements.Where(a => a.RenterId == userId).ToListAsync();
        }

        public async Task<List<Agreement>> GetAgreementsByLandlordIdAsync(int landlordId)
        {
            return await _context.Agreements.Where(a => a.LandlordId == landlordId).ToListAsync();
        }

        public async Task<int> GetTotalAgreementCountAsync()
        {
            return await _context.Agreements.CountAsync();
        }

        public async Task<int> GetApprovedAgreementCountAsync()
        {
            return await _context.Agreements.CountAsync(a => a.Status == "Approved");
        }

        public async Task<List<Agreement>> GetExpiredAgreementsAsync()
        {
            return await _context.Agreements
                .Where(a => a.EndDate < DateTime.UtcNow)
                .ToListAsync();
        }

        public async Task<List<Agreement>> GetExpiredAgreementsByRenterIdAsync(int renterId)
        {
            return await _context.Agreements
                .Where(a => a.RenterId == renterId && a.EndDate < DateTime.UtcNow)
                .ToListAsync();
        }


        public async Task UpdatePropertyStatusForAllExpiredAgreementsAsync()
        {
            try
            {
                // Find agreements that have expired but are not yet marked as "Expired"
                var expiredAgreements = await _context.Agreements
                    .Where(a => a.EndDate < DateTime.UtcNow && a.Status != "Expired")
                    .ToListAsync();

                _logger.LogInformation($"Found {expiredAgreements.Count} expired agreements to process");

                if (expiredAgreements.Any())
                {
                    // Get the booking IDs associated with the expired agreements
                    var bookingIds = expiredAgreements.Select(a => a.BookingId).ToList();
                    _logger.LogInformation($"Found {bookingIds.Count} booking IDs associated with expired agreements: {string.Join(",", bookingIds)}");

                    // Update the status of each expired agreement to "Expired" - DO NOT DELETE AGREEMENTS
                    foreach (var agreement in expiredAgreements)
                    {
                        agreement.Status = "Expired";
                        _logger.LogInformation($"Marking agreement {agreement.AgreementId} as Expired (EndDate: {agreement.EndDate})");
                    }

                    // Get ONLY the bookings associated with these agreements
                    var bookings = await _context.Bookings
                        .Where(b => bookingIds.Contains(b.BookingId))
                        .ToListAsync();

                    _logger.LogInformation($"Found {bookings.Count} bookings to update with IDs: {string.Join(",", bookings.Select(b => b.BookingId))}");

                    // Get the property IDs from the bookings before updating them
                    var propertyIds = bookings.Select(b => b.PropertyId).Distinct().ToList();
                    _logger.LogInformation($"Found {propertyIds.Count} properties to update with IDs: {string.Join(",", propertyIds)}");

                    // Update booking status to "Pending" instead of deleting them
                    foreach (var booking in bookings)
                    {
                        booking.Status = "Expired";
                        _logger.LogInformation($"Updating booking {booking.BookingId} status to 'Expired'");
                    }

                    // Update the status of ONLY the properties associated with these bookings
                    var properties = await _context.Properties
                        .Where(p => propertyIds.Contains(p.PropertyId))
                        .ToListAsync();

                    foreach (var property in properties)
                    {
                        _logger.LogInformation($"Updating property {property.PropertyId} status from '{property.Status}' to 'Available'");
                        property.Status = "Available";
                    }

                    // Save all changes to the database
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Successfully saved all changes to the database");

                    _logger.LogInformation($"Updated {expiredAgreements.Count} agreements to 'Expired' status, changed {bookings.Count} bookings to 'Pending' status, and updated {properties.Count} properties to 'Available' status");
                }
                else
                {
                    _logger.LogInformation("No expired agreements found that need processing");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdatePropertyStatusForAllExpiredAgreementsAsync");
                throw; // rethrow to ensure the controller handles it
            }
        }
    }
}