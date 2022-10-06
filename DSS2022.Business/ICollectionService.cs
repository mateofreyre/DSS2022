using DSS2022.DataTransferObjects.Collection;
using DSS2022.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2022.Business
{
    public interface ICollectionService
    {
        Task<CollectionDTO> GetByIdAsync(int id);
        Task<Collection> Create(CreateCollectionDTO dto);
        Task<List<CollectionDTO>> GetAll();
    }
}
