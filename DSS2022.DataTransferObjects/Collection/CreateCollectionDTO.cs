using DSS2022.DataTransferObjects.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
