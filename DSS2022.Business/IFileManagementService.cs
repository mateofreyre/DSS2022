using System;
using System.IO;
using System.Threading.Tasks;

namespace DSS2022.Business
{
    public interface IFileManagementService
    {
        Task SaveFile(string fileName, Stream fileStream, string whereToStore);
    }
}
