using AutoMapper;
using DSS2022.Data;
using DSS2022.DataTransferObjects.Collection;
using DSS2022.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2022.Business.Implementation
{
    public class CollectionService : ICollectionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public CollectionService(IUnitOfWork unitOfWork,
                                 IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Collection> Create(CreateCollectionDTO collectionCreateDTO)
        {
            var collection = _mapper.Map<Collection>(collectionCreateDTO);
            await _unitOfWork.CollectionRepository.AddAsync(collection);

            await _unitOfWork.Complete();

            return collection;
        }

        public async Task<CollectionDTO> GetByIdAsync(int id)
        {
            var collection = await _unitOfWork.CollectionRepository.ReadAsync(id);

            var collectionDTO = _mapper.Map<CollectionDTO>(collection);
            return collectionDTO;
        }
    }
}
