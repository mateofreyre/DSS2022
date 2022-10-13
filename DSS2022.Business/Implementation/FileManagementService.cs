using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DSS2022.Business.Implementation
{
    public class FileManagementService: IFileManagementService
    {
        private readonly ILogger _logger;
        private IConfiguration _configuration;

        public FileManagementService(IConfiguration configuration, ILogger<FileManagementService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task SaveFile(string fileName,  Stream fileStream, string whereToSave)
        {
                string storagePath = Directory.GetCurrentDirectory();
                storagePath = Path.Combine(storagePath, whereToSave);
                string rootPath = FileUploadChecklist(fileName, storagePath);
                await SaveFileInLocalStorage(rootPath, fileStream);

        }
        private string FileUploadChecklist(string fileName, string storagePath)
        {
            // Validate if StoragePath exists
            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }
            // Storage Name
            string uuid = Guid.NewGuid().ToString();
            string storageName = fileName;
            // File Path
            var rootPath = Path.Combine(storagePath, storageName);
            if (File.Exists(storagePath))
            {
                throw new Exception($"The ({storageName}) already exists in the path ({storagePath}).");
            }
            
            return rootPath;
        }
        private async Task SaveFileInLocalStorage(string storagePath, Stream fileStream)
        {
            using (var stream = new FileStream(storagePath, FileMode.CreateNew))
            {
                await fileStream.CopyToAsync(stream);
            }
        }
    }
}
