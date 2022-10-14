using DSS2022.DataTransferObjects.Collection;
using DSS2022.Model;

namespace DSS2022.Business
{
    public interface ICollectionService
    {
        Task<CollectionDTO> GetByIdAsync(int id, string token);
        Task<Collection> Create(CreateCollectionDTO dto, string token, string sessionId);
        Task<List<CollectionDTO>> GetAll();
    }
}
