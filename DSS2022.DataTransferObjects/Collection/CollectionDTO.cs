using DSS2022.DataTransferObjects.Model;
using DSS2022.DataTransferObjects.ModelType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2022.DataTransferObjects.Collection
{
    public class CollectionDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int PlazoFabricacion { get; set; }
        public DateTime FechaLanzamientoEstimada { get; set; }
        public ICollection<ModelDTO> NombreDeModelos { get; set; } = new List<ModelDTO>();
        public ICollection<ModelTypeDTO> TipoDeModelos { get; set; }
    }
}
