using AutoMapper;
using DSS2022.Data;
using DSS2022.DataTransferObjects.Collection;
using DSS2022.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSS2022.Business.Helpers;

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

            await StartProcess(1);

            await SetVariable(collection);

            await _unitOfWork.Complete();

            return collection;
        }

        public async Task<CollectionDTO> GetByIdAsync(int id, string token)
        {
            var collection = await _unitOfWork.CollectionRepository.ReadAsync(id);

            await StartProcess(1);

            var collectionDTO = _mapper.Map<CollectionDTO>(collection);
            return collectionDTO;
        }

        public async Task<List<CollectionDTO>> GetAll()
        {
            var collectionList = await _unitOfWork.CollectionRepository.ReadAllAsync();

            var collectionDTO = collectionList.ToList().Select(i => _mapper.Map<CollectionDTO>(i)).ToList();
            return collectionDTO;
        }


        public async Task<string> StartProcess(int id)
        {
            AuthenticationHelper authenticationHelper = new AuthenticationHelper();
            var token = "";
            token = await authenticationHelper.Login();

            token = token.Split("=")[1];
            var handler = new HttpClientHandler();
            using (var client = new HttpClient(handler))
            {
                const string url = "http://localhost:8080/bonita/";

                var content = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("ticket_account", ""),
                    new KeyValuePair<string, string>("ticket_description", ""),
                    new KeyValuePair<string, string>("ticket_subject", "")
                });
                var uri = new Uri(url);
                client.BaseAddress = uri;

                client.DefaultRequestHeaders.Add("X-Bonita-API-Token", token);
                HttpResponseMessage response = await client.PostAsync("API/bpm/process/"+id+"/instantiation", content);


                var pepe = true;

                return "";
            }
        }

        public async Task SetVariable(Collection collection)
        {
            AuthenticationHelper authenticationHelper = new AuthenticationHelper();
            var token = "";
            token = await authenticationHelper.Login();

            token = token.Split("=")[1];
            var handler = new HttpClientHandler();
            using (var client = new HttpClient(handler))
            {
                const string url = "http://localhost:8080/bonita/";
                var uri = new Uri(url);
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Add("X-Bonita-API-Token", token);


                HttpResponseMessage responseTaskID = await client.GetAsync("/API/bpm/userTask/" + 1);

                var content1 = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("ticket_account", ""),
                    new KeyValuePair<string, string>("ticket_description", ""),
                    new KeyValuePair<string, string>("ticket_subject", "")
                });

                var taskId = 1;

                HttpResponseMessage response = await client.GetAsync("/API/bpm/caseVariable/"+taskId+"/"+collection);

                var pepe = true;
            }

        }
    }
}
