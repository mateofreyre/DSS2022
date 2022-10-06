using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DSS2022.Model
{
    public class Model : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ModelTypes ModelType { get; set; }
        public int CollectionId { get; set; }
        public virtual Collection Collection { get; set; }

    }
}
