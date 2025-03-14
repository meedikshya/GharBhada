using GharBhada.Models;

namespace GharBhada.Repositories.SpecificRepositories.PropertyRepositories

{
    public interface IPropertyRepositories
    {
        IEnumerable<Property> GetPropertiesByLandlordId(int landlordId);
        Task UpdatePropertyStatusAsync(int propertyId, string status);
    }
}
