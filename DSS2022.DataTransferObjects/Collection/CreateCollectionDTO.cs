using DSS2022.DataTransferObjects.Model;
using Microsoft.AspNetCore.Http;

namespace DSS2022.DataTransferObjects.Collection
{
    public class CreateCollectionDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ManufacturingTime { get; set; }
        public DateTime ReleaseDate { get; set; }
        public ICollection<ModelDTO> Models { get; set; } = new List<ModelDTO>();
    }
}
