using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2022.Model
{
    public class Collection : BaseEntity
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public int PlazoFabricacion { get; set; }
        public DateTime FechaLanzamientoEstimada { get; set; }
        public ICollection<Model> NombreDeModelos { get; set; } = new List<Model>();
        public ICollection<ModelType> TipoDeModelos { get; set; } = new List<ModelType>();

    }
}
