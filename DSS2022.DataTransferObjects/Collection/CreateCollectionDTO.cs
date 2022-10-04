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
        public int PlazoFabricacion { get; set; }
        public DateTime FechaLanzamientoEstimada { get; set; }
        public ICollection<string> NombreDeModelos { get; set; } = new List<string>();
        public ICollection<string> TipoDeModelos { get; set; }
    }
}
