using AutoMapper;
using DSS2022.DataTransferObjects.Collection;
using DSS2022.DataTransferObjects.Model;
using DSS2022.Model;

namespace DSS2002.Api.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Colletions   
            CreateMap<Collection, CollectionDTO>();
            CreateMap<CollectionDTO, Collection>();
            CreateMap<CreateCollectionDTO, Collection>();
            #endregion

            #region Model
            CreateMap<Model, ModelDTO>();
            CreateMap<ModelDTO, Model>();
            #endregion


        }
    }
}