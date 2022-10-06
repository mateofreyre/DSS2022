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
        public int ManufacturingTime { get; set; }
        public DateTime ReleaseDate { get; set; }
        public virtual ICollection<Model> Models { get; set; } = new List<Model>();

    }
}
