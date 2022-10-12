using AutoMapper;
using DSS2022.Data;
using DSS2022.DataTransferObjects.Collection;
using DSS2022.Model;
using DSS2022.Business.Helpers;

namespace DSS2022.Business.Implementation
{
    public class CollectionService : ICollectionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private String bonitaUrl = "http://localhost:38169/bonita/";
        private IBonitaBpmService _bonitaBpmService;


        public CollectionService(IUnitOfWork unitOfWork, IMapper mapper, IBonitaBpmService bonitaBpmService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _bonitaBpmService = bonitaBpmService;
        }

        public async Task<Collection> Create(CreateCollectionDTO collectionCreateDTO, string token, string sessionId)
        {

            var collection = _mapper.Map<Collection>(collectionCreateDTO);
            await _unitOfWork.CollectionRepository.AddAsync(collection);

            var processId = await _bonitaBpmService.GetProcessId(token, sessionId);
            //await _bonitaBpmService.StartProcess(processId, token, sessionId);
            await _bonitaBpmService.CreateCase(collection, processId, token, sessionId);

            //await _bonitaBpmService.SetVariable(collection);

            await  _unitOfWork.Complete();

            return collection;
        }

        public async Task<CollectionDTO> GetByIdAsync(int id, string token)
        {
            var collection = await _unitOfWork.CollectionRepository.ReadAsync(id);

          //  await _bonitaBpmService.StartProcess(1, token, ses);

            var collectionDTO = _mapper.Map<CollectionDTO>(collection);
            return collectionDTO;
        }

        public async Task<List<CollectionDTO>> GetAll()
        {
            var collectionList = await _unitOfWork.CollectionRepository.ReadAllAsync();

            var collectionDTO = collectionList.ToList().Select(i => _mapper.Map<CollectionDTO>(i)).ToList();
            return collectionDTO;
        }
        
    }
}
