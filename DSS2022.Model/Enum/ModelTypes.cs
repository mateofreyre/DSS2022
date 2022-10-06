using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DSS2022.Model
{
    public enum ModelTypes
    {
        [EnumMember]
        [Description("Lentes de sol")]
        Sunglasses = 1,

        [EnumMember]
        [Description("Lentes de lectura")]
        Readingglasses = 2
    }
}
