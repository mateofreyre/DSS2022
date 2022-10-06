
using DSS2022.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2022.DataTransferObjects.Model
{
    public class ModelDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ModelTypes ModelType { get; set; }

    }
}
