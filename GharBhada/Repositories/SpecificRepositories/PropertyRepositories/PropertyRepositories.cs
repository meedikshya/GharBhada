using GharBhada.Data;
using GharBhada.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GharBhada.Repositories.SpecificRepositories.PropertyRepositories
{
    public class PropertyRepositories : IPropertyRepositories
    {
        private readonly GharBhadaContext _context;

        public PropertyRepositories(GharBhadaContext context)
        {
            _context = context;
        }

        public IEnumerable<Property> GetPropertiesByLandlordId(int landlordId)
        {
            return _context.Properties.Where(p => p.LandlordId == landlordId).ToList();
        }

        public async Task UpdatePropertyStatusAsync(int propertyId, string status)
        {
            var property = await _context.Properties.FindAsync(propertyId);
            if (property != null)
            {
                property.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetTotalPropertyCountAsync()
        {
            return await _context.Properties.CountAsync();
        }
    }
}