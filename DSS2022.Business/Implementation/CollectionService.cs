using AutoMapper;
using DSS2022.Data;
using DSS2022.DataTransferObjects.Collection;
using DSS2022.Model;
using DSS2022.Business.Helpers;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace DSS2022.Business.Implementation
{
    public class CollectionService : ICollectionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private String bonitaUrl = "http://localhost:8080/bonita/";


        public CollectionService(IUnitOfWork unitOfWork,
                                 IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Collection> Create(CreateCollectionDTO collectionCreateDTO, string bonitaSessionId, string bonitaApiKey)
        {

            var collection = _mapper.Map<Collection>(collectionCreateDTO);
            await _unitOfWork.CollectionRepository.AddAsync(collection);

            await StartProcess(5483034383519259897, bonitaSessionId, bonitaApiKey);

            await SetVariable(collection, bonitaSessionId, bonitaApiKey);

            await _unitOfWork.Complete();

            return collection;
        }

        public async Task<CollectionDTO> GetByIdAsync(int id)
        {
            var collection = await _unitOfWork.CollectionRepository.ReadAsync(id);


            var collectionDTO = _mapper.Map<CollectionDTO>(collection);
            return collectionDTO;
        }

        public async Task<List<CollectionDTO>> GetAll()
        {
            var collectionList = await _unitOfWork.CollectionRepository.ReadAllAsync();

            var collectionDTO = collectionList.ToList().Select(i => _mapper.Map<CollectionDTO>(i)).ToList();
            return collectionDTO;
        }


        public async Task<string> StartProcess(long id, string bonitaSessionId, string bonitaApiKey)
        {

            var handler = new HttpClientHandler();
            using (var client = new HttpClient(handler))
            {
                //const string url = "http://localhost:8080/bonita/";

                var content = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("ticket_account", ""),
                    new KeyValuePair<string, string>("ticket_description", ""),
                    new KeyValuePair<string, string>("ticket_subject", "")
                });
                var uri = new Uri(bonitaUrl);
                client.BaseAddress = uri;

                client.DefaultRequestHeaders.Add("X-Bonita-API-Token", bonitaApiKey);
                client.DefaultRequestHeaders.Add("JSESSIONID", bonitaSessionId);

                var cookie = "JSESSIONID=" + bonitaSessionId + ";"+ "X-Bonita-API-Token=" + bonitaApiKey;
                client.DefaultRequestHeaders.Add("Cookie", cookie);


                HttpResponseMessage response = await client.GetAsync(bonitaUrl + "API/bpm/process?f=name=Elaboración de lentes");

                var responseBodyAsText = await response.Content.ReadAsStringAsync();

                var separado = responseBodyAsText.Split(",");
                var processIdField = separado[6];
                var precessId = Regex.Match(processIdField, @"\d+").Value;



                HttpResponseMessage otraresponse = await client.PostAsync(bonitaUrl +"API/bpm/process/"+ precessId+"/instantiation", null);


                var pepe = true;

                return "";
            }
        }

        public async Task SetVariable(Collection collection, string bonitaSessionId, string bonitaApiKey)
        {
            AuthenticationHelper authenticationHelper = new AuthenticationHelper();

            var handler = new HttpClientHandler();
            using (var client = new HttpClient(handler))
            {
               // const string url = "http://localhost:8080/bonita/";
                var uri = new Uri(bonitaUrl);
                client.BaseAddress = uri;

                client.DefaultRequestHeaders.Add("X-Bonita-API-Token", bonitaApiKey);
                client.DefaultRequestHeaders.Add("JSESSIONID", bonitaSessionId);

                var cookie = "JSESSIONID=" + bonitaSessionId + ";" + "X-Bonita-API-Token=" + bonitaApiKey;
                client.DefaultRequestHeaders.Add("Cookie", cookie);


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
